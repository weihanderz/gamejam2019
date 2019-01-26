using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private Animator animator;

	// Use this for initialization
	protected void Start () {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
        this.boxCollider = GetComponent<BoxCollider2D>();
        this.rb2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	protected void FixedUpdate () {
        Vector3 moveVec = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        moveVec *= this.speed * Time.deltaTime;

        //Disable the collider so that linecast doesn't hit this object's own collider.
        this.boxCollider.enabled = false;

        //Cast a line from start point to end point checking collision on blockingLayer.
        Vector3 curr = this.transform.position;
        RaycastHit2D hit = Physics2D.BoxCast(curr, this.boxCollider.size, 0, moveVec, moveVec.magnitude);
        
        //Re-enable collider after linecast
        this.boxCollider.enabled = true;
        
        //Check if anything was hit
        if(hit.transform != null)
        {
            moveVec = moveVec.normalized * hit.distance;
            Debug.Log("HIT!");
        }
        this.rb2D.transform.position += moveVec;
	}
}
