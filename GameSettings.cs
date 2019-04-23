using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Handles the IO of game settings.
/// </summary>
public class GameSettings : MonoBehaviour
{

    public static readonly string FILE_NAME = "settings.json";
    public int soundLevel;

    public bool validData;

    /// <summary>
    /// Reads from the file and stores the set values.
    /// </summary>
    public void Start()
    {
        validData = File.Exists(FILE_NAME);
        if (!validData)
        {
            return;
        }

        TextReader reader = new StreamReader(FILE_NAME);

        try
        {
            string fileContents = reader.ReadToEnd();
            if (string.IsNullOrEmpty(fileContents))
            {
                validData = false;
                return;
            }

            SettingsFile settings = JsonUtility.FromJson<SettingsFile>(fileContents);
            this.soundLevel = settings.soundLevel;
        }
        finally
        {
            if (reader != null) reader.Close();
        }

    }

    /// <summary>
    /// Writes the variables to file.
    /// </summary>
    public void Save()
    {
        TextWriter writer = null;
        try
        {
            string contents = JsonUtility.ToJson(this);
            writer = new StreamWriter(FILE_NAME, false);
            writer.Write(contents);
        }
        finally
        {
            if (writer != null) writer.Close();
        }
    }

    /// <summary>
    /// Template for the settings file.
    /// </summary>
    class SettingsFile
    {
        public int soundLevel;
    }

}
