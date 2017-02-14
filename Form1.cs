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
using System.Net.NetworkInformation;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace IT_Master_Ap
{
    public partial class Form1 : Form
    {
        //allows cancel option on save method
        private int cancelFlag = 0;
        //allows auto save prompt for added tables
        private int changeFlag = 0;
        //need these for tables loaded from the database, allows them to save
        SqlDataAdapter AddedTableDataAdapter = new SqlDataAdapter();
        DataSet AddedTableDataSet = new DataSet();
        //sets full screen and form closing event
        public Form1()
        {
            
            //FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            InitializeComponent();
        

            //
            //
            //If changes are made and Ap closed this prompts to save
            this.FormClosing += itmaster_FormClosing;
        }
        //checks for added tables in database and opens devices table
        private void Form1_Load(object sender, EventArgs e)
        {

            //////
            ////
            //
            /*IMPORTANT allows grid table to be changed on button click. Tables will not load if this property is false!*/
            this.dataGridView1.AutoGenerateColumns = true;
            //bug that woulednt let me change this value so I had to set at load time
            this.searchBox.UseWaitCursor = false;
            //
            //
            //checks if tables other than the ones expected are in database and makes a button for each.
            string con = "Data Source=NMCSQL2;Initial Catalog=itmaster;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
            SqlConnection connection = new SqlConnection(con);
            try
            {
                connection.Open();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            
            DataTable t = connection.GetSchema("Tables");
            List<string> TableNames = new List<string>();
            List<string> TableNamesIgnored = new List<string>() 
            { 
                "servers", 
                "pc",
                "passwords",
                "mobileDevices",
                "dataPorts",
                "devices",
                "Login",
                "Devices+"
            };
            foreach (DataRow row in t.Rows)
            {
                TableNames.Add(row[2].ToString());
            }
            for (int i = 0; i < TableNames.Count; i++)
            {
                if (TableNamesIgnored.Contains(TableNames[i].ToString()))
                {
                    //ignore since button exists or should be ignored
                }
                else
                {
                    //must create button to load table in datagridview
                    var newTableButton = newButton(TableNames[i].ToString());
                }
                
            }
               
            connection.Close();
            devices_btn.PerformClick();
        }
        
        //makes this form completely exit the program on closing
        private void itmaster_FormClosing(object sender, FormClosingEventArgs e)
        {
            //
            //
            // Check if there are unsaved changes
            if (this.itmasterDataSet2.HasChanges() || changeFlag == 1)
            {
                changeFlag = 0;
                //
                //
                // Cancel the Closing event from closing the form.
                e.Cancel = true;
                //
                //
                // Perform optional save
                save.PerformClick();
                if (cancelFlag == 1)
                {
                    //canceling form close
                    cancelFlag = 0;
                    return;
                }
                else
                {
                    e.Cancel = false;
                }
               
            }
            Application.Exit();
            
        }
        
        //
        //
        //Display table buttons 
        //open Mobile Devices table
        private void mobile_btn_Click(object sender, EventArgs e)
        {
            //original bad idea, make each table its own page, just here for reference #whatnottodo
            //mobile_devices_window display = new mobile_devices_window();
            //display.Show();

            if (this.itmasterDataSet2.HasChanges() || changeFlag == 1)
            {
                changeFlag = 0;
                //
                //
                // Perform optional save if changes to previous table unsaved
                save.PerformClick();
                if (cancelFlag == 1)
                {
                    return;
                }
            }

            //
            //
            //must clear filter for full table view incase something previously searched
            dataGridView2.Visible = false;
            dataGridView1.Dock = DockStyle.Fill;
            mobileDevicesBindingSource.Filter = "";
            dataGridView1.DataSource = mobileDevicesBindingSource;
            mobileDevicesTableAdapter.Fill(this.itmasterDataSet2.mobileDevices);
            
            //
            //
            //clear and fill combo box with column names for search and reset search cue
            comboBox1.Items.Clear();
            comboBox1.ResetText();
            //searchBox.Clear();
            searchBox.Text = "Select column and enter search value.";
            searchBox.ForeColor = Color.Silver;
            for (int i = 1; i < dataGridView1.ColumnCount; i++ )
            {
                comboBox1.Items.Add(dataGridView1.Columns[i].Name);
            }    
        }
       
        //open PC table
        private void pc_btn_Click(object sender, EventArgs e)
        {

            if (this.itmasterDataSet2.HasChanges() || changeFlag == 1)
            {
                changeFlag = 0;
                //
                //
                // Perform optional save if changes to previous table unsaved
                save.PerformClick();
                if (cancelFlag == 1)
                {
                    return;
                }
            }
            //
            //
            //clear possible filter
            dataGridView2.Visible = false;
            dataGridView1.Dock = DockStyle.Fill;
            pcBindingSource.Filter = "";
            dataGridView1.DataSource = pcBindingSource;
            this.pcTableAdapter.Fill(this.itmasterDataSet2.pc);

            //
            //
            //Fill combo box with column names for search and reset search cue
            comboBox1.Items.Clear();
            comboBox1.ResetText();
            //searchBox.Clear();
            searchBox.Text = "Select column and enter search value.";
            searchBox.ForeColor = Color.Silver;
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                comboBox1.Items.Add(dataGridView1.Columns[i].Name);
            }
            
           
        }
        //
        //
        //open devices and top table on program load
        private void devices_btn_Click(object sender, EventArgs e)
        {
            if (this.itmasterDataSet2.HasChanges() || changeFlag == 1)
            {
                changeFlag = 0;
                //
                //
                // Perform optional save if changes to previous table unsaved
                save.PerformClick();
                if (cancelFlag == 1)
                {
                    return;
                }
            }
            dataGridView2.Visible = true;
            
            //setting up devices table with table above
            dataGridView1.Dock = DockStyle.Bottom;       
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.BringToFront();
            dataGridView2.DataSource = devicesBindingSource;
            this.devices_TableAdapter.Fill(this.itmasterDataSet2._Devices_);            
            devicesBindingSource1.Filter = "";
            dataGridView1.DataSource = devicesBindingSource1;
            this.devicesTableAdapter.Fill(this.itmasterDataSet2.devices);

            //
            //
            //Fill combo box with column names for search and reset serach cue
            comboBox1.Items.Clear();
            comboBox1.ResetText();
            //searchBox.Clear();
            searchBox.Text = "Select column and enter search value.";
            searchBox.ForeColor = Color.Silver;
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                comboBox1.Items.Add(dataGridView1.Columns[i].Name);
            }
        }
        //
        //TODO NEED DATA for passwords!
        //open Passwords table
        private void passwords_btn_Click(object sender, EventArgs e)
        {
            if (this.itmasterDataSet2.HasChanges() || changeFlag == 1)
            {
                changeFlag = 0;
                //
                //
                // Perform optional save if changes to previous table unsaved
                save.PerformClick();
                if (cancelFlag == 1)
                {
                    return;
                }
            }

            dataGridView2.Visible = false;
            dataGridView1.Dock = DockStyle.Fill;
            passwordsBindingSource.Filter = "";
            dataGridView1.DataSource = passwordsBindingSource;
            this.passwordsTableAdapter.Fill(this.itmasterDataSet2.passwords);

            //
            //
            //Fill combo box with column names for search and reset search cue
            comboBox1.Items.Clear();
            comboBox1.ResetText();
            //searchBox.Clear();
            searchBox.Text = "Select column and enter search value.";
            searchBox.ForeColor = Color.Silver;
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                comboBox1.Items.Add(dataGridView1.Columns[i].Name);
            }
        }
        //
        //TODO NEED DATA for data ports!
        //open data ports table
        private void dataPorts_btn_Click(object sender, EventArgs e)
        {
            if (this.itmasterDataSet2.HasChanges() || changeFlag == 1)
            {
                changeFlag = 0;
                //
                //
                // Perform optional save if changes to previous table unsaved
                save.PerformClick();
                if (cancelFlag == 1)
                {
                    return;
                }
            }

            dataGridView2.Visible = false;
            dataGridView1.Dock = DockStyle.Fill;
            dataPortsBindingSource1.Filter = "";
            dataGridView1.DataSource = dataPortsBindingSource1;
            this.dataPortsTableAdapter.Fill(this.itmasterDataSet2.dataPorts);

            //
            //
            //Fill combo box with column names for search and reset search cue
            comboBox1.Items.Clear();
            comboBox1.ResetText();
            //searchBox.Clear();
            searchBox.Text = "Select column and enter search value.";
            searchBox.ForeColor = Color.Silver;
            
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                comboBox1.Items.Add(dataGridView1.Columns[i].Name);
            }
        }
        
        //open Servers table
        private void servers_btn_Click(object sender, EventArgs e)
        {
            if (this.itmasterDataSet2.HasChanges() || changeFlag == 1)
            {
                changeFlag = 0;
                //
                //
                // Perform optional save if changes to previous table unsaved
                save.PerformClick();
                if (cancelFlag == 1)
                {
                    return;
                }
            }
            //clear possible filter
            dataGridView2.Visible = false;
            dataGridView1.Dock = DockStyle.Fill;
            serversBindingSource.Filter = "";
            dataGridView1.DataSource = serversBindingSource;
            this.serversTableAdapter.Fill(this.itmasterDataSet2.servers);

            //
            //
            //Fill combo box with column names for search and reset search cue
            comboBox1.Items.Clear();
            comboBox1.ResetText();
            //searchBox.Clear();
            searchBox.Text = "Select column and enter search value.";
            searchBox.ForeColor = Color.Silver;
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                comboBox1.Items.Add(dataGridView1.Columns[i].Name);
            }
        }


        //
        //
        //exit button
        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //
        //
        //Loads row data to text box(also see form1.designer.cs rowEnter function)
        //private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        //{

        //    if (dataGridView1.SelectedRows.Count > 0)
        //    {
        //        selectedTextBox.Clear();
        //        for (int i = 1; i < dataGridView1.Columns.Count; i++)
        //        {
        //            selectedTextBox.AppendText(dataGridView1.Columns[i].Name + ": " + dataGridView1.SelectedRows[0].Cells[i].Value);
        //            selectedTextBox.AppendText("\r\n");
        //        }
        //        //sets textbox to top of entry
        //        selectedTextBox.SelectionStart = 0;
        //        selectedTextBox.ScrollToCaret();
        //    }
        //}
       
        //
        //
        // cell click event , handles check box event logic
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
                //
                //
                //mobile devices table checkbox logic for regular and filtered views only used in "EditInGrid" mode
            try
            {
                if ((dataGridView1.DataSource == mobileDevicesBindingSource || (((System.Data.DataRowView)(((System.Windows.Forms.BindingSource)(dataGridView1.DataSource)).Current)).Row).Table.ToString() == itmasterDataSet2.mobileDevices.ToString()) && dataGridView1.IsCurrentCellDirty)
                {
                     //Commiting changes to table
                    dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    //
                    //
                    // Changing checkbox values appropriatly
                    if ((dataGridView1.CurrentCell.ColumnIndex == itmasterDataSet2.mobileDevices.NoStipendColumn.Ordinal) && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.Equals(true))
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.__25Column.Ordinal].Value = false;
                        dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.__50Column.Ordinal].Value = false;
                        if (dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.HaveDocumentColumn.Ordinal].Value.Equals(false))
                        {
                            this.dataGridView1.CancelEdit();
                            dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.NoStipendColumn.Ordinal].Value = false;
                            dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.__25Column.Ordinal].Value = false;
                            dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.__50Column.Ordinal].Value = false;

                        }

                    }
                    else if ((dataGridView1.CurrentCell.ColumnIndex == itmasterDataSet2.mobileDevices.__25Column.Ordinal) && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.Equals(true))
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.NoStipendColumn.Ordinal].Value = false;
                        dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.__50Column.Ordinal].Value = false;
                        if (dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.HaveDocumentColumn.Ordinal].Value.Equals(false))
                        {
                            this.dataGridView1.CancelEdit();
                            dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.NoStipendColumn.Ordinal].Value = false;
                            dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.__25Column.Ordinal].Value = false;
                            dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.__50Column.Ordinal].Value = false;

                        }
                    }
                    else if ((dataGridView1.CurrentCell.ColumnIndex == itmasterDataSet2.mobileDevices.__50Column.Ordinal) && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.Equals(true))
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.NoStipendColumn.Ordinal].Value = false;
                        dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.__25Column.Ordinal].Value = false;
                        if (dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.HaveDocumentColumn.Ordinal].Value.Equals(false))
                        {
                            this.dataGridView1.CancelEdit();
                            dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.NoStipendColumn.Ordinal].Value = false;
                            dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.__25Column.Ordinal].Value = false;
                            dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.__50Column.Ordinal].Value = false;

                        }
                    }
                    else if ((dataGridView1.CurrentCell.ColumnIndex == itmasterDataSet2.mobileDevices.HaveDocumentColumn.Ordinal) && dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.HaveDocumentColumn.Ordinal].Value.Equals(false))
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.NoStipendColumn.Ordinal].Value = false;
                        dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.__25Column.Ordinal].Value = false;
                        dataGridView1.Rows[e.RowIndex].Cells[itmasterDataSet2.mobileDevices.__50Column.Ordinal].Value = false;
                    }


                }
            }
            catch
            {

            }
               
        }
        //
        //
        //Hiding Identity column
        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {
            if (this.dataGridView1.Columns.Count > 1)
            {
                try
                {
                    //
                    //
                    //hide the ID column when tables are displayed
                    //also hiding the insert panel if it is open
                    this.dataGridView1.Columns[0].Visible = false;
                    insertPanel.Visible = false;
                    //flowLayoutPanel1.Controls.Clear();

                }
                catch
                {

                }
            }
            
        }
        //
        //
        //delete button click method
        private void delete_Click(object sender, EventArgs e)
        {
            DialogResult result = new DialogResult();
            try
            {
                result = MessageBox.Show("Do you want to delete this row from the " + ((System.Windows.Forms.BindingSource)(dataGridView1.DataSource)).DataMember.ToString() + "'s table and save your changes?", "My Application", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }
            catch
            {
                result = MessageBox.Show("Do you want to delete this row from the table and save your changes?", "My Application", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);

            }
            
            if (result == DialogResult.Yes)
            {
                foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
                {
                    try
                    {
                        dataGridView1.Rows.RemoveAt(item.Index);
                    }
                    catch
                    {

                    }

                }
                //saving changes
                Save();
                if (AddedTableDataSet != null)
                {
                    try
                    {
                        this.AddedTableDataAdapter.Update(this.AddedTableDataSet);
                    }
                    catch
                    {

                    }
                }
            }
            changeFlag = 0;
        }
        //save button click method
        private void Save_Click(object sender, EventArgs e)
        {
            cancelFlag = 0;
            DialogResult result = MessageBox.Show("Do you want to save your changes?", "My Application", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                // 
                //
                // Call method to save changes...
                Save();
                
            }
            else if(result ==  DialogResult.Cancel) {
                cancelFlag = 1;
                return;
            }
            else
            {
                //
                //
                //'No' selected, discarding changes and refreshing table
                itmasterDataSet2.RejectChanges();
                changeFlag = 0;
                return;
            }
            //must set change flag to avoid asking for save more than once
            changeFlag = 0;
        }
        //drop down box index changed event to reset text
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
            //
            //select searchbox when index changes #functionality
            searchBox.Select();
        }
        //setting searchbox into filter mode
        private void searchBox_GotFocus(object sender, EventArgs e)
        {
            searchBox.Text = "";
            searchBox.ForeColor = Color.Black;

        }
        //setting serachbox into cue mode
        private void searchBox_LostFocus(object sender, EventArgs e)
        {

            if (searchBox.Text == "")
            {
                searchBox.Text = "Select column and enter search value.";
                searchBox.ForeColor = Color.Silver;
            }
        }
        //
        //text is reset when changing table must try/catch to avoid exception
        //if statement to avoid search before combo box is filled
        private void searchBox_TextChanged(object sender, EventArgs e)
        {           
            if (comboBox1.Items.Count > 1)
            {
                try
                {
                    //
                    //
                    //dynamically search for text based on category
                    BindingSource bs = new BindingSource();
                    bs.DataSource = dataGridView1.DataSource;
                    bs.Filter = "["+comboBox1.SelectedItem + "] like '%" + searchBox.Text + "%'";
                    //                    
                    //                   
                    //reset filter to reload table when combo box changes    
                    if (searchBox.Text == "" || searchBox.Text == "Select column and enter search value.")
                    {
                        bs.Filter = "";
                    }

                    dataGridView1.DataSource = bs;
                }
                catch 
                {
                    //MessageBox.Show(ex.ToString());
                }
            }
        }
        //adds new entry to datagrid TODO add save on click?
        private void insert_Click(object sender, EventArgs e)
        {
            //insert panel docks my flow layout panel
            flowLayoutPanel1.Controls.Clear();
            insertPanel.Visible = true;
            insertPanel.BringToFront();
            insertPanel.Dock = DockStyle.Top;
            updateRowBtn.Enabled = false;
            //flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            for (int i = 1; i < dataGridView1.Columns.Count; i++)
            {
                //declaring value that will be textbox of
                
                Panel p = new Panel();
                p.Name = dataGridView1.Columns[i].Name + "InsertPanel";      
                p.Width = 250;
                p.Height = 70;
                //p.BorderStyle = BorderStyle.FixedSingle;
                //flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
                Label l = new Label();
                l.Name = dataGridView1.Columns[i].Name + "InsertLabel";
                l.Text = dataGridView1.Columns[i].Name;
                l.TextAlign = ContentAlignment.MiddleCenter;
                l.Dock = DockStyle.Top;

                //checking for checkbox cell
                if (dataGridView1.Columns[i].CellType.FullName == "System.Windows.Forms.DataGridViewCheckBoxCell")
                {
                    CheckBox t = new CheckBox();
                    t.Name = dataGridView1.Columns[i].Name + "InsertValue";
                    t.Dock = DockStyle.Top;
                    t.CheckAlign = ContentAlignment.MiddleCenter;
                    
                    p.Controls.Add(t);
                }
                else
                {
                    //include a ping button for devices
                    if (dataGridView1.Columns[i].Name.ToString() == "IPAddress")
                    {
                        p.Height = 125;
                        TextBox t = new TextBox();
                        t.Name = dataGridView1.Columns[i].Name + "InsertValue";
                        t.Dock = DockStyle.Top;
                        t.Enabled = false;
                        Button pingBtn = new Button();
                        pingBtn.Name = "pingBtn";
                        pingBtn.Dock = DockStyle.Top;
                        pingBtn.Text = "Ping";
                        pingBtn.Click += (s, ex) =>
                            {
                                pingBtn.Visible = false;
                                t.Text = "Please wait...";
                                this.Cursor = Cursors.WaitCursor;
                                this.Enabled = false;                             
                                t.Text = PingNetwork();
                                this.Cursor = Cursors.Default;
                                this.Enabled = true;
                            };
                        RadioButton east = new RadioButton();
                        RadioButton west = new RadioButton();
                        east.Name = "nmgEastRadioButton";
                        east.Text = "NMG East";
                        east.Dock = DockStyle.Top;
                        east.Checked = true;
                        west.Name = "nmgWestRadioButton";
                        west.Text = "NMG West";
                        west.Dock = DockStyle.Top;
                        p.Controls.Add(west);
                        p.Controls.Add(east);
                        p.Controls.Add(pingBtn);
                        p.Controls.Add(t);
                    }
                        //making the combobox for all columns and filling with each unique value in table
                    else //if (dataGridView1.Columns[i].Name.ToString() == "Type")
                    {
                        ComboBox t = new ComboBox();
                        t.Name = dataGridView1.Columns[i].Name + "InsertValue";
                        t.Dock = DockStyle.Top;
                        //setting type dropdown values
                        string con = "Data Source=NMCSQL2;Initial Catalog=itmaster;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
                        //string query = "select DISTINCT type from devices";
                        string query = "select DISTINCT[" + dataGridView1.Columns[i].Name + "] from [" + dataGridView1.Columns[0].Name.ToString().Substring(0, dataGridView1.Columns[0].Name.Length - 2) + "]";
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
                                    if (!reader.IsDBNull(0))
                                    {
                                        try
                                        {
                                            t.Items.Add(reader.GetString(0));
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }
                            }
                            finally
                            {
                                reader.Close();
                                connection.Close();
                            }

                        }
                        p.Controls.Add(t);
                    }
                    //else
                    //{
                    //    TextBox t = new TextBox();
                    //    t.Name = dataGridView1.Columns[i].Name + "InsertValue";
                    //    t.Dock = DockStyle.Top;
                    //    p.Controls.Add(t);
                    //}
                    
                }
                p.Controls.Add(l);
                flowLayoutPanel1.Controls.Add(p);
            }         
        }
        //method for finding next available IP
        private string PingNetwork()
        {
            Ping pingSender = new Ping();
            string endIp, startingIp, searchIP;
            int max = 0;
            List<string> myIps = new List<string>();
            searchIP = "10.2.2.%";
            startingIp = "10.2.2.";
            endIp = "10.2.2.251";
            //else sratingIP = "10.2.6." endIp = "10.2.6.251"
            //finding IP in printer range
            if (flowLayoutPanel1.Controls["TypeInsertPanel"].Controls["TypeInsertValue"].Text.ToString() == "Printer")
            {
                //TODO determine if NMG East or NMG West
                if (((RadioButton)(flowLayoutPanel1.Controls["IPAddressInsertPanel"].Controls["nmgEastRadioButton"])).Checked == true)
                {
                    searchIP = "10.2.2.%";
                    endIp = "10.2.2.251";
                }
                else
                {
                    searchIP = "10.4.2.%";
                    endIp = "10.4.2.251";
                }
            }
                //finding IP in static range
            else if (flowLayoutPanel1.Controls["TypeInsertPanel"].Controls["TypeInsertValue"].Text.ToString() != "")
            {
                if (((RadioButton)(flowLayoutPanel1.Controls["IPAddressInsertPanel"].Controls["nmgEastRadioButton"])).Checked == true)
                {
                    searchIP = "10.2.6.%";
                    endIp = "10.2.6.251";
                }
                else
                {
                    searchIP = "10.4.6.%";
                    endIp = "10.4.6.251";
                }
            }
            else
            {
                MessageBox.Show("Please select device Type before you ping.");
                try
                {
                    //getting exception when insert ping clicked and no Type field selected
                    flowLayoutPanel1.Controls["IPAddressInsertPanel"].Controls["pingBtn"].Visible = true;
                }
                catch
                {
                    //flowLayoutPanel1.Controls["IPAddressInsertPanel"].Controls["Ping"].Visible = true;
                }
                
                return "";
            }

            string con = "Data Source=NMCSQL2;Initial Catalog=itmaster;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
            //check device if type printer ip = 10.2.2 else 10.2.6
            string query = "sp_GetMaxIp";
             this.Validate();
             using (SqlConnection connection = new SqlConnection(con))
             {
                 //using stored procedure to get all values in database for printer or static device
                 SqlCommand command = new SqlCommand(query, connection);
                 command.CommandType = CommandType.StoredProcedure;
                 command.Parameters.Add("@input", SqlDbType.VarChar).Value = searchIP;
                 try
                 {
                     connection.Open();
                 }
                 catch (SqlException ex)
                 {
                     MessageBox.Show(ex.Message);
                     
                 }
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        //iterating through and adding each IP to a list and compairing an int version to the current highest IP
                        myIps.Add(reader.GetString(0).ToString());
                        //takes each result removes all non numeric characters
                        string result = Regex.Replace(reader.GetString(0).ToString(), @"[^\d]", "");
                        if (Int32.Parse(result.ToString()) > max)
                        {
                            startingIp = reader.GetString(0).ToString();
                            max = Int32.Parse(result.ToString());
                        }
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
             }
            //incriment ip to get one above the highest and begin pinging process
             var ip = IPAddress.Parse(startingIp);
             var bytes = ip.GetAddressBytes();
             bytes[3] += 1;
             var temp = bytes[0] + "." + bytes[1] + "." + bytes[2] + "." + bytes[3];
             ip = IPAddress.Parse(temp);
            int pingFlag = 0;          
            //loop through range of ip addresses ending if it hits *.*.*.251
            while (ip.ToString() != endIp)
            {
                PingReply reply = pingSender.Send(ip);
                //ping replied, incrimenting and trying next IP
                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine(reply.Status);
                    if (bytes[3] == 255)
                    {
                        bytes[2] += 1;
                        bytes[3] += 1;
                    }
                    else
                    {
                        bytes[3] += 1;
                    }
                    var ip2 = bytes[0] + "." + bytes[1] + "." + bytes[2] + "." + bytes[3];
                    ip = IPAddress.Parse(ip2);
                }
                else
                {
                    //initial ping failed to reply, trying 4 more times
                    PingReply[] replies = new PingReply[] { pingSender.Send(ip), pingSender.Send(ip), pingSender.Send(ip), pingSender.Send(ip) };
                    for (int i = 0; i < 4; i++)
                    {
                        if (replies[i].Status != IPStatus.Success)
                        {
                            pingFlag = 1;
                        }
                        else
                        {
                            pingFlag = 0;
                            break;
                        }
                    }
                    if (pingFlag == 1)
                    {
                        //query lansweeper and check if dateLastUsed > 6 months ago and handle ip going over 250
                        //Finally resolve DNS
                        //timespan will give us a difference from now to the date last seen in days
                        TimeSpan ts = new TimeSpan();
                        string con2 = "Data Source=nmcsql2;Initial Catalog=lansweeperdb;Integrated Security=SSPI";
                        string query2 = "select Lastseen FROM [lansweeperdb].[dbo].[tblAssets] where IPAddress = '"+ip.ToString()+"'";
                        //string query2 = "select Lastseen FROM [lansweeperdb].[dbo].[tblAssets] where IPAddress = '10.2.8.237'";
                        this.Validate();
                        using (SqlConnection connection = new SqlConnection(con2))
                        {
                            SqlCommand command = new SqlCommand(query2, connection);
                            try
                            {
                                connection.Open();
                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show(ex.Message);
                                return "";
                            }
                            SqlDataReader reader = command.ExecuteReader();
                            try
                            {
                               
                                if (reader.Read())
                                {
                                    ts = DateTime.Now.Subtract(reader.GetDateTime(0));
                                    //checking if last seen date > 6months
                                    if (ts.TotalDays > 182.5)
                                    {
                                        //safe to dns
                                        try
                                        {

                                            IPHostEntry hostInfo = Dns.GetHostEntry(ip.ToString());
                                            //Console.WriteLine("My host info " + hostInfo.HostName + " " + hostInfo.Aliases);
                                        }
                                        catch (System.Net.Sockets.SocketException)
                                        {
                                            return ip.ToString();
                                        }
                                    }
                                }

                                else
                                {
                                    //query returned no results so still safe to dns
                                    //if exception is thrown then dns returned no results
                                    try
                                    {
                                        IPHostEntry hostInfo = Dns.GetHostEntry(ip.ToString());
                                        //Console.WriteLine("My host info " + hostInfo.HostName + " " + hostInfo.Aliases);
                                    }
                                    catch (System.Net.Sockets.SocketException)
                                    {
                                       //found our IP
                                        return ip.ToString();
                                    }
                                }
                            }
                            catch
                            {
                                
                            }
                            finally
                            {
                                reader.Close();
                                connection.Close();
                            }
                        }
                        //incriment IP and contiue searching
                    }
                    if (bytes[3] == 255)
                    {
                        bytes[2] += 1;
                        bytes[3] = 0;
                    }
                    else
                    {
                        bytes[3] += 1;
                    }
                    //incriment ip and try again
                    var ip2 = bytes[0] + "." + bytes[1] + "." + bytes[2] + "." + bytes[3];
                    //Console.WriteLine(ip2.ToString());
                    ip = IPAddress.Parse(ip2);

                }
            }
            //handle for starting IP scanning process over from 1
            ip = IPAddress.Parse(startingIp);
            bytes = ip.GetAddressBytes();
            bytes[3] = 1;
            temp = bytes[0] + "." + bytes[1] + "." + bytes[2] + "." + bytes[3];
            ip = IPAddress.Parse(temp);
            while (ip.ToString() != endIp)
            {
                //scaning IP's in database for any gap from the begining
                while (myIps.Contains(ip.ToString()))
                {
                    bytes = ip.GetAddressBytes();
                    bytes[3] += 1;
                    temp = bytes[0] + "." + bytes[1] + "." + bytes[2] + "." + bytes[3];
                    ip = IPAddress.Parse(temp);
                }
               //found a missing IP trying to ping
                PingReply reply = pingSender.Send(ip);
                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine(reply.Status);
                    if (bytes[3] == 255)
                    {
                        bytes[2] += 1;
                        bytes[3] += 1;
                    }
                    else
                    {
                        bytes[3] += 1;
                    }

                    var ip2 = bytes[0] + "." + bytes[1] + "." + bytes[2] + "." + bytes[3];
                    ip = IPAddress.Parse(ip2);
                }
                else
                {
                    //initial ping of missing ip misses
                    PingReply[] replies = new PingReply[] { pingSender.Send(ip), pingSender.Send(ip), pingSender.Send(ip), pingSender.Send(ip) };
                    for (int i = 0; i < 4; i++)
                    {
                        if (replies[i].Status != IPStatus.Success)
                        {
                            pingFlag = 1;
                        }
                        else
                        {
                            pingFlag = 0;
                            break;
                        }
                    }
                    if (pingFlag == 1)
                    {
                        //query lansweeper and check if dateLastUsed > 6 months ago and handle ip going over 250
                        //Finally resolve DNS
                        TimeSpan ts = new TimeSpan();
                        string con2 = "Data Source=nmcsql2;Initial Catalog=lansweeperdb;Integrated Security=SSPI";
                        string query2 = "select Lastseen FROM [lansweeperdb].[dbo].[tblAssets] where IPAddress = '"+ip.ToString()+"'";
                        //string query2 = "select Lastseen FROM [lansweeperdb].[dbo].[tblAssets] where IPAddress = '10.2.2.2'";
                        this.Validate();
                        using (SqlConnection connection = new SqlConnection(con2))
                        {
                            SqlCommand command = new SqlCommand(query2, connection);


                            try
                            {
                                connection.Open();

                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show(ex.Message);
                                return "";
                            }
                            SqlDataReader reader = command.ExecuteReader();
                            try
                            {

                                if (reader.Read())
                                {
                                    ts = DateTime.Now.Subtract(reader.GetDateTime(0));
                                    //checking if last seen date > 6months
                                    if (ts.TotalDays > 182.5)
                                    {
                                        //safe to dns
                                        try
                                        {

                                            IPHostEntry hostInfo = Dns.GetHostEntry(ip.ToString());
                                            //Console.WriteLine("My host info " + hostInfo.HostName + " " + hostInfo.Aliases);
                                        }
                                        catch (System.Net.Sockets.SocketException)
                                        {
                                            //MessageBox.Show(ip.ToString() + " is the next IP available.");
                                            return ip.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    //safe to dns
                                    try
                                    {

                                        IPHostEntry hostInfo = Dns.GetHostEntry(ip.ToString());
                                        //Console.WriteLine("My host info " + hostInfo.HostName + " " + hostInfo.Aliases);
                                    }
                                    catch (System.Net.Sockets.SocketException)
                                    {
                                        //MessageBox.Show(ip.ToString() + " is the next IP available.");
                                        return ip.ToString();
                                    }
                                }
                            }
                            catch
                            {

                            }
                            finally
                            {
                                reader.Close();
                                connection.Close();
                            }
                        }
                    }
                    if (bytes[3] == 255)
                    {
                        bytes[2] += 1;
                        bytes[3] = 0;
                    }
                    else
                    {
                        bytes[3] += 1;
                    }

                    var ip2 = bytes[0] + "." + bytes[1] + "." + bytes[2] + "." + bytes[3];
                    ip = IPAddress.Parse(ip2);
                    //use this ip to check against database values until gap is found again
                }

            }
            return "No IP found... ";
        }


        //pasting multiple new lines 
        private void ctrlPaste_KeyDown(object sender, KeyEventArgs e)
        {
            //code to past in rows
            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                try
                {
                    //
                    //Splitting what is copied, checking if selected row is the last in the table
                    //Making appropriate paste
                    //
                    dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    string s = Clipboard.GetText();
                    string[] lines = s.Split('\n');
                    
                    int iRow = dataGridView1.CurrentCell.RowIndex;
                    int iCol = dataGridView1.CurrentCell.ColumnIndex;
                    DataGridViewCell oCell;
                    if (iRow == dataGridView1.Rows.Count - 1  && lines.Length > 1)
                    {
                        //loop to generate rows if adding new rows to the table
                        BindingSource bs = new BindingSource();
                        bs.DataSource = dataGridView1.DataSource;
                        for (int i = 0; i < lines.Length - 1; i++)
                        {
                            bs.AddNew();
                        }   
                        dataGridView1.DataSource = bs;
                    }
               
                    foreach (string line in lines)
                    {
                        if (iRow < dataGridView1.RowCount && line.Length > 0)
                        {
                            string[] sCells = line.Split('\t');
                            for (int i = 0; i < sCells.GetLength(0); ++i)
                            {
                                if (iCol + i < this.dataGridView1.ColumnCount)
                                {
                                    oCell = dataGridView1[iCol + i, iRow];
                                    if (!oCell.ReadOnly)
                                    {
                                        
                                        oCell.Value = Convert.ChangeType(sCells[i], oCell.ValueType);
                                          
                                    }
                                }
                                else
                                { 
                                    break; 
                                }
                            }
                            iRow++;
                            
                            
                        }
                        else
                        { 
                            break; 
                        }
                    }
                }
                catch (FormatException)
                {
                    return;
                }
                 
            }
            
        }

        //opens the add table prompt
        private void addTable_Click(object sender, EventArgs e)
        {
            addTableForm display = new addTableForm();
            display.Show();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            changeFlag = 1;
        }
       
        //
        //
        //setting up a new button at page load
        public Button newButton(string name)
        {
            Button btn = new Button();


            btn.Name = name;
            btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(136)))), ((int)(((byte)(229)))));
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
            btn.Dock = System.Windows.Forms.DockStyle.Fill;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btn.ForeColor = System.Drawing.Color.White;
            //btn.Location = new System.Drawing.Point(3, 93);


            btn.Size = new System.Drawing.Size(173, 34);
            btn.TabIndex = 4;
            btn.Text = name;
            btn.UseVisualStyleBackColor = false;
            this.Controls.Add(btn);
            btn.BringToFront();
            //properly position tables in tablelayoutpanel
            this.tableLayoutPanel1.RowCount += 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Controls.Add(btn, 0, tableLayoutPanel1.RowCount - 2 );
            //btn.Click += new System.EventHandler(this.btn_click);
            btn.Click += (s, e) =>
                {
                    if (this.itmasterDataSet2.HasChanges() || changeFlag == 1)
                    {
                        changeFlag = 0;
                        //
                        //
                        // Perform optional save if changes to previous table unsaved
                        save.PerformClick();
                        if (cancelFlag == 1)
                        {
                            return;
                        }
                    }
                    string con = "Data Source=NMCSQL2;Initial Catalog=itmaster;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
                    string query = "select * from [" + name + "]";
                    SqlConnection c = new SqlConnection();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con); //c.con is the connection string

                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                    DataSet ds = new DataSet();
                    AddedTableDataAdapter = dataAdapter;
                    AddedTableDataSet = ds;
                    try
                    {
                        dataGridView1.Dock = DockStyle.Fill;
                        dataAdapter.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        //
                        //
                        //Fill combo box with column names for search and reset search cue
                        dataGridView2.Visible = false;
                        
                        comboBox1.Items.Clear();
                        comboBox1.ResetText();
                        //searchBox.Clear();
                        searchBox.Text = "Select column and enter search value.";
                        searchBox.ForeColor = Color.Silver;
                        for (int i = 1; i < dataGridView1.ColumnCount; i++)
                        {
                            comboBox1.Items.Add(dataGridView1.Columns[i].Name);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Table can not be displayed. Please check your database to make sure it is still there.");
                    }    
                };
            return btn;
        

        }
        //open add user form
        private void addUser_Click(object sender, EventArgs e)
        {
            AddUserForm adduserform = new AddUserForm();
            adduserform.Show();
        }
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {

            MessageBox.Show("Incorrect Data Type");

            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Commit error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change");
            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("parsing error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error");
            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

                anError.ThrowException = false;
            }
        }
        
        //double click row reader to display content TODO make this open insert option and fill with values
        private void dataGridView1_RowHeaderMouseDoubleClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                //benath method creates popup box with row data
                //string rowContent = "";
                ////selectedTextBox.Clear();
                //for (int i = 1; i < dataGridView1.Columns.Count; i++)
                //{
                //    rowContent += (dataGridView1.Columns[i].Name + ": " + dataGridView1.SelectedRows[0].Cells[i].Value);
                //    rowContent += ("\r\n");
                //}
                ////sets textbox to top of entry
                ////selectedTextBox.SelectionStart = 0;
                ////selectedTextBox.ScrollToCaret();
                //MessageBox.Show(rowContent);


                //populating insert table with current data TODO make update button so only current row is affected!!!!!
                flowLayoutPanel1.Controls.Clear();
                insertPanel.Visible = true;
                insertPanel.BringToFront();
                insertPanel.Dock = DockStyle.Top;
                updateRowBtn.Enabled = true;

                for (int i = 1; i < dataGridView1.Columns.Count; i++)
                {
                    //declaring value that will be textbox of

                    Panel p = new Panel();
                    p.Name = dataGridView1.Columns[i].Name + "InsertPanel";

                    p.Width = 250;
                    p.Height = 70;
                    //p.BorderStyle = BorderStyle.FixedSingle;
                    //flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
                    Label l = new Label();
                    l.Name = dataGridView1.Columns[i].Name + "InsertLabel";
                    l.Text = dataGridView1.Columns[i].Name;
                    l.TextAlign = ContentAlignment.MiddleCenter;
                    l.Dock = DockStyle.Top;

                    //insertPanel.Controls.Add(l);
                    //flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
                    //checking for checkbox cell
                    if (dataGridView1.Columns[i].CellType.FullName == "System.Windows.Forms.DataGridViewCheckBoxCell")
                    {
                        CheckBox t = new CheckBox();
                        t.Name = dataGridView1.Columns[i].Name + "InsertValue";
                        t.Dock = DockStyle.Top;
                        //determining if checkbox is checked
                        t.Checked = dataGridView1.SelectedRows[0].Cells[i].Value.Equals(true);
                        t.CheckAlign = ContentAlignment.MiddleCenter;

                        p.Controls.Add(t);
                    }
                    //include a ping button for devices
                    else if (dataGridView1.Columns[i].Name.ToString() == "IPAddress")
                    {
                        p.Height = 125;
                        TextBox t = new TextBox();
                        t.Name = dataGridView1.Columns[i].Name + "InsertValue";
                        t.Dock = DockStyle.Top;
                        t.Text = dataGridView1.SelectedRows[0].Cells[i].Value.ToString();
                        t.Enabled = false;
                        Button pingBtn = new Button();
                        pingBtn.Dock = DockStyle.Top;
                        pingBtn.Name = "pingBtn";
                        pingBtn.Text = "Ping";
                        //click event for ping btn
                        pingBtn.Click += (s, ex) =>
                        {
                            pingBtn.Visible = false;
                            t.Text = "Please wait...";
                            this.Cursor = Cursors.WaitCursor;
                            this.Enabled = false;
                            t.Text = PingNetwork();
                            this.Cursor = Cursors.Default;
                            this.Enabled = true;
                        };
                        //making radio buttons to determine which site to find IP
                        RadioButton east = new RadioButton();
                        RadioButton west = new RadioButton();
                        east.Name = "nmgEast";
                        east.Text = "NMG East";
                        east.Dock = DockStyle.Top;
                        east.Checked = true;
                        west.Name = "nmgWest";
                        west.Text = "NMG West";
                        west.Dock = DockStyle.Top;
                        p.Controls.Add(west);
                        p.Controls.Add(east);
                        p.Controls.Add(pingBtn);
                        p.Controls.Add(t);
                    }
                    //making the combobox for devices
                    else //if (dataGridView1.Columns[i].Name.ToString() == "Type")
                    {
                        ComboBox t = new ComboBox();
                        t.Name = dataGridView1.Columns[i].Name + "InsertValue";
                        t.Dock = DockStyle.Top;
                        string con = "Data Source=NMCSQL2;Initial Catalog=itmaster;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
                        string query = "select DISTINCT[" + dataGridView1.Columns[i].Name + "] from [" + dataGridView1.Columns[0].Name.ToString().Substring(0, dataGridView1.Columns[0].Name.Length - 2) + "]";
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
                                    if (!reader.IsDBNull(0))
                                    {
                                        try
                                        {
                                            t.Items.Add(reader.GetString(0));
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }
                            }
                            finally
                            {
                                reader.Close();
                                connection.Close();
                            }

                        }                     
                        t.Text = dataGridView1.SelectedRows[0].Cells[i].Value.ToString();

                        p.Controls.Add(t);
                    }
                    //else
                    //{
                    //    TextBox t = new TextBox();
                    //    t.Name = dataGridView1.Columns[i].Name + "InsertValue";
                    //    t.Dock = DockStyle.Top;
                    //    t.Text = dataGridView1.SelectedRows[0].Cells[i].Value.ToString();
                    //    p.Controls.Add(t);
                    //}

                    p.Controls.Add(l);
                    flowLayoutPanel1.Controls.Add(p);
                    //TODO test if update button works
                }
            }  
        }
        //these are parameters that work for datagridview2 as is, if changes are made to the format this needs to be changed.
        private void dataGridView2_RowHeaderMouseDoubleClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                string rowContent = "";
                //selectedTextBox.Clear();
                if (dataGridView2.SelectedRows[0].Index > 4)
                {
                    for (int i = 0; i < dataGridView2.Columns.Count; i++)
                    {
                        //skip if cell is empty
                        if (dataGridView2.SelectedRows[0].Cells[i].Value != "")
                        {
                            switch (i)
                            {
                                case 1:
                                    rowContent += "Stow, Corp: ";
                                    break;
                                case 3:
                                    rowContent += "Stow, HPD: ";
                                    break;
                                case 4:
                                    rowContent += "Stow, IPD: ";
                                    break;
                                case 5:
                                    rowContent += "Tempe: ";
                                    break;
                            }
                            rowContent += (dataGridView2.SelectedRows[0].Cells[i].Value);
                            rowContent += ("\r\n");
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < dataGridView2.Columns.Count; i++)
                    {
                        if (dataGridView2.SelectedRows[0].Cells[i].Value != "")
                        {
                            rowContent += (dataGridView2.SelectedRows[0].Cells[i].Value);
                            rowContent += ("\r\n");
                        }
                    }
                }
                
                //sets textbox to top of entry
                //selectedTextBox.SelectionStart = 0;
                //selectedTextBox.ScrollToCaret();
                MessageBox.Show(rowContent);
            }
        }

        private void editMode_CheckedChanged(object sender, EventArgs e)
        {
            if (editMode.Checked == true)
            {
                dataGridView1.ReadOnly = false;
                if (Program.Admin == 1)
                {
                    dataGridView2.ReadOnly = false;
                }
            }
            else
            {
                dataGridView1.ReadOnly = true;
                dataGridView2.ReadOnly = true;
            }
        }
        //close insert panel button
        private void button2_Click(object sender, EventArgs e)
        {
            insertPanel.Visible = false;
            flowLayoutPanel1.Controls.Clear();

        }
        //insert panel add row button also saves
        private void addRowBtn_Click(object sender, EventArgs e)
        {
            int iRow = dataGridView1.Rows.Count - 1;
            //int iCol = 1;
            BindingSource bs = new BindingSource();
            bs.DataSource = dataGridView1.DataSource;
            bs.AddNew();
            int i = 1;
            
            DataGridViewCell oCell;
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            foreach (Control c in flowLayoutPanel1.Controls)
            {
                try
                {
                    var t = c.Controls.Find(dataGridView1.Columns[i].Name.ToString() + "InsertValue", false);
                    var textbox = t[0];
                    //handle if the input is a checkbox
                    var ischeck = textbox as CheckBox;
                    if (ischeck != null)
                    {
                        if (ischeck.CheckState == CheckState.Checked)
                        {
                            textbox.Text = "true";
                        }
                        else
                        {
                            textbox.Text = "false";
                        }
                    }
                    if (textbox.Text == "" && textbox.Name != "OldIPAddressInsertValue")
                    {
                        MessageBox.Show("Must fill in all row values.");
                        this.dataGridView1.CancelEdit();
                        bs.CancelEdit();
                        changeFlag = 0;
                        return;
                    }
                    //Console.WriteLine(textbox.Text);
                    oCell = dataGridView1[i, iRow];
                    oCell.Value = Convert.ChangeType(textbox.Text, oCell.ValueType);
                    //dataGridView1.Rows[iRow].Cells[i].Value = textbox.Text;
                    i++;
                }
                catch
                {

                }  
            }
            dataGridView1.DataSource = bs;
            
            //saving
            Save();
            insertPanel.Visible = true;
        }
        //update row button.
        private void updateButton_Click(object sender, EventArgs e)
        {
            int i = 1;
            int iRow = dataGridView1.SelectedRows[0].Index;
            DataGridViewCell oCell;
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            foreach (Control c in flowLayoutPanel1.Controls)
            {
                try
                {
                    var t = c.Controls.Find(dataGridView1.Columns[i].Name.ToString() + "InsertValue", false);
                    var textbox = t[0];
                    //handle if the input is a checkbox
                    var ischeck = textbox as CheckBox;
                    if (ischeck != null)
                    {
                        if (ischeck.CheckState == CheckState.Checked)
                        {
                            textbox.Text = "true";
                        }
                        else
                        {
                            textbox.Text = "false";
                        }
                    }
                    if (textbox.Text == "")
                    {
                        MessageBox.Show("Must fill in all row values.");
                        this.dataGridView1.CancelEdit();
                        changeFlag = 0;
                        return;
                    }
                    //Console.WriteLine(textbox.Text);
                    oCell = dataGridView1[i, iRow];
                    oCell.Value = Convert.ChangeType(textbox.Text, oCell.ValueType);
                    //dataGridView1.Rows[iRow].Cells[i].Value = textbox.Text;
                    i++;
                }
                catch
                {

                }
            }
            
            insertPanel.Visible = false;
            Save();
        }
        private void Save()
        {
            try
            {

                //changeFlag = 1;
                this.Validate();
                this.mobileDevicesBindingSource.EndEdit();
                this.devicesBindingSource1.EndEdit();
                this.dataPortsBindingSource1.EndEdit();
                this.passwordsBindingSource.EndEdit();
                this.pcBindingSource.EndEdit();
                this.serversBindingSource.EndEdit();
                this.devicesBindingSource.EndEdit();
                /*************************************/
                this.mobileDevicesTableAdapter.Update(this.itmasterDataSet2.mobileDevices);
                this.pcTableAdapter.Update(this.itmasterDataSet2.pc);
                this.serversTableAdapter.Update(this.itmasterDataSet2.servers);
                this.devicesTableAdapter.Update(this.itmasterDataSet2.devices);
                this.dataPortsTableAdapter.Update(this.itmasterDataSet2.dataPorts);
                this.passwordsTableAdapter.Update(this.itmasterDataSet2.passwords);
                this.devices_TableAdapter.Update(this.itmasterDataSet2._Devices_);
            }
            catch
            {

            }
            if (AddedTableDataSet != null)
            {
                try
                {
                    this.AddedTableDataAdapter.Update(this.AddedTableDataSet);
                }
                catch
                {

                }
            }
            changeFlag = 0;
        }
    }
}
