using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    public float speed = 1.0f;
    public int maxHealth = 5;

    public int health { get { return currentHealth; } }

    private bool isAttacking = false;
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    int currentHealth;



    //public float timeInvincible = 1.0f;
    //bool isInvincible;
    //float invincibleTimer;


    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    private StateMachine _stateMachine;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    Transform player;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        horizontal = -1.0f;
        vertical = 0.0f;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        _stateMachine = new StateMachine();

        var closeIn = new CloseIn(this, player);
        var patrol = new Patrol(this, new Vector2(-1.0f, 0.0f));

        At(patrol, closeIn, () => IsAttacking);
        At(closeIn, patrol, () => Vector2.Distance(player.position, rigidbody2d.position) < 0.2f);
        //At(hasBallState, idle, () => !hasBall);
        //_stateMachine.SetState(closeIn);
        _stateMachine.SetState(patrol);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.Tick();

        //if (isInvincible)
        //{
        //    invincibleTimer -= Time.deltaTime;
        //    if (invincibleTimer < 0)
        //        isInvincible = false;
        //}
    }

    public void SetMoveDirection(Vector2 direction, bool changeLook = true)
    {
        if (direction.SqrMagnitude() > 1.0f)
        {
            direction.Normalize();
        }
        if ((!Mathf.Approximately(direction.x, 0.0f) || !Mathf.Approximately(direction.y, 0.0f)) && changeLook)
        {
            lookDirection.Set(direction.x, direction.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", direction.magnitude);

        horizontal = direction.x;
        vertical = direction.y;
    }

    void FixedUpdate()
    {
        if (!IsAttacking)
        {
            Vector2 position = rigidbody2d.position;
            position.x = position.x + speed * horizontal * Time.deltaTime;
            position.y = position.y + speed * vertical * Time.deltaTime;

            rigidbody2d.MovePosition(position);
        }
    }

    public void Attack()
    {
        //Debug.Log("started attack");
        IsAttacking = true;
        animator.SetTrigger("Attack");
    }

    void CompleteAttack()
    {
        //Debug.Log("completed attack");
        IsAttacking = false;

        //horizontal = -horizontal;
    }

    public void ChangeHealth(int amount)
    {
        //if (amount < 0)
        //{
        //    if (isInvincible)
        //        return;

        //    //animator.SetTrigger("Hit");
        //    isInvincible = true;
        //    invincibleTimer = timeInvincible;
        //}

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }

}
