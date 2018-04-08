using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdhesionController : MonoBehaviour {

	[SerializeField] Adhesion character;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		character.SetAdhesionMode(Input.GetAxis("Jump") > 0);
	}
}
