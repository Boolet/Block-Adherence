using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//let the cube tumble on the XY plane; I'll use transform.up and keep transform.forward along the z-axis

public class MouseTumbleControl : MonoBehaviour {

	[SerializeField] TumbleBox character;

	Plane p;
	bool nudgedLastFrame = false;

	void Awake () {
		p = new Plane(Vector3.forward, PlayPlane.playPlaneOffset);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Fire1") > 0){
			TryMoveCharacter();
		}
		if (Input.GetAxis("Fire2") > 0 && !nudgedLastFrame){
			TryNudgeCharacter();
		} else if (Input.GetAxis("Fire2") < Mathf.Epsilon){
			nudgedLastFrame = false;
		}
	}

	// outputs the position of the mouse on the detection plane, if true
	bool MousePlanePosition(out Vector3 dir){
		dir = Vector3.zero;
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		float dist;
		if(p.Raycast(mouseRay, out dist)){
			dir = mouseRay.GetPoint(dist);
			return true;
		}
		return false;
	}

	// if the mouse's planar position is not null, sends the point to the movement method of the TumbleBox
	void TryMoveCharacter(){
		Vector3 dir;
		if (MousePlanePosition(out dir)){
			dir -= character.transform.position;
			character.AddTorqueTowards(dir);
		}
	}

	// if th mouse's planar position is not null and there is a nudge assistant, sends the point to the NudgeAssist
	void TryNudgeCharacter(){
		NudgeAssist assist = character.GetAssistant();
		if (assist == null)
			return;
		Vector3 dir;
		if(MousePlanePosition(out dir)){
			dir -= character.transform.position;
			assist.AddAssistForce(dir, true);
			nudgedLastFrame = true;
		}
	}

}
