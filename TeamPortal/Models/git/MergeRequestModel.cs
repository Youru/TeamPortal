using System;

namespace TeamPortal.Models
{
    public class MergeRequestModel
    {
        public int iid { get; set; }
        public string title { get; set; }
        public Author author { get; set; }
        public DateTime updated_at { get; set; }
        public string web_url { get; set; }
        public string state { get; set; }
        public bool IsApproved { get; set; }
    }

    public class Author
    {
        public string name { get; set; }
    }

    public class ApprovalModel
    {
        public bool approved { get; set; }
    }
}
