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
    public partial class Vaksinasi : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        string nik;
        public Vaksinasi()
        {
            InitializeComponent();
            loaddokter();
            loadgrid();
            loadvaksin();

            lblnama.Text = Session.username;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy / HH:mm:ss");
        }

        void loadvaksin()
        {
            string sql = "select * from jenis_vaksin";
            combo_vaksin.DataSource = Command.getdata(sql);
            combo_vaksin.DisplayMember = "nama_vaksin";
            combo_vaksin.ValueMember = "id";
        }

        void loaddokter()
        {
            string sql = "select * from admin where level = 2";
            combo_dokter.DataSource = Command.getdata(sql);
            combo_dokter.DisplayMember = "nama";
            combo_dokter.ValueMember = "id";
        }

        void loadgrid()
        {
            string sql = "select warga.nama, warga.nik, detail_vaksinasi.*, admin.nama as nama_dokter from warga join vaksinasi on vaksinasi.nik = warga.nik join detail_vaksinasi on vaksinasi.id = detail_vaksinasi.id_vaksinasi join admin on admin.id = detail_vaksinasi.id_dokter";
            dataGridView1.DataSource = Command.getdata(sql);
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin logout ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                MainLogin main = new MainLogin();
                this.Hide();
                main.ShowDialog();
                Logout.logout();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin menutup ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            Selected.nik = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string sql = "select * from warga where nik = '"+textBox1.Text+"'";
            SqlCommand command = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                DateTime dob = Convert.ToDateTime(reader["tanggal_lahir"].ToString());
                lblnik.Text = reader["nik"].ToString();
                lblname.Text = reader["nama"].ToString();
                lblttl.Text = reader["tempat_lahir"].ToString() + ", " + dob.ToString("dd-MM-yyyy");
                textBox5.Text = reader["alamat"].ToString();

                connection.Close();
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand("select count (nik) as num from vaksinasi where nik = '"+lblnik.Text+"'", connection);
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                dataReader.Read();
                int num = Convert.ToInt32(dataReader["num"]);
                connection.Close();
                if (num == 0)
                {
                    lblperiode.Text = "1";
                }
                else if (num == 1)
                    lblperiode.Text = "2";
                else if (num >= 2)
                    lblperiode.Text = lblname.Text + " Telah melakukan vaksin dosis dua";
            }
            else
            {
                connection.Close();
                MessageBox.Show("Warga tidak dapat ditemukan", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow.Selected == false)
            {
                MessageBox.Show("Mohon pilih salah satu data untuk melihat detail!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DetailVaksinasi detail = new DetailVaksinasi();
                detail.nik = nik;
                this.Hide();
                detail.ShowDialog();
            }
        }

        bool val()
        {
            if(lblnik.Text.Length < 1)
            {
                MessageBox.Show("Mohon pilih salah satu warga", "Terjadi kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(lblperiode.Text != "2" && lblperiode.Text != "1")
            {
                MessageBox.Show("Mohon maaf warga telah melakukan vaksin dosis 2\nMohon menunggu info untuk dosis selanjutnya", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (val())
            {
                string sql = "insert into vaksinasi values('" + lblnik.Text + "')";
                try
                {
                    Command.exec(sql);
                    SqlCommand command = new SqlCommand("select top(1) * from vaksinasi order by id desc", connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    int iv = Convert.ToInt32(reader["id"]);
                    connection.Close();

                    string com = "insert into detail_vaksinasi values(" + iv + ", " + Convert.ToInt32(lblperiode.Text) + ", getdate(), " + Convert.ToInt32(combo_vaksin.SelectedValue) + ", " + Convert.ToInt32(combo_dokter.SelectedValue) + ")";
                    try
                    {
                        Command.exec(com);
                        MessageBox.Show("Berhasil menambahkan data vaksin!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        loadgrid();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        MessageBox.Show("" + ex);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    connection.Close();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void clear()
        {
            textBox1.Text = "";
            lblname.Text = "";
            lblnik.Text = "";
            lblperiode.Text = "";
            textBox5.Text = "";
            lblttl.Text = "";
        }

        private void panel_master_Click(object sender, EventArgs e)
        {
            ReportVaksin show = new ReportVaksin();
            this.Hide();
            show.ShowDialog();
        }
    }
}
