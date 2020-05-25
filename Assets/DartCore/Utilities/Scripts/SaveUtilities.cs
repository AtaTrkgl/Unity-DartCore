using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace DartCore.Utilities
{
    public class SaveUtilities : MonoBehaviour
    {
        public static void SaveValue(string fileName, object value)
        {
            if (value == null)
                Debug.LogWarning("Tried to save a null value to the \"" + fileName + "\" save file");

            CreateSaveDirectoryIfNecessary();

            string path = GetPathFromFileName(fileName);

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            binaryFormatter.Serialize(stream, value);
            stream.Close();
        }

        public static object GetValue(string fileName, object defaultValue)
        {
            string path = GetPathFromFileName(fileName);

            if (File.Exists(path))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                var value = binaryFormatter.Deserialize(stream) as object;
                stream.Close();

                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        public static void CleanValue(string fileName)
        {
            string path = GetPathFromFileName(fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private static string GetPathFromFileName(string fileName)
        { 
            return Application.persistentDataPath + "/saves/" + fileName + ".save";
        }

        private static void CreateSaveDirectoryIfNecessary()
        {
            if (!Directory.Exists(Application.persistentDataPath + "/saves"))
                Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }
    }

}
