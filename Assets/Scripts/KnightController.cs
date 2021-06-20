using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    public float speed = 1.0f;


    const float ENGAGEMENT_RANGE = 1.0f;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    private StateMachine _stateMachine;

    Attacker attacker;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    Transform player;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        attacker = GetComponent<Attacker>();
        horizontal = -1.0f;
        vertical = 0.0f;

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
    }

    public void SetMoveDirection(Vector2 direction, bool changeLook = true)
    {
        if (direction.SqrMagnitude() > 1.0f)
        {
            direction.Normalize();
        }
        if (changeLook && (!Mathf.Approximately(direction.x, 0.0f) || !Mathf.Approximately(direction.y, 0.0f)))
        {
            SetLookDirection(direction);
        }

        animator.SetFloat("Speed", direction.magnitude);

        horizontal = direction.x;
        vertical = direction.y;
    }

    public void SetLookDirection(Vector2 direction)
    {
        lookDirection.Set(direction.x, direction.y);
        lookDirection.Normalize();
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
    }

    void FixedUpdate()
    {
        if (!attacker.IsAttacking)
        {
            Vector2 position = rigidbody2d.position;
            position.x = position.x + speed * horizontal * Time.deltaTime;
            position.y = position.y + speed * vertical * Time.deltaTime;

            rigidbody2d.MovePosition(position);
        }
    }
}
