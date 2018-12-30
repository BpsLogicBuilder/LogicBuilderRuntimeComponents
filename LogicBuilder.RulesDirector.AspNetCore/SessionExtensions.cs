using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicBuilder.RulesDirector.AspNetCore
{
    internal static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value, JsonSerializerSettings settings = null)
        {
            session.SetString(key, JsonConvert.SerializeObject(value, settings));
        }

        public static T Get<T>(this ISession session, string key, JsonSerializerSettings settings = null)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :
                                  JsonConvert.DeserializeObject<T>(value, settings);
        }
    }
}
