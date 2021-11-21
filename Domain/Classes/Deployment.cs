using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Deployment : IDeployment
    {
        private string _id;
        private string _releaseId;
        private string _environmentId;
        private DateTime _deployedAt;

        public Deployment(string id, string releaseId, string environmentId, DateTime deployedAt)
        {
            _id = id;
            _releaseId = releaseId;
            _environmentId = environmentId;
            _deployedAt = deployedAt;
        }

        public string Id 
        {
            get
            {
                return _id;
            }
        }

        public string ReleaseId
        {
            get
            {
                return _releaseId;
            }
        }

        public string EnvironmentId
        {
            get
            {
                return _environmentId;
            }
        }

        public DateTime DeployedAt
        {
            get
            {
                return _deployedAt;
            }
        }
    }
}
