using System.IO;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Lab13_BryanPumacayo.Data;

namespace Lab13_BryanPumacayo.Services
{
    public class ExcelExampleService
    {
        private readonly ReportsRepository _reportsRepository;

        public ExcelExampleService(ReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        public string FirstExample()
        {
            var reportsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
            Directory.CreateDirectory(reportsFolder);

            var filePath = Path.Combine(reportsFolder, "archivo.xlsx");

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Hoja1");

            worksheet.Cell(1, 1).Value = "Nombre";
            worksheet.Cell(1, 2).Value = "Edad";

            worksheet.Cell(2, 1).Value = "Juan";
            worksheet.Cell(2, 2).Value = 28;

            workbook.SaveAs(filePath);
            return filePath;
        }

        public string SecondExample()
        {
            var reportsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
            var filePath = Path.Combine(reportsFolder, "archivo.xlsx");

            using var workbook = new XLWorkbook(filePath);

            var worksheet = workbook.Worksheet(1);

            worksheet.Cell(2, 2).Value = 30;

            workbook.Save();

            return filePath;
        }

        public string ThirdExample()
        {
            var reportsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
            Directory.CreateDirectory(reportsFolder);

            var filePath = Path.Combine(reportsFolder, "tabla.xlsx");

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Datos");

            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Nombre";
            worksheet.Cell(1, 3).Value = "Edad";

            worksheet.Cell(2, 1).Value = 1;
            worksheet.Cell(2, 2).Value = "Juan";
            worksheet.Cell(2, 3).Value = 28;

            worksheet.Cell(3, 1).Value = 2;
            worksheet.Cell(3, 2).Value = "María";
            worksheet.Cell(3, 3).Value = 34;

            var range = worksheet.Range("A1:C3");
            range.CreateTable("Personas");

            workbook.SaveAs(filePath);

            return filePath;
        }

        public string FourthExample()
        {
            var reportsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
            Directory.CreateDirectory(reportsFolder);

            var filePath = Path.Combine(reportsFolder, "archivo_con_estilos.xlsx");

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Estilos");

            var headerRow = worksheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.Cyan;
            headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Nombre";
            worksheet.Cell(1, 3).Value = "Edad";

            worksheet.Cell(2, 1).Value = 1;
            worksheet.Cell(2, 2).Value = "Juan";
            worksheet.Cell(2, 3).Value = 28;

            workbook.SaveAs(filePath);

            return filePath;
        }

        public async Task<string> GenerateClientsReportAsync()
        {
            var reportsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
            Directory.CreateDirectory(reportsFolder);

            var filePath = Path.Combine(reportsFolder, "ReporteClientes.xlsx");

            var clients = await _reportsRepository.GetClientOrdersSummaryAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Clientes");

            worksheet.Cell(1, 1).Value = "ClientId";
            worksheet.Cell(1, 2).Value = "Nombre";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Cantidad de pedidos";
            worksheet.Cell(1, 5).Value = "Monto total";

            var currentRow = 2;
            foreach (var client in clients)
            {
                worksheet.Cell(currentRow, 1).Value = client.ClientId;
                worksheet.Cell(currentRow, 2).Value = client.Name;
                worksheet.Cell(currentRow, 3).Value = client.Email;
                worksheet.Cell(currentRow, 4).Value = client.OrdersCount;
                worksheet.Cell(currentRow, 5).Value = client.TotalAmount;

                currentRow++;
            }

            worksheet.Columns().AdjustToContents();

            workbook.SaveAs(filePath);
            return filePath;
        }
        
        public async Task<string> GenerateProductsReportAsync()
        {
            var reportsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
            Directory.CreateDirectory(reportsFolder);

            var filePath = Path.Combine(reportsFolder, "ReporteProductos.xlsx");

            var products = await _reportsRepository.GetProductSalesSummaryAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Productos");

            
            worksheet.Cell(1, 1).Value = "ProductId";
            worksheet.Cell(1, 2).Value = "Producto";
            worksheet.Cell(1, 3).Value = "Cantidad total";
            worksheet.Cell(1, 4).Value = "Monto total";

            var currentRow = 2;
            foreach (var product in products)
            {
                worksheet.Cell(currentRow, 1).Value = product.ProductId;
                worksheet.Cell(currentRow, 2).Value = product.Name;
                worksheet.Cell(currentRow, 3).Value = product.TotalQuantity;
                worksheet.Cell(currentRow, 4).Value = product.TotalAmount;

                currentRow++;
            }

            worksheet.Columns().AdjustToContents();

            workbook.SaveAs(filePath);
            return filePath;
        }
    }
}
