using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;

internal delegate void PlayerActionEffect();

internal class PlayerAction
{
    public UnityEngine.KeyCode keycode;
    public float cooldown;
    public float cooldownRemaining = 0.0f;
    public PlayerActionEffect effect;

    public PlayerAction(UnityEngine.KeyCode keycode, float cooldown, PlayerActionEffect effect)
    {
        this.keycode = keycode;
        this.cooldown = cooldown;
        this.effect = effect;
    }
}

public class Player : Character
{
    public int health;
    public int size;

    public float attackRadius;

    private List<PlayerAction> moveset = new List<PlayerAction>();

	// Use this for initialization
	protected override void Start () {
        base.Start();

        moveset.Add(new PlayerAction(KeyCode.Space, 0.5f, Attack));
	}
	
	// Update is called once per frame
	protected override void FixedUpdate () {
        base.FixedUpdate();
        Vector3 moveVec = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        this.Move(moveVec);
	}

    protected void Update()
    {
        foreach (var move in moveset)
        {
            if (Input.GetKey(move.keycode) && move.cooldownRemaining <= 0)
            {
                move.effect();
                move.cooldownRemaining = move.cooldown;
            }
            else if (move.cooldownRemaining > 0)
            {
                move.cooldownRemaining -= Time.deltaTime;
            }
        }
    }

    protected void Grow ()
    {
        this.size += 1;
        this.animator.SetTrigger("PlayerGrow");
    }

    protected void Attack ()
    {
        this.animator.SetTrigger("PlayerAttack");
        // disable own collider so we don't attack ourselves
        this.boxCollider.enabled = false;
        Collider2D[] hit = Physics2D.OverlapCircleAll(
            this.transform.position, this.attackRadius
        );
        this.boxCollider.enabled = true;
        foreach(Collider2D target in hit) {
            if (target.gameObject.CompareTag("Enemy")) {
                Debug.Log("I hit an enemy!");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Exit")
        {
            Debug.Log("I'm outta here!");
            Invoke("Restart",1f);
            enabled = false;
        }
        if (other.gameObject.CompareTag("Enemy")) {
            this.Ouch(other, 10, 10);
            Debug.Log("Ouch!");
;       }
    }

    	private void Restart ()
		{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		}

    void Ouch(Collider2D enemyCollider, int damage, float impact)
    {
        Vector3 impactVector = this.rb2D.transform.position - enemyCollider.transform.position;
        this.velocity = impactVector.normalized * impact;
    }
}
