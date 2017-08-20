using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFirstPerson : MonoBehaviour {


	Rigidbody rigidbody;
	public Vector3 velocity;
	Vector3 rotation;
	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		velocity = new Vector3(Input.GetAxisRaw ("Horizontal"),0,Input.GetAxisRaw("Vertical"));
		if (Input.GetButton("Fire3")){
			velocity = velocity * 5;
		}

		if (Input.GetKeyDown(KeyCode.Q)){
			rotation = new Vector3 (0, -1, 0);
		}
		if (Input.GetKeyUp (KeyCode.Q)) {
			rotation = new Vector3 (0, 0, 0);
		}
		if (Input.GetKeyDown(KeyCode.E)){
			rotation = new Vector3 (0, 1, 0);
		}
		if (Input.GetKeyUp (KeyCode.E)) {
			rotation = new Vector3 (0, 0, 0);
		}

	}

	void FixedUpdate() {
		//rigidbody.AddForce (velocity * Time.fixedDeltaTime);
		rigidbody.MovePosition (rigidbody.position + velocity * Time.fixedDeltaTime);
		rigidbody.MoveRotation (rigidbody.rotation * Quaternion.Euler(rotation) * Quaternion.Euler(0, Time.fixedDeltaTime, 0));

	}
}
