﻿using Rapidity.Json.JsonPath;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rapidity.Json
{
    /// <summary>
    /// JsonElement
    /// </summary>
    public abstract class JsonElement
    {
        public abstract JsonElementType ElementType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JsonElement Create(string json)
        {
            return Create(json, new JsonOption());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static JsonElement Create(string json, JsonOption option)
        {
            using (var reader = new JsonReader(json))
            {
                return new JsonSerializer(option).Deserialize<JsonElement>(reader);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonPath"></param>
        /// <returns></returns>
        public JsonElement Filter(string jsonPath)
        {
            return Filters(jsonPath)?.FirstOrDefault();
        }

        /// <summary>
        /// 匹配多个
        /// </summary>
        /// <param name="jsonPath"></param>
        /// <returns></returns>
        public IEnumerable<JsonElement> Filters(string jsonPath)
        {
            var filters = new DefaultJsonPathResolver().Resolve(jsonPath);
            IEnumerable<JsonElement> current = null;
            foreach (var filter in filters)
            {
                current = filter.Filter(this, current);
                if (current == null || current.Count() == 0) break;
            }
            return current;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual object To(Type type)
        {
            return new JsonSerializer().Deserialize(this, type);
        }

        public virtual object To(Type type, JsonOption option)
        {
            return new JsonSerializer(option).Deserialize(this, type);
        }

        public virtual T To<T>()
        {
            return To<T>(new JsonOption());
        }

        public virtual T To<T>(JsonOption option)
        {
            return new JsonSerializer(option).Deserialize<T>(this);
        }

        public override string ToString()
        {
            var option = new JsonOption
            {
                SkipValidated = true
            };
            return ToString(option);
        }

        public string ToString(JsonOption option)
        {
            return new JsonSerializer(option).Serialize(this);
        }

        #region 基本类型转换

        public static implicit operator JsonElement(string value) => new JsonString(value);
        public static implicit operator JsonElement(bool value) => new JsonBoolean(value);
        public static implicit operator JsonElement(byte value) => new JsonNumber(value);
        public static implicit operator JsonElement(short value) => new JsonNumber(value);
        public static implicit operator JsonElement(int value) => new JsonNumber(value);
        public static implicit operator JsonElement(long value) => new JsonNumber(value);
        public static implicit operator JsonElement(float value) => new JsonNumber(value);
        public static implicit operator JsonElement(double value) => new JsonNumber(value);
        public static implicit operator JsonElement(decimal value) => new JsonNumber(value);

        #endregion
    }
}