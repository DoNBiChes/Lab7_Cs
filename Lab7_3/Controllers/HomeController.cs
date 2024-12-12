using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab7_3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!_context.MyEntities.Any())
            {
                _context.MyEntities.Add(new MyEntity { Name = "Test 1", CreatedAt = DateTime.Now });
                _context.MyEntities.Add(new MyEntity { Name = "Test 2", CreatedAt = DateTime.Now });
                await _context.SaveChangesAsync();
            }
            var data = await _context.MyEntities.ToListAsync();
            return View(data);
        }

        public IActionResult ExportToExcel()
        {
            var data = _context.MyEntities.ToList();
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Report");
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "CreatedAt";

                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = data[i].Id;
                    worksheet.Cell(i + 2, 2).Value = data[i].Name;
                    worksheet.Cell(i + 2, 3).Value = data[i].CreatedAt;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
                }
            }
        }
    }
}
