namespace Unity.Conventions
{
    public class SameNameNamingConvention : NamingConvention
    {
        #region Overrides of NamingConvention

        public override bool Match(string interfaceName, string className)
        {
            return interfaceName == string.Format("I{0}", className);
        }

        #endregion
    }
}