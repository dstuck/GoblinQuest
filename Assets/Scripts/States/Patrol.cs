using UnityEngine;
using UnityEngine.AI;

public class Patrol : IState
{
    private KnightController _knight;
    private Attacker _attacker;
    private Vector2 _patrolDirection;
    private float planTimer;
    float patrolLength = 2.0f;


    public Patrol(KnightController knight, Vector2 patrolDirection)
    {
        _knight = knight;
        _patrolDirection = patrolDirection.normalized;
        _attacker = _knight.GetComponent<Attacker>();
    }

    public void OnEnter()
    {
        planTimer = 0.0f;
        Debug.Log("Entering: " + this.GetType().Name);
    }

    public void Tick()
    {
        if (!_attacker.IsAttacking)
        {
            planTimer += Time.deltaTime;
            _knight.SetMoveDirection(_patrolDirection);
            if (planTimer > patrolLength)
            {
                _patrolDirection *= -1.0f;
                _attacker.Attack();
                planTimer = 0.0f;
            }
        }
    }

    public void OnExit()
    {
    }
}
