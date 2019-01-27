using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public int attackDamage;
    public string [] attackTargetTags;
    public float attackKnockback;
    public float attackDuration;
    public float attackDelay;

    private Collider2D attackCollider;
    private Nullable<float> attackTime = null;
    private SpriteRenderer attackSprite;

	// Use this for initialization
	void Start () {
        this.attackCollider = this.GetComponent<Collider2D>();
        this.attackSprite = this.GetComponent<SpriteRenderer>();
        this.attackCollider.enabled = false;
        this.attackCollider.isTrigger = true;

        if(this.attackSprite)
            this.attackSprite.enabled = false;
	}

    public void Execute()
    {
        if (this.attackTime == null)
        {
            this.attackTime = Time.time;
        }
    }

    public void Update()
    {
        if (this.attackTime != null) {
            if (
                !this.attackCollider.enabled &&
                this.attackTime + this.attackDelay <= Time.time
            ) {
                this.attackCollider.enabled = true;
                if(this.attackSprite)
                    this.attackSprite.enabled = true;
            } else if (
                this.attackCollider.enabled &&
                this.attackTime + this.attackDelay + this.attackDuration <= Time.time
            ) {
                this.attackCollider.enabled = false;
                if(this.attackSprite)
                    this.attackSprite.enabled = false;
                this.attackTime = null;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string targetTag in this.attackTargetTags)
        {
            if (other.gameObject.CompareTag(targetTag)) {
                Character ch = other.gameObject.GetComponent<Character>();
                ch.Ouch(this.attackCollider, this.attackDamage, this.attackKnockback);
                Debug.Log("Ouch!");
           }
        }
    }
}
