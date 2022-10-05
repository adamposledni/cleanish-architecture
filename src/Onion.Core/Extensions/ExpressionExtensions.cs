using Onion.Core.Helpers;
using System.Linq.Expressions;

namespace Onion.Core.Extensions;
public static class ExpressionExtensions
{
    public static string ToEvaluatedString(this Expression expression)
    {
        return ExpressionParser.ToString(expression);
    }
}
