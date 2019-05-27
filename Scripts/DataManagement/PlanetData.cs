using System;

[Serializable]

public class PlanetData
{
    public bool hasBeenDiscovered;
    public bool hasBeenCompleted;
    public bool[] openChests = new bool[50];

    int planetID;

    public PlanetData(GameSession gameSession, int planetID)
    {
        hasBeenDiscovered = gameSession.HasBeenDiscovered[planetID];
        hasBeenCompleted = gameSession.HasBeenCompleted[planetID];
        for (int chest = 0; chest < openChests.Length; chest++)
        {
            openChests[chest] = gameSession.OpenChests[planetID, chest];
        }
    }
}
