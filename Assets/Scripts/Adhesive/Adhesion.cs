using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adhesion : MonoBehaviour {

	[SerializeField] bool indestructibleJoints = true;
	[SerializeField] float jointStrength = 5f;
	[SerializeField] float minimalContactArea = 0.2f;

	bool adhesionOn = false;
	Dictionary<Adherent, Joint> attachedObjects = new Dictionary<Adherent, Joint>();

	Rigidbody body;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!adhesionOn && attachedObjects.Count > 0){
			Disperse();
		}
		if (body.IsSleeping())
			body.WakeUp();
	}

	//sets the adhesion system to active or inactive
	public void SetAdhesionMode(bool on){
		adhesionOn = on;
	}

	// used for finding new things to adhere to
	void OnCollisionStay(Collision col){
		AdhesionSystem(col);
	}

	//runs the adhesion system
	void AdhesionSystem(Collision col){
		Adherent other = col.gameObject.GetComponent<Adherent>();
		if (!adhesionOn || other == null || attachedObjects.ContainsKey(other) || !IsValidForAdhesion(col))
			return;
		Adhere(other);
	}

	//performs the actual adherence
	void Adhere(Adherent other){
		FixedJoint joint = gameObject.AddComponent<FixedJoint>();
		joint.connectedBody = other.GetBody();
		if(!indestructibleJoints)
			joint.breakForce = jointStrength;
		attachedObjects.Add(other, joint);
		print("Adhering to " + other);
	}

	//un-adheres from all adherents
	void Disperse(){
		foreach(KeyValuePair<Adherent, Joint> kv in attachedObjects){
			Destroy(kv.Value);
		}
		attachedObjects.Clear();
	}

	void OnJointBreak(float breakForce){
		foreach(KeyValuePair<Adherent, Joint> kv in attachedObjects){
			if (kv.Value == null)
				attachedObjects.Remove(kv.Key);
		}
	}

	//very picky system for choosing whether something can be adhered to
	bool IsValidForAdhesion(Collision col){
		if (col.contacts.Length < 4)
			return false;
		List<ContactPoint> uniqueContacts = new List<ContactPoint>();
		foreach (ContactPoint contact in col.contacts){
			bool isUnique = true;
			//print(contact.point);
			foreach (ContactPoint foundContact in uniqueContacts){
				if (TooClose(contact.point, foundContact.point) /*contact.point == foundContact.point*/){
					isUnique = false;
					break;
				}
			}
			if (isUnique)
				uniqueContacts.Add(contact);
		}
		if (uniqueContacts.Count >= 3){
			for (int j = 0; j < uniqueContacts.Count-2; ++j){
				for (int k = j+1; k < uniqueContacts.Count-1; ++k){
					for (int l = k+1; l < uniqueContacts.Count; ++l){
						if(AreCooriented(uniqueContacts[j],
							uniqueContacts[k],
							uniqueContacts[l])){
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	//returns whether the three contact points share the same normal vector
	bool AreCooriented(ContactPoint a, ContactPoint b, ContactPoint c){
		return a.normal == b.normal && a.normal == c.normal;
	}

	//returns whether four given vertices are coplanar
	bool AreCoplanar(ContactPoint a, ContactPoint b, ContactPoint c, ContactPoint d){
		Plane p = new Plane(a.point, b.point, c.point);
		return p.GetDistanceToPoint(d.point) < float.Epsilon;
	}

	//returns whether the vertices are closer than the minimalContactArea
	bool TooClose(Vector3 a, Vector3 b){
		return Vector3.Distance(a, b) < minimalContactArea;
	}

}
