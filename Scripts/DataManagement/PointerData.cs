using System;

[Serializable]

public class PointerData
{
    public float[] position;

    public PointerData(GameSession gameSession)
    {
        position = new float[3];
        position[0] = gameSession.PositionPointer.x;
        position[1] = gameSession.PositionPointer.y;
        position[2] = gameSession.PositionPointer.z;
    }

}