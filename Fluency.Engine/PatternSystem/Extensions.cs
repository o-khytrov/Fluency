using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluency.Engine.PatternSystem;

/// <summary>
/// Useful extensions
/// </summary>
public static class Extensions {

    /// <summary>
    /// Gets a MemberInfo from a member expression.
    /// </summary>
    public static PropertyInfo GetMember<T, TProperty>(this Expression<Func<T, TProperty>> expression) {
        var memberExp = RemoveUnary(expression.Body) as MemberExpression;

        if (memberExp == null) {
            return null;
        }

        Expression currentExpr = memberExp.Expression;

        // Unwind the expression to get the root object that the expression acts upon.
        while (true) {
            currentExpr = RemoveUnary(currentExpr);

            if (currentExpr != null && currentExpr.NodeType == ExpressionType.MemberAccess) {
                currentExpr = ((MemberExpression)currentExpr).Expression;
            } else {
                break;
            }
        }

        if (currentExpr == null || currentExpr.NodeType != ExpressionType.Parameter) {
            return null; // We don't care if we're not acting upon the model instance.
        }

        return memberExp.Member as  PropertyInfo;
    }

    private static Expression RemoveUnary(Expression toUnwrap) {
        if (toUnwrap is UnaryExpression) {
            return ((UnaryExpression)toUnwrap).Operand;
        }

        return toUnwrap;
    }
}
public static class AccessorCache<T> {
	private static readonly ConcurrentDictionary<Key, Delegate> Cache = new ConcurrentDictionary<Key, Delegate>();

	/// <summary>
	/// Gets an accessor func based on an expression
	/// </summary>
	/// <typeparam name="TProperty"></typeparam>
	/// <param name="member">The member represented by the expression</param>
	/// <param name="expression"></param>
	/// <param name="bypassCache"></param>
	/// <param name="cachePrefix">Cache prefix</param>
	/// <returns>Accessor func</returns>
	public static Func<T, TProperty> GetCachedAccessor<TProperty>(MemberInfo member, Expression<Func<T, TProperty>> expression, bool bypassCache = false, string cachePrefix = null) {
		if (member == null || bypassCache) {
			return expression.Compile();
		}

		var key = new Key(member, expression, cachePrefix);
		return (Func<T,TProperty>)Cache.GetOrAdd(key, k => expression.Compile());
	}

	public static void Clear() {
		Cache.Clear();
	}

	private class Key {
		private readonly MemberInfo _memberInfo;
		private readonly string _expressionDebugView;

		public Key(MemberInfo member, Expression expression, string cachePrefix) {
			_memberInfo = member;
			_expressionDebugView = cachePrefix != null ? cachePrefix + expression.ToString() : expression.ToString();
		}

		protected bool Equals(Key other) {
			return Equals(_memberInfo, other._memberInfo) && string.Equals(_expressionDebugView, other._expressionDebugView);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Key) obj);
		}

		public override int GetHashCode() {
			unchecked {
				return ((_memberInfo != null ? _memberInfo.GetHashCode() : 0)*397) ^ (_expressionDebugView != null ? _expressionDebugView.GetHashCode() : 0);
			}
		}
	}

}
