using MatrixFinder.Matrices;
using MatrixFinder.Matrices.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace MatrixFinder.HelpTools
{
    public static class JsonHelper
    {
        private const string START_STRING = "{\"Items\": ";
        private const string END_STRING = "}";

        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(START_STRING + json + END_STRING);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;

            var jsonString = JsonUtility.ToJson(wrapper, true);

            Save(jsonString);

            return jsonString;
        }

        /// <summary>
        /// Extract matrices from json located in Resources folder.
        /// </summary>
        /// <param name="jsonName">Name of the json file without an extension</param>
        /// <returns></returns>
        public static IEnumerable<Matrix4x4> ExtractMatricesFromJson(string jsonName)
        {
            var jsonString = Resources.Load<TextAsset>(jsonName);

            var matrixDataArray = FromJson<MatrixData>(jsonString.text);

            return matrixDataArray.Select(matrix => matrix.ToMatrix4x4());
        }

        private static void Save(string jsonString)
        {
            var dt = DateTime.Now;
            File.WriteAllText(Application.dataPath + $"/Resources/result{dt.Day}{dt.Month}{dt.Year}{dt.Hour}{dt.Minute}{dt.Second}.json", jsonString);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }

    }
}