using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffixToGamePlane : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Rigidbody body = GetComponent<Rigidbody>();
		if(body != null)
			body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
		OrientToPlane();
		MoveToPlane();
	}

	void Update(){
		OrientToPlane();
		MoveToPlane();
	}

	//guaranteed setup; may also be called to ensure everything stays in correct rotation
	void OrientToPlane(){
		Vector3 newUp = Vector3.ProjectOnPlane(transform.up, Vector3.forward);
		transform.rotation = Quaternion.LookRotation(Vector3.forward, newUp);
	}

	//guaranteed setup; may also be called to keep everything on the plane
	void MoveToPlane(){
		Vector3 newPosition = Vector3.ProjectOnPlane(transform.position, Vector3.forward);
		transform.position = newPosition + Vector3.forward * PlayPlane.playPlaneOffset;
	}
}
