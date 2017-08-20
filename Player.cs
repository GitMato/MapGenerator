using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	//SphereCollider collider;
	Rigidbody rigidbody;
	Vector3 velocity;


	// Use this for initialization
	void Start () {
		
		rigidbody = GetComponent<Rigidbody> ();
		//rigidbody.MovePosition (new Vector3());
		
	}
	
	// Update is called once per frame
	void Update () {
		
		velocity = new Vector3(Input.GetAxisRaw ("Horizontal"),0,Input.GetAxisRaw("Vertical"));
	}

	void FixedUpdate() {
		//rigidbody.AddForce (velocity * Time.fixedDeltaTime);
		rigidbody.MovePosition (rigidbody.position + velocity * Time.fixedDeltaTime);
	}
}
