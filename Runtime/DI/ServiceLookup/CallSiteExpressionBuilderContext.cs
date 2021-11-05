using System.Linq.Expressions;

namespace UnityLib.DI
{
    internal class CallSiteExpressionBuilderContext
    {
        public ParameterExpression ScopeParameter { get; set; }
        public bool RequiresResolvedServices { get; set; }
    }
}