using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonDetection : MonoBehaviour {

	[SerializeField] GameObject buttonTop;
	[SerializeField] ButtonManager manager;

	void OnTriggerEnter(Collider other){
		if (other.gameObject == buttonTop){
			manager.Press();
		}
	}

	void OnTriggerStay(Collider other){
		if(other.gameObject == buttonTop){
			manager.Hold();
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject == buttonTop){
			manager.Release();
		}
	}
}
