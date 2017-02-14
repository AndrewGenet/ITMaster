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

namespace IT_Master_Ap
{
    public partial class AddUserForm : Form
    {
        public AddUserForm()
        {
            InitializeComponent();
        }

        private void addUser_btn_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {

                if (MessageBox.Show("Do you want to add " + comboBox1.Text + " as a user?", "My Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string con = "Data Source=NMCSQL2;Initial Catalog=itmaster;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
                    string query = "insert into [dbo].[Login] values ('" + comboBox1.Text + "', 'b44dda1dadd351948fcace1856ed97366e679239', '1','0')";
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
                                MessageBox.Show("User " + comboBox1.Text + " added.");
                                this.Close();                                
                                connection.Close();
                            }
                        }
                        catch
                        {
                            MessageBox.Show("ERROR: user not added");
                        }

                    }
                    
                }

            }
        }
        //making enter press add user button
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                addUser_btn.PerformClick();

                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        //change password button click
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                //reseting password to default
                if (MessageBox.Show("Do you want to reset " + comboBox1.Text + "'s password?", "My Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string con = "Data Source=NMCSQL2;Initial Catalog=itmaster;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
                    string query = "update [dbo].[Login] set Password = 'b44dda1dadd351948fcace1856ed97366e679239', ChangePassword = 1 where LoginID = '" + comboBox1.Text + "'";
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
                                MessageBox.Show("User " + comboBox1.Text + "'s password reset.");
                                this.Close();

                                connection.Close();
                            }
                            else
                            {
                                MessageBox.Show("ERROR: password not reset.");
                            }
                        }
                        catch
                        {
                            MessageBox.Show("ERROR: password not reset.");
                        }
                    }
                }
            }
        }
        //delete user button
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {

                if (MessageBox.Show("Do you want to delete user " + comboBox1.Text + "?", "My Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string con = "Data Source=NMCSQL2;Initial Catalog=itmaster;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
                    string query = "delete from [dbo].[Login] where LoginID = '" + comboBox1.Text + "'";
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
                                MessageBox.Show("User " + comboBox1.Text + " deleted.");
                                this.Close();

                                connection.Close();
                            }
                            else
                            {
                                MessageBox.Show("ERROR: user not deleted.");
                            }
                        }
                        catch
                        {
                            MessageBox.Show("ERROR: user not deleted.");
                        }
                    }
                }
            }
        }
        //add admin
        private void button3_Click(object sender, EventArgs e)
        {
            if (Program.Admin == 1)
            {
                if (comboBox1.Text != "")
                {

                    if (MessageBox.Show("Do you want to make " + comboBox1.Text + " an admin?", "My Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        string con = "Data Source=NMCSQL2;Initial Catalog=itmaster;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
                        string query = "update [dbo].[Login] set Admin = '1' where LoginID = '" + comboBox1.Text + "'";
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
                                    MessageBox.Show("User " + comboBox1.Text + " is now an Admin.");
                                    this.Close();

                                    connection.Close();
                                }
                                else
                                {
                                    MessageBox.Show("ERROR: Admin not set.");
                                }
                            }
                            catch
                            {
                                MessageBox.Show("ERROR: Admin not set.");
                            }
                        }
                    }           
                }
            }
            else
            {
                MessageBox.Show("You do not have permission to perform this action.");
            }
        }
        //loop through users and add them to combo box
        private void AddUserForm_Load(object sender, EventArgs e)
        {
            string con = "Data Source=NMCSQL2;Initial Catalog=itmaster;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
            string query = "select LoginId from Login";
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
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader.GetString(0));
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }

            }

        }
    }
}
