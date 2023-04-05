using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LabAutTool;
using System.Text.Json;

namespace LabAutTool
{
    [Serializable]
    public class Report
    {
        public string ReportId { get; set; }
        public DateTime Reporttime { get; set; }
        public double Reportvalue { get; set; }
        public string Reportunit { get; set; }
        public string Reportdesc { get; set; }
        public string SampleId { get; set; }
        public int labid { get; set; }

        public Report(string Report_id,DateTime Report_time,double Report_value,string Report_unit,string Report_desc,string Sample_id,int Lab_id)
        {
            this.ReportId = Report_id;
            this.Reporttime = Report_time;
            this.Reportvalue = Report_value;
            this.Reportunit= Report_unit;
            this.Reportdesc = Report_desc;
            this.SampleId = Sample_id;
            this.labid= Lab_id;
        }

        public void GetReportArguments()
        {
            try
            {
                Console.WriteLine("Sample ID: " + SampleId);
                Console.WriteLine("Report ID: " + ReportId);
                Console.WriteLine("Report time: " + Reporttime);
                Console.WriteLine("Report value: " + Reportvalue);
                Console.WriteLine("Report unit: " + Reportunit);
                Console.WriteLine("Report Description: " + Reportdesc);
                Console.WriteLine("Lab id:" + labid);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
    internal class Class2
    {
        public static void ReadReports(string filepath)
        {
            List<Report> reports = new List<Report>();

            // Read the contents of the sample file
            string textString=null;
            try { textString = File.ReadAllText(filepath); }
            catch(IOException e)
            {
                Console.WriteLine("File not Found");
            }

            // Parse the JSON string to a JsonDocument
            JsonDocument document = JsonDocument.Parse(textString);

            // Get the root element of the document
            JsonElement root = document.RootElement;

            // Extract the values for the "Report_id", "Sample_id", "Lab_id" and all properties and store them in variables
            string Report_id="";
            DateTime Report_time = new DateTime();
            double Report_value=0;
            string Report_unit = "";
            string Report_desc = null;
            string Sample_id = null;
            int Lab_id=0;

            try
            {
                Report_id = root.GetProperty("Report_id").GetString();
                Report_time = root.GetProperty("Report_time").GetDateTime();
                Report_value = root.GetProperty("Report_value").GetDouble();
                Report_unit = root.GetProperty("Report_unit").GetString();
                Report_desc = root.GetProperty("Report_desc").GetString();
                Sample_id = root.GetProperty("Sample_id").GetString();
                Lab_id = root.GetProperty("Lab_id").GetInt32();
            }

            catch(System.InvalidOperationException e1)
            {
                Console.WriteLine("Error while Extracting values from JSON file" + e1.Message+ "\n");
            }

            catch(System.FormatException e2)
            {
                Console.WriteLine(e2.Message+ "\n");
            }

            //Creating Binary filepath as string
            string binaryFilePath = "D:\\Xampp\\htdocs\\binaryfile\\reports.bin";
            
            //Creating report object to store the values 
            Report report = new Report(Report_id,Report_time,Report_value,Report_unit,Report_desc,Sample_id,Lab_id);

            //Adding report object to reports list
            try
            {
                reports.Add(report);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            //Serializing reports list to binary file
            try
            {
                using (FileStream fs = new FileStream(binaryFilePath, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, reports);
                }
            } 
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Access Denied: " + e.Message);
            }

            Class1.StoreReports(binaryFilePath);
        }
    }
}
