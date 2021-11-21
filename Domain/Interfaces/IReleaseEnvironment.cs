using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IReleaseEnvironment
    {
        string Id { get; }
        string Name { get;}
    }
}
