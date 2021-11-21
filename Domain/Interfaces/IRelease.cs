using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IRelease
    {
        string Id { get; }
        string ProjectId { get;}
        string Version { get;}
        DateTime Created { get;}
    }
}
