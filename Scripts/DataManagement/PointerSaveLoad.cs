using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public static class PointerSaveLoad
{
    public static void SavePointer(string path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate))
        {
            GameSession gameSession = GameObject.FindWithTag("GameSession").gameObject.GetComponent<GameSession>();
            PointerData data = new PointerData(gameSession);
            binaryFormatter.Serialize(fileStream, data);
        }
    }

    public static void LoadPointer(string path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(path, FileMode.Open))
        {
            PointerData data = (PointerData)binaryFormatter.Deserialize(fileStream);
            GameSession gameSession = GameObject.FindWithTag("GameSession").gameObject.GetComponent<GameSession>();
            Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
            gameSession.PositionPointer = position;
            Pointer pointer = GameObject.FindWithTag("Pointer").gameObject.GetComponent<Pointer>();
            pointer.transform.position = position;
        }
    }
}
