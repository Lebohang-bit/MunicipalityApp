using System;

namespace MunicipalityApp
{
    public class IssueReport
    {
        public string ReportID { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string AttachmentPath { get; set; }
        public DateTime ReportDate { get; set; }
        public string Status { get; set; }
    }
}