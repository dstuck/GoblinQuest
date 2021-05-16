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

    const float ENGAGEMENT_RANGE = 1.0f;


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

    // COMBAT
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;


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
        var engage = new Engage(this, player);
        var patrol = new Patrol(this, new Vector2(-1.0f, 0.0f));

        At(patrol, closeIn, () => IsAttacking);
        At(closeIn, engage, () => Vector2.Distance(player.position, rigidbody2d.position) < ENGAGEMENT_RANGE);

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

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            //enemy.GetComponent<Damageable>
            Debug.Log("We hit " + enemy.name);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void CompleteAttack()
    {
        //Debug.Log("completed attack");
        IsAttacking = false;
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
