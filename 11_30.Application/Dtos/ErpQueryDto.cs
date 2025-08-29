using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Application.Dtos
{
    public record ErpQueryDto(string Table, string Filter, string Columns, string Orderby);

    public record EJiaDto(string UserName, string PassWord, DateTime StartDate, DateTime EndDate, string NewsType);
}
