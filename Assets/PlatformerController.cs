using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerController : PhysicsObject {

    public float jumpTakeOffSpeed = 7;
    public float maxSpeed = 7f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

	// Use this for initialization
	void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
	}

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
         else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
                velocity.y = velocity.y * 0.5f;
        }

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);


        targetVelocity = move * maxSpeed;

    }
}
