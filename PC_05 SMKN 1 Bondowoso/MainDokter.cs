using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_05_SMKN_1_Bondowoso
{
    public partial class MainDokter : Form
    {
        public MainDokter()
        {
            InitializeComponent(); 
            lblnama.Text = Session.username;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy / HH:mm:ss");
        }

        private void panel_master_Click(object sender, EventArgs e)
        {
            ReportVaksin data = new ReportVaksin();
            this.Hide();
            data.ShowDialog();
        }

        private void panel_vaksin_Click(object sender, EventArgs e)
        {
            Vaksinasi v = new Vaksinasi();
            this.Hide();
            v.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin menutup ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin logout ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MainLogin main = new MainLogin();
                this.Hide();
                main.ShowDialog();
            }
        }
    }
}
