using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public int attackDamage;
    public string [] attackTargetTags;
    public float attackKnockback;
    public float attackDuration;

    private Collider2D attackCollider;
    private float attackTime;

	// Use this for initialization
	void Start () {
        this.attackCollider = this.GetComponent<Collider2D>();
        this.attackCollider.enabled = false;
        this.attackCollider.isTrigger = true;
	}

    public void Execute()
    {
        this.attackCollider.enabled = true;
        this.attackTime = Time.time;
    }

    public void Update()
    {
        if (this.attackCollider.enabled && this.attackTime + this.attackDuration <= Time.time)
            this.attackCollider.enabled = false;
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
