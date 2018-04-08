using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AffixToGamePlane))]
public class TumbleBox : MonoBehaviour {

	[SerializeField] float angleFactor = 30f;	//an impulse force proportional to the angle offset; at full value at 90 degrees
	[SerializeField] float maxTorque = 30f;		//the maximum allowed impulse force. Values higher than the angle factor do nothing
	[SerializeField] bool obtuseAngleMaximized = false;

	Rigidbody body;
	NudgeAssist assistant;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody>();
		assistant = GetComponent<NudgeAssist>();
	}

	/// <summary>
	/// adds torque proportional to the cross product of the normalized up vector and the normalized target vector
	/// </summary>
	/// <param name="target">Target. The vector that the 'up' direction of the object will try to point at</param>
	public void AddTorqueTowards(Vector3 target){
		Vector3 targetPlane = Vector3.ProjectOnPlane(target, Vector3.forward);
		Vector3 upPlane = Vector3.ProjectOnPlane(transform.up, Vector3.forward);

		Vector3 crossed = Vector3.Cross(upPlane.normalized, targetPlane.normalized);	//calculate the cross product
		float angleOffset = Vector3.Angle(upPlane, targetPlane);						//and the angle between the vectors...

		float baseMagnitude = crossed.magnitude;
		if(obtuseAngleMaximized)
			baseMagnitude = angleOffset > 90 ? 1 : crossed.magnitude;		//so that we can account for obtuse angles
		
		float torqueMagnitude = Mathf.Min(baseMagnitude * angleFactor, maxTorque) * Time.deltaTime;

		body.AddTorque(crossed.normalized * torqueMagnitude, ForceMode.Impulse);
	}

	/// <summary>
	/// Adds torque to the right in proportion to the direction parameter, to a max of 1 or a min of -1
	/// </summary>
	/// <param name="direction">Direction. The direction and magnitude of the torque</param>
	public void AddTorqueTowards(float direction){
		direction = Mathf.Clamp(direction, -1, 1);
		body.AddTorque(Vector3.forward * Mathf.Min(angleFactor, maxTorque) * direction, ForceMode.Impulse);
	}

	public NudgeAssist GetAssistant(){
		return assistant;
	}
}
