using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Infrastructure.External.SOA
{
    public interface IErpService
    {
        public Task<List<Dictionary<string, object>>> XmlToListDic(string xmlStr);
        public Task<string> GetDataFromErpAsync(string columns, string table, string filter, string orderby);

    }
}
