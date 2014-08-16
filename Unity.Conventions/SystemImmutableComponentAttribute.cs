using System;

namespace Unity.Conventions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class SystemImmutableComponentAttribute : Attribute
    {
    }
}