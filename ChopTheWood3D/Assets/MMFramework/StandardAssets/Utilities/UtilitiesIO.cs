using Boomlagoon.JSON;
using MildMania.Scripts.GeneralHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

static partial class Utilities
{
    public static string CombinePaths(params string[] paths)
    {
        if (paths == null)
        {
            throw new ArgumentNullException("paths");
        }
        return paths.Aggregate(Path.Combine);
    }

    public static void SaveJSONToFile(string filePath, string serializedJSON, bool canFormat = false)
    {
        string extension = "";
        if (!filePath.EndsWith(".json", StringComparison.Ordinal))
            extension = ".json";

        filePath += extension;

        if (canFormat)
            serializedJSON = JsonHelper.FormatJson(serializedJSON);

        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(serializedJSON);
                writer.Close();
                writer.Dispose();
            }

            fs.Close();
            fs.Dispose();
        }

#if UNITY_IOS
        UnityEngine.iOS.Device.SetNoBackupFlag(Path.GetDirectoryName(filePath));
#endif
    }

    public static void SaveTextToFile(string filePath, string text)
    {
        string extension = "";
        if (!filePath.EndsWith(".txt", StringComparison.Ordinal))
            extension = ".txt";

        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        File.WriteAllText(filePath + extension, text);

#if UNITY_IOS
        UnityEngine.iOS.Device.SetNoBackupFlag(filePath + ".txt");
#endif
    }

    public static void SaveJSONToFile(string filePath, JSONObject saveObject, bool canFormat = false)
    {
        string extension = "";
        if (!filePath.EndsWith(".json", StringComparison.Ordinal))
            extension = ".json";
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        string jsonStr = saveObject.ToString();

        if (canFormat)
            jsonStr = JsonHelper.FormatJson(jsonStr);

        File.WriteAllText(filePath + extension, jsonStr);

#if UNITY_IOS
        UnityEngine.iOS.Device.SetNoBackupFlag(Path.GetDirectoryName(filePath));
#endif
    }

    public static void DeleteJSONFile(string filePath)
    {
        string extension = "";
        if (!filePath.EndsWith(".json", StringComparison.Ordinal))
            extension = ".json";

        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            return;

        if (File.Exists(filePath + extension))
            File.Delete(filePath + extension);
    }

    public static JSONObject LoadResourceJSON(string filePath)
    {
        TextAsset ta = Resources.Load(filePath) as TextAsset;
        if (ta == null)
            return null;
        return JSONObject.Parse(ta.text);
    }

    public static JSONObject LoadJSONFile(string filePath)
    {
        string extension = "";
        if (!filePath.EndsWith(".json", StringComparison.Ordinal))
            extension = ".json";
        if (!File.Exists(filePath + extension))
            return null;

        string content = File.ReadAllText(filePath + extension);

        return JSONObject.Parse(content);
    }

    public static void LoadTXTText(string filePath, out string loadedText)
    {
        string extension = "";
        if (!filePath.EndsWith(".txt", StringComparison.Ordinal))
            extension = ".txt";

        if (!FileExist(filePath + extension))
        {
            loadedText = "";
            return;
        }

        using (FileStream fs = new FileStream(filePath + extension, FileMode.Open))
        {
            using (StreamReader reader = new StreamReader(fs))
            {
                loadedText = reader.ReadToEnd();

                reader.Close();
                reader.Dispose();
            }

            fs.Close();
            fs.Dispose();
        }
    }

    public static void LoadJSONText(string filePath, out string loadedText)
    {
        string extension = "";
        if (!filePath.EndsWith(".json", StringComparison.Ordinal))
            extension = ".json";

        if (!FileExist(filePath + extension))
        {
            loadedText = "";
            return;
        }

        using (FileStream fs = new FileStream(filePath + extension, FileMode.Open))
        {
            using (StreamReader reader = new StreamReader(fs))
            {
                loadedText = reader.ReadToEnd();

                reader.Close();
                reader.Dispose();
            }

            fs.Close();
            fs.Dispose();
        }
    }

    public static void LoadAllTexts(string folderPath, out List<string> loadedTextList)
    {
        loadedTextList = new List<string>();

        if (!Directory.Exists(folderPath))
            return;

        string[] fileEntries = GetFiles(folderPath, "*.json|*.txt", SearchOption.AllDirectories);

        foreach (string fileEntry in fileEntries)
        {
            string contents = File.ReadAllText(fileEntry);

            if (!string.IsNullOrEmpty(contents))
            {
                loadedTextList.Add(contents);
            }
        }
    }

    static string[] GetFiles(string sourceFolder, string filters, SearchOption searchOption)
    {
        return filters.Split('|').SelectMany(filter => Directory.GetFiles(sourceFolder, filter, searchOption)).ToArray();
    }

    public static bool FileExist(string fileFullPath)
    {
        return File.Exists(fileFullPath);
    }
}
