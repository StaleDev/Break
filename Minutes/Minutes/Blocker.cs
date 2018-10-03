using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
namespace Minutes
{
    class Blocker
    {

        public void Check()
        {
            database db = new database();
            List<String> programs = (db.Read("JohnDoe").Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList());
            List<String> blockedCRC = new List<string>();
            foreach (string p in programs)
            {
                blockedCRC.Add(p.Split('?').Last().ToString());
            }

            Process[] processes = Process.GetProcesses();
            foreach (Process p in processes.Where(n => n.MainWindowTitle != "")){
                string processPath = p.MainModule.FileName;
                string CRC = CalcCRC32(p.MainModule.FileName);
                if (blockedCRC.Contains(CRC))
                {
                    p.Kill();
                }
            }

        }

        String CalcCRC32(string file)
        {
            var crc32 = new Crc32();
            var hash = String.Empty;

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToLower();
            return hash;
        }

    }
}
