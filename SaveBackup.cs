using System;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;

namespace JustCause3_SaveBackup
{
    class SaveBackup
    {
        static void Main(string[] args)
        {
            //Create fileinfo for config file
            FileInfo configFileInfo = new FileInfo("config.json");
            
            //Check if config file exsists, and create one if it does not
            if (!configFileInfo.Exists)
            {
                Console.WriteLine("Could not find config.json file.");
                return;
            }

            //Deserialize config file to object and verify its inputs
            ConfigFile config = JsonConvert.DeserializeObject<ConfigFile>(File.ReadAllText("config.json"));
            if (config == null)
            {
                Console.WriteLine("config.json is malformed");
                return;
            }
            DirectoryInfo saveLocation = new DirectoryInfo(config.SaveLocation);
            if (!saveLocation.Exists)
            {
                Console.Write("Could not find save location. Please ensure the save locaion in config.json points to a valid folder");
                Console.Read();
                return;
            }
            DirectoryInfo backupLocation = new DirectoryInfo(config.BackupLocation);
            if (!backupLocation.Exists)
            {
                Directory.CreateDirectory(backupLocation.FullName);
            }
            //Backup directory to zip
            var path = Path.Combine(backupLocation.FullName, $"{DateTime.Now:MM-dd-yyyy h_mm_ss tt}.zip");
            ZipFile.CreateFromDirectory(config.SaveLocation,path);
        }
    }

    class ConfigFile
    {
        public string SaveLocation { get; set; }
        public string BackupLocation { get; set; }
    }
}