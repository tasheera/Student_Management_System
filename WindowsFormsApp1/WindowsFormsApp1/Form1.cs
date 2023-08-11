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

namespace WindowsFormsApp1
{
    public partial class Login_Form : System.Windows.Forms.Form
    {
        public Login_Form()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-8BTCQSS2;Initial Catalog=Student;Integrated Security=True"); // create connection with database
        private void button1_Click(object sender, EventArgs e)
        {
            con.Open(); // open connection
            string username = textBox1.Text;
            string password = textBox2.Text;
            string query_select = "Select * From Logins WHERE username='" + username + "' AND password ='" + password  + "'"; 
            SqlCommand cmnd = new SqlCommand(query_select, con); // create sql command to read data from database
            SqlDataReader row = cmnd.ExecuteReader();  // execute sql command 


            if (row.HasRows) // check login details 
           {
                Registration_Form rg = new Registration_Form();  // create object from registration foam class
                rg.Show(); // display student registration foam
                this.Hide(); // hide login foam
           }
         else
           {
              MessageBox.Show("Invalid Login credentails, please check Username and Password and try again ","Invalid login details", MessageBoxButtons.OK, MessageBoxIcon.Error ); // display invalid login details message
          }
            con.Close(); // close connection
        }

        private void button2_Click(object sender, EventArgs e) // clear button click function  
        {
            textBox1.Clear(); // clear 
            textBox2.Clear(); // clear
            textBox1.Focus(); // active username box
        }

        private void exit(object sender, EventArgs e) // exit button click function 
        {
            DialogResult result;
            result = MessageBox.Show("Are you sure, Do you really want to Exit...?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question); // display exit message yes/ no
            if (result == DialogResult.Yes) //check the answer 
            {
                Application.Exit(); // exit from application
            }
            else
            {
                this.Show(); // show the foam
            }
        }
    }
}
