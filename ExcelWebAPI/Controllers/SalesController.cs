using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ExcelWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        [HttpGet]
        public JsonResult Get()
        {
            
            string query = @"select top 1000 Region,Country, ""Item Type"" ,
                        ""Sales Channel"" ,
                        ""Order Priority"",
                        ""Order Date"" ,
                        ""Order ID"" ,
                        ""Ship Date"" ,
                        ""Units Sold"" ,	
                        ""Unit Price"" ,
                        ""Unit Cost"" ,
                        ""Total Revenue"" ,
                        ""Total Cost"" ,
                        ""Total Profit"" from dbo.sales";
            DataTable table = new DataTable();
            SqlDataReader dataReader;
            using (SqlConnection dbConnection = new SqlConnection("Data Source=LAPTOP-FMTH1U07\\SQLEXPRESS;Initial Catalog=interview;User ID=user;Password=Newuser"))
            {
                dbConnection.Open();
                using (SqlCommand command = new SqlCommand(query,dbConnection))
                {
                    dataReader = command.ExecuteReader();
                    table.Load(dataReader);
                    dataReader.Close();
                    dbConnection.Close();
                }
            }

            return new JsonResult(table);
        }
    }
}
