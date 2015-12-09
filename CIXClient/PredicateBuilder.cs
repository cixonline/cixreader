// *****************************************************
// CIXReader
// PredicateBuilder.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 08/06/2015 20:39
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CIXClient.Collections;

namespace CIXClient
{
    /// <summary>
    /// Class to build predicates
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// A list of supported operations in a single
        /// filter.
        /// </summary>
        public enum Op
        {
            /// <summary>
            /// Operation equals value
            /// </summary>
            Equals,

            /// <summary>
            /// Operation is greater than value
            /// </summary>
            GreaterThan,

            /// <summary>
            /// Operation is less than value
            /// </summary>
            LessThan,

            /// <summary>
            /// Operation is greater than or equal to value
            /// </summary>
            GreaterThanOrEqual,

            /// <summary>
            /// Operation is less than or equal to value
            /// </summary>
            LessThanOrEqual,

            /// <summary>
            /// Operation contains value
            /// </summary>
            Contains,

            /// <summary>
            /// Operation starts with value
            /// </summary>
            StartsWith,

            /// <summary>
            /// Operation ends with value
            /// </summary>
            EndsWith
        }

        /// <summary>
        /// A single filter defines one operation in an expression.
        /// </summary>
        public sealed class Filter
        {
            /// <summary>
            /// The name of the property whose value is used by
            /// the operation.
            /// </summary>
            public string PropertyName { get; set; }

            /// <summary>
            /// The operation to apply against the property value.
            /// </summary>
            public Op Operation { get; set; }

            /// <summary>
            /// The value against which the operation is applied
            /// against the property value.
            /// </summary>
            public object Value { get; set; }
        }

        private static readonly MethodInfo containsMethod = typeof(string).GetMethod("Contains");
        private static readonly MethodInfo startsWithMethod =
        typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        private static readonly MethodInfo endsWithMethod =
        typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

        /// <summary>
        /// Create an Expression from a list of filters that, applied to a collection of type T,
        /// will return a new collection filtered by the conditions in the expression.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the expression</typeparam>
        /// <param name="type">The group type (All or Any)</param>
        /// <param name="filters">A list of filters that each define one part of the expression</param>
        /// <returns>An Expression that filters a list of type T based on the filters</returns>
        public static Expression<Func<T,bool>> GetExpression<T>(RuleGroupType type, IList<Filter> filters)
        {
            if (filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            if (filters.Count == 1)
                exp = GetExpression(param, filters[0]);
            else if (filters.Count == 2)
                exp = GetExpression(type, param, filters[0], filters[1]);
            else
            {
                while (filters.Count > 0)
                {
                    var f1 = filters[0];
                    var f2 = filters[1];

                    exp = (exp == null) ? 
                        GetExpression(type, param, filters[0], filters[1]) : 
                        type == RuleGroupType.All ?
                            Expression.AndAlso(exp, GetExpression(type, param, filters[0], filters[1])) :
                            Expression.OrElse(exp, GetExpression(type, param, filters[0], filters[1]));

                    filters.Remove(f1);
                    filters.Remove(f2);

                    if (filters.Count == 1)
                    {
                        exp = Expression.AndAlso(exp, GetExpression(param, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }
            return exp == null ? null : Expression.Lambda<Func<T, bool>>(exp, param);
        }

        private static Expression GetExpression(Expression param, Filter filter)
        {
            Expression member = param;
            member = filter.PropertyName.Split('.').Aggregate(member, Expression.PropertyOrField);

            ConstantExpression constant = Expression.Constant(filter.Value);

            switch (filter.Operation)
            {
                case Op.Equals:
                    return Expression.Equal(member, constant);

                case Op.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case Op.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case Op.LessThan:
                    return Expression.LessThan(member, constant);

                case Op.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case Op.Contains:
                    return Expression.Call(member, containsMethod, constant);

                case Op.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant);

                case Op.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant);
            }

            return null;
        }

        private static BinaryExpression GetExpression(RuleGroupType type, Expression param, Filter filter1,
            Filter filter2)
        {
            Expression bin1 = GetExpression(param, filter1);
            Expression bin2 = GetExpression(param, filter2);
            return (type == RuleGroupType.All ? Expression.AndAlso(bin1, bin2) : Expression.OrElse(bin1, bin2));
        }
    }
}