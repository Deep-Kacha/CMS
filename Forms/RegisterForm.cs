using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace CMS
{
    public partial class RegisterForm : Form
    {
        private Label lblUsername, lblPassword, lblConfirmPassword, lblEmail, lblFirstName, lblLastName, lblError;
        private TextBox txtUsername, txtPassword, txtConfirmPassword, txtEmail, txtFirstName, txtLastName;
        private Button btnRegister, btnCancel;
        private IContainer components;

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.SuspendLayout();

            this.Text = "Register New Account";
            this.Size = new Size(450, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "Create New Account";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 122, 204);
            lblTitle.Location = new Point(100, 20);
            lblTitle.Size = new Size(250, 40);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitle);

            int yPos = 80;
            int labelWidth = 120;
            int textBoxWidth = 200;

            // First Name
            lblFirstName = new Label();
            lblFirstName.Text = "First Name:";
            lblFirstName.Location = new Point(50, yPos);
            lblFirstName.Size = new Size(labelWidth, 20);
            lblFirstName.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(lblFirstName);

            txtFirstName = new TextBox();
            txtFirstName.Location = new Point(180, yPos);
            txtFirstName.Size = new Size(textBoxWidth, 25);
            txtFirstName.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(txtFirstName);
            yPos += 40;

            // Last Name
            lblLastName = new Label();
            lblLastName.Text = "Last Name:";
            lblLastName.Location = new Point(50, yPos);
            lblLastName.Size = new Size(labelWidth, 20);
            lblLastName.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(lblLastName);

            txtLastName = new TextBox();
            txtLastName.Location = new Point(180, yPos);
            txtLastName.Size = new Size(textBoxWidth, 25);
            txtLastName.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(txtLastName);
            yPos += 40;

            // Email
            lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Location = new Point(50, yPos);
            lblEmail.Size = new Size(labelWidth, 20);
            lblEmail.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(lblEmail);

            txtEmail = new TextBox();
            txtEmail.Location = new Point(180, yPos);
            txtEmail.Size = new Size(textBoxWidth, 25);
            txtEmail.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(txtEmail);
            yPos += 40;

            // Username
            lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.Location = new Point(50, yPos);
            lblUsername.Size = new Size(labelWidth, 20);
            lblUsername.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(lblUsername);

            txtUsername = new TextBox();
            txtUsername.Location = new Point(180, yPos);
            txtUsername.Size = new Size(textBoxWidth, 25);
            txtUsername.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(txtUsername);
            yPos += 40;

            // Password
            lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.Location = new Point(50, yPos);
            lblPassword.Size = new Size(labelWidth, 20);
            lblPassword.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(lblPassword);

            txtPassword = new TextBox();
            txtPassword.Location = new Point(180, yPos);
            txtPassword.Size = new Size(textBoxWidth, 25);
            txtPassword.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            txtPassword.PasswordChar = '•';
            this.Controls.Add(txtPassword);
            yPos += 40;

            // Confirm Password
            lblConfirmPassword = new Label();
            lblConfirmPassword.Text = "Confirm Password:";
            lblConfirmPassword.Location = new Point(50, yPos);
            lblConfirmPassword.Size = new Size(labelWidth, 20);
            lblConfirmPassword.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(lblConfirmPassword);

            txtConfirmPassword = new TextBox();
            txtConfirmPassword.Location = new Point(180, yPos);
            txtConfirmPassword.Size = new Size(textBoxWidth, 25);
            txtConfirmPassword.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            txtConfirmPassword.PasswordChar = '•';
            this.Controls.Add(txtConfirmPassword);
            yPos += 50;

            // Buttons
            btnRegister = new Button();
            btnRegister.Text = "Register";
            btnRegister.Location = new Point(100, yPos);
            btnRegister.Size = new Size(120, 35);
            btnRegister.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnRegister.BackColor = Color.FromArgb(40, 167, 69);
            btnRegister.ForeColor = Color.White;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Click += btnRegister_Click;
            this.Controls.Add(btnRegister);

            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(240, yPos);
            btnCancel.Size = new Size(120, 35);
            btnCancel.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            btnCancel.BackColor = Color.Gray;
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Click += btnCancel_Click;
            this.Controls.Add(btnCancel);

            yPos += 50;

            // Error Label
            lblError = new Label();
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(50, yPos);
            lblError.Size = new Size(350, 40);
            lblError.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblError.TextAlign = ContentAlignment.MiddleCenter;
            lblError.Visible = false;
            this.Controls.Add(lblError);

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

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            string query = "INSERT INTO Users (Username, Password, Email, FirstName, LastName) " +
                          "VALUES (@Username, @Password, @Email, @FirstName, @LastName)";

            SqlParameter[] parameters = {
                new SqlParameter("@Username", txtUsername.Text.Trim()),
                new SqlParameter("@Password", txtPassword.Text),
                new SqlParameter("@Email", txtEmail.Text.Trim()),
                new SqlParameter("@FirstName", txtFirstName.Text.Trim()),
                new SqlParameter("@LastName", txtLastName.Text.Trim())
            };

            int result = DatabaseHelper.ExecuteNonQuery(query, parameters);

            if (result > 0)
            {
                MessageBox.Show("Registration successful! Please login.", "Success",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                ShowError("Registration failed. Please try again.");
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowError("All fields are required.");
                return false;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                ShowError("Passwords do not match.");
                return false;
            }

            if (txtPassword.Text.Length < 6)
            {
                ShowError("Password must be at least 6 characters long.");
                return false;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                ShowError("Please enter a valid email address.");
                return false;
            }

            string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            SqlParameter[] checkParams = { new SqlParameter("@Username", txtUsername.Text.Trim()) };
            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(checkQuery, checkParams));

            if (count > 0)
            {
                ShowError("Username already exists. Please choose a different one.");
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

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}