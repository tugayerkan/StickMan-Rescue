using System;

namespace SencanUtils.Runtime
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class NeedHelpAttribute : Attribute
    {
        public string Problem { get; }

        public NeedHelpAttribute(string problem)
        {
            Problem = problem;
        }
    }
}
