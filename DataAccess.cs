using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;
using Windows.System;

namespace WinUI3FormApp
{
    public static class DataAccess
    {
        // =============================================================================================================================
        public async static void InitializeDatabase()
        {
            string path = "C:\\Users\\andri\\Desktop\\Code\\microform\\Microform\\Data\\";
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);
            await folder.CreateFileAsync("sqliteSample.db", CreationCollisionOption.OpenIfExists);
            //await ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteSample.db", CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(path, "sqliteSample.db");
            
            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                string tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS MyTable (Primary_Key INTEGER PRIMARY KEY, " +
                    "Text_Entry NVARCHAR(2048) NULL)";

                var createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        } // End InitializeDatabase
        // =============================================================================================================================
        public static void AddData(string inputText)
        {
            //string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            string path = "C:\\Users\\andri\\Desktop\\Code\\microform\\Microform\\Data\\";
            string dbpath = Path.Combine(path, "sqliteSample.db");

            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry);";
                insertCommand.Parameters.AddWithValue("@Entry", inputText);

                insertCommand.ExecuteReader();
            }

        } // End AddData
        // =============================================================================================================================
        public static List<string> GetData()
        {
            var entries = new List<string>();
            //string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            string path = "C:\\Users\\andri\\Desktop\\Code\\microform\\Microform\\Data\\";
            string dbpath = Path.Combine(path, "sqliteSample.db");
            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                var selectCommand = new SqliteCommand
                    ("SELECT Text_Entry from MyTable", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                }
            }
            return entries;
        } // End GetData
        // =============================================================================================================================
        public static void DeleteAllData()
        {
            //string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            string path = "C:\\Users\\andri\\Desktop\\Code\\microform\\Microform\\Data\\";
            string dbpath = Path.Combine(path, "sqliteSample.db");

            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var deleteAllCommand = new SqliteCommand();
                deleteAllCommand.Connection = db;
                deleteAllCommand.CommandText = "DELETE FROM MyTable;";
                deleteAllCommand.ExecuteReader();
            }

        } // End delete all data
        public static Dictionary<string, string> ReadConfig()
        {
            string configPath = "Data\\config.csv";
            var lines = File.ReadLines(configPath);
            Dictionary<string, string> out_config = new Dictionary<string, string>();
            foreach (string line in lines)
            {
                string[] kv = line.Split(",");
                out_config.Add(kv[0].Trim(), kv[1].Trim());
            }
            return out_config;
        }
    } // End DataAccess Class
} // End App1
