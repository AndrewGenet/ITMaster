using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace IT_Master_Ap
{
    public partial class changePasswordForm : Form
    {
        public changePasswordForm()
        {
            InitializeComponent();
        }

        //
        //
        //save button
        private void button1_Click(object sender, EventArgs e)
        {
            //passwords don't match
            if (textBox1.Text != textBox2.Text)
            {
                MessageBox.Show("Passwords do not match, please try again.");
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Focus();
            }
            else if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Invalid entry.");
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Focus();
            }
                //passwords match, updating entry
            else
            {
                //update database and display form1
                //hashing password to store
                String pass = Program.hash(textBox1.Text.ToString());
                string con = "Data Source=NMCSQL2;Initial Catalog=itmaster;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
                string query = "update [dbo].[Login] set Password = '"+pass.ToString()+"', ChangePassword = 0 where LoginID = '" + Program.LoginID.ToString() +"'";
                this.Validate();
                using (SqlConnection connection = new SqlConnection(con))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    try
                    {
                        
                        if (command.ExecuteNonQuery() != 0)
                        {
                            //reader.Close();
                            MessageBox.Show("Password changed.");
                            this.Close();
                            Form1 form1 = new Form1();
                            form1.Show();
                            connection.Close();
                        }
                    }
                    catch
                    {

                    }

                }
            }
        }
        //press enter to click save event
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
              
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }
    }
}
