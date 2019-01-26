using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected void FixedUpdate () {
        Vector3 moveVec = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        this.Move(moveVec);
	}
}
