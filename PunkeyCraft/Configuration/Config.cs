using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace PunkeyCraft.Configuration
{
    public class Config
    {
        [XmlIgnore]
        public static Config Singleton;

        public Config()
        {
            Port = 25565;
            IP = "0.0.0.0";
            OnlineMode = true;
        }

        public static void Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            string path = Path.Combine(Environment.CurrentDirectory, "config.xml");
            if (!File.Exists(path)) 
            {
                Singleton = new Config();
                Save();
            }
            else
            {
                try
                {
                    using (FileStream fs = File.OpenRead(path))
                        Singleton = (Config)serializer.Deserialize(fs);
                }
                catch
                { 
                    Singleton = new Config();
                    Save();
                }
            }
        }

        public static void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            string path = Path.Combine(Environment.CurrentDirectory, "config.xml");
            using (FileStream fs = File.Create(path))
                serializer.Serialize(fs, Singleton);
        }

        // Vanilla Settings
        public string IP { get; set;}
        public int Port { get; set; }
        public bool OnlineMode { get; set; }

        // Fun Settings
        public int StartLevel { get; set; }
    }
}
