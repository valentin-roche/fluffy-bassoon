using GameState;
using System;

public interface ICommunicateWithGameplay
{
    public void GiveContex(PouchManager pouch, TerrainManager terrainManager);
}
