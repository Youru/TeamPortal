using System;
using System.Collections.Generic;

namespace TeamPortal.Models
{
    public interface IMergeRequestModel { }

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

    public class MergeRequestModelDevops
    {
        public List<MergeValue> Value { get; set; }
    }

    public class MergeValue
    {
        public int pullRequestId { get; set; }
        public string title { get; set; }
        public AuthorDevops createdBy { get; set; }
        public Repository repository { get; set; }
        public DateTime creationDate { get; set; }
        public string status { get; set; }
        public bool IsApproved { get; set; }
        public string url { get; set; }
    }

    public class Repository
    {
        public Project project { get; set; }
    }

    public class Project
    {
        public string id { get; set; }
    }

    public class AuthorDevops
    {
        public string displayName { get; set; }
    }

    public static partial class HelperModel
    {
        public static MergeRequestModel MapValueToMergeRequest(this MergeValue value)
        {
            return new MergeRequestModel
            {
                iid = value.pullRequestId,
                state = value.status,
                title = value.title,
                updated_at = value.creationDate,
                author = new Author { name = value.createdBy.displayName },
                IsApproved = value.IsApproved
            };
        }
    }
    public class ValueApprovalDevopsModel
    {
        public List<ApprovalDevopsModel> value { get; set; }
    }


    public class ApprovalDevopsModel
    {
        public string status { get; set; }
    }
}
