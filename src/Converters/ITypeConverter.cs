﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Rapidity.Json.Converters
{
    public interface ITypeConverter
    {
        Type Type { get; }

        object FromReader(JsonReader reader, JsonOption option);

        object FromElement(JsonElement element, JsonOption option);

        void ToWriter(JsonWriter writer, object obj, JsonOption option);
    }
}