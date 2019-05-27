using System;

[Serializable]

public class PlayerData
{
    public float[] position;
    public float currentFuel;
    public float maxFuel;
    public int currentHealth;
    public int maxHealth;

    public PlayerData(GameSession gameSession)
    {
        currentFuel = gameSession.CurrentFuelSpacePlayer;
        maxFuel = gameSession.MaxFuelSpacePlayer;
        currentHealth = gameSession.CurrentHealthSpacePlayer;
        maxHealth = gameSession.MaxHealthSpacePlayer;
        position = new float[3];
        position[0] = gameSession.PositionSpacePlayer.x;
        position[1] = gameSession.PositionSpacePlayer.y;
        position[2] = gameSession.PositionSpacePlayer.z;
    }

}