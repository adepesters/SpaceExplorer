using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public class PlanetSaveLoad
{
    public static void SavePlanet(string path, int planetID)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate))
        {
            GameSession gameSession = GameObject.FindWithTag("GameSession").gameObject.GetComponent<GameSession>();
            PlanetData data = new PlanetData(gameSession, planetID);
            binaryFormatter.Serialize(fileStream, data);
        }
    }

    public static void LoadPlanet(string path, int planetID)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(path, FileMode.Open))
        {
            PlanetData data = (PlanetData)binaryFormatter.Deserialize(fileStream);
            GameSession gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
            gameSession.HasBeenCompleted[planetID] = data.hasBeenCompleted;
            gameSession.HasBeenDiscovered[planetID] = data.hasBeenDiscovered;
            for (int chest = 0; chest < data.openChests.Length; chest++)
            {
                gameSession.OpenChests[planetID, chest] = data.openChests[chest];
            }
        }
    }
}
