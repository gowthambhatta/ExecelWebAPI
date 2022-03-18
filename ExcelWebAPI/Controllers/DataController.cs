using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Data.SqlClient;
using System.IO.Compression;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExcelWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        // GET: api/<DataController>
        [HttpGet]
        public IEnumerable<bool> Get()
        {
            bool complete = true;
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
            webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            webClient.DownloadFile(new Uri("http://eforexcel.com/wp/wp-content/uploads/2017/07/1000000%20Sales%20Records.zip"), "1000000 Sales Records.zip");
            string zipFilePath = @"C:\Users\gowth\source\repos\ExcelWebAPI\1000000 Sales Records.zip";
            string extractionPath = @"C:\Users\gowth\source\repos\ExcelWebAPI";
            ZipFile.ExtractToDirectory(zipFilePath, extractionPath);
            string csv_path = @"C:\Users\gowth\source\repos\ExcelWebAPI\1000000 Sales Records.csv";
            DataTable csvDataTable = LoadDataToDB(csv_path);
            DataTabletoDB(csvDataTable);
            yield return true;
        }

        private static DataTable LoadDataToDB(string csv_path)
        {
            DataTable csvDataTable = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvDataTable.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvDataTable.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return csvDataTable;
        }

        private static void DataTabletoDB(DataTable csvDataTable)
        {
            using (SqlConnection dbConnection = new SqlConnection("Data Source=LAPTOP-FMTH1U07\\SQLEXPRESS;Initial Catalog=interview;User ID=user;Password=Newuser"))
            {
                dbConnection.Open();
                using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                {
                    s.DestinationTableName = "dbo.sales";

                    foreach (var column in csvDataTable.Columns)
                        s.ColumnMappings.Add(column.ToString(), column.ToString());

                    s.WriteToServer(csvDataTable);
                }
            }
        }

    }
}
