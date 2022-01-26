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
    public partial class ReportVaksin : Form
    {
        public ReportVaksin()
        {
            InitializeComponent();
            loadgrid();

            lblnama.Text = Session.username;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy / HH:mm:ss");
        }

        void loadgrid()
        {
            string sql = "select vaksinasi.nik, warga.nama, detail_vaksinasi.tanggal_vaksin, admin.nama as nama_dokter, jenis_vaksin.nama_vaksin from vaksinasi join detail_vaksinasi on detail_vaksinasi.id_vaksinasi = vaksinasi.id join warga on warga.nik = vaksinasi.nik join jenis_vaksin on jenis_vaksin.id = detail_vaksinasi.id_jenis_vaksin join admin on detail_vaksinasi.id_dokter = admin.id";
            dataGridView1.DataSource = Command.getdata(sql);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataReport data = new DataReport();
            CrystalReport1 report1 = new CrystalReport1();
            dt.Columns.Add("NIK", typeof(string));
            dt.Columns.Add("Nama", typeof(string));
            dt.Columns.Add("Tanggal_Vaksin", typeof(string));
            dt.Columns.Add("Jenis_Vaksin", typeof(string));
            dt.Columns.Add("Nama_Dokter", typeof(string));

            for(int i = 0; i < dataGridView1.RowCount; i++)
            {
                dt.Rows.Add(dataGridView1.Rows[i].Cells[0].Value.ToString(), dataGridView1.Rows[i].Cells[1].Value.ToString(), dataGridView1.Rows[i].Cells[2].Value.ToString(), dataGridView1.Rows[i].Cells[3].Value.ToString(), dataGridView1.Rows[i].Cells[4].Value.ToString());
                
            }
            
            //dt.AsDataView();
            data.CreateDataReader(dt);
            ShowReport show = new ShowReport();
            show.crystalReportViewer1.ReportSource = dt;
            show.ShowDialog();
        }

        private void panel_vaksin_Click(object sender, EventArgs e)
        {
            Vaksinasi vaksinasi = new Vaksinasi();
            this.Hide();
            vaksinasi.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin menutup ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin logout ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MainLogin main = new MainLogin();
                this.Hide();
                main.ShowDialog();
                Logout.logout();
            }
        }
    }
}
