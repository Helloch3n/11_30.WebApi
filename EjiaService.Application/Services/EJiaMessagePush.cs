using EjiaService.Application.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EjiaService.Application.Services
{
    public class EJiaMessagePush : IEjiaMessagePush
    {
        private readonly IConfiguration _configuration;
        public EJiaMessagePush(IConfiguration configuration)
        {
            _configuration=configuration;
        }
        /// <summary>
        /// 计算PUB TOKEN
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string ComputeSha1(string input)
        {
            using SHA1 sha1 = SHA1.Create();
            byte[] bytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2")); // 转换为十六进制小写
            }
            return sb.ToString();
        }

        /// <summary>
        /// 发送HTTP请求
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public async Task<(HttpStatusCode statusCode, string responseBody)> HttpPostDataAsync(HttpClient client, string url, string jsonData)
        {
            using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            using var response = await client.PostAsync(url, content);

            string responseBody = await response.Content.ReadAsStringAsync();
            return (response.StatusCode, responseBody);
        }

        /// <summary>
        /// 判断是否准入
        /// </summary>
        /// <param name="sysCode"></param>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public string IsZhunRu(string sysCode, string userCode)
        {
            string? user;
            if (sysCode=="ZhunRu")
            {
                user = userCode.Length==7 && userCode[0] == '0' ? userCode.Substring(userCode.Length - 6) : userCode;
            }
            else
            {
                user = userCode;
            }
            return user;
        }

        async Task<string> IEjiaMessagePush.EJiaMessagePush(string sysCode, string userCode, string message)
        {
            var user = IsZhunRu(sysCode, userCode);
            //获取PUB和PUBKEY
            string? pubkey = _configuration["EJia:Old:PubKey"];
            string? pub = _configuration["EJia:Old:Pub"];
            string time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

            //构建参数数组并进行字典序排序
            //公共号密钥验证规则pubtoken=sha(no,pub,公共号.密钥pubkey,nonce,time)
            var parameters = new[] { "101", pub, pubkey, "123456", time };
            Array.Sort(parameters, StringComparer.Ordinal);

            // 连接字符串并计算 SHA1
            string combined = string.Concat(parameters);
            string pubtoken = ComputeSha1(combined);

            // 构建 JSON 请求体
            var requestBody = new
            {
                from = new
                {
                    no = "101",
                    pub = pub,
                    time = time,
                    nonce = "123456",
                    pubtoken = pubtoken
                },
                to = new[]
                {
                new
                {
                    no = "101",
                    user = new[] { user },
                    code = "2"
                }
            },
                type = 2,
                msg = new
                {
                    text = message
                }
            };

            string jsonBody = JsonSerializer.Serialize(requestBody);
            //获取E+消息推送URL
            string? url = _configuration["EJia:Old:Url"];

            // 发送 HTTP 请求
            using var client = new HttpClient();
            var (statusCode, content) = await HttpPostDataAsync(client, url, jsonBody);

            return statusCode.ToString();
        }
    }
}
