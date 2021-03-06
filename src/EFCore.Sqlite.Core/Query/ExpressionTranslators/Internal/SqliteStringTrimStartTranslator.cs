// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class SqliteStringTrimStartTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo _trimStart = typeof(string).GetTypeInfo()
            .GetDeclaredMethods(nameof(string.TrimStart))
            .Single(m => m.GetParameters().Count() == 1 && m.GetParameters()[0].ParameterType == typeof(char[]));


        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Expression Translate(MethodCallExpression methodCallExpression)
        {
            if (_trimStart.Equals(methodCallExpression.Method))
            {
                var sqlArguments = new List<Expression> { methodCallExpression.Object };
                var charactersToTrim = (methodCallExpression.Arguments[0] as ConstantExpression)?.Value as char[];
                if (charactersToTrim?.Length > 0)
                {
                    sqlArguments.Add(Expression.Constant(new string(charactersToTrim), typeof(string)));
                }
                return new SqlFunctionExpression("ltrim", methodCallExpression.Type, sqlArguments);
            }

            return null;
        }
    }
}
