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
    public partial class MasterJenisVaksin : Form
    {
        int id, cond;
        SqlConnection connection = new SqlConnection(Utils.conn);
        public MasterJenisVaksin()
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
            if (result == DialogResult.Yes)
            {
                MainLogin main = new MainLogin();
                this.Hide();
                main.ShowDialog();
                Logout.logout();
            }
        }

        void loadgrid()
        {
            string sql = "select * from jenis_vaksin";
            dataGridView1.DataSource = Command.getdata(sql);
        }

        void dis()
        {
            btntambah.Enabled = true;
            btnedit.Enabled = true;
            btndel.Enabled = true;
            btnsimpan.Enabled = false;
            btncancel.Enabled = false;
            textBox2.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string com = "select * from jenis_vaksin where nama_vaksin like '%" + textBox1.Text + "%'";
            dataGridView1.DataSource = Command.getdata(com);
        }

        private void btntambah_Click(object sender, EventArgs e)
        {
            cond = 1;
            enable();
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow.Selected != false)
            {
                cond = 2;
                enable();
            }
        }
        

        private void btndel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected != false)
            {
                DialogResult result = MessageBox.Show("Apakah anda yakin ingin menghapus?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if(result == DialogResult.Yes)
                {
                    SqlCommand command = new SqlCommand("select * from detail_vaksinasi where id_jenis_vaksin = " + id, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        MessageBox.Show("Mohon maaf jenis ini tidak dapat dihapus", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        connection.Close();
                    }else
                    {
                        connection.Close();
                        string command1 = "delete from jenis_vaksin where id = " + id;
                        try
                        {
                            Command.exec(command1);
                            MessageBox.Show("Berhasil Menghapus", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadgrid();
                            textBox2.Text = "";
                            dis();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("" + ex);
                            connection.Close();
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin menutup ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void btnsimpan_Click(object sender, EventArgs e)
        {
            if(textBox2.TextLength > 0)
            {
                if(cond == 1)
                {
                    string sql = "insert into jenis_vaksin values('" + textBox2.Text + "')";
                    try
                    {
                        Command.exec(sql);
                        MessageBox.Show("Berhasil Menambahkan!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadgrid();
                        dis();
                        textBox2.Text = "";
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        MessageBox.Show("" + ex);
                    }
                }
                if(cond == 2)
                {
                    string sql = "update jenis_vaksin set nama_vaksin = '" + textBox2.Text + "' where id =" + id;
                    try
                    {
                        Command.exec(sql);
                        MessageBox.Show("Berhasil Mengubah!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadgrid();
                        dis();
                        textBox2.Text = "";
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        MessageBox.Show("" + ex);
                    }
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetterOrDigit(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == 8);
        }

        void enable()
        {
            btntambah.Enabled = false;
            btnedit.Enabled = false;
            btndel.Enabled = false;
            btnsimpan.Enabled = true;
            btncancel.Enabled = true;
            textBox2.Enabled = true;
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
    }
}
