using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using CMS.Models;

namespace CMS
{
    public partial class CustomerDetailForm : Form
    {
        private Customer _customer;
        private bool _isEditMode;
        private Label lblName, lblEmail, lblPhone, lblAddress;
        private TextBox txtName, txtEmail, txtPhone, txtAddress;
        private Button btnSave, btnCancel;
        private IContainer components = null;

        public CustomerDetailForm()
        {
            _customer = new Customer();
            _isEditMode = false;
            InitializeComponent();
        }

        public CustomerDetailForm(Customer customer)
        {
            _customer = customer;
            _isEditMode = true;
            InitializeComponent();
            LoadCustomerData();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.SuspendLayout();

            // Form Properties
            this.Text = _isEditMode ? "Edit Customer" : "Add Customer";
            this.ClientSize = new Size(450, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            int yPos = 30;
            int labelWidth = 80;
            int textBoxWidth = 250;

            // Name
            this.lblName = new Label();
            this.lblName.Text = "Name:";
            this.lblName.Location = new Point(30, yPos);
            this.lblName.Size = new Size(labelWidth, 20);
            this.lblName.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(this.lblName);

            this.txtName = new TextBox();
            this.txtName.Location = new Point(120, yPos);
            this.txtName.Size = new Size(textBoxWidth, 25);
            this.txtName.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(this.txtName);
            yPos += 40;

            // Email
            this.lblEmail = new Label();
            this.lblEmail.Text = "Email:";
            this.lblEmail.Location = new Point(30, yPos);
            this.lblEmail.Size = new Size(labelWidth, 20);
            this.lblEmail.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(this.lblEmail);

            this.txtEmail = new TextBox();
            this.txtEmail.Location = new Point(120, yPos);
            this.txtEmail.Size = new Size(textBoxWidth, 25);
            this.txtEmail.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(this.txtEmail);
            yPos += 40;

            // Phone
            this.lblPhone = new Label();
            this.lblPhone.Text = "Phone:";
            this.lblPhone.Location = new Point(30, yPos);
            this.lblPhone.Size = new Size(labelWidth, 20);
            this.lblPhone.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(this.lblPhone);

            this.txtPhone = new TextBox();
            this.txtPhone.Location = new Point(120, yPos);
            this.txtPhone.Size = new Size(textBoxWidth, 25);
            this.txtPhone.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(this.txtPhone);
            yPos += 40;

            // Address
            this.lblAddress = new Label();
            this.lblAddress.Text = "Address:";
            this.lblAddress.Location = new Point(30, yPos);
            this.lblAddress.Size = new Size(labelWidth, 20);
            this.lblAddress.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(this.lblAddress);

            this.txtAddress = new TextBox();
            this.txtAddress.Location = new Point(120, yPos);
            this.txtAddress.Size = new Size(textBoxWidth, 80);
            this.txtAddress.Multiline = true;
            this.txtAddress.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(this.txtAddress);
            yPos += 100;

            // Buttons
            this.btnSave = new Button();
            this.btnSave.Text = "Save";
            this.btnSave.Location = new Point(120, yPos);
            this.btnSave.Size = new Size(100, 35);
            this.btnSave.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            this.btnSave.BackColor = Color.FromArgb(0, 122, 204);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.Controls.Add(this.btnSave);

            this.btnCancel = new Button();
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Location = new Point(240, yPos);
            this.btnCancel.Size = new Size(100, 35);
            this.btnCancel.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.btnCancel.BackColor = Color.Gray;
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.Controls.Add(this.btnCancel);

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

        private void LoadCustomerData()
        {
            txtName.Text = _customer.Name;
            txtEmail.Text = _customer.Email;
            txtPhone.Text = _customer.Phone;
            txtAddress.Text = _customer.Address;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            string query;
            SqlParameter[] parameters;

            if (_isEditMode)
            {
                query = @"UPDATE Customers SET Name = @Name, Email = @Email, 
                         Phone = @Phone, Address = @Address WHERE CustomerID = @CustomerID";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", txtName.Text.Trim()),
                    new SqlParameter("@Email", txtEmail.Text.Trim()),
                    new SqlParameter("@Phone", txtPhone.Text.Trim()),
                    new SqlParameter("@Address", txtAddress.Text.Trim()),
                    new SqlParameter("@CustomerID", _customer.CustomerID)
                };
            }
            else
            {
                query = @"INSERT INTO Customers (Name, Email, Phone, Address, CreatedBy) 
                         VALUES (@Name, @Email, @Phone, @Address, @CreatedBy)";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", txtName.Text.Trim()),
                    new SqlParameter("@Email", txtEmail.Text.Trim()),
                    new SqlParameter("@Phone", txtPhone.Text.Trim()),
                    new SqlParameter("@Address", txtAddress.Text.Trim()),
                    new SqlParameter("@CreatedBy", LoginForm.CurrentUserID)
                };
            }

            int result = DatabaseHelper.ExecuteNonQuery(query, parameters);

            if (result > 0)
            {
                MessageBox.Show(_isEditMode ? "Customer updated successfully!" : "Customer added successfully!",
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
                MessageBox.Show("Please enter customer name.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (!string.IsNullOrEmpty(txtEmail.Text) && !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
