using UnityEngine;
using UnityEngine.AI;

public class CloseIn : IState
{
    private KnightController _knight;

    public CloseIn(KnightController knight)
    {
        _knight = knight;
    }

    public void OnEnter()
    {
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
    }
}
