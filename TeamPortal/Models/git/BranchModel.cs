using System;

namespace TeamPortal.Models
{
    public class BranchModel
    {
        public string Repository { get; set; }
        public string name { get; set; }
        public CommitModel commit { get; set; }
    }

    public class CommitModel
    {
        public DateTime created_at { get; set; }
        public string committer_name { get; set; }
    }
}
