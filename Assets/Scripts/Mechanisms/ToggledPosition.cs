using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggledPosition : MonoBehaviour {

	[SerializeField] Transform startPositionProxy;
	[SerializeField] Transform endPositionProxy;
	[SerializeField] float transitionTime = 1f;
	[SerializeField] float overrideTimeForward = 0f;
	[SerializeField] float overrideTimeReset = 0f;
	[SerializeField] bool isParentOfBodies = false;

	Rigidbody[] bodies;
	Vector3[] offsets;
	Vector3 startPosition;
	Vector3 endPosition;
	bool isOn = false;
	float timer = 0f;

	// Use this for initialization
	void Start () {
		if (isParentOfBodies){
			bodies = GetComponentsInChildren<Rigidbody>();
			offsets = new Vector3[bodies.Length];
			for (int i = 0; i < bodies.Length; ++i){
				offsets[i] = bodies[i].transform.position - startPosition;
			}
		} else{
			bodies = new Rigidbody[1];
			bodies[0] = GetComponent<Rigidbody>();
			offsets = new Vector3[1];
			offsets[0] = Vector3.zero;
		}
		
		startPosition = startPositionProxy.position;
		endPosition = endPositionProxy.position;

		if (transitionTime == 0f){
			throw new UnityException("Transition time is zero on " + this);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		AdjustTimer();
		Move();
	}

	void AdjustTimer(){
		if (isOn){
			timer += Time.fixedDeltaTime / (overrideTimeForward != 0 ? overrideTimeForward : transitionTime);
		} else{
			timer -= Time.fixedDeltaTime / (overrideTimeReset != 0 ? overrideTimeReset : transitionTime);
		}
		timer = Mathf.Clamp(timer, 0, 1);
	}

	void Move(){
		for(int i = 0; i < bodies.Length; ++i){
			bodies[i].MovePosition(Vector3.Lerp(startPosition + offsets[i], endPosition + offsets[i], timer));
		}
	}

	public void On(){
		isOn = true;
	}

	public void Off(){
		isOn = false;
	}
}
