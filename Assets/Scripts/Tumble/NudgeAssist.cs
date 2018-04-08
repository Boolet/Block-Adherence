using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NudgeAssist : MonoBehaviour {

	[SerializeField] float assistForceMagnitude = 1f;
	[SerializeField] ForceMode forceMode = ForceMode.Impulse;

	Rigidbody body;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody>();
	}

	//adds force in the given direction
	public void AddAssistForce(Vector3 target, bool instant){
		body.AddForce(target.normalized * assistForceMagnitude * (instant ? 1 : Time.deltaTime), forceMode);
	}
}
