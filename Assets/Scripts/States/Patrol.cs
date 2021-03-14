using UnityEngine;
using UnityEngine.AI;

public class Patrol : IState
{
    private KnightController _knight;
    private Vector2 _patrolDirection;
    private float planTimer;
    float patrolLength = 2.0f;


    public Patrol(KnightController knight, Vector2 patrolDirection)
    {
        _knight = knight;
        _patrolDirection = patrolDirection.normalized;
    }

    public void OnEnter()
    {
        planTimer = 0.0f;
    }

    public void Tick()
    {
        planTimer += Time.deltaTime;
        if (planTimer > patrolLength)
        {
            _patrolDirection *= -1.0f;
            planTimer = 0.0f;
        }
        _knight.SetMoveDirection(_patrolDirection);
    }

    public void OnExit()
    {
    }
}
