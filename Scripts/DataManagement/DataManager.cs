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
    GameObject[] planets;

    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        planets = GameObject.FindGameObjectsWithTag("Planet");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (gameSession.SceneType == "space")
            {
                folderPath = Path.Combine(Application.persistentDataPath, folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // saving space player
                dataPath = Path.Combine(folderPath, "playerData" + fileExtension);
                PlayerSaveLoad.SavePlayer(dataPath);

                // saving planets
                foreach (GameObject planet in planets)
                {
                    int planetID = planet.GetComponent<Planet>().PlanetID;
                    dataPath = Path.Combine(folderPath, "planet" + planetID + "Data" + fileExtension);
                    PlanetSaveLoad.SavePlanet(dataPath, planetID);
                }

                Debug.Log("Data Space saved");
            }
            else if (gameSession.SceneType == "planet")
            {
                folderPath = Path.Combine(Application.persistentDataPath, folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // saving current planet
                int planetID = gameSession.CurrentPlanetID;
                dataPath = Path.Combine(folderPath, "planet" + planetID + "Data" + fileExtension);
                PlanetSaveLoad.SavePlanet(dataPath, planetID);
                Debug.Log("Data Planet saved");
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (gameSession.SceneType == "space")
            {
                // loading space player
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

                // loading planets
                foreach (GameObject planet in planets)
                {
                    int planetID = planet.GetComponent<Planet>().PlanetID;
                    filePath = Path.Combine(Application.persistentDataPath, folderName, "planet" + planetID + "Data" + fileExtension);

                    if (File.Exists(filePath))
                    {
                        PlanetSaveLoad.LoadPlanet(filePath, planetID);
                        Debug.Log("Data planet loaded");
                    }
                    else
                    {
                        Debug.LogError("File does not exist at path " + filePath);
                    }
                }
            }

            else if (gameSession.SceneType == "planet")
            {
                // loading current planet
                int planetID = gameSession.CurrentPlanetID;
                filePath = Path.Combine(Application.persistentDataPath, folderName, "planet" + planetID + "Data" + fileExtension);

                if (File.Exists(filePath))
                {
                    PlanetSaveLoad.LoadPlanet(filePath, planetID);
                    Debug.Log("Data planet loaded");
                }
                else
                {
                    Debug.LogError("File does not exist at path " + filePath);
                }
            }
        }
    }

}
