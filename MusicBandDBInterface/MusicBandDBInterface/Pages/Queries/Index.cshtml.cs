using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MusicBandDBInterface.Pages.Queries
{
    public class IndexModel : PageModel
    {
        public List<Customer> listCustomer = new List<Customer>();
        public void OnGet()
        {
            try
            {

                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=PROJECT;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String query = "SELECT p.FirstName, p.LastName, p.Email, i.InvoiceDate, i.Total\r\n" +
                        "FROM Person p\r\n" +
                        "INNER JOIN Customer c ON p.PersonId = c.CustomerId\r\n" +
                        "INNER JOIN Invoice i ON c.CustomerId = i.CustomerId\r\n" +
                        "WHERE MONTH(i.InvoiceDate) = 1\r\n" +
                        "AND DAY(i.InvoiceDate) < 15\r\n" +
                        "AND c.CustomerId NOT IN " +
                        "(SELECT i2.CustomerId\r\n    " +
                        "FROM Invoice i2\r\n    " +
                        "WHERE MONTH(i2.InvoiceDate) = 2\r\n)\r\n" +
                        "AND i.Total > 1.5\r\n" +
                        "ORDER BY i.Total ASC;";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Customer customer = new Customer();
                                customer.firstName = "" + reader.GetString(0);
                                customer.lastName =  reader.GetString(1);
                                customer.email = reader.GetString(2);
                                customer.invoiceDate = reader.GetDateTime(3).ToString();
                                customer.total = reader.GetDecimal(4).ToString();

                                listCustomer.Add(customer);

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

    public class Customer
    {
        public String firstName = "";
        public String lastName = "";
        public String email = "";
        public String invoiceDate = "";
        public String total = "";
    }
}
