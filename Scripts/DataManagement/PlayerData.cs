using System;

[Serializable]

public class PlayerData
{
    public float[] position;
    public float currentFuel;
    public float maxFuel;
    public int health;

    public PlayerData(Player player)
    {
        currentFuel = player.CurrentFuel;
        maxFuel = player.MaxFuel;
        health = player.Health;
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }

}