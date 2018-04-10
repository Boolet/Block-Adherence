using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedFollow : MonoBehaviour {

	[SerializeField] Transform[] targets;
	[SerializeField] bool xFollow,yFollow,zFollow;
	[SerializeField] float minFollowSpeed = 1f;
	[SerializeField] float offsetFactor = 2f;

	//will need to scale the camera frustum (zoom in or out) to keep all players on screen
	//I want to try turning the camera a little to give the scene more depth

	Vector3 camTarget;

	// Use this for initialization
	void Awake () {
		camTarget = LocusCenter();
	}
	
	// Update is called once per frame
	void Update () {
		AdjustTarget();
		Follow();
	}

	//changes the location of the point the camera is trying to look at,
	void AdjustTarget(){
		Vector3 targetPos = LocusCenter();
		if (!xFollow){
			targetPos = Vector3.ProjectOnPlane(targetPos, Vector3.right);
		}
		if (!yFollow){
			targetPos = Vector3.ProjectOnPlane(targetPos, Vector3.up);
		}
		if (!zFollow){
			targetPos = Vector3.ProjectOnPlane(targetPos, Vector3.forward);
		}
		camTarget = targetPos;
	}

	//moves the camera towards the point i is trying to look at, without adjusting its position in the axes
	//that it is not supposed to follow in
	void Follow(){
		Vector3 camPos = transform.position;
		if (!xFollow){
			camPos = Vector3.ProjectOnPlane(camPos, Vector3.right);
		}
		if (!yFollow){
			camPos = Vector3.ProjectOnPlane(camPos, Vector3.up);
		}
		if (!zFollow){
			camPos = Vector3.ProjectOnPlane(camPos, Vector3.forward);
		}

		Vector3 offset = camTarget - camPos;
		float maxMovement = Mathf.Max(minFollowSpeed, offset.magnitude * offsetFactor) * Time.deltaTime;
		maxMovement = Mathf.Min(offset.magnitude, maxMovement);

		offset = offset.normalized * maxMovement;
		transform.position += offset;
	}

	Vector3 LocusCenter(){
		Vector3 center = Vector3.zero;
		foreach (Transform t in targets){
			center += t.position;
		}
		return center;
	}
}
