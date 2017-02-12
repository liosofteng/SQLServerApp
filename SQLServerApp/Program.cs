/*
 *  C# sample app testing SQL server 2016 vNext on Ubuntu 10.06 machine
 *  dropping, creating DB
 *  creating, deleting, and reading rows in new DB
 *  by Rogelio Hernandez
 */

using System;
using System.Text;
using System.Data.SqlClient;

namespace SqlServerSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Build connection string
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "localhost";   //update if needed
                builder.UserID = "your_username";   //update if needed
                builder.Password = "your_password"; //update if needed
                builder.InitialCatalog = "master";

                //connect to sql server
                Console.Write("connecting to SQL server.......");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    //opening connedtion
                    connection.Open();
                    Console.WriteLine("Done!");

                    //Creating a sample database
                    Console.Write("Dropping and creating database 'sampleDB' ....");
                    String sql = "DROP DATABASE IF EXISTS [sampleDB]; CREATE DATABASE [SampleDB]";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("SampleDB created!");

                    }

                    //Creating a table and inserting sample data
                    Console.Write("Creating sample table with data, press any key to continue..");
                    Console.ReadKey(true);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Use SampleDB;");
                    sb.Append("CREATE TABLE Employees ( ");
                    sb.Append(" Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, ");
                    sb.Append(" Name NVARCHAR(50), ");
                    sb.Append(" Location NVARCHAR(50) ");
                    sb.Append("); ");
                    sb.Append("INSERT INTO Employees (Name, Location) VALUES ");
                    sb.Append("(N'Eric', N'Japan'), ");
                    sb.Append("(N'Suzy', N'Korea'), ");
                    sb.Append("(N'Park Shin Hye', N'Korea');");
                    sql = sb.ToString();

                    //execute sql comamnd
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Done!");
                    }

                    //inserting
                    Console.Write("Inserting a new row into table, press any key to continue....");
                    Console.ReadKey(true);
                    sb.Clear();
                    sb.Append("INSERT Employees (Name, Location) ");
                    sb.Append("VALUES (@name, @location);");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", "Jake");
                        command.Parameters.AddWithValue("@Location", "United States");
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " rows(s) inserted!");
                    }

                    //updating
                    string userToUpdate = "Suzy";
                    Console.Write("Updating 'Location' for user '" + userToUpdate + "', press any key to continue....");
                    Console.ReadKey(true);
                    sb.Clear();
                    sb.Append("UPDATE Employees SET Location = N'United States' WHERE Name = @name");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", userToUpdate);
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " rows(s) updated!");
                    }

                    //deleting
                    String userToDelete = "Eric";
                    Console.Write("Deleting user '" + userToDelete + "', press any key to contiue...");
                    Console.ReadKey(true);
                    sb.Clear();
                    sb.Append("DELETE FROM Employees WHERE NAME = @name;");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", userToDelete);
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " rows(s) deleted!");
                    }

                    //reading
                    Console.WriteLine("Reading data from table, press any key to continue...");
                    Console.ReadKey(true);
                    sql = "SELECT Id, Name, Location FROM Employees";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("All done. Press any key to finish...");
            Console.ReadKey(true);
        }
    }
}
