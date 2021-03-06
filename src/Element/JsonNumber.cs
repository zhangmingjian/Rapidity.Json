﻿using System;

namespace Rapidity.Json
{
    public class JsonNumber : JsonElement, IEquatable<JsonNumber>
    {
        private string _value; //为了避免精度问题，使用string来存储

        public override JsonElementType ElementType => JsonElementType.Number;

        public JsonNumber() => _value = "0";

        public JsonNumber(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (!double.TryParse(value, out double val))
                throw new JsonException($"无效的JSON Number值：{value}");
            _value = value;
        }

        public JsonNumber(int value) => _value = value.ToString();

        public JsonNumber(short value) => _value = value.ToString();

        public JsonNumber(long value) => _value = value.ToString();

        public JsonNumber(ulong value) => _value = value.ToString();

        public JsonNumber(float value) => _value = value.ToString();

        public JsonNumber(double value) => _value = value.ToString();

        public JsonNumber(decimal value) => _value = value.ToString();

        public bool TryGetByte(out byte value) => byte.TryParse(_value, out value);

        public bool TryGetSByte(out sbyte value) => sbyte.TryParse(_value, out value);

        public bool TryGetInt(out int value) => int.TryParse(_value, out value);

        public bool TryGetUInt(out uint value) => uint.TryParse(_value, out value);

        public bool TryGetShort(out short value) => short.TryParse(_value, out value);

        public bool TryGetUShort(out ushort value) => ushort.TryParse(_value, out value);

        public bool TryGetLong(out long value) => long.TryParse(_value, out value);

        public bool TryGetULong(out ulong value) => ulong.TryParse(_value, out value);

        public bool TryGetFloat(out float value) => float.TryParse(_value, out value);

        public bool TryGetDouble(out double value) => double.TryParse(_value, out value);

        public bool TryGetDecimal(out decimal value) => decimal.TryParse(_value, out value);

        public override string ToString() => _value;

        public bool Equals(JsonNumber other) => other != null && this._value.Equals(other._value);

        public override bool Equals(object obj) => obj is JsonNumber jsonNumber && Equals(jsonNumber);

        public override int GetHashCode() => _value.GetHashCode();

        #region Operator

        public static implicit operator JsonNumber(byte value) => new JsonNumber(value);
        public static implicit operator JsonNumber(short value) => new JsonNumber(value);
        public static implicit operator JsonNumber(int value) => new JsonNumber(value);
        public static implicit operator JsonNumber(long value) => new JsonNumber(value);
        public static implicit operator JsonNumber(float value) => new JsonNumber(value);
        public static implicit operator JsonNumber(double value) => new JsonNumber(value);
        public static implicit operator JsonNumber(decimal value) => new JsonNumber(value);

        public static bool operator <(JsonNumber num1, JsonNumber num2)
        {
            double v1 = 0;
            double v2 = 0;
            if (num1.TryGetDouble(out v1) && num2.TryGetDouble(out v2)) return v1 < v2;
            return false;
        }

        public static bool operator >(JsonNumber num1, JsonNumber num2)
        {
            double v1 = 0;
            double v2 = 0;
            if (num1.TryGetDouble(out v1) && num2.TryGetDouble(out v2)) return v1 > v2;
            return false;
        }
        public static bool operator >=(JsonNumber num1, JsonNumber num2)
        {
            double v1 = 0;
            double v2 = 0;
            if (num1.TryGetDouble(out v1) && num2.TryGetDouble(out v2)) return v1 >= v2;
            return false;
        }

        public static bool operator <=(JsonNumber num1, JsonNumber num2)
        {
            double v1 = 0;
            double v2 = 0;
            if (num1.TryGetDouble(out v1) && num2.TryGetDouble(out v2)) return v1 <= v2;
            return false;
        }

        public static bool operator ==(JsonNumber num1, JsonNumber num2)
        {
            double v1 = 0;
            double v2 = 0;
            if (num1.TryGetDouble(out v1) && num2.TryGetDouble(out v2)) return v1 == v2;
            return false;
        }

        public static bool operator !=(JsonNumber num1, JsonNumber num2)
        {
            double v1 = 0;
            double v2 = 0;
            if (num1.TryGetDouble(out v1) && num2.TryGetDouble(out v2)) return v1 != v2;
            return false;
        }

        public static bool operator <(JsonNumber num1, int num2)
        {
            double v1 = 0;
            if (num1.TryGetDouble(out v1)) return v1 < num2;
            return false;
        }

        public static bool operator >(JsonNumber num1, int num2)
        {
            double v1 = 0;
            if (num1.TryGetDouble(out v1)) return v1 > num2;
            return false;
        }
        #endregion
    }
}
