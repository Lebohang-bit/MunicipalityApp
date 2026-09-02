using System;
using System.Drawing;
using System.Windows.Forms;

namespace MunicipalityApp
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
            ApplyNavyWhiteTheme();
            DisableFutureFeatures();
        }

        private void ApplyNavyWhiteTheme()
        {
            this.BackColor = Color.White;
            this.ForeColor = Color.FromArgb(10, 25, 47);
            this.Font = new Font("Times New Roman", 12);
        }

        private void DisableFutureFeatures()
        {
            btnLocalEvents.Enabled = false;
            btnServiceStatus.Enabled = false;
            btnLocalEvents.BackColor = Color.LightGray;
            btnServiceStatus.BackColor = Color.LightGray;
            lblComingSoon1.Visible = true;
            lblComingSoon2.Visible = true;
        }

        private void btnReportIssues_Click(object sender, EventArgs e)
        {
            ReportIssuesForm reportForm = new ReportIssuesForm();
            reportForm.Show();
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.btnReportIssues = new Button();
            this.btnLocalEvents = new Button();
            this.btnServiceStatus = new Button();
            this.btnExit = new Button();
            this.lblComingSoon1 = new Label();
            this.lblComingSoon2 = new Label();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.Text = "MUNICIPAL SERVICES";
            this.lblTitle.Font = new Font("Times New Roman", 24, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(10, 25, 47);
            this.lblTitle.Location = new Point(120, 30);
            this.lblTitle.Size = new Size(400, 50);
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // btnReportIssues
            this.btnReportIssues.Text = "1. Report Issues";
            this.btnReportIssues.Size = new Size(250, 50);
            this.btnReportIssues.Location = new Point(195, 120);
            this.btnReportIssues.BackColor = Color.FromArgb(10, 25, 47);
            this.btnReportIssues.ForeColor = Color.White;
            this.btnReportIssues.FlatStyle = FlatStyle.Flat;
            this.btnReportIssues.Font = new Font("Times New Roman", 14);
            this.btnReportIssues.Click += new EventHandler(this.btnReportIssues_Click);

            // btnLocalEvents
            this.btnLocalEvents.Text = "2. Local Events (Coming Soon)";
            this.btnLocalEvents.Size = new Size(250, 50);
            this.btnLocalEvents.Location = new Point(195, 190);
            this.btnLocalEvents.BackColor = Color.LightGray;
            this.btnLocalEvents.ForeColor = Color.DarkGray;
            this.btnLocalEvents.FlatStyle = FlatStyle.Flat;
            this.btnLocalEvents.Font = new Font("Times New Roman", 12);
            this.btnLocalEvents.Enabled = false;

            // btnServiceStatus
            this.btnServiceStatus.Text = "3. Service Status (Coming Soon)";
            this.btnServiceStatus.Size = new Size(250, 50);
            this.btnServiceStatus.Location = new Point(195, 260);
            this.btnServiceStatus.BackColor = Color.LightGray;
            this.btnServiceStatus.ForeColor = Color.DarkGray;
            this.btnServiceStatus.FlatStyle = FlatStyle.Flat;
            this.btnServiceStatus.Font = new Font("Times New Roman", 12);
            this.btnServiceStatus.Enabled = false;

            // btnExit
            this.btnExit.Text = "Exit";
            this.btnExit.Size = new Size(100, 35);
            this.btnExit.Location = new Point(270, 350);
            this.btnExit.BackColor = Color.FromArgb(10, 25, 47);
            this.btnExit.ForeColor = Color.White;
            this.btnExit.FlatStyle = FlatStyle.Flat;
            this.btnExit.Font = new Font("Times New Roman", 12);
            this.btnExit.Click += new EventHandler(this.btnExit_Click);

            // lblComingSoon1
            this.lblComingSoon1.Text = "🔒 Disabled";
            this.lblComingSoon1.Location = new Point(460, 200);
            this.lblComingSoon1.Size = new Size(80, 25);
            this.lblComingSoon1.ForeColor = Color.Red;
            this.lblComingSoon1.Font = new Font("Arial", 9, FontStyle.Italic);

            // lblComingSoon2
            this.lblComingSoon2.Text = "🔒 Disabled";
            this.lblComingSoon2.Location = new Point(460, 270);
            this.lblComingSoon2.Size = new Size(80, 25);
            this.lblComingSoon2.ForeColor = Color.Red;
            this.lblComingSoon2.Font = new Font("Arial", 9, FontStyle.Italic);

            // MainMenuForm
            this.Text = "Municipal Services - Main Menu";
            this.Size = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnReportIssues);
            this.Controls.Add(this.btnLocalEvents);
            this.Controls.Add(this.btnServiceStatus);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblComingSoon1);
            this.Controls.Add(this.lblComingSoon2);
            this.ResumeLayout(false);
        }

        private Label lblTitle;
        private Button btnReportIssues;
        private Button btnLocalEvents;
        private Button btnServiceStatus;
        private Button btnExit;
        private Label lblComingSoon1;
        private Label lblComingSoon2;
    }
}