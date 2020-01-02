using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Fail2Rdp.Service
{
    [Serializable]
    public class Settings
    {
        public List<string> Bans { get; set; } = new List<string>();
        public int Threshold { get; set; } = 3;
        public List<string> Whitelist { get; set; } = new List<string>();

        public void Save()
        {
            using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\settings.bin", FileMode.OpenOrCreate))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, this);
            }
        }

        public static Settings Load()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\settings.bin"))
                return new Settings();
            using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\settings.bin", FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return bf.Deserialize(fs) as Settings;
            }
        }
    }
}
