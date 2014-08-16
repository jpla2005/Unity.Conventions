using System;
using Microsoft.Practices.Unity;

namespace Unity.Conventions
{
    public class InstanceRegisterEventArgs : RegisterInstanceEventArgs
    {
        public bool IsDuplicated { get; set; }

        public InstanceRegisterEventArgs(Type registeredType, object instance,
                                         string name, LifetimeManager lifeTimeManager, bool isDuplicated)
            : base(registeredType, instance, name, lifeTimeManager)
        {
            IsDuplicated = isDuplicated;
        }

        public InstanceRegisterEventArgs(RegisterInstanceEventArgs e, bool isDuplicated)
            : this(e.RegisteredType, e.Instance, e.Name, e.LifetimeManager, isDuplicated)
        {
        }
    }
}