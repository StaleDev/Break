using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

using System.Runtime.InteropServices;
namespace Minutes
{
    public partial class Main : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        public Main()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlDrag_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void pnlDrag_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            database db = new database();
            List<String> programs = (db.Read("JohnDoe").Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList());
            foreach (string p in programs)
            {
                if (p != "")
                {
                    string pname = (p.Split('\\').Last()).Split('?').First().ToString();
                    string filepath = (p.Split('?')[0]).ToString();
                    string crc = (p.Split('?').Last()).ToString();
                    this.dataGridView1.Rows.Insert(0, false, pname, filepath, crc);
                }
            }
            //timer1.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            database db = new database();

            FileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog(); ;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            string file = "";
            string CRC = "";
            if (result == DialogResult.OK) // Test result.
            {
                file = openFileDialog1.FileName;
                CRC = CalcCRC32(file);
            }
            if (file != "") db.AddProgram(String.Format("{0}?{1}", file, CRC));
            loopCheck();
        }

        String CalcCRC32(string file)
        {
            var crc32 = new Crc32();
            var hash = String.Empty;

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToLower();
            return hash;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Blocker b = new Blocker();
            Thread checkthread = new Thread(b.Check);
            checkthread.Start();
        }

        private void loopCheck()
        {
            dataGridView1.Rows.Clear();
            database db = new database();
            List<String> programs = (db.Read("JohnDoe").Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList());
            foreach (string p in programs)
            {
                if (p != "")
                {
                    string pname = (p.Split('\\').Last()).Split('?').First().ToString();
                    string filepath = (p.Split('?')[0]).ToString();
                    string crc = (p.Split('?').Last()).ToString();
                    this.dataGridView1.Rows.Insert(0, false, pname, filepath, crc);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loopCheck();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string ppath = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            string pcrc = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            database db = new database();
            db.RemoveProgram(String.Format("{0}?{1}", ppath, pcrc));
            loopCheck();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Blocker b = new Blocker();
            b.Check();
        }
    }
}
