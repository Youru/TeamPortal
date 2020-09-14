using System.Collections.Generic;

namespace TeamPortal.Models
{
    public class BuildResponseModel
    {
        public int count { get; set; }
        public IEnumerable<BuildsModel> value { get; set; }

    }
    public class BuildsModel
    {
        public int id { get; set; }
    }
}