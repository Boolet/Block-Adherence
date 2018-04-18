using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TumbleBox))]
public class TumbleArcVisual : MonoBehaviour {

	[SerializeField] LineRenderer up;
	[SerializeField] LineRenderer cursor;
	[SerializeField] LineRenderer arc;

	[SerializeField] float fadeTime = 1f;
	[SerializeField] int arcSegments = 100;
	[SerializeField] float indicatorLength = 4f;
	[SerializeField] Color lowTorqueColor;
	[SerializeField] Color highTorqueColor;

	TumbleBox box;
	float transparencyTimer = 1f;

	// Use this for initialization
	void Start () {
		box = GetComponent<TumbleBox>();
		CreateLines();
	}

	void CreateLines(){
		up = Instantiate(up);
		cursor = Instantiate(cursor);
		arc = Instantiate(arc);
		up.positionCount = 2;
		cursor.positionCount = 2;
		arc.positionCount = arcSegments;
	}
	
	// Update is called once per frame
	void Update () {
		Fade();
	}

	public void ShowTumbleArc(Vector3 boxUp, Vector3 cursorPosition, float torqueMagnitude){
		print(torqueMagnitude + "/" + box.maxTorque * Time.fixedDeltaTime);
		transparencyTimer = 1f;

		up.SetPositions(new Vector3[]{transform.position, boxUp.normalized * indicatorLength + transform.position});
		cursor.SetPositions(new Vector3[]{transform.position, cursorPosition.normalized * indicatorLength + transform.position});

		float powerPortion = torqueMagnitude / (box.maxTorque * Time.fixedDeltaTime);

		Vector3[] newArc = new Vector3[arcSegments];
		Vector3 arcStart = Vector3.Lerp(Vector3.zero, boxUp.normalized * indicatorLength, powerPortion);
		for (int i = 0; i < arcSegments; ++i){
			newArc[i] = transform.position + Vector3.RotateTowards(arcStart,
				cursorPosition,
				Vector3.Angle(arcStart, cursorPosition) * Mathf.Deg2Rad * (float)i / (float)arcSegments,
				0);
		}
		arc.SetPositions(newArc);

		SetColors(powerPortion);
	}

	void SetColors(float colorLerpValue){
		colorLerpValue = Mathf.Clamp(colorLerpValue, 0, 1);
		Color targetColor = lowTorqueColor * (1-colorLerpValue) + highTorqueColor * colorLerpValue;
		up.startColor = up.endColor = cursor.startColor = cursor.endColor = arc.startColor = arc.endColor = targetColor;
	}

	void Fade(){
		transparencyTimer -= Time.deltaTime / fadeTime;
		transparencyTimer = Mathf.Clamp(transparencyTimer, 0f, 1f);

		Color oldColor = up.startColor;
		oldColor.a = transparencyTimer;
		up.startColor = oldColor;
		oldColor = up.endColor;
		oldColor.a = transparencyTimer;
		up.endColor = oldColor;

		oldColor = cursor.startColor;
		oldColor.a = transparencyTimer;
		cursor.startColor = oldColor;
		oldColor = cursor.endColor;
		oldColor.a = transparencyTimer;
		cursor.endColor = oldColor;

		oldColor = arc.startColor;
		oldColor.a = transparencyTimer;
		arc.startColor = oldColor;
		oldColor = arc.endColor;
		oldColor.a = transparencyTimer;
		arc.endColor = oldColor;
	}
}
