using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Release : IRelease
    {
        private string _id;
        private string _projectId;
        private string _version;
        private DateTime _created;

        public Release(string id, string projectId, string version, DateTime created)
        {
            _id = id;
            _projectId = projectId;
            _version = version;
            _created = created;
        }

        public string Id 
        {
            get
            {
                return _id;
            }
        }

        public string ProjectId
        {
            get
            {
                return _projectId;
            }
        }

        public string Version
        {
            get
            {
                return _version;
            }
        }

        public DateTime Created
        {
            get
            {
                return _created;
            }
        }
    }
}
