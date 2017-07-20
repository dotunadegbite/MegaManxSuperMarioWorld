using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

    public float gravityModifier = 1f;
    public float minNormalY = 0.65f;

    protected Vector2 targetVelocity;
    protected bool isGrounded;
    protected Vector2 groundNormal;

    protected Vector2 velocity;
    protected Rigidbody2D rb2D;
    
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    private void OnEnable()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
	}

    protected virtual void ComputeVelocity() { }

    private void FixedUpdate()
    {
        velocity += (gravityModifier * Physics2D.gravity * Time.deltaTime);
        velocity.x = targetVelocity.x;

        isGrounded = false;



        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        // Detect movement along x-axis
        Vector2 move = moveAlongGround * deltaPosition.x;
        Movement(move, false);

        //Detect movement along y-axis
        move = Vector2.up * deltaPosition.y;
        Movement(move, true);
    }


    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;
        if (distance > minMoveDistance)
        {
            int count = rb2D.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for(int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            foreach (RaycastHit2D hitBuffer in hitBufferList)
            {
                Vector2 currentNormal = hitBuffer.normal;
                if(currentNormal.y > minNormalY)
                {
                    isGrounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }

                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if(projection < 0)
                {
                    velocity = velocity - (projection * currentNormal); 
                }

                float modifiedDistance = hitBuffer.distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }

        }
        rb2D.position = rb2D.position + move.normalized * distance;

    }
}
