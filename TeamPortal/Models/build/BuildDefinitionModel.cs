using System;

namespace TeamPortal.Models
{
    public class BuildDefinitionModel
    {
        public string result { get; set; }
        public DefinitionModel definition { get; set; }
        public DateTime finishTime { get; set; }
        public Links _links { get; set; }
    }
    public class DefinitionModel
    {
        public string name { get; set; }
    }
    public class Links
    {
        public Web web { get; set; }
    }
    public class Web
    {
        public string href { get; set; }
    }
}