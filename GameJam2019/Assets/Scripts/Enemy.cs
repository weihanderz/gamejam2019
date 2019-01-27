using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    public int touchDamage = 10;
    public float touchKnockback = 10;

    public float huntRadius = 5.0f;

    private Attack attackMelee;
    private float attackMeleeRadius;
    private CircleCollider2D hurtBox;

	// Use this for initialization
	protected override void Start () {
        base.Start();

        this.hurtBox = this.GetComponent<CircleCollider2D>();
        this.attackMelee = this.transform.Find("MeleeAttack").GetComponent<Attack>();
        this.attackMeleeRadius = this.attackMelee.GetComponent<CircleCollider2D>().radius;
	}

	// Update is called once per frame
	protected override void FixedUpdate () {
        base.FixedUpdate();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPos = playerObj.transform.position;
        float playerDistance = Vector2.Distance(this.transform.position, playerPos);

        if (playerDistance < this.attackMeleeRadius)
            this.AttackMelee();
        else if (playerDistance < huntRadius)
            this.Move((playerPos - this.transform.position).normalized);
        else
            this.Move(Vector3.zero);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            Player player = other.gameObject.GetComponent<Player>();
            player.Ouch(this.hurtBox, this.touchDamage, this.touchKnockback);
       }
    }

    protected void AttackMelee ()
    {
        this.animator.SetTrigger("Attack");
        this.attackMelee.Execute();
    }
}
