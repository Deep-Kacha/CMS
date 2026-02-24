using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using CMS.Models;

namespace CMS
{
    public partial class ProductDetailForm : Form
    {
        private Product _product;
        private bool _isEditMode;
        private Label lblName, lblPrice, lblQuantity, lblDescription;
        private TextBox txtName, txtPrice, txtQuantity, txtDescription;
        private Button btnSave, btnCancel;
        private IContainer components;

        public ProductDetailForm()
        {
            _product = new Product();
            _isEditMode = false;
            InitializeComponent();
        }

        public ProductDetailForm(Product product)
        {
            _product = product;
            _isEditMode = true;
            InitializeComponent();
            LoadProductData();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.SuspendLayout();

            this.Text = _isEditMode ? "Edit Product" : "Add Product";
            this.Size = new Size(450, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            int yPos = 30;
            int labelWidth = 80;
            int textBoxWidth = 250;

            // Name
            lblName = new Label();
            lblName.Text = "Name:";
            lblName.Location = new Point(30, yPos);
            lblName.Size = new Size(labelWidth, 20);
            lblName.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(lblName);

            txtName = new TextBox();
            txtName.Location = new Point(120, yPos);
            txtName.Size = new Size(textBoxWidth, 25);
            txtName.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(txtName);
            yPos += 40;

            // Price
            lblPrice = new Label();
            lblPrice.Text = "Price:";
            lblPrice.Location = new Point(30, yPos);
            lblPrice.Size = new Size(labelWidth, 20);
            lblPrice.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(lblPrice);

            txtPrice = new TextBox();
            txtPrice.Location = new Point(120, yPos);
            txtPrice.Size = new Size(textBoxWidth, 25);
            txtPrice.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(txtPrice);
            yPos += 40;

            // Quantity
            lblQuantity = new Label();
            lblQuantity.Text = "Quantity:";
            lblQuantity.Location = new Point(30, yPos);
            lblQuantity.Size = new Size(labelWidth, 20);
            lblQuantity.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(lblQuantity);

            txtQuantity = new TextBox();
            txtQuantity.Location = new Point(120, yPos);
            txtQuantity.Size = new Size(textBoxWidth, 25);
            txtQuantity.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(txtQuantity);
            yPos += 40;

            // Description
            lblDescription = new Label();
            lblDescription.Text = "Description:";
            lblDescription.Location = new Point(30, yPos);
            lblDescription.Size = new Size(labelWidth, 20);
            lblDescription.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(lblDescription);

            txtDescription = new TextBox();
            txtDescription.Location = new Point(120, yPos);
            txtDescription.Size = new Size(textBoxWidth, 80);
            txtDescription.Multiline = true;
            txtDescription.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(txtDescription);
            yPos += 100;

            // Buttons
            btnSave = new Button();
            btnSave.Text = "Save";
            btnSave.Location = new Point(120, yPos);
            btnSave.Size = new Size(100, 35);
            btnSave.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnSave.BackColor = Color.FromArgb(0, 122, 204);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Click += btnSave_Click;
            this.Controls.Add(btnSave);

            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(240, yPos);
            btnCancel.Size = new Size(100, 35);
            btnCancel.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            btnCancel.BackColor = Color.Gray;
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Click += btnCancel_Click;
            this.Controls.Add(btnCancel);

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

        private void LoadProductData()
        {
            txtName.Text = _product.Name;
            txtPrice.Text = _product.Price.ToString();
            txtQuantity.Text = _product.Quantity.ToString();
            txtDescription.Text = _product.Description;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            string query;
            SqlParameter[] parameters;

            if (_isEditMode)
            {
                query = @"UPDATE Products SET Name = @Name, Price = @Price, 
                         Quantity = @Quantity, Description = @Description WHERE ProductID = @ProductID";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", txtName.Text.Trim()),
                    new SqlParameter("@Price", decimal.Parse(txtPrice.Text)),
                    new SqlParameter("@Quantity", int.Parse(txtQuantity.Text)),
                    new SqlParameter("@Description", txtDescription.Text.Trim()),
                    new SqlParameter("@ProductID", _product.ProductID)
                };
            }
            else
            {
                query = @"INSERT INTO Products (Name, Price, Quantity, Description, CreatedBy) 
                         VALUES (@Name, @Price, @Quantity, @Description, @CreatedBy)";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", txtName.Text.Trim()),
                    new SqlParameter("@Price", decimal.Parse(txtPrice.Text)),
                    new SqlParameter("@Quantity", int.Parse(txtQuantity.Text)),
                    new SqlParameter("@Description", txtDescription.Text.Trim()),
                    new SqlParameter("@CreatedBy", LoginForm.CurrentUserID)
                };
            }

            int result = DatabaseHelper.ExecuteNonQuery(query, parameters);

            if (result > 0)
            {
                MessageBox.Show(_isEditMode ? "Product updated successfully!" : "Product added successfully!",
                              "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Operation failed. Please try again.", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter product name.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal priceValue) || priceValue < 0)
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantityValue) || quantityValue < 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}