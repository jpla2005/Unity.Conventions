using System;
using Microsoft.Practices.Unity;

namespace Unity.Conventions
{
    public class TypeRegisterEventArgs : RegisterEventArgs
    {
        public bool IsDuplicated { get; set; }

        public TypeRegisterEventArgs(Type typeFrom, Type typeTo, string name, LifetimeManager lifeTimeManager,
            bool isDuplicated) : base(typeFrom, typeTo, name, lifeTimeManager)
        {
            IsDuplicated = isDuplicated;
        }

        public TypeRegisterEventArgs(RegisterEventArgs e, bool isDuplicated)
            : this(e.TypeFrom, e.TypeTo, e.Name, e.LifetimeManager, isDuplicated)
        {
        }
    }
}