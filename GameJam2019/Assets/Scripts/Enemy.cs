using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    public int touchDamage;
    public float touchKnockback;

    private CircleCollider2D hurtBox;

	// Use this for initialization
	protected override void Start () {
        base.Start();

        this.hurtBox = this.GetComponent<CircleCollider2D>();
	}

	// Update is called once per frame
	protected override void FixedUpdate () {
        base.FixedUpdate();
        this.Move(Vector3.zero);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            Player player = other.gameObject.GetComponent<Player>();
            player.Ouch(this.hurtBox, this.touchDamage, this.touchKnockback);
;       }
    }
}
