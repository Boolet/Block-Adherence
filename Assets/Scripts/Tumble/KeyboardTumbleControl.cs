using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardTumbleControl : MonoBehaviour {

	[SerializeField] TumbleBox character;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		MoveBox();
	}

	void MoveBox(){
		float direction = -Input.GetAxis("Horizontal");
		character.AddTorqueTowards(direction);
	}
}
