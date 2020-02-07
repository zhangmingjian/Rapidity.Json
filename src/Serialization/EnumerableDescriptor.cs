﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rapidity.Json.Serialization
{
    /// <summary>
    /// 可迭代集合类型
    /// </summary>
    internal class EnumerableDescriptor : TypeDescriptor
    {
        public override TypeKind TypeKind => TypeKind.List;
        public Type ItemType { get; protected set; }

        public EnumerableDescriptor(Type type) : this(type, typeof(object))
        {
        }

        public EnumerableDescriptor(Type type, Type itemType) : base(type)
        {
            ItemType = itemType;
        }

        protected override Func<object> BuildCreateInstanceMethod(Type type)
        {
            NewExpression newExp = Expression.New(type);
            Expression<Func<object>> expression = Expression.Lambda<Func<object>>(newExp);
            return expression.Compile();
        }

        private Action<object, object> _addItem;
        public Action<object, object> AddItem => _addItem = _addItem ?? BuildAddItemMethod(Type);

        protected virtual Action<object, object> BuildAddItemMethod(Type type)
        {
            var listExp = Expression.Parameter(typeof(object), "list");
            var itemExp = Expression.Parameter(typeof(object), "item");
            var instanceExp = Expression.Convert(listExp, type);
            var argumentExp = Expression.Convert(itemExp, ItemType);
            var addMethod = type.GetMethod(AddMethodName);
            var callExp = Expression.Call(instanceExp, addMethod, argumentExp);
            Expression<Action<object, object>> addItemExp = Expression.Lambda<Action<object, object>>(callExp, listExp, itemExp);
            return addItemExp.Compile();
        }

        protected string AddMethodName { get; set; } = "Add";

        private Func<object, IEnumerator> _getEnumerator;

        public Func<object, IEnumerator> GetEnumerator => _getEnumerator = _getEnumerator ?? BuildGetEnumberatorMethod(Type);

        private Func<object, IEnumerator> BuildGetEnumberatorMethod(Type type)
        {
            var paramExp = Expression.Parameter(typeof(object));
            var listExp = Expression.TypeAs(paramExp, type);
            var method = type.GetMethod(nameof(IEnumerable.GetEnumerator));
            var callExp = Expression.Call(listExp, method);
            var body = Expression.TypeAs(callExp, typeof(IEnumerator));
            var expression = Expression.Lambda<Func<object, IEnumerator>>(body, paramExp);
            return expression.Compile();
        }
    }

    internal class ArrayDescriptor : EnumerableDescriptor
    {
        public override TypeKind TypeKind => TypeKind.Array;

        public ArrayDescriptor(Type type) :
            base(typeof(List<>).MakeGenericType(type.GetElementType()), type.GetElementType())
        {
        }

        private Func<object, object> _toArray;
        public Func<object, object> ToArray => _toArray = _toArray ?? BuildToArrayMethod(Type);

        protected Func<object, object> BuildToArrayMethod(Type type)
        {
            var objExp = Expression.Parameter(typeof(object), "list");

            var equalExp = Expression.Equal(objExp, Expression.Constant(null));

            var listExp = Expression.TypeAs(objExp, type);
            var method = type.GetMethod("ToArray");
            var call = Expression.Call(listExp, method);
            //判断入参是否为null
            var body = Expression.Condition(equalExp,
                                objExp,
                                Expression.TypeAs(call, typeof(object)));
            var expression = Expression.Lambda<Func<object, object>>(body, objExp);
            return expression.Compile();
        }
    }
}