using System;

[Serializable]

public class Planet1Data
{
    public bool hasBeenDiscovered;
    public bool hasBeenCompleted;
    public bool[] openChests = new bool[2];

    public Planet1Data(GameSession gameSession)
    {
        hasBeenDiscovered = gameSession.HasBeenDiscovered[1];
        hasBeenCompleted = gameSession.HasBeenCompleted[1];
        for (int chest = 0; chest < openChests.Length; chest++)
        {
            openChests[chest] = gameSession.OpenChests[1, chest];
        }
    }
}
