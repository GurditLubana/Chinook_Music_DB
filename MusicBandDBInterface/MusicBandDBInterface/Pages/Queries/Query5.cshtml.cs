using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MusicBandDBInterface.Pages.Queries
{
    public class Query5Model : PageModel
    {
        public List<CustomerClass2> customerList = new List<CustomerClass2>();
        public void OnGet()
        {
            try
            {

                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=PROJECT;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String query = "SELECT c.FirstName, c.LastName, t.Name AS TrackName, a.Name AS ArtistName\r\n                        " +
                        "FROM PERSON c JOIN CUSTOMER cu ON c.PERSONID = cu.CUSTOMERID\r\n                        " +
                        "JOIN INVOICE i ON cu.CUSTOMERID = i.CUSTOMERID\r\n                        " +
                        "JOIN INVOICE_LINE il ON i.InvoiceID = il.InvoiceID\r\n                        " +
                        "JOIN TRACK t ON il.TrackID = t.TrackID\r\n                        " +
                        "JOIN ALBUM al ON t.AlbumID = al.AlbumID\r\n                        " +
                        "JOIN ARTIST a ON al.ArtistID = a.ArtistID\r\n                        " +
                        "WHERE (c.FirstName LIKE 'F%' OR c.FirstName LIKE 'R%')\r\n                        " +
                        "AND (a.Name LIKE 'A%' OR a.Name LIKE 'B%')\r\n                        " +
                        "ORDER BY t.Name ASC;";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomerClass2 newCustomer = new CustomerClass2();

                                newCustomer.fName = "" + reader.GetString(0);
                                newCustomer.lName = reader.GetString(1);
                                newCustomer.track = reader.GetString(2);    
                                newCustomer.artist = reader.GetString(3);
                                

                                customerList.Add(newCustomer);

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

    public class CustomerClass2
    {
        public String fName = "";
        public String lName = "";
        public String track = "";
        public String artist= "";

    }
}
