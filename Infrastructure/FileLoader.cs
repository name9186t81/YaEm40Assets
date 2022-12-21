using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class FileLoader
{
    protected readonly string FilePath;

    protected FileLoader(string filePath)
    {
        FilePath = filePath;
    }

    protected string[] ReadOrCreateFile(params string[] content)
    {
        if (!File.Exists(FilePath))
        {
            CreateStandartFile(content);
        }
        return ReadFile();
    }

    protected string[] ReadFile()
    {
        List<string> result = new List<string>();
        using (StreamReader sr = new StreamReader(FilePath))
        {
            while (!sr.EndOfStream)
            {
                result.Add(sr.ReadLine());
            }
        }
        return result.ToArray();
    }

    protected void CreateStandartFile(params string[] content)
    {
        using (StreamWriter sw = new StreamWriter(FileUtils.CreateFileWtihFolders(FilePath)))
        {
            for (int i = 0, length = content.Length; i < length; i++)
            {
                sw.WriteLine(content[i]);
            }
        }
        Debug.Log("Created file with path " + FilePath);
    }
}
