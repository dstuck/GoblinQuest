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
        if (!_knight.IsAttacking)
        {
            planTimer += Time.deltaTime;
            _knight.SetMoveDirection(_patrolDirection);
            if (planTimer > patrolLength)
            {
                _patrolDirection *= -1.0f;
                _knight.Attack();
                planTimer = 0.0f;
            }
        }
    }

    public void OnExit()
    {
    }
}
