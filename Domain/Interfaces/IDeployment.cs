using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IDeployment
    {
        string Id { get; }
        string ReleaseId { get;}
        string EnvironmentId { get;}
        DateTime DeployedAt { get; }
    }
}
