using UnityEngine;
using UnityEngine.AI;

public class Engage : IState
{
    private KnightController _knight;
    private Attacker _attacker;
    private Transform _target;
    private Transform _self;
    private bool facingRight;
    private Vector2 RIGHT = new Vector2(1.0f, 0.0f);
    private Vector2 ZERO = new Vector2(0.0f, 0.0f);
    Vector2 nextTarget;

    private float planTimer;
    private float attackTime = 3.0f;

    static public float attackDistance = 0.8f;

    public float FacingSign { get { return facingRight ? 1.0f : -1.0f; } }

    public Engage(KnightController knight, Transform target)
    {
        _knight = knight;
        _self = _knight.GetComponent<Transform>();
        _attacker = _knight.GetComponent<Attacker>();
        _target = target;
    }

    public void OnEnter()
    {
        Debug.Log("Entering: " + this.GetType().Name);
        planTimer = 0.0f;
        facingRight = _target.position.x > _self.position.x;
        SetFacing();
    }

    public void Tick()
    {
        planTimer += Time.deltaTime;

        nextTarget = _target.position;
        nextTarget.x -= FacingSign * attackDistance;
        _knight.SetMoveDirection(nextTarget - (Vector2) _self.position, false);

        bool newFace = _target.position.x > _self.position.x;
        if(newFace != facingRight)
        {
            facingRight = newFace;
            SetFacing();
        }
        if(planTimer > attackTime)
        {
            _attacker.Attack();
            planTimer = 0.0f;
        }
    }

    private void SetFacing()
    {
        _knight.SetLookDirection(FacingSign * RIGHT);
    }

    public void OnExit()
    {
        _knight.SetMoveDirection(ZERO, false);
    }
}
