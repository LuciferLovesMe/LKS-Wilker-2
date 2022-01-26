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
    public partial class MasterData : Form
    {
        int cond, id;
        SqlConnection connection = new SqlConnection(Utils.conn);
        public MasterData()
        {
            InitializeComponent();
            lblnama.Text = Session.username; 
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy / HH:mm:ss");
            loadgrid();
            dis();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yaking ingin logout?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                MainLogin main = new MainLogin();
                this.Hide();
                main.ShowDialog();
                Logout.logout();
            }
        }

        void loadgrid()
        {
            string sql = "select * from admin where level = 1";
            dataGridView1.DataSource = Command.getdata(sql);
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[4].Visible = false;

        }

        void dis()
        {
            btnsimpan.Enabled = false;
            btncancel.Enabled = false;
            btnedit.Enabled = true;
            btntambah.Enabled = true;
            btndel.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
        }

        void enable()
        {
            btnsimpan.Enabled = true;
            btncancel.Enabled = true;
            btnedit.Enabled = false;
            btntambah.Enabled = false;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
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
            MasterOperator master = new MasterOperator();
            this.Hide();
            master.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin menutup ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();

        }

        private void btntambah_Click(object sender, EventArgs e)
        {
            cond = 1;
            enable();
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow != null)
            {
                cond = 2;
                enable();
                textBox3.Enabled = false;
                textBox4.Enabled = false;
            }
        }

        private void btndel_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow!= null)
            {
                if(Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[3].Value) == 0)
                {
                    DialogResult result = MessageBox.Show("Apakah anda yakin ingin mengaktifkan kembali?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        string sql = "update admin set status_aktif = 1 where id = " + id;
                        try
                        {
                            Command.exec(sql);
                            MessageBox.Show("Berhasil mengaktifkan kembali", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadgrid();
                            dis();
                            cond = 0;
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show("" + ex);
                            connection.Close();
                        }
                    }
                }
                else if(Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[3].Value) == 1)
                {
                    DialogResult result = MessageBox.Show("Apakah anda yakin ingin menon-aktifkan?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(result == DialogResult.Yes)
                    {
                        string sql = "update admin set status_aktif = 0 where id = " + id;
                        try
                        {
                            Command.exec(sql);
                            MessageBox.Show("Berhasil menon-aktifkan", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadgrid();
                            dis();
                            cond = 0;
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show("" + ex);
                            connection.Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Mohon pilih salah satu user", "Terjadi kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        bool getuser()
        {
            SqlCommand command = new SqlCommand("select * from admin where username = '" + textBox2.Text + "'", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                MessageBox.Show("Username telah digunakan!", "Terjadi kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connection.Close();
                return false;
            }
            connection.Close();
            return true;
        }

        bool val()
        {
            if(textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || textBox5.TextLength < 1)
            {
                MessageBox.Show("Semua field harus diisi!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;

            }
            if(textBox4.Text != textBox3.Text)
            {
                MessageBox.Show("Konfirmasi password tidak benar!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            return true;
        }

        bool val_up()
        {
            if(textBox2.TextLength < 1 || textBox5.TextLength < 1)
            {
                MessageBox.Show("Semua field harus diisi!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; ;
            }
            return true;
        }

        void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void btnsimpan_Click(object sender, EventArgs e)
        {
            if(cond == 1 && val() && getuser())
            {
                string com = "insert into admin values('" + textBox2.Text + "', '" + textBox3.Text + "', 1, 1, '"+textBox5.Text+"')";
                try
                {
                    Command.exec(com);
                    loadgrid();
                    MessageBox.Show("Berhasil menambahkan", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dis();
                    clear();
                }
                catch (Exception ex)
                {
                    connection.Close();
                    MessageBox.Show(ex.Message);
                }
            }
            else if(cond == 2 && val_up() && getuser())
            {
                string com = "update admin set nama = '" + textBox5.Text + "', username = '" + textBox2.Text + "' where level = 1 and id = " + id;
                try
                {
                    Command.exec(com);
                    loadgrid();
                    MessageBox.Show("Berhasil mengubah", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dis();
                    clear();
                }
                catch (Exception ex)
                {
                    connection.Close();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            btndel.Enabled = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox5.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            if (Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[3].Value) == 0)
            {
                btndel.Text = "Aktifkan";
            }
            else if (Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[3].Value) == 1)
                btndel.Text = "Nonaktifkan";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox3.PasswordChar = '\0';
                textBox4.PasswordChar = '\0';
            }
            else if (!checkBox1.Checked)
            {
                textBox3.PasswordChar = '*';
                textBox4.PasswordChar = '*';
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            dis();
            clear();
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == 8);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sql = "select * from admin where level = 1 and nama like '%"+textBox1.Text+"%' or username like '%" + textBox1.Text + "%'";
            dataGridView1.DataSource = Command.getdata(sql);
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[2].Visible = false;
        }
    }
}
