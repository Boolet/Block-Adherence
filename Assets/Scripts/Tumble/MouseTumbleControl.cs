using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//let the cube tumble on the XY plane; I'll use transform.up and keep transform.forward along the z-axis

public class MouseTumbleControl : MonoBehaviour {

	[SerializeField] TumbleBox character;

	Plane p;


	void Awake () {
		p = new Plane(Vector3.forward, PlayPlane.playPlaneOffset);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)){
			TryMoveCharacter();
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

	void TryMoveCharacter(){
		Vector3 dir;
		if (MousePlanePosition(out dir)){
			dir -= character.transform.position;
			character.AddTorqueTowards(dir);
		}
	}

}
