using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MySql.Data.MySqlClient;

namespace LabAutTool
{
    internal class Class1
    {
        private static string connectionString = "Server=localhost;Port=3307;Database=lab_auto;Uid=root;Pwd=Sandeep@123;";

        public static void StoreReports(string filePath)
        {
            List<Report> reports = new List<Report>();
            
            try
            {
                // Deserialize the binary file to a List of Report objects
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    reports = (List<Report>)bf.Deserialize(fs);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deserializing binary file: " + e.Message);
                return;
            }

            // Save the reports to the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    foreach (Report r in reports)
                    {
                        command.CommandText = "INSERT INTO report (Report_id, Report_time, Report_value, Report_unit, Report_desc, Sample_id, lab_id) " +
                            "VALUES (@ReportId, @Reporttime, @Reportvalue, @Reportunit, @Reportdesc, @SampleId, @labid)";
                        command.Parameters.AddWithValue("@ReportId", r.ReportId);
                        command.Parameters.AddWithValue("@Reporttime", r.Reporttime);
                        command.Parameters.AddWithValue("@Reportvalue", r.Reportvalue);
                        command.Parameters.AddWithValue("@Reportunit", r.Reportunit);
                        command.Parameters.AddWithValue("@Reportdesc", r.Reportdesc);
                        command.Parameters.AddWithValue("@SampleId", r.SampleId);
                        command.Parameters.AddWithValue("@labid", r.labid);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }
                    Console.WriteLine("Values inserted Successfully to the Database");
                }
                catch (SqlException e)
                {
                    Console.WriteLine("Error saving reports: " + e.Message);
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
        }
    }
}