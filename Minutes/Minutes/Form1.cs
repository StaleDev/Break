using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Minutes
{
    public partial class Login : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        bool defaultUsername = true;
        bool defaultPassword = true;

        public Login()
        {
            InitializeComponent();
        }

        private void pnlDrag_Paint(object sender, PaintEventArgs e)
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

        private void tbUsername_TextChanged(object sender, EventArgs e)
        {
            if (defaultUsername == true)
            {
                defaultUsername = false;
                tbUsername.Text = "";
            }
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            if (defaultPassword == true)
            {
                defaultPassword = false;
                tbPassword.Text = "";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            database db = new database();
            if (db.Login(tbUsername.Text, tbPassword.Text))
            {
                lblError.Visible = false;
                this.Close();
            }
            else
            {
                lblError.Visible = true;
            }

        }
    }
}

