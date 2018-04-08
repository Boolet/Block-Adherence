using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonManager : MonoBehaviour {

	[SerializeField] bool staysDownOncePressed = false;

	[SerializeField] UnityEvent onPress;
	[SerializeField] UnityEvent whilePressed;
	[SerializeField] UnityEvent onRelease;

	[SerializeField] ConstantForce buttonTop;

	public void Press(){
		onPress.Invoke();
		if (staysDownOncePressed)
			LockButtonDown();
	}

	public void Hold(){
		whilePressed.Invoke();
	}

	public void Release(){
		onRelease.Invoke();
	}

	void LockButtonDown(){
		buttonTop.relativeForce = Vector3.down;
	}
}
