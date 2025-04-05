using System;

public interface ICommunicateWithGameplay
{
    public void GiveContex();

    public event Action SendDataSelection;
}
