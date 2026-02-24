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
using System.Xml.Linq;

namespace CIE2_CMS_Project
{
    public partial class Form4 : Form
    {

        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CIE2_CMS;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trusted_Connection = True";

        int selectedId = 0; 


        public Form4()
        {
            InitializeComponent();
        }

        // Load Data Method
        void LoadData()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Customers", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        // Clear Method
        void Clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            selectedId = 0;
        }


        // Veiw Button
        private void button1_Click(object sender, EventArgs e)
        {
            LoadData();
            Clear();
        }


        private void button7_Click(object sender, EventArgs e)
        {
            Form3 HomeScreen = new Form3();
            HomeScreen.Show();
            this.Hide();
        }

        // Add Button
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" ||
               (!radioButton1.Checked && !radioButton2.Checked))
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Customers (Name, Email, Phone, Address, Gender) VALUES (@Name, @Email, @Phone, @Address, @Gender)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                cmd.Parameters.AddWithValue("@Email", textBox2.Text);
                cmd.Parameters.AddWithValue("@Phone", textBox3.Text);
                cmd.Parameters.AddWithValue("@Address", textBox4.Text);
                cmd.Parameters.AddWithValue("@Gender", radioButton1.Checked ? "Male" : "Female");

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Record Added!");
                LoadData();
                Clear();
            }


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                selectedId = Convert.ToInt32(row.Cells["Id"].Value);
                textBox1.Text = row.Cells["Name"].Value.ToString();
                textBox2.Text = row.Cells["Email"].Value.ToString();
                textBox3.Text = row.Cells["Phone"].Value.ToString();
                textBox4.Text = row.Cells["Address"].Value.ToString();

                // Gender Radio Buttons
                string gender = row.Cells["Gender"].Value.ToString();
                if (gender == "Male")
                    radioButton1.Checked = true;
                else
                    radioButton2.Checked = true;
            }
        }

        // Update Button    
        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Please select a record to update!");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Customers SET Name=@Name, Email=@Email, Phone=@Phone, Address=@Address, Gender=@Gender WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                cmd.Parameters.AddWithValue("@Email", textBox2.Text);
                cmd.Parameters.AddWithValue("@Phone", textBox3.Text);
                cmd.Parameters.AddWithValue("@Address", textBox4.Text);
                cmd.Parameters.AddWithValue("@Gender", radioButton1.Checked ? "Male" : "Female");
                cmd.Parameters.AddWithValue("@Id", selectedId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Record Updated!");
                LoadData();
                Clear();
            }
        }

        // Delete Button
        private void button4_Click(object sender, EventArgs e)


        {
            if (selectedId == 0)
            {
                MessageBox.Show("Please select a record to delete!");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Customers WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", selectedId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Record Deleted!");
                LoadData();
                Clear();
            }
        }

        // FORM LOAD → SHOW DATA AUTOMATICALLY (optional)
        private void Customer_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
