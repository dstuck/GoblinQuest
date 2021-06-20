using UnityEngine;
using UnityEngine.AI;

public class Dead : IState
{
    public void OnEnter()
    {
        Debug.Log("Entering: " + this.GetType().Name);
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
    }
}
