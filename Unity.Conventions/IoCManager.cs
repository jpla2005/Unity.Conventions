using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Practices.Unity;

namespace Unity.Conventions
{
    public static class IoCManager
    {
        #region Fields

        private static IUnityContainer _container;

        #endregion

        #region Properties

        private static IUnityContainer Container
        {
            get { return _container ?? (_container = CreateContainer()); }
        }

        #endregion

        private static IUnityContainer CreateContainer()
        {
            var ass = System.Reflection.Assembly.GetAssembly(typeof(IoCManager));
            var path = new Uri(ass.CodeBase.Substring(0, ass.CodeBase.LastIndexOf("/", StringComparison.Ordinal))).AbsolutePath;
            var ext = new AssemblyNamingConventionScanExtension(path);

            var inp = new FileStream(Path.Combine(path, "ioc.config"), FileMode.Open);

            try
            {
                var root = XElement.Load(inp);
                var conventions = root.Elements("NamingConvention");
                foreach (var convention in conventions)
                {
                    var type = convention.Attribute("type");
                    if (type != null)
                    {
                        var convType = Type.GetType(type.Value);
                        if (convType != null)
                        {
                            var obj = Activator.CreateInstance(convType);
                            var conv = obj as NamingConvention;
                            if (conv != null)
                            {
                                var assemblies = convention.Elements("Assembly").Select(asm =>
                                    new Assembly
                                    {
                                        Name = asm.Attribute("name").Value,
                                        FullName = asm.Attribute("fullname").Value
                                    });

                                conv.Assemblies = assemblies;
                                ext.RegisterNamingConvention(conv);
                            }
                        }
                    }
                }

                var container = new UnityContainer();
                container.AddExtension(ext);

                return container;
            }
            catch (Exception)
            {
                throw new IoCException("IoC configuration corrupted");
            }
            finally
            {
                inp.Close();
            }
        }

        public static void Register<TInterface, TClass>() where TClass : TInterface
        {
            Container.RegisterType<TInterface, TClass>();
        }

        public static void Register<TInterface, TClass>(LifetimeManager lifetimeManager) where TClass : TInterface
        {
            Container.RegisterType<TInterface, TClass>(lifetimeManager);
        }

        public static void Register<TInterface, TClass>(string name, LifetimeManager lifetimeManager) where TClass : TInterface
        {
            Container.RegisterType<TInterface, TClass>(name, lifetimeManager);
        }

        public static void Register(Type from, Type to)
        {
            Container.RegisterType(from, to);
        }

        public static void Register(Type from, Type to, LifetimeManager lifetimeManager)
        {
            Container.RegisterType(from, to, lifetimeManager);
        }

        public static void Register(Type from, Type to, string name, LifetimeManager lifetimeManager)
        {
            Container.RegisterType(from, to, name, lifetimeManager);
        }

        public static TInterface Resolve<TInterface>()
        {
            return Container.Resolve<TInterface>();
        }

        public static TInterface Resolve<TInterface>(string name)
        {
            return Container.Resolve<TInterface>(name);
        }

        public static object Resolve(Type type)
        {
            return Container.Resolve(type);
        }

        public static object Resolve(Type type, string name)
        {
            return Container.Resolve(type, name);
        }
    }
}