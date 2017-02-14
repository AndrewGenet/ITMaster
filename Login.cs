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
using IT_Master_Ap;

namespace IT_Master_Ap
{
    public partial class Login : Form
    {
        
        //Abcd1234 = b44dda1dadd351948fcace1856ed97366e679239
        public Login()
        {
            InitializeComponent();
            //WindowState = FormWindowState.Maximized;
        }

        //
        //
        //login button
        private void button1_Click(object sender, EventArgs e)
        {
            if (password.Text != "")
            {
                //hash passwprd and check for exact match
                String pass = Program.hash(password.Text.ToString());
                string con = "Data Source=NMCSQL2;Initial Catalog=itmaster;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
                string query = "select * from [dbo].[Login] where LoginID = '" + userName.Text.ToString() +"'";
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
                  
                    
                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow);
                    try
                    {
                        
                        if (reader.Read())
                        {
                            //change pass required
                            if (reader.GetInt32(2) == 1 && (reader.GetString(1) == pass) )
                            {
                               
                                //open change passwords form
                                //when saved clicked compare them
                                //if match then hash pass, update database(password and changepassword = 0, close window and open ap
                                //if no match do nothing
                                
                                Program.LoginID = userName.Text.ToString();
                                this.Hide();
                                changePasswordForm displayChangePass = new changePasswordForm();
                                displayChangePass.Show();
                                
                            }
                                //validation successful
                            else if (reader.GetValue(1).ToString() == pass)
                            {
                                //close this form and open app
                                Program.LoginID = userName.Text.ToString();
                                
                                Program.Admin = reader.GetInt32(3);
                                //Console.WriteLine(Program.Admin.ToString());
                                this.Hide();
                                Form1 form1 = new Form1();
                                form1.Show();
                            }
                            else
                            {
                                //wrong password
                                
                                MessageBox.Show("Wrong username or password");
                            }
                        }

                        
                    }
                    finally
                    {
                        // Always call Close when done reading.
                        reader.Close();
                        connection.Close();
                    }
                }

                //Form1 display = new Form1();
                //display.Show();
            }
        }

        //
        //
        //enter key down event for login
        private void password_KeyDown(object sender, KeyEventArgs e)
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
