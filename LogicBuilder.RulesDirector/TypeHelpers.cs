using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LogicBuilder.RulesDirector
{
    public static class TypeHelpers
    {
        internal static bool TryParse(this string toParse, Type type, out object result)
        {
            if (type == null)
            {
                result = null;
                return false;
            }

            if (type == typeof(string))
            {
                result = toParse;
                return true;
            }

            if (type.IsNullable())
                type = Nullable.GetUnderlyingType(type);

            MethodInfo method = type.GetMethods().Single(s => s.Name == "TryParse" && s.GetParameters().Length == 2);

            object[] args = new object[] { toParse, null };
            bool success = (bool)method.Invoke(null, args);
            result = success ? args[1] : null;

            return success;
        }

        internal static bool CanBeAssignedNull(this Type type) 
            => !type.IsValueType || type.IsNullable();

        private static Dictionary<Type, HashSet<Type>> NumbersDictionary = new Dictionary<Type, HashSet<Type>>()
        {
            { typeof(decimal), new HashSet<Type> { typeof(byte), typeof(sbyte), typeof(char), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong) } },
            { typeof(double), new HashSet<Type> { typeof(byte), typeof(sbyte), typeof(char), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float) } },
            { typeof(float), new HashSet<Type> { typeof(byte), typeof(sbyte), typeof(char), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong) } },
            { typeof(ulong), new HashSet<Type> { typeof(byte), typeof(char), typeof(ushort), typeof(uint) } },
            { typeof(long), new HashSet<Type> { typeof(byte), typeof(sbyte), typeof(char), typeof(short), typeof(ushort), typeof(int), typeof(uint) } },
            { typeof(uint), new HashSet<Type> { typeof(byte), typeof(char), typeof(ushort) } },
            { typeof(int), new HashSet<Type> { typeof(byte), typeof(sbyte), typeof(char), typeof(short), typeof(ushort) } },
            { typeof(ushort), new HashSet<Type> { typeof(byte), typeof(char) } },
            { typeof(short), new HashSet<Type> { typeof(byte), typeof(sbyte) } }
        };

        internal static bool IsNullable(this Type type) 
            => type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));

        public static bool AssignableFrom(this Type to, Type from)
        {
            if (to.IsAssignableFrom(from))
                return true;

            if (!(!to.IsNullable() && from.IsNullable()))
            {//Anything but To is NOT nullable and From IS nullable
                to = to.IsNullable() ? Nullable.GetUnderlyingType(to) : to;
                from = from.IsNullable() ? Nullable.GetUnderlyingType(from) : from;

                if (NumbersDictionary.ContainsKey(to) && NumbersDictionary[to].Contains(from))
                    return true;
            }

            bool ReturnTypeValid(Type returnType) => returnType == to || (NumbersDictionary.ContainsKey(to) && NumbersDictionary[to].Contains(returnType));
            bool ParameterValid(Type parameterType) => (parameterType == from) || (NumbersDictionary.ContainsKey(parameterType) && NumbersDictionary[parameterType].Contains(from));
            bool MatchImplicitOperator(MethodInfo m) => m.Name == "op_Implicit"
                                                        && ReturnTypeValid(m.ReturnType)
                                                        && ParameterValid(m.GetParameters().Single().ParameterType);

            return from.GetMethods(BindingFlags.Public | BindingFlags.Static).Any(MatchImplicitOperator)
                    || to.GetMethods(BindingFlags.Public | BindingFlags.Static).Any(MatchImplicitOperator);
        }
    }
}
