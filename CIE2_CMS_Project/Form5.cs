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
    public partial class Form5 : Form
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CIE2_CMS;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trusted_Connection = True";
        int selectedId = 0;
        public Form5()
        {
            InitializeComponent();
        }


        private void button7_Click(object sender, EventArgs e)
        {
            Form3 HomeScreen = new Form3();
            HomeScreen.Show();
            this.Hide();
        }

        // Veiw Button
        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Products";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        // Add Button
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Products (Name, Price, Quantity, Category, Description) VALUES (@Name, @Price, @Quantity, @Category, @Description)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                cmd.Parameters.AddWithValue("@Price", decimal.Parse(textBox2.Text));
                cmd.Parameters.AddWithValue("@Quantity", int.Parse(textBox3.Text));
                cmd.Parameters.AddWithValue("@Category", comboBox1.Text);
                cmd.Parameters.AddWithValue("@Description", textBox4.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Product Added Successfully!");
                ClearFields();
                button1_Click(sender, e);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedId = Convert.ToInt32(row.Cells["Id"].Value);
                textBox1.Text = row.Cells["Name"].Value.ToString();
                textBox2.Text = row.Cells["Price"].Value.ToString();
                textBox3.Text = row.Cells["Quantity"].Value.ToString();
                comboBox1.Text = row.Cells["Category"].Value.ToString();
                textBox4.Text = row.Cells["Description"].Value.ToString();
            }
        }


        // Update Button
        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Please select a product to update!");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Products SET Name=@Name, Price=@Price, Quantity=@Quantity, Category=@Category, Description=@Description WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", selectedId);
                cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                cmd.Parameters.AddWithValue("@Price", decimal.Parse(textBox2.Text));
                cmd.Parameters.AddWithValue("@Quantity", int.Parse(textBox3.Text));
                cmd.Parameters.AddWithValue("@Category", comboBox1.Text);
                cmd.Parameters.AddWithValue("@Description", textBox4.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Product Updated Successfully!");
                ClearFields();
                button1_Click(sender, e);
            }
        }


        // Delete Button
        private void button4_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Please select a product to delete!");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Products WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", selectedId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Product Deleted Successfully!");
                ClearFields();
                button1_Click(sender, e);
            }
        }

        // CLEAR INPUT FIELDS
        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
            textBox4.Clear();
            selectedId = 0;
        }
    }
}
