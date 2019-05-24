using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public static class PlayerSaveLoad
{
    public static void SavePlayer(string path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate))
        {
            Player player = GameObject.FindWithTag("Player").gameObject.GetComponent<Player>();
            PlayerData data = new PlayerData(player);
            binaryFormatter.Serialize(fileStream, data);
        }
    }

    public static void LoadPlayer(string path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(path, FileMode.Open))
        {
            PlayerData data = (PlayerData)binaryFormatter.Deserialize(fileStream);
            Player player = GameObject.FindWithTag("Player").gameObject.GetComponent<Player>();
            player.CurrentFuel = data.currentFuel;
            player.MaxFuel = data.maxFuel;
            player.Health = data.health;
            Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
            player.transform.position = position;
        }
    }
}
