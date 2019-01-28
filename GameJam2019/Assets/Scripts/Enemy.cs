using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    private static string[] SHELL_DISTRIBUTION = new string[] {
        "Shell1", "Shell1", "Shell1",
        "Shell2", "Shell2",
        "Shell3"
    };

    public GameObject[] shellPickups;

    public int touchDamage = 10;
    public float touchKnockback = 10;
    public float attackCooldown = 3.0f;

    public float huntRadius = 5.0f;

    private Attack attackMelee;
    private CircleCollider2D attackMeleeCollider;
    private float attackCooldownRemaining;

    private CircleCollider2D hurtBox;

    private string shellType;

	// Use this for initialization
	protected override void Start () {
        base.Start();

        this.hurtBox = this.GetComponent<CircleCollider2D>();
        this.attackMelee = this.transform.Find("MeleeAttack").GetComponent<Attack>();
        this.attackMeleeCollider = this.attackMelee.GetComponent<CircleCollider2D>();

        // spawn with a random type of shell
        this.shellType = SHELL_DISTRIBUTION[
            (int)(Random.value * (SHELL_DISTRIBUTION.Length - 1))
        ];
        foreach(string shellTypeName in new HashSet<string>(SHELL_DISTRIBUTION))
        {
            if (this.shellType != shellTypeName)
            {
                SpriteRenderer shellSprite = this.transform.Find(shellTypeName).GetComponent<SpriteRenderer>();
                shellSprite.enabled = false;
            }
        }
	}

	// Update is called once per frame
	protected override void FixedUpdate () {
        base.FixedUpdate();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj)
        {
            Collider2D playerCollider = playerObj.GetComponent<BoxCollider2D>();
            float playerDistance = Vector2.Distance(this.transform.position, playerObj.transform.position);

            //enable collider to calculate distance, then restore to prev state
            bool enabledState = this.attackMeleeCollider.enabled;
            this.attackMeleeCollider.enabled = true;
            ColliderDistance2D playerColliderDistance = Physics2D.Distance(this.attackMeleeCollider, playerCollider);
            this.attackMeleeCollider.enabled = enabledState;

            if (playerColliderDistance.distance <= 0)
                this.AttackMelee();
            else if (playerDistance < huntRadius)
                this.Move((playerObj.transform.position - this.transform.position).normalized);
            else
                this.Move(Vector3.zero);
        }

        if (this.attackCooldownRemaining > 0)
        {
            this.attackCooldownRemaining -= Time.deltaTime;
        }
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
        if (this.attackCooldownRemaining <= 0)
        {
            this.animator.SetTrigger("Attack");
            this.attackMelee.Execute();
            this.attackCooldownRemaining = this.attackCooldown;
        }
    }

    public override void Kill()
    {
        if (this.shellType == "Shell1")
            Instantiate(shellPickups[0], this.transform.position, Quaternion.identity);
        else if (this.shellType == "Shell2")
            Instantiate(shellPickups[1], this.transform.position, Quaternion.identity);
        else if (this.shellType == "Shell3")
            Instantiate(shellPickups[2], this.transform.position, Quaternion.identity);
        base.Kill();
    }
}
