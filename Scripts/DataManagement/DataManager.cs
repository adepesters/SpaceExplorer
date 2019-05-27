using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    const string folderName = "SavedGameData";
    const string fileExtension = ".dat";
    string folderPath;
    string dataPath;
    string filePath;

    GameSession gameSession;

    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("s");
            if (gameSession.SceneType == "space")
            {
                folderPath = Path.Combine(Application.persistentDataPath, folderName);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                dataPath = Path.Combine(folderPath, "playerData" + fileExtension);
                PlayerSaveLoad.SavePlayer(dataPath);
                dataPath = Path.Combine(folderPath, "planet1Data" + fileExtension);
                Planet1SaveLoad.SavePlanet1(dataPath);
                Debug.Log("Data Space saved");
            }
            else if (gameSession.SceneType == "planet")
            {
                folderPath = Path.Combine(Application.persistentDataPath, folderName);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                dataPath = Path.Combine(folderPath, "planet1Data" + fileExtension);
                Planet1SaveLoad.SavePlanet1(dataPath);
                Debug.Log("Data Planet saved");
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (gameSession.SceneType == "space")
            {
                filePath = Path.Combine(Application.persistentDataPath, folderName, "playerData" + fileExtension);

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

            else if (gameSession.SceneType == "planet")
            {
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

}
