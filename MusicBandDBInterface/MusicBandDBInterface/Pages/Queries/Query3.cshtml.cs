using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MusicBandDBInterface.Pages.Queries
{
    public class Query3Model : PageModel
    {
        public List<Customers> Customerlist = new List<Customers>();
        public void OnGet()
        {
            try
            {

                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=PROJECT;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String query = "SELECT TOP 10\r\n    CONCAT(p.FirstName, ' ', p.LastName) AS CustomerName,\r\n    " +
                        "SUM(il.UnitPrice * il.Quantity) AS TotalAmountSpent,\r\n    " +
                        "(SELECT TOP 1 g.Name\r\n        " +
                        "FROM \r\n            " +
                        "INVOICE_LINE il2  INNER JOIN TRACK t2 ON il2.TrackID = t2.TrackID \r\n            " +
                        "INNER JOIN GENRE g ON t2.GenreID = g.GenreID \r\n        " +
                        "WHERE il2.InvoiceID IN (SELECT i.InvoiceID\r\n            " +
                        "FROM INVOICE i\r\n            " +
                        "WHERE i.CustomerID = c.CustomerID)\r\n        " +
                        "GROUP BY g.Name\r\n        " +
                        "ORDER BY COUNT(t2.TrackID) DESC) AS MostPopularGenre\r\n" +
                        "FROM \r\n    " +
                        "CUSTOMER c INNER JOIN PERSON p ON c.CustomerID = p.PersonId\r\n    " +
                        "INNER JOIN INVOICE i ON c.CustomerID = i.CustomerID \r\n    " +
                        "INNER JOIN INVOICE_LINE il ON i.InvoiceID = il.InvoiceID \r\n" +
                        "GROUP BY c.CustomerID, p.FirstName, p.LastName\r\n" +
                        "ORDER BY TotalAmountSpent DESC \r\n";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Customers cust = new Customers();

                                cust.name = reader.GetString(0);
                                cust.amount = reader.GetDecimal(1).ToString();
                                cust.genre = reader.GetString(2);

                                Customerlist.Add(cust);

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

    public class Customers
    {
        public String name = "";
        public String amount = "";
        public String genre = "";
    
    }
}
