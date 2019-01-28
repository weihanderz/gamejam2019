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
    public int size;

    private Attack attackMelee;
    private List<PlayerAction> moveset = new List<PlayerAction>();

    private SpriteRenderer playerShellSprite;

	// Use this for initialization
	protected override void Start () {
        base.Start();

        this.attackMelee = this.transform.Find("PlayerMeleeAttack").GetComponent<Attack>();
        this.playerShellSprite = this.transform.Find("PlayerShell").GetComponent<SpriteRenderer>();

        moveset.Add(new PlayerAction(KeyCode.Space, 0.5f, AttackMelee));
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

    protected void AttackMelee ()
    {
        this.animator.SetTrigger("PlayerAttack");
        this.attackMelee.Execute();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Exit")
        {
            Invoke("Restart",1f);
            enabled = false;
        }
        else if (other.tag == "Pickup")
        {
            this.playerShellSprite.sprite = other.GetComponent<SpriteRenderer>().sprite;
            GameObject.Destroy(other.gameObject);
        }
    }

    private void Restart ()
		{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		}

    public override void Kill()
    {
        // set player as inactive instead of default destroy object
        // don't break references for camera, etc.
        this.gameObject.SetActive(false);
        GameManager.instance.GameOver();

    }

}
