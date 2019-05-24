using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public PlayerData playerData;
    const string folderName = "SavedGameData";
    const string fileExtension = ".dat";

    static void SavePlayer(string path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate))
        {
            Player player = GameObject.FindWithTag("Player").gameObject.GetComponent<Player>();
            PlayerData data = new PlayerData(player);
            binaryFormatter.Serialize(fileStream, data);
        }
    }

    static void LoadPlayer(string path)
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            string folderPath = Path.Combine(Application.persistentDataPath, folderName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string dataPath = Path.Combine(folderPath, "playerData" + fileExtension);

            SavePlayer(dataPath);
            Debug.Log("Data saved");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            string filePath = Path.Combine(Application.persistentDataPath, folderName, "playerData" + fileExtension);

            if (File.Exists(filePath))
            {
                LoadPlayer(filePath);
                Debug.Log("Data loaded");
            }
            else
            {
                Debug.LogError("File does not exist at path " + filePath);
            }
        }
    }

}
