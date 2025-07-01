using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SaveGame
{
    public class FileDataService : IDataService
    {
        private ISerializer serializer;
        private string dataPath;
        private string fileExtension;

        public FileDataService(ISerializer serializer)
        {
            this.serializer = serializer;
            this.fileExtension = "json";
            this.dataPath = Application.persistentDataPath;
        }

        string GetPathToFile(string fileName) => Path.Combine(dataPath, string.Concat(fileName, ".", fileExtension));
        public void Save(GameData data, bool overwrite = true)
        {
            string fileLocation = GetPathToFile(data.Name);
            if (!overwrite && File.Exists(fileLocation))
            {
                throw new IOException($"The file '{data.Name}.{fileExtension}' already exists and cannot be overwritten");
            }
            File.WriteAllText(fileLocation, serializer.Serialize(data));
        }

        public GameData Load(string name)
        {
            string fileLocation = GetPathToFile(name);
            if (!File.Exists(fileLocation))
            {
                throw new ArgumentException($"No persisted GameData with name '{name}'");
            }

            return serializer.Deserialize<GameData>(File.ReadAllText(fileLocation));
        }

        public bool CanLoad(string name)
        {
            string fileLocation = GetPathToFile(name);
            return File.Exists(fileLocation);
        }

        public void Delete(string name)
        {
            string fileLocation = GetPathToFile(name);
            if(File.Exists(fileLocation)) File.Delete(fileLocation);
        }

        public void DeleteAll()
        {
            foreach (string filePath in Directory.GetFiles(dataPath))
            {
                File.Delete(filePath);
            }
        }

        public IEnumerable<string> ListSaves()
        {
            foreach (string path in Directory.EnumerateFiles(dataPath))
            {
                if (Path.GetExtension(path) == fileExtension) yield return Path.GetFileNameWithoutExtension(path);
            }
        }
    }
}