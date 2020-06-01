﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharePointWebAPI.NetCore.Serialization
{
    /// <summary>
    /// TimeSpanToStringConverter used to serialize TimeSpan to string instead of Struct Timespan which will result in deserialization error.
    /// The reason for this is that .NET Core 3.0 replaced JSON.NET with a new, bult-in JSON serializer, System.Text.Json. 
    /// This serializer doesn't support TimeSpan. The new serializer is faster, doesn't allocate in most cases, but doesn't cover all the cases JSON.NET did.
    /// https://stackoverflow.com/questions/58283761/net-core-3-0-timespan-deserialization-error#58284103
    /// </summary>
    public class TimeSpanToStringConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            TimeSpan.Parse(reader.GetString(), CultureInfo.InvariantCulture);

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options) =>
            writer.WriteStringValue(((TimeSpan)value).ToString(null, CultureInfo.InvariantCulture));
    }
}