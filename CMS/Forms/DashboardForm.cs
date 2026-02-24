using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace CMS
{
    public partial class DashboardForm : Form
    {
        private MenuStrip menuStrip;
        private Label lblWelcome;
        private IContainer components;

        public DashboardForm()
        {
            InitializeComponent();
            lblWelcome.Text = $"Welcome, {LoginForm.CurrentUsername}!";
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.SuspendLayout();

            this.Text = "Dashboard - Customer Management System";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Menu Strip
            menuStrip = new MenuStrip();
            menuStrip.BackColor = Color.FromArgb(45, 45, 65);
            menuStrip.ForeColor = Color.White;

            ToolStripMenuItem customersMenu = new ToolStripMenuItem("Customers");
            customersMenu.ForeColor = Color.White;
            customersMenu.DropDownItems.Add("Manage Customers", null, ManageCustomers_Click);

            ToolStripMenuItem productsMenu = new ToolStripMenuItem("Products");
            productsMenu.ForeColor = Color.White;
            productsMenu.DropDownItems.Add("Manage Products", null, ManageProducts_Click);

            ToolStripMenuItem logoutMenu = new ToolStripMenuItem("Logout");
            logoutMenu.ForeColor = Color.White;
            logoutMenu.Click += Logout_Click;

            menuStrip.Items.AddRange(new ToolStripItem[] { customersMenu, productsMenu, logoutMenu });
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            // Welcome Label
            lblWelcome = new Label();
            lblWelcome.Text = "Welcome!";
            lblWelcome.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(0, 122, 204);
            lblWelcome.Location = new Point(50, 50);
            lblWelcome.Size = new Size(500, 50);
            this.Controls.Add(lblWelcome);

            // Dashboard Content
            Label lblDashboard = new Label();
            lblDashboard.Text = "Dashboard Overview";
            lblDashboard.Font = new Font("Segoe UI", 14, FontStyle.Regular);
            lblDashboard.ForeColor = Color.FromArgb(64, 64, 64);
            lblDashboard.Location = new Point(50, 120);
            lblDashboard.Size = new Size(300, 30);
            this.Controls.Add(lblDashboard);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ManageCustomers_Click(object sender, EventArgs e)
        {
            CustomerForm customerForm = new CustomerForm();
            customerForm.ShowDialog();
        }

        private void ManageProducts_Click(object sender, EventArgs e)
        {
            ProductForm productForm = new ProductForm();
            productForm.ShowDialog();
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout",
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
        }
    }
}