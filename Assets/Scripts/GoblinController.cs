using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
    Attacker attacker;
    Damageable damageable;
    Mover2D mover;
    Vector2 moveDirection = new Vector2(0.0f, 0.0f);

    void Start()
    {
        attacker = GetComponent<Attacker>();
        damageable = GetComponent<Damageable>();
        mover = GetComponent<Mover2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mover.IsFrozen |= damageable.IsDead;
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.y = Input.GetAxis("Vertical");

        mover.SetMoveDirection(moveDirection);

        if (Input.GetKeyDown(KeyCode.C))
        {
            attacker.Attack();
        }
    }
}
