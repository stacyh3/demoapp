using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace VulnerableConsoleApp
{
    class Program
    {
        // Hardcoded credentials (vulnerability)
        private static string dbUser = "admin";
        private static string dbPass = "password123";

        static void Main(string[] args)
        {
            Console.WriteLine("Enter SQL WHERE clause (e.g., \"username='bob'\"):");
            string userInput = Console.ReadLine();

            // SQL Injection vulnerability
            string sql = "SELECT * FROM Users WHERE " + userInput;
            Console.WriteLine("Running SQL: " + sql);
            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=DemoDb;User ID=" + dbUser + ";Password=" + dbPass))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("User: " + reader["username"]);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DB Error: " + ex.Message);
                }
            }

            

            Console.WriteLine("Enter a filename to list directory contents:");
            string fileInput = Console.ReadLine();
            // Command Injection vulnerability
            Process.Start("cmd.exe", "/C dir " + fileInput);

            // Insecure random number generation
            Random rand = new Random();
            int token = rand.Next();
            Console.WriteLine("Random token: " + token);

            // Weak hash function
            Console.WriteLine("Enter a value to hash (MD5):");
            string toHash = Console.ReadLine();
            string hash = GetMd5Hash(toHash);
            Console.WriteLine("MD5 hash: " + hash);
        }

        // Weak hash function (MD5)
        public static string GetMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}