using UnityEngine;
using UnityEngine.AI;

public class Engage : IState
{
    private KnightController _knight;
    private Transform _target;
    private Transform _self;
    private bool facingRight;
    private Vector2 RIGHT = new Vector2(1.0f, 0.0f);
    Vector2 nextTarget;

    static public float attackDistance = 0.8f;

    public float FacingSign { get { return facingRight ? 1.0f : -1.0f; } }

    public Engage(KnightController knight, Transform target)
    {
        _knight = knight;
        _self = _knight.GetComponent<Transform>();
        _target = target;
    }

    public void OnEnter()
    {
        Debug.Log("Entering: " + this.GetType().Name);
        facingRight = _target.position.x > _self.position.x;
        SetFacing();
    }

    public void Tick()
    {
        nextTarget = _target.position;
        nextTarget.x -= FacingSign * attackDistance;
        _knight.SetMoveDirection(nextTarget - (Vector2) _self.position, false);

        bool newFace = _target.position.x > _self.position.x;
        if(newFace != facingRight)
        {
            facingRight = newFace;
            SetFacing();
        }
    }

    private void SetFacing()
    {
        _knight.SetLookDirection(FacingSign * RIGHT);
    }

    public void OnExit()
    {
    }
}
