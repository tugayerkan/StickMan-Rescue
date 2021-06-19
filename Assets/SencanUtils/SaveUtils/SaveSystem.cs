using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEngine;

namespace SencanUtils.SaveUtils
{
    public static class SaveSystem
    {
        private static readonly string PersistentPath = Application.persistentDataPath + "/" + Application.productName;
        private static readonly string PersistentNonerasablePath = Application.persistentDataPath + "/Nonerasable/" + Application.productName;

        private static readonly string AssetPath = Application.dataPath + "/SencanUtils/Saves/";

        private static readonly string AssetPathNonerasablePath = Application.dataPath + "/SencanUtils/Saves/Nonerasable/";

        public enum SaveType
        {
            PlayerPrefs,
            TextFile,
            BINARY,
            JSON
        }

        public enum SavePathType
        {
            Game_Erasable,
            Game_Nonerasable,
            Editor_Erasable,
            Editor_Nonerasable
        }

       public static bool Save<T>(string name, T data, SaveType saveType, SavePathType savePathType = SavePathType.Game_Erasable)
        {
            if (!typeof(T).IsSerializable || (saveType == SaveType.PlayerPrefs && !IsReadable(savePathType)) || (saveType == SaveType.TextFile && typeof(T) != typeof(string)))
                return false;
            if (!Directory.Exists(GetPath(savePathType)))
                Directory.CreateDirectory(GetPath(savePathType));
            
            if (saveType == SaveType.PlayerPrefs)
                return SavePlayerPrefs(name, data);
            if (saveType == SaveType.TextFile)
                return SaveText(name, data?.ToString(), GetPath(savePathType));
            if (saveType == SaveType.BINARY)
                return SaveBinary(name, data, GetPath(savePathType));
            return SaveJson(name, data, GetPath(savePathType));
        }
       
       public static bool Save<T>(string name, T data, SaveType saveType, string customPath)
       {
           if (!typeof(T).IsSerializable || saveType == SaveType.PlayerPrefs || (saveType == SaveType.TextFile && typeof(T) != typeof(string)))
               return false;
           if (!Directory.Exists(customPath))
               Directory.CreateDirectory(customPath);

           if (saveType == SaveType.TextFile)
               return SaveText(name, data?.ToString(), customPath);
           if (saveType == SaveType.BINARY)
               return SaveBinary(name, data, customPath);
           return SaveJson(name, data, customPath);
       }

       public static T Load<T>(string name, SaveType saveType, SavePathType savePathType = SavePathType.Game_Erasable)
        {
            if (!typeof(T).IsSerializable || !Directory.Exists((GetPath(savePathType))) || (saveType == SaveType.PlayerPrefs && !IsReadable(savePathType)) || (saveType == SaveType.TextFile && typeof(T) != typeof(string)))
                return default;
            
            if (saveType == SaveType.PlayerPrefs)
                return LoadPlayerPrefs<T>(name);
            if (saveType == SaveType.TextFile)
                return (T)(object)LoadText(name, GetPath(savePathType));
            if (saveType == SaveType.BINARY)
                return LoadBinary<T>(name, GetPath(savePathType));

            return LoadJson<T>(name, GetPath(savePathType));
        }
       
       public static T Load<T>(string name, SaveType saveType, string customPath)
       {
           if (!typeof(T).IsSerializable || !Directory.Exists(customPath) || saveType == SaveType.PlayerPrefs || (saveType == SaveType.TextFile && typeof(T) != typeof(string)))
               return default;
           
           if (saveType == SaveType.TextFile)
               return (T)(object)LoadText(name, customPath);
           if (saveType == SaveType.BINARY)
               return LoadBinary<T>(name, customPath);

           return LoadJson<T>(name, customPath);
       }

        public static bool HasSaveFile(string name, SaveType saveType, SavePathType savePathType)
        {
            if (saveType == SaveType.PlayerPrefs && !IsReadable(savePathType))
                return false;
            
            if (saveType == SaveType.PlayerPrefs)
                return PlayerPrefs.HasKey(name);

            var path = GetPath(savePathType) + "/" + name;
            if (saveType == SaveType.BINARY)
                path += ".save";
            else if (saveType == SaveType.JSON)
                path += "json";
            else if (saveType == SaveType.TextFile)
                path += ".txt";

            return File.Exists(path);
        }
        
