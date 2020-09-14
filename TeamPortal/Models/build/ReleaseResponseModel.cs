using System.Collections.Generic;

namespace TeamPortal.Models
{
    public class ReleaseResponseModel
    {
        public int count { get; set; }
        public IEnumerable<ReleasesModel> value { get; set; }

    }
    public class ReleasesModel
    {
        public int id { get; set; }
    }
}