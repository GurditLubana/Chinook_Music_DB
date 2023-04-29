using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MusicBandDBInterface.Pages.Queries
{
    public class Query2Model : PageModel
    {
        
        public List<Employee> listEmployees = new List<Employee>();
        public void OnGet()
        {
            try
            {

                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=PROJECT;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String query = "SELECT CONCAT(p.FirstName, ' ', p.LastName) AS EmployeeName,\r\n    " +
                        "CONCAT(pr.FirstName, ' ', pr.LastName) AS CustomerName,\r\n    " +
                        "DATEDIFF(YEAR, e.HireDATE, GETDATE()) AS YearsOfExperience,\r\n    " +
                        "mt.Name as MediaType,\r\n " +        
                        "COUNT(*) as TotalSales\r\n" +
                        "FROM EMPLOYEE e\r\n" +
                        "INNER JOIN PERSON p ON e.EMPLOYEEID = p.PERSONID\r\n" +
                        "INNER JOIN CUSTOMER c ON e.EMPLOYEEID = c.SupportRepID\r\n" +
                        "INNER JOIN PERSON pr ON c.CUSTOMERID = pr.PERSONID\r\n" +
                        "INNER JOIN INVOICE i ON c.CUSTOMERID = i.CUSTOMERID\r\n" +
                        "INNER JOIN INVOICE_LINE il ON i.InvoiceID = il.InvoiceID\r\n" +
                        "INNER JOIN TRACK t ON il.TrackID = t.TrackID\r\n" +
                        "INNER JOIN MEDIATYPE mt ON t.MediaTypeID = mt.MediaTypeID\r\n" +
                        "GROUP BY CONCAT(p.FirstName, ' ', p.LastName),\r\n    " +
                        "CONCAT(pr.FirstName, ' ', pr.LastName),\r\n    " +
                        "DATEDIFF(YEAR, e.HireDATE, GETDATE()),\r\n    " +
                        "mt.Name\r\nORDER BY TotalSales DESC;";



                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Employee employees = new Employee();

                                employees.eName = reader.GetString(0);
                                employees.cName = reader.GetString(1);
                                employees.experience = reader.GetInt32(2).ToString();
                                employees.mediaType = reader.GetString(3);
                                employees.totalSales = reader.GetInt32(4).ToString();

                                listEmployees.Add(employees);

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
            }
        }
    }

    public class Employee
    {
        public String eName = "";
        public String cName = "";
        public String experience = "";
        public String mediaType = "";
        public String totalSales = "";
    }
}
