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
using System.Reflection;

namespace WindowsFormsApp1
{
    public partial class Registration_Form : System.Windows.Forms.Form
    {
        public Registration_Form()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-8BTCQSS2;Initial Catalog=Student;Integrated Security=True"); // create connection with database

        private void reg_Click(object sender, EventArgs e) // register button click function 
        {
            try
            {
                string firstName = txtFname.Text;
                string lastName = txtLname.Text;

                string gender;
                if (rbMale.Checked)
                {
                    gender = "male";
                }
                else
                {
                    gender = "Female";
                }

                string address = txtAddress.Text;
                string email = txtEmail.Text;

                int mobilePhone = int.Parse(txtMobile.Text);
                int homePhone = int.Parse(txtHphone.Text);
                int parentContact = int.Parse(contactTxt.Text);
                string parentName = parentTxt.Text;
                string nic = nicTxt.Text;
                
                DateTime dateTime = dateTimePicker1.Value;

                string insertQuery = "insert into Registration values('" + firstName + "','" + lastName + "','" + dateTime + "','" + gender + "','" + address + "','" + email + "','" + mobilePhone + "','" + homePhone + "','" + parentName + "','" + nic + "','" + parentContact + "')";
                if (checkData())
                {
                    con.Open();  // open connection
                    SqlCommand cmnd = new SqlCommand(insertQuery, con); // create sql command to insert data to database
                    cmnd.ExecuteNonQuery(); // execute sql command 
                    MessageBox.Show("Record added Susceessfully", "Register Student", MessageBoxButtons.OK, MessageBoxIcon.Information); // display record added message 
                    con.Close(); // close connection
                    cmbReg.Items.Clear(); // clear items in combo box
                    Registration_Form_Load(sender, e); // call function to add reg numbers to combox items

                }
            }
            catch (SqlException ex) // error handele 
            {
                string message = "Insert Error";
                message += ex.Message; 
            }
        }

        private void update_Click(object sender, EventArgs e) // update button click function
        {
            try
            {
                string no = cmbReg.Text;
            if (no!= "New Register")
            {
                string firstName = txtFname.Text;
                string lastName = txtLname.Text;

                string gender;
                if (rbMale.Checked)
                {
                    gender = "male";
                }
                else
                {
                    gender = "Female";
                }

                string address = txtAddress.Text;
                string email = txtEmail.Text;
                int mobilePhone = int.Parse(txtMobile.Text);
                int homePhone = int.Parse(txtHphone.Text);
                string parentName = parentTxt.Text;
                string nic = nicTxt.Text;
                int parentContact = int.Parse(contactTxt.Text);
                DateTime dateTime = dateTimePicker1.Value;

               

                string query_update = $"Update Registration SET firstName= '{firstName}', lastName= '{lastName}', dateOfBirth= '{dateTime}', gender= '{gender}', address= '{address}', email= '{email}', mobilePhone= '{mobilePhone}', homePhone= '{homePhone}', parentName= '{parentName}',nic= '{nic}', contactNo= '{parentContact}' where regNo= '{no}'";

                    if (checkData())
                    {


                        con.Open(); // open connection
                        SqlCommand cmnd = new SqlCommand(query_update, con); // create sql command to update records 
                        cmnd.ExecuteNonQuery(); // execute sql command 
                        con.Close(); // close connection
                        MessageBox.Show("Record Updated Susceessfully", "Update Student", MessageBoxButtons.OK, MessageBoxIcon.Information); // display record added message 
                    }
                }
            }
            catch (SqlException ex)
            {
                string message = "Update Error";
                message += ex.Message;
            }

        }

