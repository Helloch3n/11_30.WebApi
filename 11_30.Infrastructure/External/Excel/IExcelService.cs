using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Infrastructure.External.Excel
{
    public interface IExcelService
    {
        public MemoryStream ExportToExcel(List<Dictionary<string, object>> rows);
    }
}
