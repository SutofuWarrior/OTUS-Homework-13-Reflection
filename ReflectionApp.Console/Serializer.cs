using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ReflectionApp
{
    /// <summary> Serializer </summary>
    public static class Serializer
    {
        private static char FieldSeparator = ';';
        private static string LineSeparator = Environment.NewLine;

        /// <summary> Serialize from object to CSV </summary>
        /// <param name="obj">any object</param>
        /// <returns>CSV</returns>
        public static string SerializeFromObjectToCSV(object obj)
        {
            Type objType = obj.GetType();
            var lstProperties = objType.GetProperties();

            var lstSerialize = new Dictionary<string, string>();

            for (int i = 0; i < lstProperties.Length; i++)
                lstSerialize.Add(lstProperties[i].Name, lstProperties[i].GetValue(obj).ToString());

            var csvString = new StringBuilder();
            csvString.AppendJoin(FieldSeparator, lstSerialize.Keys).Append(Environment.NewLine);
            csvString.AppendJoin(FieldSeparator, lstSerialize.Values);

            return csvString.ToString();
        }

        /// <summary> Deserialize from CSV to object</summary>
        /// <param name="csv">string in CSV format</param>
        /// <returns>object</returns>
        public static object DeserializeFromCSVToObject<T>(string csv)
            where T : class, new()
        {
            Type objType = typeof(T);
            object obj = new T();

            var lines = csv.Split(LineSeparator);
            var fields = lines[0].Split(FieldSeparator);
            var values = lines[1].Split(FieldSeparator);

            for (int i = 0; i < fields.Length; i++)
            {
                var prop = objType.GetProperty(fields[i]);

                if (prop == null)
                    continue;

                prop.SetValue(obj, Convert.ChangeType(values[i], prop.PropertyType));
            }

            return obj;
        }
    }
}
