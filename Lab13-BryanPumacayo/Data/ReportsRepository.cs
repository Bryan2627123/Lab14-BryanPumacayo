using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Lab13_BryanPumacayo.Data
{
    public class ClientOrderSummary
    {
        public int ClientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int OrdersCount { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class ProductSalesSummary
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class ReportsRepository
    {
        private readonly string _connectionString;

        public ReportsRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new System.Exception("No se encontró la cadena de conexión 'DefaultConnection'.");
        }

        public async Task<List<ClientOrderSummary>> GetClientOrdersSummaryAsync()
        {
            const string sql = @"
                SELECT
                    c.ClientId,
                    c.Name,
                    c.Email,
                    COUNT(DISTINCT o.OrderId) AS OrdersCount,
                    SUM(od.Quantity * p.Price) AS TotalAmount
                FROM Clients c
                LEFT JOIN Orders o        ON c.ClientId = o.ClientId
                LEFT JOIN OrderDetails od ON o.OrderId  = od.OrderId
                LEFT JOIN Products p      ON od.ProductId = p.ProductId
                GROUP BY c.ClientId, c.Name, c.Email
                ORDER BY c.ClientId;";

            var result = new List<ClientOrderSummary>();

            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var item = new ClientOrderSummary
                {
                    ClientId    = reader.GetInt32(0),
                    Name        = reader.GetString(1),
                    Email       = reader.GetString(2),
                    OrdersCount = reader.IsDBNull(3) ? 0  : reader.GetInt32(3),
                    TotalAmount = reader.IsDBNull(4) ? 0m : reader.GetDecimal(4)
                };

                result.Add(item);
            }

            return result;
        }

        public async Task<List<ProductSalesSummary>> GetProductSalesSummaryAsync()
        {
            const string sql = @"
                SELECT
                    p.ProductId,
                    p.Name,
                    SUM(ISNULL(od.Quantity, 0))               AS TotalQuantity,
                    SUM(ISNULL(od.Quantity, 0) * p.Price)     AS TotalAmount
                FROM Products p
                LEFT JOIN OrderDetails od ON p.ProductId = od.ProductId
                GROUP BY p.ProductId, p.Name
                ORDER BY p.ProductId;";

            var result = new List<ProductSalesSummary>();

            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var item = new ProductSalesSummary
                {
                    ProductId     = reader.GetInt32(0),
                    Name          = reader.GetString(1),
                    TotalQuantity = reader.IsDBNull(2) ? 0  : reader.GetInt32(2),
                    TotalAmount   = reader.IsDBNull(3) ? 0m : reader.GetDecimal(3)
                };

                result.Add(item);
            }

            return result;
        }
    }
}
