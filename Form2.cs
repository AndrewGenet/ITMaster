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
    public partial class addTableForm : Form
    {
        public addTableForm()
        {
            InitializeComponent();
        }
        //savie table button action
        private void saveTable_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save your changes?", "My Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string con = "Data Source=NMCSQL2;Initial Catalog=itmaster;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
                string query = "CREATE TABLE [dbo].[" + tableName.Text.ToString() + "]([" + tableName.Text.ToString() + "ID] INT NOT NULL PRIMARY KEY IDENTITY ";
                DataTable t = new DataTable(tableName.Text.ToString());
                t.Columns.Add(tableName.Text.ToString() + "ID");
                // 
                //
                //build custom query based on number of columns you are trying to add
                foreach (DataGridViewRow row in addColumns.Rows)
                {
                    if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                    {
                        query += ", [" + row.Cells[0].Value.ToString() + "] " + row.Cells[1].Value.ToString() + " " + row.Cells[2].Value.ToString() + " ";
                        t.Columns.Add(row.Cells[0].Value.ToString());
                    }        

                }
                query += ")";
                try
                {

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
                              
                            }
                        }
                        finally
                        {
                            //opening updated form
                            reader.Close();
                            connection.Close();
                            Form1 f1 = new Form1();
                            f1.Show();
                        }
                    }
                    MessageBox.Show("Update successful");
                }
                catch
                {
                    MessageBox.Show("Update failed");
                }
            }
            else
            {
                //
                //
                //'No' selected, discarding changes and refreshing table
            }

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addColumns_RowEnter(object sender, DataGridViewRowEventArgs e)
        {

        }
      
    }
}
