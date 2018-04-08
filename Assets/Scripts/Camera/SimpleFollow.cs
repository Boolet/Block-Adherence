using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour {

	[SerializeField] Transform target;
	[SerializeField] bool xFollow,yFollow,zFollow;
	[SerializeField] bool slowFollow = false;
	[SerializeField] float minSlowFollowSpeed = 1f;
	[SerializeField] float offsetFactor = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Follow();
	}

	void Follow(){
		if (!slowFollow)
			FastFollow();
		else
			SlowFollow();
	}

	void FastFollow(){
		transform.position = new Vector3(xFollow ? target.position.x : transform.position.x,
			yFollow ? target.position.y : transform.position.y,
			zFollow ? target.position.z : transform.position.z);
	}

	void SlowFollow(){
		Vector3 camPos = transform.position;
		Vector3 targetPos = target.position;
		if (!xFollow){
			camPos = Vector3.ProjectOnPlane(camPos, Vector3.right);
			targetPos = Vector3.ProjectOnPlane(targetPos, Vector3.right);
		}
		if (!yFollow){
			camPos = Vector3.ProjectOnPlane(camPos, Vector3.up);
			targetPos = Vector3.ProjectOnPlane(targetPos, Vector3.up);
		}
		if (!zFollow){
			camPos = Vector3.ProjectOnPlane(camPos, Vector3.forward);
			targetPos = Vector3.ProjectOnPlane(targetPos, Vector3.forward);
		}

		Vector3 offset = targetPos - camPos;
		float maxMovement = Mathf.Max(minSlowFollowSpeed, offset.magnitude * offsetFactor) * Time.deltaTime;
		maxMovement = Mathf.Min(offset.magnitude, maxMovement);

		offset = offset.normalized * maxMovement;
		transform.position += offset;
	}
}
