using System;
using System.Linq;
using System.Data;
//using Mono.Data.Sqlite;

namespace Expenses.Test
{
    class MainClass
    {
        public static void Main (string[] args)
        {
//            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=expenses.sqlite;Version=3;");
//            m_dbConnection.Open();
//
//            try
//            {
//                string sql = "select * from Users";
//                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
//
//                SQLiteDataReader reader = command.ExecuteReader();
//
//                while (reader.Read())
//                    Console.WriteLine("Name: " + reader["Login"]);
//
//                Console.ReadLine();
//            }
//            catch (Exception exc)
//            {
//                return;
//            }
//            finally
//            {
//                m_dbConnection.Close();
//            }
            using (var context = new ContextProvider ().CreateContext ()) 
            {
                Console.WriteLine(context.Users.ToList ().Count);
            }
        }
    }
}
