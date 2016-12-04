/*
//Author:   Kevin Wakatama
//Desc:     Simple Program to connect to an existing 3NF SQL Server DB and perform a series of functions with said DB
//Created:  2016-12-01
//Modified: 
//
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace dbConnect
{
    class Program
    {

        public static string connectionString = "user id=username;" +
                                                 "password=password;server=DESKTOP-SDB5USS;" +
                                                 "Trusted_Connection=yes;" +
                                                 "database=company; " +
                                                 "connection timeout=30";


        static void Main(string[] args)
        {           
            empRead();           
        }

        public static void bulkInsert()
        {
            SqlConnection myConnection = new SqlConnection(connectionString);

            connect(myConnection);

            string script = File.ReadAllText(@"Scripts\Insert employees and jobs.sql");

            SqlCommand cmd = new SqlCommand(script, myConnection);

            execute(cmd);

            disconnect(myConnection);

           Console.ReadLine();



        }

        public static void empInsert(string first, string last)
        {
            SqlConnection myConnection = new SqlConnection(connectionString);

            connect(myConnection);
            
            SqlCommand cmd = new SqlCommand("INSERT INTO employee(emp_FName, emp_LName) VALUES('" + first + "', '" + last + "')", myConnection);

            execute(cmd);

            disconnect(myConnection);

            Console.ReadLine();
            


        }

        public static void projInsert(string name, string desc, DateTime start, DateTime end)
        {
            SqlConnection myConnection = new SqlConnection(connectionString);

            connect(myConnection);

            SqlCommand cmd = new SqlCommand("INSERT INTO project(project_Name, project_Desc, project_Start, project_End) VALUES('" + name + "', '" + desc + "', '" + start + "', '" + end + "')", myConnection);

            execute(cmd);

            SqlCommand read = new SqlCommand("SELECT * FROM project", myConnection);
            SqlDataReader reader = read.ExecuteReader();
            {
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3) + reader.GetString(4));
                }
            }

            disconnect(myConnection);

            Console.ReadLine();



        }

        public static void empRead()
        {
            SqlConnection myConnection = new SqlConnection(connectionString);
            connect(myConnection);

            SqlCommand read = new SqlCommand("SELECT * FROM EMPLOYEE", myConnection);
            SqlDataReader reader = read.ExecuteReader();
            {
                while (reader.Read())
                {
                    //Console.WriteLine(reader.GetString(1) + " " + reader.GetString(2));
                    Console.WriteLine(reader.GetStringOrNull(1) + " " + reader.GetStringOrNull(2));
                }
            }

            disconnect(myConnection);

            Console.ReadLine();

        }

        public static void projRead()
        {
            SqlConnection myConnection = new SqlConnection(connectionString);
            connect(myConnection);

            SqlCommand read = new SqlCommand("SELECT * FROM project", myConnection);
            SqlDataReader reader = read.ExecuteReader();

            for (int i = 1; i < reader.FieldCount; i++) 
            {
                Console.Write(reader.GetName(i) + "\t");
            }
            Console.WriteLine();

            {
                while (reader.Read())
                {
                    //Console.WriteLine(reader.GetString(1) + " " + reader.GetString(2) + reader.GetDateTime(3).ToString() + reader.GetDateTime(4).ToString());
                    Console.WriteLine(reader.GetStringOrNull(1) + "\t\t" + reader.GetStringOrNull(2) + "\t\t" +  reader.GetValue(3).ToString() + "\t\t" + reader.GetValue(4).ToString());
                }
            }

            disconnect(myConnection);

            Console.ReadLine();

        }


        public static void deptRead()
        {
            SqlConnection myConnection = new SqlConnection(connectionString);
            connect(myConnection);

            string script = File.ReadAllText(@"Scripts\selectDept.sql");

            SqlCommand read = new SqlCommand(script, myConnection);
            SqlDataReader reader = read.ExecuteReader();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write(reader.GetName(i) + "\t\t");
            }
            Console.WriteLine();

            {
                while (reader.Read())
                {
                    //Console.WriteLine(reader.GetString(1) + " " + reader.GetString(2) + reader.GetDateTime(3).ToString() + reader.GetDateTime(4).ToString());
                    Console.WriteLine(reader.GetValue(0) + "\t\t" + reader.GetValue(1));
                }
            }

            disconnect(myConnection);

            Console.ReadLine();

        }

        public static void connect(SqlConnection conn)
        {
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void disconnect(SqlConnection conn)
        {
            try
            {
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void execute(SqlCommand comm)
        {
            try
            {
                comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadLine();
            }
        }
    }

    public class cEmployee
    {
        public string fname;
        public string lname;
    }

    public class cProject
    {
        public string pName;
        public string pDesc;
        public DateTime pStartDate;
        public DateTime pEnddate;
    }

    public class cDept
    {
        public string dName;
    }

    public class cJob
    {
        public string jTitle;
        public string jDesc;
    }

    public class empDetails
    {
        public int emID;
        public int jobID;
    }

    public class projEmployees
    {
        public int projID;
        public int emID;        
    }

    public class projDepts
    {
        public int projID;
        public int deptID;
    }

    public static class DataReaderExtensions
    {
        public static string GetStringOrNull(this IDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }

        public static string GetStringOrNull(this IDataReader reader, string columnName)
        {
            return reader.GetStringOrNull(reader.GetOrdinal(columnName));
        }






    }



}
