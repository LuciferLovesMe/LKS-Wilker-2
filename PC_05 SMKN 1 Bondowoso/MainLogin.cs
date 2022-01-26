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
    public partial class MainLogin : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        int level;
        public MainLogin()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin menutup ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.TextLength < 1 || textBox2.TextLength < 1 || comboBox1.Text.Length < 1)
            {
                MessageBox.Show("Semua Field Harap Diisi!", "Terjadi Masalah", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    SqlCommand command = new SqlCommand("select * from admin where username = '" + textBox1.Text + "' and password = @psw and status_aktif = 1 and level = " + level, connection);
                    command.Parameters.AddWithValue("@psw", textBox2.Text.ToString());
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        Session.id = Convert.ToInt32(reader["id"]);
                        Session.username = reader["username"].ToString();
                        Session.level = Convert.ToInt32(reader["level"]);
                        connection.Close();

                        if (level == 1)
                        {
                            MainAdmin main = new MainAdmin();
                            this.Hide();
                            main.ShowDialog();
                        }

                        else if (level == 2)
                        {
                            MainDokter main = new MainDokter();
                            this.Hide();
                            main.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Pengguna tidak dapat ditemukan", "Terjadi Masalah", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text.ToLower() == "admin")
                level = 1;
            else if (comboBox1.Text.ToLower() == "dokter")
                level = 2;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.PasswordChar = '\0';
            else if (!checkBox1.Checked)
                textBox2.PasswordChar = '*';
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == 8);
        }
    }
}
