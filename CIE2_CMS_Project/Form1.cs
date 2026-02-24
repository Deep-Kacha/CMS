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
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CIE2_CMS;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trusted_Connection = True";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Validation
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Please enter Email and Password!");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Email=@Email AND Password=@Password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", textBox1.Text);
                cmd.Parameters.AddWithValue("@Password", textBox2.Text);

                con.Open();
                int count = (int)cmd.ExecuteScalar();
                con.Close();

                if (count == 1)
                {
                    MessageBox.Show("Login Successful!");

                    // Open HomeScreen Form here
                    Form3 HomeScreen = new Form3();
                    HomeScreen.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid Email or Password!");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 signup = new Form2();
            signup.Show();
            this.Hide();
        }
    }
}
