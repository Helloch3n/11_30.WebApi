using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ActiveDirectoryService.Infrastructure.ActiveDirectory
{
    public class ActiveDirectoryClient : IActiveDirectoryClient
    {
        private readonly ActiveDirectoryOptions _opt;
        private readonly IDistributedCache _cache;
        private readonly ILogger<ActiveDirectoryClient> _logger;

        public ActiveDirectoryClient(IOptions<ActiveDirectoryOptions> options, IDistributedCache cache, ILogger<ActiveDirectoryClient> logger)
        {
            _opt = options.Value;
            _cache = cache;
            _logger=logger;
        }

        public bool ChangePasswordAsync(string userName, string oldPassword, string newPassword)
        {
            string ldapServer = _opt.DomainIp; // 域名

            using var connection = new LdapConnection(new LdapDirectoryIdentifier(ldapServer));

            connection.SessionOptions.Sealing = true;
            connection.SessionOptions.Signing = true;
            connection.AuthType = AuthType.Negotiate;
            connection.SessionOptions.ProtocolVersion = 3;
            try
            {
                //验证用户密码是否正确
                connection.Bind(new NetworkCredential($"{userName}@{_opt.Domain}", oldPassword));
                //链接管理员账号
                connection.Bind(new NetworkCredential($"{_opt.AdminUsername}@{_opt.Domain}", _opt.AdminPassword));
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTime.Now}链接域控服务器失败： {ex.Message}");
                return false;
            }
            try
            {

                string dn = GetUserDistinguishedName(connection, userName);
                if (dn == null)
                {
                    _logger.LogInformation($"{DateTime.Now}重置密码失败，未找到用户：{userName}");
                    return false;
                }

                // 3. 构建修改密码的请求
                DirectoryAttributeModification passwordMod = new DirectoryAttributeModification
                {
                    Name = "unicodePwd",
                    Operation = DirectoryAttributeOperation.Replace
                };

                // 4. 密码格式转换（需转为UTF-16LE字节数组）
                passwordMod.Add(Encoding.Unicode.GetBytes($"\"{newPassword}\""));

                // 5. 创建修改请求
                var request = new ModifyRequest(dn, passwordMod);

                // 6. 发送请求
                var response = (ModifyResponse)connection.SendRequest(request);
                _logger.LogInformation($"{DateTime.Now}修改密码成功，用户：{userName}   ");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTime.Now}修改密出错： {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 修改域账号密码-验证码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="newPassword"></param>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ChangePasswordByCapchaAsync(string userName, string newPassword, string currentCapcha)
        {
            //获取验证码
            var cacheKey = $"AD:{userName}";
            var capcha = await _cache.GetStringAsync(cacheKey);
            //校验验证码
            if (capcha==currentCapcha)
            {
                try
                {
                    await _cache.RemoveAsync(cacheKey);
                    string ldapServer = _opt.DomainIp; // 域名

                    // 1. 创建LADP连接
                    using var connection = new LdapConnection(new LdapDirectoryIdentifier(ldapServer));

                    connection.SessionOptions.Sealing = true;
                    connection.SessionOptions.Signing = true;
                    connection.AuthType = AuthType.Negotiate;
                    connection.SessionOptions.ProtocolVersion = 3;

                    try
                    {
                        connection.Bind(new NetworkCredential($"{_opt.AdminUsername}@{_opt.Domain}", _opt.AdminPassword));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"{DateTime.Now}链接域控服务器失败： {ex.Message}");
                        return false;
                    }


                    // 2.校验用户是否存在
                    string dn = GetUserDistinguishedName(connection, userName);
                    if (dn == null)
                    {
                        _logger.LogInformation($"{DateTime.Now}重置密码失败，未找到用户：{userName}");
                        return false;
                    }


                    // 3. 构建修改密码的请求
                    DirectoryAttributeModification passwordMod = new DirectoryAttributeModification
                    {
                        Name = "unicodePwd",
                        Operation = DirectoryAttributeOperation.Replace
                    };

                    // 4. 密码格式转换（需转为UTF-16LE字节数组）
                    passwordMod.Add(Encoding.Unicode.GetBytes($"\"{newPassword}\""));

                    // 5. 创建修改请求
                    var request = new ModifyRequest(dn, passwordMod);

                    // 6. 发送请求
                    var response = (ModifyResponse)connection.SendRequest(request);
                    _logger.LogInformation($"{DateTime.Now}重置密码成功，用户：{userName}");
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"{DateTime.Now}重置密码出错： {ex.Message}");
                    return false;
                }

            }
            _logger.LogWarning($"{DateTime.Now}验证码错误");
            return false;

        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<string> SendCapchaAsync(string userName)
        {
            //生成验证码
            byte[] bytes = new byte[2]; // 2字节足够存 0~65535
            RandomNumberGenerator.Fill(bytes);
            string capcha = (BitConverter.ToUInt16(bytes, 0) % 900000 + 100000).ToString(); // 保证 1000~9999

            //存储验证码
            await _cache.SetStringAsync($"AD:{userName}", capcha, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) //五分钟过期
            });

            return capcha;
        }

        /// <summary>
        /// 获取AD域账号用户名
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="samAccountName"></param>
        /// <returns></returns>
        private string GetUserDistinguishedName(LdapConnection connection, string samAccountName)
        {
            var request = new SearchRequest(
                "DC=tbea-dl,DC=com,DC=cn", // 根节点
                $"(sAMAccountName={samAccountName})",
                SearchScope.Subtree,
                "distinguishedName"
            );

            var response = (SearchResponse)connection.SendRequest(request);
            if (response.Entries.Count == 0) return null;

            return response.Entries[0].DistinguishedName;
        }
    }
}
