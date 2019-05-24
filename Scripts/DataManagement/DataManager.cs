using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    const string folderName = "SavedGameData";
    const string fileExtension = ".dat";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            string folderPath = Path.Combine(Application.persistentDataPath, folderName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string dataPath = Path.Combine(folderPath, "playerData" + fileExtension);
            PlayerSaveLoad.SavePlayer(dataPath);
            dataPath = Path.Combine(folderPath, "planet1Data" + fileExtension);
            Planet1SaveLoad.SavePlanet1(dataPath);
            Debug.Log("Data saved");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            string filePath = Path.Combine(Application.persistentDataPath, folderName, "playerData" + fileExtension);

            if (File.Exists(filePath))
            {
                PlayerSaveLoad.LoadPlayer(filePath);
                Debug.Log("Data player loaded");
            }
            else
            {
                Debug.LogError("File does not exist at path " + filePath);
            }

            filePath = Path.Combine(Application.persistentDataPath, folderName, "planet1Data" + fileExtension);

            if (File.Exists(filePath))
            {
                Planet1SaveLoad.LoadPlanet1(filePath);
                Debug.Log("Data planet1 loaded");
            }
            else
            {
                Debug.LogError("File does not exist at path " + filePath);
            }
        }
    }

}
