using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EjiaService.Application.Interface
{
    public interface IEjiaMessagePush
    {
        public Task<string> EJiaMessagePush(string sysCode, string userCode, string message);

        public string ComputeSha1(string input);

        public Task<(HttpStatusCode statusCode, string responseBody)> HttpPostDataAsync(
       HttpClient client, string url, string jsonData);

        public string IsZhunRu(string sysCode, string userCode);
    }
}
