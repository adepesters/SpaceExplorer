using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public class Planet1SaveLoad
{
    public static void SavePlanet1(string path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate))
        {
            GameSession gameSession = GameObject.FindWithTag("GameSession").gameObject.GetComponent<GameSession>();
            Planet1Data data = new Planet1Data(gameSession);
            binaryFormatter.Serialize(fileStream, data);
        }
    }

    public static void LoadPlanet1(string path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(path, FileMode.Open))
        {
            Planet1Data data = (Planet1Data)binaryFormatter.Deserialize(fileStream);
            GameSession gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
            gameSession.HasBeenCompleted[1] = data.hasBeenCompleted;
            gameSession.HasBeenDiscovered[1] = data.hasBeenDiscovered;
            for (int chest = 0; chest < data.openChests.Length; chest++)
            {
                gameSession.OpenChests[1, chest] = data.openChests[chest];
            }
        }
    }
}
