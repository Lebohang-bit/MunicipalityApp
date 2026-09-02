using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MunicipalityApp
{
    public partial class ReportIssuesForm : Form
    {
        // Data structure to store issues - using List
        private List<IssueReport> issueList = new List<IssueReport>();
        private string attachedFilePath = "";
        private int progressValue = 0;

        public ReportIssuesForm()
        {
            InitializeComponent();
            ApplyNavyWhiteTheme();
            PopulateCategories();
            SetupProgressBar();
            UpdateEncouragementMessage(0);
        }

        private void ApplyNavyWhiteTheme()
        {
            this.BackColor = Color.White;
            this.ForeColor = Color.FromArgb(10, 25, 47);
            this.Font = new Font("Times New Roman", 12);
        }

        private void PopulateCategories()
        {
            cmbCategory.Items.AddRange(new string[] {
                "Sanitation",
                "Roads",
                "Utilities (Water/Electricity)",
                "Street Lighting",
                "Public Transport",
                "Parks and Recreation",
                "Waste Management",
                "Other"
            });
            cmbCategory.SelectedIndex = 0;
        }

        private void SetupProgressBar()
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            progressBar1.Step = 20;
            progressBar1.ForeColor = Color.FromArgb(10, 25, 47);
        }

        private void UpdateEncouragementMessage(int step)
        {
            string[] messages = {
                "📝 Start by telling us where the issue is...",
                "📍 Great! Now select the category.",
                "✍️ Describe the issue in detail - you're helping your community!",
                "📎 Add a photo or document if you have one.",
                "🌟 Almost done! Your voice matters.",
                "✅ Thank you! Your report is being submitted..."
            };

            int index = Math.Min(step, messages.Length - 1);
            lblEngagement.Text = messages[index];
            lblEngagement.ForeColor = Color.FromArgb(10, 25, 47);
            lblEngagement.Font = new Font("Times New Roman", 11, FontStyle.Italic);
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Document Files|*.pdf;*.doc;*.docx;*.txt|All Files|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    attachedFilePath = openFileDialog.FileName;
                    lblAttachmentStatus.Text = $"Attached: {Path.GetFileName(attachedFilePath)}";
                    lblAttachmentStatus.ForeColor = Color.Green;
                    progressValue = Math.Min(progressValue + 20, 100);
                    progressBar1.Value = progressValue;
                    UpdateEncouragementMessage(progressValue / 20);
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show("Please enter the location of the issue.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtLocation.Focus();
                return;
            }

            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category for the issue.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(rtbDescription.Text))
            {
                MessageBox.Show("Please provide a description of the issue.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                rtbDescription.Focus();
                return;
            }

            // Complete progress
            progressValue = 100;
            progressBar1.Value = 100;
            UpdateEncouragementMessage(5);

            // Create and store report using IssueReport data structure
            IssueReport report = new IssueReport
            {
                ReportID = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                Location = txtLocation.Text,
                Category = cmbCategory.SelectedItem.ToString(),
                Description = rtbDescription.Text,
                AttachmentPath = attachedFilePath,
                ReportDate = DateTime.Now,
                Status = "Submitted"
            };

            issueList.Add(report);

            // Show success message with feedback
            MessageBox.Show($"✅ Issue reported successfully!\n\nReport ID: {report.ReportID}\nThank you for helping your community!",
                "Submission Successful",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // Reset form
            ResetForm();

            // Display all reports for demonstration
            DisplayAllReports();
        }

        private void ResetForm()
        {
            txtLocation.Clear();
            cmbCategory.SelectedIndex = 0;
            rtbDescription.Clear();
            attachedFilePath = "";
            lblAttachmentStatus.Text = "No file attached";
            lblAttachmentStatus.ForeColor = Color.Gray;
            progressValue = 0;
            progressBar1.Value = 0;
            UpdateEncouragementMessage(0);
        }

        private void DisplayAllReports()
        {
            string allReports = "=== ALL REPORTED ISSUES ===\n\n";
            foreach (var report in issueList)
            {
                allReports += $"ID: {report.ReportID}\n";
                allReports += $"Location: {report.Location}\n";
                allReports += $"Category: {report.Category}\n";
                allReports += $"Description: {report.Description}\n";
                allReports += $"Date: {report.ReportDate}\n";
                allReports += $"Status: {report.Status}\n";
                allReports += "----------------------------\n\n";
            }

            MessageBox.Show(allReports, "All Reports (Demo)", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MainMenuForm mainMenu = new MainMenuForm();
            mainMenu.Show();
            this.Close();
        }

        // Track progress as user fills fields
        private void txtLocation_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtLocation.Text) && progressValue < 20)
            {
                progressValue = 20;
                progressBar1.Value = 20;
                UpdateEncouragementMessage(1);
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (progressValue < 40)
            {
                progressValue = 40;
                progressBar1.Value = 40;
                UpdateEncouragementMessage(2);
            }
        }

        private void rtbDescription_TextChanged(object sender, EventArgs e)
        {
            if (rtbDescription.Text.Length > 10 && progressValue < 60)
            {
                progressValue = 60;
                progressBar1.Value = 60;
                UpdateEncouragementMessage(3);
            }
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.lblLocation = new Label();
            this.txtLocation = new TextBox();
            this.lblCategory = new Label();
            this.cmbCategory = new ComboBox();
            this.lblDescription = new Label();
            this.rtbDescription = new RichTextBox();
            this.btnAttach = new Button();
            this.lblAttachmentStatus = new Label();
            this.btnSubmit = new Button();
            this.btnBack = new Button();
            this.progressBar1 = new ProgressBar();
            this.lblEngagement = new Label();
            this.SuspendLayout();

           
            this.lblTitle.Text = "REPORT AN ISSUE";
            this.lblTitle.Font = new Font("Times New Roman", 20, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(10, 25, 47);
            this.lblTitle.Location = new Point(100, 10);
            this.lblTitle.Size = new Size(400, 40);
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            
            this.lblLocation.Text = "Location:";
            this.lblLocation.Location = new Point(30, 70);
            this.lblLocation.Size = new Size(100, 25);

            
            this.txtLocation.Location = new Point(140, 70);
            this.txtLocation.Size = new Size(380, 30);
            this.txtLocation.Font = new Font("Times New Roman", 12);
            this.txtLocation.TextChanged += new EventHandler(this.txtLocation_TextChanged);

           
            this.lblCategory.Text = "Category:";
            this.lblCategory.Location = new Point(30, 115);
            this.lblCategory.Size = new Size(100, 25);

            
            this.cmbCategory.Location = new Point(140, 115);
            this.cmbCategory.Size = new Size(380, 30);
            this.cmbCategory.Font = new Font("Times New Roman", 12);
            this.cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCategory.SelectedIndexChanged += new EventHandler(this.cmbCategory_SelectedIndexChanged);

            
            this.lblDescription.Text = "Description:";
            this.lblDescription.Location = new Point(30, 160);
            this.lblDescription.Size = new Size(100, 25);

           
            this.rtbDescription.Location = new Point(140, 160);
            this.rtbDescription.Size = new Size(380, 100);
            this.rtbDescription.Font = new Font("Times New Roman", 12);
            this.rtbDescription.TextChanged += new EventHandler(this.rtbDescription_TextChanged);

          
            this.btnAttach.Text = "📎 Attach File";
            this.btnAttach.Location = new Point(140, 280);
            this.btnAttach.Size = new Size(130, 35);
            this.btnAttach.BackColor = Color.FromArgb(10, 25, 47);
            this.btnAttach.ForeColor = Color.White;
            this.btnAttach.FlatStyle = FlatStyle.Flat;
            this.btnAttach.Font = new Font("Times New Roman", 11);
            this.btnAttach.Click += new EventHandler(this.btnAttach_Click);

            this.lblAttachmentStatus.Text = "No file attached";
            this.lblAttachmentStatus.Location = new Point(280, 285);
            this.lblAttachmentStatus.Size = new Size(240, 25);
            this.lblAttachmentStatus.ForeColor = Color.Gray;
            this.lblAttachmentStatus.Font = new Font("Arial", 9);

            this.btnSubmit.Text = "✅ SUBMIT REPORT";
            this.btnSubmit.Location = new Point(140, 335);
            this.btnSubmit.Size = new Size(180, 40);
            this.btnSubmit.BackColor = Color.FromArgb(10, 25, 47);
            this.btnSubmit.ForeColor = Color.White;
            this.btnSubmit.FlatStyle = FlatStyle.Flat;
            this.btnSubmit.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            this.btnSubmit.Click += new EventHandler(this.btnSubmit_Click);

            this.btnBack.Text = "← Back to Main Menu";
            this.btnBack.Location = new Point(340, 335);
            this.btnBack.Size = new Size(180, 40);
            this.btnBack.BackColor = Color.LightGray;
            this.btnBack.ForeColor = Color.FromArgb(10, 25, 47);
            this.btnBack.FlatStyle = FlatStyle.Flat;
            this.btnBack.Font = new Font("Times New Roman", 11);
            this.btnBack.Click += new EventHandler(this.btnBack_Click);

            this.progressBar1.Location = new Point(140, 395);
            this.progressBar1.Size = new Size(380, 20);
            this.progressBar1.Style = ProgressBarStyle.Continuous;
            this.progressBar1.BackColor = Color.LightGray;
            this.progressBar1.ForeColor = Color.FromArgb(10, 25, 47);

            this.lblEngagement.Text = "📝 Start by telling us where the issue is...";
            this.lblEngagement.Location = new Point(140, 420);
            this.lblEngagement.Size = new Size(380, 30);
            this.lblEngagement.ForeColor = Color.FromArgb(10, 25, 47);
            this.lblEngagement.Font = new Font("Times New Roman", 11, FontStyle.Italic);
            this.lblEngagement.TextAlign = ContentAlignment.MiddleCenter;

            this.Text = "Report Issues";
            this.Size = new Size(600, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.rtbDescription);
            this.Controls.Add(this.btnAttach);
            this.Controls.Add(this.lblAttachmentStatus);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblEngagement);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblTitle;
        private Label lblLocation;
        private TextBox txtLocation;
        private Label lblCategory;
        private ComboBox cmbCategory;
        private Label lblDescription;
        private RichTextBox rtbDescription;
        private Button btnAttach;
        private Label lblAttachmentStatus;
        private Button btnSubmit;
        private Button btnBack;
        private ProgressBar progressBar1;
        private Label lblEngagement;
    }
}