        public static bool HasSaveFile(string name, SaveType saveType, string path)
        {
            if (saveType == SaveType.BINARY)
                path += ".save";
            else if (saveType == SaveType.JSON)
                path += "json";
            else if (saveType == SaveType.TextFile)
                path += ".txt";

            return File.Exists(path);
        }

        public static void DeleteAllData(bool includeNonErasable = false, params string[] customPaths)
        {
            PlayerPrefs.DeleteAll();
            
            if(Directory.Exists(GetPath(SavePathType.Game_Erasable)))
                Directory.Delete(GetPath(SavePathType.Game_Erasable), true);
            
            if(Directory.Exists(GetPath(SavePathType.Editor_Erasable)))
                Directory.Delete(GetPath(SavePathType.Editor_Erasable), true);

            if (includeNonErasable)
            {
                if(Directory.Exists(GetPath(SavePathType.Editor_Nonerasable)))
                    Directory.Delete(GetPath(SavePathType.Editor_Nonerasable), true);
                if(Directory.Exists(GetPath(SavePathType.Game_Nonerasable)))
                    Directory.Delete(GetPath(SavePathType.Game_Nonerasable), true);
            }
            
            foreach (var customPath in customPaths)
            {
                if(Directory.Exists(customPath))
                    Directory.Delete(customPath, true);
            }
        }

        private static bool SavePlayerPrefs<T>(string name, T data)
        {
            var type = typeof(T);
            
            if (type == typeof(int))
            {
                PlayerPrefs.SetInt(name, int.Parse(data.ToString()));
                PlayerPrefs.Save();
                return true;
            }
            if (type == typeof(float))
            {
                PlayerPrefs.SetFloat(name, float.Parse(data.ToString()));
                PlayerPrefs.Save();
                return true;
            }
            if (type == typeof(string))
            {
                PlayerPrefs.SetString(name, data.ToString());
                PlayerPrefs.Save();
                return true;
            }

            if (type == typeof(bool))
            {
                PlayerPrefs.SetInt(name, data.ToString() == "True" ? 1 : 0);
                PlayerPrefs.Save();
                return true;
            }

            return false;
        }

        private static bool SaveText(string name, string data, string path)
        {
            File.WriteAllText(path + "/" + name + ".txt", data);
            return true;
        }

        private static bool SaveBinary<T>(string name, T data, string path)
        {
            FileStream file = File.Create(path + "/" + name + ".save");
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);
            file.Close();
        
            return true;
        }

        private static bool SaveJson<T>(string name, T data, string path)
        {
            File.WriteAllText(path + "/" + name + ".json", JsonConvert.SerializeObject(data));
            return true;
        }
        
        private static string LoadText(string name, string path)
        {
            if (!File.Exists(path + "/" + name + ".txt"))
                return string.Empty;
            
            return File.ReadAllText(path + "/" + name + ".txt");
        }

        private static T LoadBinary<T>(string name, string path)
        {
            if (!File.Exists(path + "/" + name + ".save"))
                return default;
        
            FileStream file = File.OpenRead(path + "/" + name + ".save");
            BinaryFormatter bf = new BinaryFormatter();
            T data = (T)bf.Deserialize(file);
            file.Close();
            return data;
        }

        private static T LoadJson<T>(string name, string path)
        {
            if (!File.Exists(path + "/" + name + ".json"))
                return default;

            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path + "/" + name + ".json"));
        }

        private static T LoadPlayerPrefs<T>(string name)
        {
            var type = typeof(T);

            if (type == typeof(int))
                return (T) (object) PlayerPrefs.GetInt(name);

            if (type == typeof(float))
                return (T) (object) PlayerPrefs.GetFloat(name);

            if (type == typeof(string))
                return (T) (object) PlayerPrefs.GetString(name);

            if (type == typeof(bool))
                return (T) (object) (PlayerPrefs.GetInt(name) == 1 ? true : false);
            
            return default;
        }

        //Utils
        private static bool IsReadable(SavePathType savePathType)
        {
            return (savePathType == SavePathType.Game_Erasable ||
                    savePathType == SavePathType.Editor_Erasable);
        }
        
        private static string GetPath(SavePathType savePathType)
        {
            if (savePathType == SavePathType.Game_Erasable)
                return PersistentPath;
            if (savePathType == SavePathType.Game_Nonerasable)
                return PersistentNonerasablePath;
            if (savePathType == SavePathType.Editor_Erasable)
                return AssetPath;
            if (savePathType == SavePathType.Editor_Nonerasable)
                return AssetPathNonerasablePath;

            return string.Empty;
        }
    }
}
