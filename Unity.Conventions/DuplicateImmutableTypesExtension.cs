using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Unity.Conventions
{
    public class DuplicateImmutableTypesExtension : UnityContainerExtension
    {
        private List<Type> _registeredTypes;
        private List<Type> _registeredInstances;

        protected override void Initialize()
        {
            _registeredTypes = new List<Type>();
            _registeredInstances = new List<Type>();

            Context.RegisteringInstance += OnRegisterInstance;
            Context.Registering += OnRegisterType;
        }

        private void OnRegisterType(object sender, RegisterEventArgs e)
        {
            object[] atts = null;
            if (e.TypeFrom != null)
            {
                atts = e.TypeFrom.GetCustomAttributes(typeof(SystemImmutableComponentAttribute), false);
            }

            object[] atts2 = null;
            if (e.TypeTo != null)
            {
                atts2 = e.TypeTo.GetCustomAttributes(typeof(SystemImmutableComponentAttribute), false);
            }

            if ((atts != null && atts.Length > 0) || (atts2 != null && atts2.Length > 0)
                && (SistemComponentTypeRegistered != null))
            {
                var duplicated = (_registeredTypes.Contains(e.TypeFrom) || (_registeredTypes.Contains(e.TypeTo)));

                SistemComponentTypeRegistered(this, new TypeRegisterEventArgs(e, duplicated));
            }

            AddType(e.TypeFrom, e.TypeTo);
        }

        private void OnRegisterInstance(object sender, RegisterInstanceEventArgs e)
        {
            var atts = e.Instance.GetType().GetCustomAttributes(typeof(SystemImmutableComponentAttribute), false);
            var atts2 = e.RegisteredType.GetCustomAttributes(typeof(SystemImmutableComponentAttribute), false);

            if ((atts.Length > 0) || (atts2.Length > 0)
                && (SistemComponentInstanceRegistered != null))
            {
                var duplicated = (_registeredInstances.Contains(e.Instance.GetType())
                                  || (_registeredInstances.Contains(e.RegisteredType)));

                SistemComponentInstanceRegistered(this, new InstanceRegisterEventArgs(e, duplicated));
            }

            AddInstance(e.Instance.GetType());
        }

        private void AddType(Type from, Type to)
        {
            if (from != null && !_registeredTypes.Contains(from))
            {
                _registeredTypes.Add(from);
            }

            if (to != null && !_registeredTypes.Contains(to))
            {
                _registeredTypes.Add(to);
            }
        }

        private void AddInstance(Type t)
        {
            if (t != null && !_registeredInstances.Contains(t))
            {
                _registeredInstances.Add(t);
            }
        }

        public event EventHandler<TypeRegisterEventArgs> SistemComponentTypeRegistered;

        public event EventHandler<InstanceRegisterEventArgs> SistemComponentInstanceRegistered;
    }
}