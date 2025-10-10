using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace CMS
{
    public partial class LoginForm : Form
    {
        public static int CurrentUserID { get; private set; }
        public static string CurrentUsername { get; private set; }

        private Label lblUsername, lblPassword, lblError;
        private TextBox txtUsername, txtPassword;
        private Button btnLogin, btnRegister;
        private IContainer components;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.SuspendLayout();

            this.Text = "Customer Management System - Login";
            this.Size = new Size(450, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "CMS Login";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 122, 204);
            lblTitle.Location = new Point(125, 40);
            lblTitle.Size = new Size(200, 40);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitle);

            // Username
            lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.Location = new Point(75, 120);
            lblUsername.Size = new Size(100, 20);
            lblUsername.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            this.Controls.Add(lblUsername);

            txtUsername = new TextBox();
            txtUsername.Location = new Point(175, 118);
            txtUsername.Size = new Size(200, 25);
            txtUsername.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            this.Controls.Add(txtUsername);

            // Password
            lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.Location = new Point(75, 170);
            lblPassword.Size = new Size(100, 20);
            lblPassword.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            this.Controls.Add(lblPassword);

            txtPassword = new TextBox();
            txtPassword.Location = new Point(175, 168);
            txtPassword.Size = new Size(200, 25);
            txtPassword.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            txtPassword.PasswordChar = '•';
            this.Controls.Add(txtPassword);

            // Login Button
            btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Location = new Point(175, 220);
            btnLogin.Size = new Size(200, 35);
            btnLogin.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnLogin.BackColor = Color.FromArgb(0, 122, 204);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Click += btnLogin_Click;
            this.Controls.Add(btnLogin);

            // Register Button
            btnRegister = new Button();
            btnRegister.Text = "Create New Account";
            btnRegister.Location = new Point(175, 270);
            btnRegister.Size = new Size(200, 30);
            btnRegister.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            btnRegister.BackColor = Color.Transparent;
            btnRegister.ForeColor = Color.FromArgb(0, 122, 204);
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += btnRegister_Click;
            this.Controls.Add(btnRegister);

            // Error Label
            lblError = new Label();
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(75, 310);
            lblError.Size = new Size(300, 30);
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            string query = "SELECT UserID, Username FROM Users WHERE Username = @Username AND Password = @Password";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", txtUsername.Text.Trim()),
                new SqlParameter("@Password", txtPassword.Text)
            };

            DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

            if (result.Rows.Count > 0)
            {
                CurrentUserID = Convert.ToInt32(result.Rows[0]["UserID"]);
                CurrentUsername = result.Rows[0]["Username"].ToString();

                DashboardForm dashboard = new DashboardForm();
                dashboard.Show();
                this.Hide();
            }
            else
            {
                ShowError("Invalid username or password.");
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowError("Please enter username.");
                txtUsername.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowError("Please enter password.");
                txtPassword.Focus();
                return false;
            }

            return true;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }
    }
}