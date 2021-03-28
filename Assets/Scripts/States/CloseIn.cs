using UnityEngine;
using UnityEngine.AI;

public class CloseIn : IState
{
    private KnightController _knight;
    private Transform _target;
    private Transform _self;

    public CloseIn(KnightController knight, Transform target)
    {
        _knight = knight;
        _self = _knight.GetComponent<Transform>();
        _target = target;
    }

    public void OnEnter()
    {
        Debug.Log("Entering: " + this.GetType().Name);
    }

    public void Tick()
    {
        _knight.SetMoveDirection(_target.position - _self.position);
    }

    public void OnExit()
    {
    }
}
