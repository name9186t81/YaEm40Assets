using System;
using System.Collections.Generic;
using System.IO;

public static class FileUtils
{
    /// <summary>
    /// Create a new file with givven path or open existing
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static FileStream CreateFileWtihFolders(string path)
    {
        string directoryPath = Path.GetDirectoryName(path);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (!File.Exists(path))
        {
            return File.Create(path);
        }

        return File.OpenRead(path);
    }

    public static void LoadAllFormatsFromFolder(string path, string format, ref List<string> container)
    {
        if (!Directory.Exists(path))
        {
            throw new System.Exception("No directory exists on path " + path);
        }

        var files = Directory.GetFiles(path, "?." + format);
        if (files != null)
        {
            container.AddRange(files);
        }

        var insideFolders = Directory.GetDirectories(path);
        if (insideFolders != null)
        {
            for (int i = 0, length = insideFolders.Length; i < length; i++)
            {
                LoadAllFormatsFromFolder(insideFolders[i], format, ref container);
            }
        }
    }
}
