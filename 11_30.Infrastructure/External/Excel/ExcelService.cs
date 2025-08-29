using ClosedXML.Excel;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Infrastructure.External.Excel
{
    public class ExcelService : IExcelService
    {
        public MemoryStream ExportToExcel(List<Dictionary<string, object>> rows)
        {
            if (rows == null || rows.Count == 0)
            {

            }
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("导出数据");

            // 获取列名
            var headers = rows[0].Keys.ToList();

            // 写入表头
            for (int i = 0; i < headers.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                worksheet.Cell(1, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            // 写入数据行
            for (int row = 0; row < rows.Count; row++)
            {
                for (int col = 0; col < headers.Count; col++)
                {
                    var value = rows[row][headers[col]];
                    worksheet.Cell(row + 2, col + 1).Value = value?.ToString() ?? string.Empty;
                }
            }

            // 自动调整列宽
            worksheet.Columns().AdjustToContents();

            // 写入到内存流
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
