using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace Unity.Conventions
{
    public class AssemblyNamingConventionScanExtension : UnityContainerExtension
    {
        private const string AssemblySearchPattern = "*.exe,*.dll";

        private readonly string _assembliesBasePath;
        private readonly List<NamingConvention> _conventions;

        public AssemblyNamingConventionScanExtension(string assembliesBasePath)
            : this(assembliesBasePath, new List<NamingConvention>())
        {
        }

        public AssemblyNamingConventionScanExtension(string assembliesBasePath, List<NamingConvention> conventions)
        {
            _assembliesBasePath = assembliesBasePath;
            _conventions = conventions;
        }

        public void RegisterNamingConvention(NamingConvention convention)
        {
            _conventions.Add(convention);
        }

        protected override void Initialize()
        {
            var result = new List<ClassInterface>();
            var physicalAssemblies = DirectoryExts.GetFiles(_assembliesBasePath, AssemblySearchPattern, SearchOption.AllDirectories).ToList();

            foreach (var convention in _conventions)
            {
                var assemblies = convention.Assemblies;
                var assembliesToLoad = physicalAssemblies.Where(asm => assemblies.Any(convAsm => asm.Substring(0, asm.Length - 4).EndsWith(convAsm.Name)));

                foreach (var assemblyFile in assembliesToLoad)
                {
                    var assembly = System.Reflection.Assembly.Load(AssemblyName.GetAssemblyName(assemblyFile));
                    var types = assembly.GetTypes();

                    foreach (var type in types)
                    {
                        var myConvention = convention;
                        var myType = type;

                        var interfaces = type.GetInterfaces().Where(@interface => myConvention.Match(@interface.Name, myType.Name));

                        result.AddRange(interfaces.Select(@interface => new ClassInterface { Interface = @interface, Type = type }));
                    }
                }

                RegisterTypes(result);
            }
        }

        private void RegisterTypes(IEnumerable<ClassInterface> types)
        {
            foreach (var type in types)
            {
                Container.RegisterType(type.Interface, type.Type);
            }
        }
    }
}