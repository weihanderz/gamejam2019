using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Character : MonoBehaviour
{
    // use some magic-ass fucking numbers that give a decent feel for this
    private static float MAX_VELOCITY_PRECISION_SQUARED = 9.0f;
    private static float IMPACT_BUFFER = 0.05f;
    private static float DRAG_COEFFICIENT = 0.9f;

    public int health;
    public float speed = 1;
    public float dmgInvulnerabilityTime = 0.5f;

    protected SpriteRenderer spriteRenderer;
    protected BoxCollider2D boxCollider;
    protected Rigidbody2D rb2D;
    protected Animator animator;
    protected Vector3 velocity;

    private float invulnerableStartTime = 0f;
    private bool invulnerable = false;

	// Use this for initialization
	protected virtual void Start () {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
        this.boxCollider = GetComponent<BoxCollider2D>();
        this.rb2D = GetComponent<Rigidbody2D>();
        this.velocity = new Vector3();
	}

    protected virtual void FixedUpdate()
    {
        if (this.velocity.sqrMagnitude > MAX_VELOCITY_PRECISION_SQUARED)
        {
            Debug.Log(this.velocity.sqrMagnitude);
            float drag = velocity.sqrMagnitude * DRAG_COEFFICIENT * Time.deltaTime;
            this.velocity -= velocity.normalized * drag;
        }
        else
        {
            this.velocity.Set(0, 0, 0);
        }

        if (
            this.invulnerable &&
            this.invulnerableStartTime + this.dmgInvulnerabilityTime <= Time.time
        ) {
            this.invulnerable = false;
        }
    }

    // Update is called once per frame
    protected void Move(Vector3 moveVec) {
        moveVec = (moveVec * this.speed * Time.deltaTime) + (velocity * Time.deltaTime);

        //Disable the collider so that linecast doesn't hit this object's own collider.
        this.boxCollider.enabled = false;

        //Cast a line from start point to end point checking collision on blockingLayer.
        Vector3 curr = this.transform.position;
        RaycastHit2D [] hit = Physics2D.BoxCastAll(curr, this.boxCollider.size, 0, moveVec, moveVec.magnitude);
        
        //Re-enable collider after linecast
        this.boxCollider.enabled = true;

        //Check if anything was hit
        if (hit.Length > 0)
        {
            // Don't allow running into envorment objects
            foreach (RaycastHit2D target in hit) {
                if (target.collider.gameObject.CompareTag("Environment")) {
                    moveVec = (target.distance - IMPACT_BUFFER) * moveVec.normalized;
                    break;
                }
            }
        }

        this.rb2D.transform.position += moveVec;
	}

    public void Ouch(Collider2D attackCollider, int damage, float impact)
    {
        Vector3 impactVector = this.rb2D.transform.position - attackCollider.transform.position;
        this.velocity = impactVector.normalized * impact;

        if (!this.invulnerable)
        {
            this.invulnerable = true;
            this.invulnerableStartTime = Time.time;

            this.health -= damage;
            if (this.health < 0)
            {
                this.Kill();
            }
        }
    }

    public void Kill()
    {
        GameObject.Destroy(this.gameObject);
    }
}
