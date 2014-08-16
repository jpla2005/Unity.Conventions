using System.Collections.Generic;

namespace Unity.Conventions
{
    public abstract class NamingConvention
    {
        public IEnumerable<Assembly> Assemblies { get; set; }

        public abstract bool Match(string interfaceName, string className);
    }
}