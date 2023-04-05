using System;
using System.IO;
using Newtonsoft.Json;

namespace LabAutTool
{
    internal class Jsonparser
    {
        static void Main(string[] args)
        {
            //Defing the server directory to watch
            string serverDirectory = @"D:\Xampp\htdocs\server";
            //Creating a fileWatcher to listen for new files in the directory
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path= serverDirectory;
            watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.Filter = "*.json";
            watcher.Created += OnNewFileCreated;
            watcher.EnableRaisingEvents = true;

            //Class1.Mainmethod(args);

            //Wait for new files
            Console.WriteLine("Press 'q' to Quit");
            while(Console.Read()!='q') {}
        }

        static void OnNewFileCreated(object sender, FileSystemEventArgs e)
        {
            // When a new file is created, fetch the file and parse it into a text file
            string filePath = e.FullPath;
            string textFilePath = Path.Combine(Path.GetDirectoryName(filePath), $"{DateTime.Now:yyyy-MM-dd}.txt");

            // Read the JSON file into a string
            string json = File.ReadAllText(filePath);

            // Parse the JSON string into a dynamic object
            dynamic obj = JsonConvert.DeserializeObject(json);

            // Write the parsed JSON to a text file
            File.WriteAllText(textFilePath, obj.ToString());

            Console.WriteLine("Parsed JSON file to text: " + textFilePath);
            Class2.ReadReports(textFilePath);
        }
        
    }
}