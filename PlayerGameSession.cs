using System;
using System.IO;
using UnityEngine;

/// <summary>
/// File handling the IO of PlayerGameSession so it can be stored between levels.
/// </summary>
public class PlayerGameSession {

    public static readonly string FILE_NAME = "session.json";

    public int lives;
    public int health;
    public int collectedCodes;
    public string respawnLevel;

    public bool validData;

    /// <summary>
    /// Empty constructor.
    /// </summary>
    public PlayerGameSession()
    {
    }

    /// <summary>
    /// Loads the contents in. If the file doesn't exist, nothing will be loaded.
    /// </summary>
    public void Load()
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

            PlayerGameSession session = JsonUtility.FromJson<PlayerGameSession>(fileContents);
            this.lives = session.lives;
            this.health = session.health;
            this.collectedCodes = session.collectedCodes;
            this.respawnLevel = session.respawnLevel;
        }
        finally
        {
            if (reader != null) reader.Close();
        }

        if (this.lives == 0) validData = false;
    }

    /// <summary>
    /// Updates the file with the current status of the game.
    /// </summary>
    public void UpdateSession(PlayerController playerController, string respawnLevel)
    {
        this.lives = playerController.playerHealth.Lives;
        this.health = playerController.playerHealth.Health;
        this.collectedCodes = playerController.collectedCodes;
        
        if (!String.IsNullOrEmpty(respawnLevel)) this.respawnLevel = respawnLevel;

        TextWriter writer = null;
        try {
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
    /// Deletes the file, it will be deleted between sessions.
    /// </summary>
    public void Reset()
    {
        if (!File.Exists(FILE_NAME)) return;
        File.Delete(FILE_NAME);
    }

}