        private void cmbReg_SelectedIndexChanged(object sender, EventArgs e) // change index function 
        {
            string no = cmbReg.Text;
            if (no != "New Register")
            {
                con.Open();
                string query_select = "select * from Registration where regNo="+no;
                SqlCommand cmd = new SqlCommand(query_select, con);
                SqlDataReader row = cmd.ExecuteReader(); // execute sql command to read data
                while (row.Read()) // check data have or not ?
                {
                    // add data text boxes in foam

                    txtFname.Text = row[1].ToString();
                    txtLname.Text = row[2].ToString();

                    dateTimePicker1.Text = row[3].ToString();
 
                    if (row[4].ToString() == "Male")
                    {
                        rbMale.Checked = false;
                        rbFemale.Checked = true;
                    }
                    else
                    {
                        rbMale.Checked = true;
                        rbFemale.Checked = false;
                    }

                    txtAddress.Text = row[5].ToString();
                    txtEmail.Text = row[6].ToString();
                    txtMobile.Text = row[7].ToString();
                    txtHphone.Text= row[8].ToString();
                    parentTxt.Text= row[9].ToString();
                    nicTxt.Text= row[10].ToString();
                    contactTxt.Text= row[11].ToString();
                                       
                }
                con.Close();
                reg.Enabled = false; // disable register button
                update.Enabled = true; // enable update button
                del.Enabled = true; //enable delete button
            }
            else
            {
                clearData(); // call function
            }
        }

        private void Registration_Form_Load(object sender, EventArgs e)
        {
            con.Open();
            string query_select = "select * from Registration";
            SqlCommand cmnd = new SqlCommand(query_select, con);
            SqlDataReader row = cmnd.ExecuteReader();
            cmbReg.Items.Add("New Register");
            while (row.Read())
            {
                cmbReg.Items.Add(row[0]).ToString(); 
            }
            con.Close();
        }

        private void clearData() // create function to clear data
        {
            txtFname.Clear();
            txtLname.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtMobile.Clear();
            txtHphone.Clear();
            parentTxt.Clear();
            nicTxt.Clear();
            contactTxt.Clear();
            dateTimePicker1.ResetText();
            update.Enabled = false;
            del.Enabled = false; ;
            reg.Enabled = true;
        }

        private void clear_Click(object sender, EventArgs e)
        {
            clearData(); // call function
            cmbReg.Text = "";
        }

        private void del_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Are you sure,Do you really want to Delete this Record...?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string no = cmbReg.Text;
                    string query_delete = "delete from Registration where regNo=" + no + "";
                    con.Open();
                    SqlCommand cmnd = new SqlCommand(query_delete, con);
                    cmnd.ExecuteNonQuery();
                    con.Close();
                    cmbReg.Items.RemoveAt(cmbReg.SelectedIndex);
                    MessageBox.Show("Record Dleted Sucessfully", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }

                else if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            catch (SqlException ex)
            {
                string message = "Delete Error";
                message += ex.Message;
                con.Close();
            }
        }

        private void logout_Click(object sender, EventArgs e)
        {
            Login_Form obj = new Login_Form();
            obj.Show();
            this.Close();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("Are you sure, Do you really want to Exit...?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                this.Show();
            }
        }

        private void checkNumbers(object sender, KeyPressEventArgs e) // create function to text boxes can only enter numbers 
        {
            if (!char.IsControl(e.KeyChar)&& !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private Boolean checkData() // create function to check data filled or not in foam
        {
            Boolean no = cmbReg.Text == "";
            Boolean firstName = txtFname.Text=="";
            Boolean lastName = txtLname.Text=="";
            Boolean address = txtAddress.Text=="";
            Boolean email = txtEmail.Text=="";
            Boolean mobilePhone = txtMobile.Text=="";
            Boolean homePhone = txtHphone.Text=="";
            Boolean parentContact = contactTxt.Text=="";
            Boolean parentName = parentTxt.Text=="";
            Boolean nic = nicTxt.Text=="";

            if (firstName || lastName || lastName || address || email || mobilePhone || homePhone || parentContact || parentContact || parentName || nic || no)
            {
                return false;
            }
            else
            {
                return true;
            }         
                       
        }
    }
}
