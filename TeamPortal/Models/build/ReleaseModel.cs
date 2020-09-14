using System.Collections.Generic;

namespace TeamPortal.Models
{
    public class ReleaseModel
    {
        public int id { get; set; }
        public string status { get; set; }
        public string name { get; set; }
        public IEnumerable<EnvironmentReleaseModel> environments { get; set; }
        public Links _links { get; set; }
    }

    public class EnvironmentReleaseModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public IEnumerable<DeployStepsModel> deploySteps { get; set; }
    }
    public class DeployStepsModel
    {
        public IEnumerable<ReleaseDeployPhaseModel> releaseDeployPhases { get; set; }
    }
    public class ReleaseDeployPhaseModel
    {
        public IEnumerable<DeploymentJobModel> deploymentJobs { get; set; }
    }
    public class DeploymentJobModel
    {
        public JobModel job { get; set; }
    }
    public class JobModel
    {
        public string finishTime { get; set; }
    }
}