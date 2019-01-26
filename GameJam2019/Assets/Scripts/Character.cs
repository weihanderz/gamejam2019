using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float speed;

    protected SpriteRenderer spriteRenderer;
    protected BoxCollider2D boxCollider;
    protected Rigidbody2D rb2D;
    protected Animator animator;

	// Use this for initialization
	protected virtual void Start () {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
        this.boxCollider = GetComponent<BoxCollider2D>();
        this.rb2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	protected void Move (Vector3 moveVec) {
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
