using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _11_30.Infrastructure.External.SOA
{
    public class ErpService : IErpService
    {
        private readonly HttpClient _httpClient;

        public ErpService(HttpClient httpClient)
        {
            _httpClient=httpClient;
        }

        public async Task<string> GetDataFromErpAsync(string columns, string table, string filter, string orderby)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://172.20.8.201:7003/ERP_IFACE_OUT-DLCOMQRY/get_comqry_wsSoapHttpPort");
            request.Headers.Add("SOAPAction", "http://tbea/oracle/apps/ws/Get_comqry_ws.wsdl/cuxQryWs");
            var content = new StringContent($"<Envelope xmlns=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:typ=\"http://tbea/oracle/apps/ws/Get_comqry_ws.wsdl/types/\">\r\n<Body><typ:cuxQryWsElement><typ:pReqData><![CDATA[<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<data><baseinfo><interfaceId/><bizTransid/><messageId/><sendtime/>\r\n<sender/><receiver/><count/><comment/>\r\n<username>delan</username>\r\n<password>123</password></baseinfo>\r\n<datainfo><columns>{columns}</columns>\r\n<table>{table}</table>\r\n<filter>{filter}</filter>\r\n<orderby>{orderby}</orderby>\r\n</datainfo></data>]]></typ:pReqData></typ:cuxQryWsElement></Body></Envelope>", null, "text/xml");
            request.Content = content;
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            var resultXml = await response.Content.ReadAsStringAsync();
            return resultXml;
        }

        public async Task<List<Dictionary<string, object>>> XmlToListDic(string xmlStr)
        {
            // 1. 解析SOAP外层
            var outerDoc = XDocument.Parse(xmlStr);
            var ns = XNamespace.Get("http://tbea/oracle/apps/ws/Get_comqry_ws.wsdl/types/");
            var resultElement = outerDoc.Descendants(ns + "result").FirstOrDefault();
            if (resultElement == null)
                throw new Exception("未找到 <result> 元素");

            // 2. 解码 HTML 实体（即嵌套 XML）
            var innerXmlString = WebUtility.HtmlDecode(resultElement.Value);

            // 3. 加载内部XML
            var innerDoc = XDocument.Parse(innerXmlString);
            var records = innerDoc.Descendants("record");

            var result = new List<Dictionary<string, object>>();
            foreach (var record in records)
            {
                var dict = new Dictionary<string, object>();
                foreach (var element in record.Elements())
                {
                    dict[element.Name.LocalName] = element.Value;
                }
                result.Add(dict);
            }

            return result;
        }
    }
}
