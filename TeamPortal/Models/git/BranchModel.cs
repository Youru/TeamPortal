using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamPortal.Models
{
    public class BranchModel
    {
        public string Repository { get; set; }
        public string name { get; set; }
        public CommitModel commit { get; set; }
        public string web_url { get; set; }
    }

    public class CommitModel
    {
        public DateTime created_at { get; set; }
        public string committer_name { get; set; }
    }

    public class BranchDevOpsModel
    {
        public List<BranchValue> Value { get; set; }
    }

    public class BranchValue
    {
        public string Repository { get; set; }
        public string name { get; set; }
        public CommitDevOpsModel commit { get; set; }
        public string web_url { get; set; }
    }

    public class CommitDevOpsModel
    {
        public CommitterDevOpsModel committer { get; set; }
    }

    public class CommitterDevOpsModel
    {
        public DateTime date { get; set; }
        public string name { get; set; }
    }


    public static partial class HelperModel
    {
        public static BranchModel MapValueToBranch(this BranchValue value)
        {
            return new BranchModel
            {
                name = value.name,
                commit = new CommitModel { committer_name = value.commit.committer.name, created_at = value.commit.committer.date }
            };
        }
    }
}
