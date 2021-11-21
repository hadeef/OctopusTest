using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ReleaseEnvironment : IReleaseEnvironment
    {
        private string _id;
        private string _name;

        public ReleaseEnvironment(string id, string name)
        {
            _id = id;
            _name = name;
        }

        public string Id 
        {
            get
            {
                return _id;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }
    }
}
