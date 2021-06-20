using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    const float ENGAGEMENT_RANGE = 1.0f;

    Rigidbody2D rigidbody2d;

    private StateMachine _stateMachine;

    Attacker attacker;
    Mover2D mover;
    Transform player;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        attacker = GetComponent<Attacker>();
        mover = GetComponent<Mover2D>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        _stateMachine = new StateMachine();

        var closeIn = new CloseIn(this, player);
        var engage = new Engage(this, player);
        var patrol = new Patrol(this, new Vector2(-1.0f, 0.0f));

        At(patrol, closeIn, () => attacker.IsAttacking);
        At(closeIn, engage, () => Vector2.Distance(player.position, rigidbody2d.position) < ENGAGEMENT_RANGE);

        _stateMachine.SetState(patrol);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.Tick();
        mover.IsFrozen = attacker.IsAttacking;
    }

    public void SetMoveDirection(Vector2 direction, bool changeLook = true)
    {
        mover.SetMoveDirection(direction, changeLook);
    }

    public void SetLookDirection(Vector2 direction)
    {
        mover.SetLookDirection(direction);
    }

}
