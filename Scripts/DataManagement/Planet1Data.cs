using System;

[Serializable]

public class Planet1Data
{
    public bool hasBeenDiscovered;
    public bool hasBeenCompleted;
    public bool[] openChests;

    public Planet1Data(Planet1 planet)
    {
        hasBeenDiscovered = planet.HasBeenDiscovered;
        hasBeenCompleted = planet.HasBeenCompleted;
        openChests = planet.OpenChests;
    }
}
