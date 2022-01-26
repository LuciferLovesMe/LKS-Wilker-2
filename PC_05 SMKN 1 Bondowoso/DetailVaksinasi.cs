using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_05_SMKN_1_Bondowoso
{
    public partial class DetailVaksinasi : Form
    {
        public string nik = Selected.nik;
        SqlConnection connection = new SqlConnection(Utils.conn);
        public DetailVaksinasi()
        {
            InitializeComponent();
            loadvaksinasi();
            loadgrid();
            lblnama.Text = Session.username;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy / HH:mm:ss");
        }

        void loadvaksinasi()
        {
            string sql = "select * from vaksinasi join warga on vaksinasi.nik = warga.nik where vaksinasi.nik = '" + nik + "'";
            SqlCommand command = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            DateTime dob = Convert.ToDateTime(reader["tanggal_lahir"].ToString());
            if (reader.HasRows)
            {
                lblnik.Text = reader["nik"].ToString();
                lblname.Text = reader["nama"].ToString();
                lblttl.Text = reader["tempat_lahir"].ToString() + ", " + dob.ToString("dd-MM-yyyy");
                textBox5.Text = reader["alamat"].ToString();

                reader.Close();
            }
        }

        void loadgrid()
        {
            string sql = "select warga.nama, warga.nik, detail_vaksinasi.*, admin.nama as nama_dokter, jenis_vaksin.nama_vaksin from warga join vaksinasi on vaksinasi.nik = warga.nik join detail_vaksinasi on vaksinasi.id = detail_vaksinasi.id_vaksinasi join admin on admin.id = detail_vaksinasi.id_dokter join jenis_vaksin on detail_vaksinasi.id_jenis_vaksin = jenis_vaksin.id where vaksinasi.nik = '" + nik + "'";
            dataGridView1.DataSource = Command.getdata(sql);
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
        }

        private void label6_Click(object sender, EventArgs e)
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
