using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover2D : MonoBehaviour
{
    public float speed = 1.0f;

    private bool _isFrozen = false;
    public bool IsFrozen { get => _isFrozen; set => _isFrozen = value; }

    float horizontal;
    float vertical;

    Rigidbody2D rigidbody2d;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        horizontal = 0.0f;
        vertical = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetMoveDirection(Vector2 direction, bool changeLook = true)
    {
        if (IsFrozen) { return;  }

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
        if (IsFrozen) { return; }

        lookDirection.Set(direction.x, direction.y);
        lookDirection.Normalize();
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
    }

    void FixedUpdate()
    {
        if (!IsFrozen)
        {
            Vector2 position = rigidbody2d.position;
            position.x = position.x + speed * horizontal * Time.deltaTime;
            position.y = position.y + speed * vertical * Time.deltaTime;

            rigidbody2d.MovePosition(position);
        }
    }
}
