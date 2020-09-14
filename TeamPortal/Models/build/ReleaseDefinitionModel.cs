using System.Collections.Generic;

namespace TeamPortal.Models
{
    public class ReleaseDefinitionModel
    {
        public int count { get; set; }
        public LastReleaseModel lastRelease { get; set; }

    }
    public class LastReleaseModel
    {
        public int id { get; set; }
    }
}