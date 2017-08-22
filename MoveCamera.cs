using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

	//needs a parent gameobject to work correctlty

	public float speed;
	public float cameraRotationSpeed;

	// Use this for initialization
	void Start () {
		 
	}
	
	// Update is called once per frame
	void Update () {
		

		Move();
	}

	void Move(){
		

		if (Input.GetKey(KeyCode.A)) {
			transform.Translate (new Vector3 (-speed * Time.deltaTime, 0.0f, 0.0f));
		}
		if (Input.GetKey(KeyCode.D)) {
			transform.Translate (new Vector3 (speed * Time.deltaTime, 0.0f, 0.0f));
		}
		if (Input.GetKey(KeyCode.W)) {
			transform.Translate (new Vector3 (0.0f, 0.0f, speed * Time.deltaTime));
		}
		if (Input.GetKey(KeyCode.S)) {
			transform.Translate (new Vector3 (0.0f, 0.0f, -speed * Time.deltaTime));
		}
		if (Input.GetKey(KeyCode.R)) {
			transform.SetPositionAndRotation (new Vector3 (0.0f, 3.0f, -6.0f), Quaternion.Euler(45,0,0));
		}

		if (Input.GetKey (KeyCode.Q)) {
			//rotate camera 
			Vector3 orgRotation = this.GetComponentInChildren<Transform> ().transform.rotation.eulerAngles;
			Vector3 rotationTo = orgRotation;
			//rotationTo = (rotationTo.x - 3, rotationTo.y, rotationTo.z + 3);
			rotationTo.y +=2;

			
			//Transform orgRotation = this.GetComponentInChildren<Transform> ().transform;
			//Transform rotation = orgRotation;


			GetComponentInChildren<Transform> ().rotation = Quaternion.Lerp (Quaternion.Euler(orgRotation), Quaternion.Euler (rotationTo), cameraRotationSpeed);
		}
		if (Input.GetKey (KeyCode.E)) {
			//rotate camera 
			Vector3 orgRotation = this.GetComponentInChildren<Transform> ().transform.rotation.eulerAngles;
			Vector3 rotationTo = orgRotation;

			rotationTo.y -=2;


			GetComponentInChildren<Transform> ().rotation = Quaternion.Lerp (Quaternion.Euler(orgRotation), Quaternion.Euler (rotationTo), cameraRotationSpeed);
		}

	}
}
