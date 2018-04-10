using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ImpactSound : MonoBehaviour {

	[SerializeField] AudioClip impactNoise;
	[SerializeField] float baseVolume = 0.3f;
	[SerializeField] float volumeSpeedFactor = 0.1f;

	AudioSource source;

	// Use this for initialization
	void Awake () {
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		//can add a pitch change later
		float volume = baseVolume + col.relativeVelocity.magnitude * volumeSpeedFactor;
		if (impactNoise != null)
			source.PlayOneShot(impactNoise, volume);
		else
			print("Playing impact noise at " + volume + " volume");
	}
}
