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
    public partial class MasterWarga : Form
    {
        int cond, id;
        string nik;
        SqlConnection connection = new SqlConnection(Utils.conn);

        public MasterWarga()
        {
            InitializeComponent();
            lblnama.Text = Session.username;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy / HH:mm:ss");
            loadgrid();
            dis();
        }

        void dis()
        {
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            dateTimePicker1.Enabled = false;
            btnedit.Enabled = true;
            btntambah.Enabled = true;
            btndel.Enabled = true;
            btnsimpan.Enabled = false;
            btncancel.Enabled = false;
        }

        void enable()
        {
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            dateTimePicker1.Enabled = true;
            btnedit.Enabled = false;
            btntambah.Enabled = false;
            btndel.Enabled = false;
            btnsimpan.Enabled = true;
            btncancel.Enabled = true;
        }

        void loadgrid()
        {
            string sql = "select * from warga ";
            dataGridView1.DataSource = Command.getdata(sql);
        }

        private void panel_master_Click(object sender, EventArgs e)
        {
            MasterDokter master = new MasterDokter();
            this.Hide();
            master.ShowDialog();
        }

        private void panel_vaksin_Click(object sender, EventArgs e)
        {
            MasterJenisVaksin master = new MasterJenisVaksin();
            this.Hide();
            master.ShowDialog();
        }

        private void panel_warga_Click(object sender, EventArgs e)
        {
            MasterWarga master = new MasterWarga();
            this.Hide();
            master.ShowDialog();
        }

        private void panel_operator_Click(object sender, EventArgs e)
        {
            MasterData master = new MasterData();
            this.Hide();
            master.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin menutup ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yaking ingin logout?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MainLogin main = new MainLogin();
                this.Hide();
                main.ShowDialog();
                Logout.logout();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sql = "select * from warga where nama like '%" + textBox1.Text + "%' or nik like '%"+textBox1.Text+"%'";
            dataGridView1.DataSource = Command.getdata(sql);
        }

        private void btntambah_Click(object sender, EventArgs e)
        {
            cond = 1;
            enable();
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows != null)
            {
                cond = 2;
                enable();
            }
        }

        private void btndel_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows != null)
            {
                DialogResult result = MessageBox.Show("Apakah anda yakin ingin menghapus?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                SqlCommand command = new SqlCommand("select * from detail_vaksinasi where id_jenis_vaksin = '"+nik+"'", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    MessageBox.Show("Mohon maaf data ini tidak dapat dihapus", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    connection.Close();
                }
                else
                {
                    string sql = "delete from admin where id = " + id;
                    string com = "delete from warga where id_user = " + id;
                    try
                    {
                        Command.exec(com);
                        Command.exec(sql);
                        MessageBox.Show("Berhasil Menghapus Warga!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadgrid();
                        clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("" + ex);
                        connection.Close();
                        throw;
                    }
                }
            }
        }

        bool val()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || textBox5.TextLength < 1 || textBox6.TextLength < 1 || dateTimePicker1.Value == null)
            {
                MessageBox.Show("Semua field harus diisi!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (textBox2.TextLength != 16)
            {
                MessageBox.Show("NIK harus 16 digit!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(textBox6.TextLength < 11)
            {
                MessageBox.Show("No HP minimal 11 digit!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand command = new SqlCommand("select * from warga where nik = '" + textBox2.Text + "'", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("NIK telah digunakan", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            connection.Close();
            return true;
        }
        bool val_up()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || textBox5.TextLength < 1 || textBox6.TextLength < 1 || dateTimePicker1.Value == null)
            {
                MessageBox.Show("Semua field harus diisi!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (textBox2.TextLength != 16)
            {
                MessageBox.Show("NIK harus 16 digit!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (textBox6.TextLength < 11)
            {
                MessageBox.Show("No HP minimal 11 digit!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        string getuser()
        {
            SqlCommand command = new SqlCommand("select count(level) as num from admin where level = 3", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int num = Convert.ToInt32(reader["num"]);
            connection.Close();
            if(num == 0)
            {
                return "warga1";
            }
            else
            {
                return "warga" + num.ToString();
            }
        }
        int getid()
        {
            SqlCommand command = new SqlCommand("select top(1) * from admin where level = 3 order by id desc", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int num = Convert.ToInt32(reader["id"]);
            connection.Close();
            if (num == 0)
            {
                return 1;
            }
            else
            {
                return num;
            }
        }

        private void btnsimpan_Click(object sender, EventArgs e)
        {
            if (val())
            {
                if(cond == 1)
                {
                    string sql = "insert into admin values('" + getuser() + "', '123123123', 1, 3, '')";
                    try
                    {
                        Command.exec(sql);

                        string com = "insert into warga values('"+textBox2.Text+"', '"+textBox3.Text+"', '"+textBox4.Text+"', @date,  '"+textBox5.Text+"', '"+textBox6.Text+"', " + getid() + ")";
                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(com, connection);
                            command.Parameters.AddWithValue("@date", Convert.ToDateTime(dateTimePicker1.Value));
                            command.ExecuteNonQuery();
                            connection.Close();

                            //Command.exec(com);
                            MessageBox.Show("Berhasil menambah warga baru!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadgrid();
                            clear();
                            dis();
                        }
                        catch (Exception ex)
                        {
                            connection.Close();
                            MessageBox.Show(ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        connection.Close();
                    }
                }
                else if(cond == 2)
                {
                    string sql = "update warga set nik = '" + textBox2.Text + "', nama = '" + textBox3.Text + "', tempat_lahir = '" + textBox4.Text + "', tanggal_lahir = '" + Convert.ToDateTime(dateTimePicker1.Value) + "', alamat = '" + textBox5.Text + "', noHP = '" + textBox6.Text + "' where id_user = " + id;
                    try
                    {
                        Command.exec(sql);
                        MessageBox.Show("Berhasil mengubah warga", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadgrid();
                        clear();
                        dis();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(""+ex);
                    }
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == 8);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == 8);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[6].Value);
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            dateTimePicker1.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            textBox6.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            dateTimePicker1.Value = DateTime.Now;
        }
    }
}
