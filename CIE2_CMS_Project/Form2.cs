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

namespace CIE2_CMS_Project
{
    public partial class Form2 : Form
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CIE2_CMS;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trusted_Connection = True";
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validation
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            if (textBox3.Text != textBox4.Text)
            {
                MessageBox.Show("Passwords do not match!");
                return;
            }

            string gender = "";
            if (radioButton1.Checked)
                gender = "Male";
            else if (radioButton2.Checked)
                gender = "Female";
            else
            {
                MessageBox.Show("Please select gender!");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email=@Email";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@Email", textBox1.Text);

                con.Open();
                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show("Email already registered!");
                    con.Close();
                    return;
                }

                string insertQuery = "INSERT INTO Users (Email, FullName, Password, Gender) VALUES (@Email, @FullName, @Password, @Gender)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                insertCmd.Parameters.AddWithValue("@Email", textBox1.Text);
                insertCmd.Parameters.AddWithValue("@FullName", textBox2.Text);
                insertCmd.Parameters.AddWithValue("@Password", textBox3.Text);  // Plain text (can hash later)
                insertCmd.Parameters.AddWithValue("@Gender", gender);

                insertCmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Registration Successful!");

                // Redirect to Login Form
                Form1 login = new Form1();
                login.Show();
                this.Hide();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Hide();
        }
    }
}
