using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Application.Interface
{
    public interface IEJiaAppService
    {
        public Task AutoNewsAsync(string email, string password, DateTime startDate, DateTime endDate, string newsType);
    }
}
