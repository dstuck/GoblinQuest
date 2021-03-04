using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    public float speed = 1.0f;
    public int maxHealth = 5;

    public int health { get { return currentHealth; } }
    int currentHealth;

    float planTimer;

    //public float timeInvincible = 1.0f;
    //bool isInvincible;
    //float invincibleTimer;


    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        planTimer = 0.0f;
        horizontal = -1.0f;
        vertical = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        planTimer += Time.deltaTime;

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        //animator.SetFloat("Look X", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        //if (isInvincible)
        //{
        //    invincibleTimer -= Time.deltaTime;
        //    if (invincibleTimer < 0)
        //        isInvincible = false;
        //}

        if (planTimer > 2.0)
        {
            Attack();
            planTimer = 0.0f;
            horizontal = -horizontal;
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
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
