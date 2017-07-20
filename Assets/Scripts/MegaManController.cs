using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaManController : MonoBehaviour {

    public float maxSpeed = 10f;
    private bool facingRight = true;
    public float jumpForce = 700f;
    bool isRunning = false;

    Rigidbody2D rb2d;

    Animator anim;

    bool grounded = false;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	
	void FixedUpdate ()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("grounded", grounded);

        anim.SetFloat("vSpeed", rb2d.velocity.y);

        float move = Input.GetAxisRaw("Horizontal");
        rb2d.velocity = new Vector2(move * maxSpeed, rb2d.velocity.y);

      

        anim.SetFloat("speed", Mathf.Abs(move));

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();

	}

    void Update()
    {
        if(grounded && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("grounded", false);
            rb2d.AddForce(new Vector2(0, jumpForce));
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
