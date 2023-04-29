using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MusicBandDBInterface.Pages.Queries
{
    public class Query4Model : PageModel
    {
        public List<CustomerClass> listExpiredCustomer = new List<CustomerClass>();
        public void OnGet()
        {
            try
            {

                String connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=PROJECT;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String query = "SELECT FirstName, LastName, Email, MAX(InvoiceDate) AS LastPurchaseDate,\r\n" +
                        "DATEDIFF(month, MAX(InvoiceDate), GETDATE()) AS MonthsSinceLastPurchase, SUM(Total) AS TotalSpent\r\n" +
                        "FROM PERSON\r\n" +
                        "JOIN CUSTOMER ON PERSON.PersonID = CUSTOMER.CustomerID\r\n" +
                        "LEFT JOIN INVOICE ON CUSTOMER.CustomerID = INVOICE.CustomerID\r\n" +
                        "WHERE InvoiceDate <= DATEADD(month, -6, GETDATE()) OR InvoiceDate IS NULL\r\n" +
                        "GROUP BY FirstName, LastName, Email\r\n" +
                        "HAVING MAX(InvoiceDate) IS NOT NULL\r\n" +
                        "ORDER BY MonthsSinceLastPurchase DESC;\r\n";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomerClass newCustomer = new CustomerClass();
                                newCustomer.firstName = "" + reader.GetString(0);
                                newCustomer.lastName = reader.GetString(1);
                                newCustomer.email = reader.GetString(2);
                                newCustomer.date  = reader.GetDateTime(3).ToString();
                                newCustomer.dateDiff = reader.GetInt32(4).ToString();
                                newCustomer.total = reader.GetDecimal(5).ToString();

                                listExpiredCustomer.Add(newCustomer);

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

    public class CustomerClass
    {
        public String firstName = "";
        public String lastName = "";
        public String email = "";
        public String date = "";
        public String dateDiff = "";
        public String total = "";
    }
}
