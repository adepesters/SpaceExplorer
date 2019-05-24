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
            Planet1 planet1 = GameObject.FindWithTag("Planet1").gameObject.GetComponent<Planet1>();
            Planet1Data data = new Planet1Data(planet1);
            binaryFormatter.Serialize(fileStream, data);
        }
    }

    public static void LoadPlanet1(string path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(path, FileMode.Open))
        {
            Planet1Data data = (Planet1Data)binaryFormatter.Deserialize(fileStream);
            Planet1 planet1 = GameObject.FindWithTag("Planet1").gameObject.GetComponent<Planet1>();
            planet1.GetComponent<YellowRadarActivator>().HasBeenDiscovered = data.hasBeenDiscovered;
            planet1.GetComponentInChildren<WorldMapIcon>().HasBeenDiscovered = data.hasBeenDiscovered;
            planet1.HasBeenCompleted = data.hasBeenCompleted;
            planet1.OpenChests = data.openChests;
        }
    }
}
