using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public static class SpacePlayerSaveLoad
{
    public static void SaveSpacePlayer(string path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate))
        {
            GameSession gameSession = GameObject.FindWithTag("GameSession").gameObject.GetComponent<GameSession>();
            PlayerData data = new PlayerData(gameSession);
            binaryFormatter.Serialize(fileStream, data);
        }
    }

    public static void LoadSpacePlayer(string path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(path, FileMode.Open))
        {
            PlayerData data = (PlayerData)binaryFormatter.Deserialize(fileStream);
            GameSession gameSession = GameObject.FindWithTag("GameSession").gameObject.GetComponent<GameSession>();
            gameSession.CurrentFuelSpacePlayer = data.currentFuel;
            gameSession.MaxFuelSpacePlayer = data.maxFuel;
            gameSession.CurrentHealthSpacePlayer = data.currentHealth;
            gameSession.MaxHealthSpacePlayer = data.maxHealth;
            Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
            gameSession.PositionSpacePlayer = position;
        }
    }
}
