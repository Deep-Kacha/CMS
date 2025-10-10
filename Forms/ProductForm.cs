using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using CMS.Models;

namespace CMS
{
    public partial class ProductForm : Form
    {
        private DataGridView dgvProducts;
        private Button btnAdd, btnEdit, btnDelete, btnRefresh, btnSearch;
        private TextBox txtSearch;
        private Label lblSearch;
        private IContainer components;

        public ProductForm()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.SuspendLayout();

            this.Text = "Product Management";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "Products Management";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 122, 204);
            lblTitle.Location = new Point(20, 15);
            lblTitle.Size = new Size(300, 30);
            this.Controls.Add(lblTitle);

            // Search Label
            lblSearch = new Label();
            lblSearch.Text = "Search:";
            lblSearch.Location = new Point(20, 60);
            lblSearch.Size = new Size(50, 20);
            lblSearch.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(lblSearch);

            // Search Box
            txtSearch = new TextBox();
            txtSearch.Location = new Point(70, 58);
            txtSearch.Size = new Size(150, 25);
            txtSearch.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(txtSearch);

            btnSearch = new Button();
            btnSearch.Text = "Search";
            btnSearch.Location = new Point(230, 58);
            btnSearch.Size = new Size(80, 25);
            btnSearch.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            btnSearch.BackColor = Color.FromArgb(0, 122, 204);
            btnSearch.ForeColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Click += BtnSearch_Click;
            this.Controls.Add(btnSearch);

            // Buttons
            btnAdd = new Button();
            btnAdd.Text = "Add Product";
            btnAdd.Location = new Point(330, 55);
            btnAdd.Size = new Size(100, 30);
            btnAdd.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            btnAdd.BackColor = Color.FromArgb(40, 167, 69);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            btnEdit = new Button();
            btnEdit.Text = "Edit Product";
            btnEdit.Location = new Point(440, 55);
            btnEdit.Size = new Size(100, 30);
            btnEdit.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            btnEdit.BackColor = Color.FromArgb(0, 123, 255);
            btnEdit.ForeColor = Color.White;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Click += BtnEdit_Click;
            this.Controls.Add(btnEdit);

            btnDelete = new Button();
            btnDelete.Text = "Delete Product";
            btnDelete.Location = new Point(550, 55);
            btnDelete.Size = new Size(100, 30);
            btnDelete.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            btnDelete.BackColor = Color.FromArgb(220, 53, 69);
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Click += BtnDelete_Click;
            this.Controls.Add(btnDelete);

            btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Location = new Point(660, 55);
            btnRefresh.Size = new Size(80, 30);
            btnRefresh.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Click += BtnRefresh_Click;
            this.Controls.Add(btnRefresh);

            // DataGridView
            dgvProducts = new DataGridView();
            dgvProducts.Location = new Point(20, 100);
            dgvProducts.Size = new Size(850, 400);
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.ReadOnly = true;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.BackgroundColor = Color.White;
            dgvProducts.BorderStyle = BorderStyle.Fixed3D;
            dgvProducts.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(dgvProducts);

            this.ResumeLayout(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void LoadProducts(string searchTerm = "")
        {
            string query = "SELECT ProductID, Name, Price, Quantity, Description FROM Products";

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query += " WHERE Name LIKE @SearchTerm OR Description LIKE @SearchTerm";
            }

            query += " ORDER BY Name";

            SqlParameter[] parameters = null;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                parameters = new SqlParameter[] {
                    new SqlParameter("@SearchTerm", $"%{searchTerm}%")
                };
            }

            DataTable products = DatabaseHelper.ExecuteQuery(query, parameters);
            dgvProducts.DataSource = products;

            if (dgvProducts.Columns.Contains("Price"))
            {
                dgvProducts.Columns["Price"].DefaultCellStyle.Format = "C2";
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (ProductDetailForm detailForm = new ProductDetailForm())
            {
                if (detailForm.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to edit.", "Information",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow row = dgvProducts.SelectedRows[0];
            Product product = new Product
            {
                ProductID = Convert.ToInt32(row.Cells["ProductID"].Value),
                Name = row.Cells["Name"].Value?.ToString() ?? "",
                Price = Convert.ToDecimal(row.Cells["Price"].Value),
                Quantity = Convert.ToInt32(row.Cells["Quantity"].Value),
                Description = row.Cells["Description"].Value?.ToString() ?? ""
            };

            using (ProductDetailForm detailForm = new ProductDetailForm(product))
            {
                if (detailForm.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to delete.", "Information",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow row = dgvProducts.SelectedRows[0];
            int productID = Convert.ToInt32(row.Cells["ProductID"].Value);
            string productName = row.Cells["Name"].Value?.ToString() ?? "";

            DialogResult result = MessageBox.Show($"Are you sure you want to delete product '{productName}'?",
                                                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM Products WHERE ProductID = @ProductID";
                SqlParameter[] parameters = { new SqlParameter("@ProductID", productID) };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Product deleted successfully.", "Success",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                }
                else
                {
                    MessageBox.Show("Failed to delete product.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadProducts();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadProducts(txtSearch.Text.Trim());
        }
    }
}