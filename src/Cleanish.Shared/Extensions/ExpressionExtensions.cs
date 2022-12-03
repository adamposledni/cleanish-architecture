using Cleanish.Shared.Helpers;
using System.Linq.Expressions;

namespace Cleanish.Shared.Extensions;
public static class ExpressionExtensions
{
    public static string ToEvaluatedString(this Expression expression)
    {
        return ExpressionParser.ToString(expression);
    }
}
