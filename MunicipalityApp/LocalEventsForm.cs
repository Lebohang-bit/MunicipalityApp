using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MunicipalityApp
{
    public partial class LocalEventsForm : Form
    {
        private EventRepository repository = new EventRepository();
        private Dictionary<string, int> searchHistory = new Dictionary<string, int>();
        private List<Event> currentResults = new List<Event>();

        private Label lblTitle;
        private Label lblCategoryFilter;
        private ComboBox cmbCategoryFilter;
        private Label lblFromDate;
        private DateTimePicker dtpFromDate;
        private Label lblToDate;
        private DateTimePicker dtpToDate;
        private Button btnSearch;
        private Button btnClear;
        private ListView lvEvents;
        private Label lblRecommendationTitle;
        private ListBox lstRecommendations;
        private Button btnBack;
        private Label lblSearchStats;

        public LocalEventsForm()
        {
            InitializeComponent();
            ApplyNavyWhiteTheme();
            repository.LoadSampleData();
            PopulateCategoryFilter();
            LoadAllEvents();
            RefreshRecommendations();
        }

        private void ApplyNavyWhiteTheme()
        {
            this.BackColor = Color.White;
            this.ForeColor = Color.FromArgb(10, 25, 47);
            this.Font = new Font("Times New Roman", 12);
        }

        private void PopulateCategoryFilter()
        {
            cmbCategoryFilter.Items.Add("All Categories");
            foreach (var cat in repository.UniqueCategories.OrderBy(c => c))
            {
                cmbCategoryFilter.Items.Add(cat);
            }
            cmbCategoryFilter.SelectedIndex = 0;
        }

        private void LoadAllEvents()
        {
            currentResults = repository.Search("All Categories", null, null);
            DisplayEvents(currentResults);
        }

        private void DisplayEvents(List<Event> events)
        {
            lvEvents.Items.Clear();

            foreach (var evt in events)
            {
                ListViewItem item = new ListViewItem(evt.EventDate.ToString("yyyy-MM-dd"));
                item.SubItems.Add(evt.Category);
                item.SubItems.Add(evt.Title);
                item.SubItems.Add(evt.Location);
                item.SubItems.Add(evt.Priority == 1 ? "High" : evt.Priority == 2 ? "Medium" : "Normal");
                item.Tag = evt;
                lvEvents.Items.Add(item);
            }

            lblSearchStats.Text = $"{events.Count} event(s) found";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string category = cmbCategoryFilter.SelectedItem?.ToString() ?? "All Categories";

            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date;

            if (toDate < fromDate)
            {
                MessageBox.Show("The 'To' date cannot be earlier than the 'From' date.",
                    "Invalid Date Range",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            currentResults = repository.Search(category, fromDate, toDate);
            DisplayEvents(currentResults);

            TrackSearch(category);

            RefreshRecommendations();

            if (currentResults.Count == 0)
            {
                MessageBox.Show("No events found matching your search criteria.",
                    "No Results",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cmbCategoryFilter.SelectedIndex = 0;
            dtpFromDate.Value = DateTime.Today;
            dtpToDate.Value = DateTime.Today.AddMonths(3);
            LoadAllEvents();
        }

        private void TrackSearch(string category)
        {
            if (category == "All Categories") return;

            if (searchHistory.ContainsKey(category))
            {
                searchHistory[category]++;
            }
            else
            {
                searchHistory[category] = 1;
            }
        }

        private void RefreshRecommendations()
        {
            lstRecommendations.Items.Clear();

            if (searchHistory.Count == 0)
            {
                List<Event> upcoming = repository.GetHighestPriority(5);
                foreach (var evt in upcoming)
                {
                    lstRecommendations.Items.Add($"⭐ {evt.Title} ({evt.EventDate:MMM dd})");
                }
                lblRecommendationTitle.Text = "Suggested Events (Highest Priority)";
                return;
            }

            var topCategories = searchHistory
                .OrderByDescending(kv => kv.Value)
                .Select(kv => kv.Key)
                .ToList();

            List<Event> recommended = new List<Event>();

            foreach (var cat in topCategories)
            {
                var events = repository.AllEvents
                    .Where(e => e.Category == cat && e.EventDate >= DateTime.Today)
                    .OrderBy(e => e.EventDate)
                    .Take(3);

                foreach (var evt in events)
                {
                    if (!recommended.Any(r => r.EventID == evt.EventID))
                    {
                        recommended.Add(evt);
                    }
                }

                if (recommended.Count >= 5) break;
            }

            foreach (var evt in recommended.Take(5))
            {
                lstRecommendations.Items.Add($"⭐ {evt.Title} ({evt.EventDate:MMM dd})");
            }

            lblRecommendationTitle.Text = $"Recommended for You (based on: {string.Join(", ", topCategories.Take(2))})";
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MainMenuForm mainMenu = new MainMenuForm();
            mainMenu.Show();
            this.Close();
        }

        private void lvEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvEvents.SelectedItems.Count == 0) return;

            Event selected = lvEvents.SelectedItems[0].Tag as Event;
            if (selected == null) return;

            TrackSearch(selected.Category);

            string details = $"📅 {selected.Title}\n\n" +
                             $"Category: {selected.Category}\n" +
                             $"Location: {selected.Location}\n" +
                             $"Date: {selected.EventDate:dddd, MMMM dd, yyyy}\n" +
                             $"Priority: {(selected.Priority == 1 ? "High" : selected.Priority == 2 ? "Medium" : "Normal")}\n\n" +
                             $"{selected.Description}";

            MessageBox.Show(details, "Event Details", MessageBoxButtons.OK, MessageBoxIcon.Information);

            RefreshRecommendations();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.lblCategoryFilter = new Label();
            this.cmbCategoryFilter = new ComboBox();
            this.lblFromDate = new Label();
            this.dtpFromDate = new DateTimePicker();
            this.lblToDate = new Label();
            this.dtpToDate = new DateTimePicker();
            this.btnSearch = new Button();
            this.btnClear = new Button();
            this.lvEvents = new ListView();
            this.lblRecommendationTitle = new Label();
            this.lstRecommendations = new ListBox();
            this.btnBack = new Button();
            this.lblSearchStats = new Label();
            this.SuspendLayout();

            this.lblTitle.Text = "LOCAL EVENTS & ANNOUNCEMENTS";
            this.lblTitle.Font = new Font("Times New Roman", 18, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(10, 25, 47);
            this.lblTitle.Location = new Point(50, 10);
            this.lblTitle.Size = new Size(700, 40);
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            this.lblCategoryFilter.Text = "Category:";
            this.lblCategoryFilter.Location = new Point(30, 70);
            this.lblCategoryFilter.Size = new Size(80, 25);

            this.cmbCategoryFilter.Location = new Point(110, 70);
            this.cmbCategoryFilter.Size = new Size(180, 30);
            this.cmbCategoryFilter.Font = new Font("Times New Roman", 11);
            this.cmbCategoryFilter.DropDownStyle = ComboBoxStyle.DropDownList;

            this.lblFromDate.Text = "From:";
            this.lblFromDate.Location = new Point(310, 70);
            this.lblFromDate.Size = new Size(50, 25);

            this.dtpFromDate.Location = new Point(360, 70);
            this.dtpFromDate.Size = new Size(150, 30);
            this.dtpFromDate.Format = DateTimePickerFormat.Short;
            this.dtpFromDate.Value = DateTime.Today;

            this.lblToDate.Text = "To:";
            this.lblToDate.Location = new Point(520, 70);
            this.lblToDate.Size = new Size(30, 25);

            this.dtpToDate.Location = new Point(550, 70);
            this.dtpToDate.Size = new Size(150, 30);
            this.dtpToDate.Format = DateTimePickerFormat.Short;
            this.dtpToDate.Value = DateTime.Today.AddMonths(3);

            this.btnSearch.Text = "🔍 Search";
            this.btnSearch.Location = new Point(710, 68);
            this.btnSearch.Size = new Size(100, 32);
            this.btnSearch.BackColor = Color.FromArgb(10, 25, 47);
            this.btnSearch.ForeColor = Color.White;
            this.btnSearch.FlatStyle = FlatStyle.Flat;
            this.btnSearch.Font = new Font("Times New Roman", 11);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);

            this.btnClear.Text = "Clear";
            this.btnClear.Location = new Point(820, 68);
            this.btnClear.Size = new Size(80, 32);
            this.btnClear.BackColor = Color.LightGray;
            this.btnClear.ForeColor = Color.FromArgb(10, 25, 47);
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.Font = new Font("Times New Roman", 11);
            this.btnClear.Click += new EventHandler(this.btnClear_Click);

            this.lblSearchStats.Text = "0 event(s) found";
            this.lblSearchStats.Location = new Point(30, 115);
            this.lblSearchStats.Size = new Size(400, 25);
            this.lblSearchStats.Font = new Font("Arial", 9, FontStyle.Italic);
            this.lblSearchStats.ForeColor = Color.Gray;

            this.lvEvents.Location = new Point(30, 145);
            this.lvEvents.Size = new Size(870, 260);
            this.lvEvents.View = View.Details;
            this.lvEvents.FullRowSelect = true;
            this.lvEvents.GridLines = true;
            this.lvEvents.Font = new Font("Times New Roman", 11);
            this.lvEvents.Columns.Add("Date", 100);
            this.lvEvents.Columns.Add("Category", 140);
            this.lvEvents.Columns.Add("Title", 280);
            this.lvEvents.Columns.Add("Location", 180);
            this.lvEvents.Columns.Add("Priority", 90);
            this.lvEvents.SelectedIndexChanged += new EventHandler(this.lvEvents_SelectedIndexChanged);

            this.lblRecommendationTitle.Text = "Recommended for You";
            this.lblRecommendationTitle.Location = new Point(30, 420);
            this.lblRecommendationTitle.Size = new Size(500, 25);
            this.lblRecommendationTitle.Font = new Font("Times New Roman", 12, FontStyle.Bold);
            this.lblRecommendationTitle.ForeColor = Color.FromArgb(10, 25, 47);

            this.lstRecommendations.Location = new Point(30, 450);
            this.lstRecommendations.Size = new Size(870, 100);
            this.lstRecommendations.Font = new Font("Times New Roman", 11);
            this.lstRecommendations.ForeColor = Color.FromArgb(10, 25, 47);

            this.btnBack.Text = "← Back to Main Menu";
            this.btnBack.Location = new Point(720, 570);
            this.btnBack.Size = new Size(180, 40);
            this.btnBack.BackColor = Color.LightGray;
            this.btnBack.ForeColor = Color.FromArgb(10, 25, 47);
            this.btnBack.FlatStyle = FlatStyle.Flat;
            this.btnBack.Font = new Font("Times New Roman", 11);
            this.btnBack.Click += new EventHandler(this.btnBack_Click);

            this.Text = "Local Events & Announcements";
            this.Size = new Size(950, 670);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblCategoryFilter);
            this.Controls.Add(this.cmbCategoryFilter);
            this.Controls.Add(this.lblFromDate);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.lblToDate);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblSearchStats);
            this.Controls.Add(this.lvEvents);
            this.Controls.Add(this.lblRecommendationTitle);
            this.Controls.Add(this.lstRecommendations);
            this.Controls.Add(this.btnBack);

            this.ResumeLayout(false);
        }
    }
}