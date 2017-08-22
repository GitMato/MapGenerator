using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	//public GameObject nodeWall;
	//int minSpawn;
	//int maxSpawn;
	int floorMask;
	float camRayLength = 100f;

	public Camera mainCamera;
	public GameObject coordText;

	private int mousePosx;
	private int mousePosz;
	private int mousePosy;

	private Vector3 mousePos;

	GameObject mousePointGameObject;

	public float TimeBetweenSpawns;
	private float timer;

	// Use this for initialization
	void Start () {
		//floorMask = -1;
		floorMask = LayerMask.GetMask ("map");
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if(Input.GetButton ("Fire1") && timer >= TimeBetweenSpawns){

			GetComponent<ObjectHandler> ().SpawnObject (mousePosx, mousePosy, mousePosz);
			//SpawnCubesOnClick ();
			//GetComponent<NodeHandler>().SpawnCubesOnClick(nodeWall, mousePosx, mousePosy, mousePosz, mousePointGameObject, mousePos);
			timer = 0;
		}

		if (Input.GetButton ("Fire2") && timer >= TimeBetweenSpawns) {

			GetComponent<ObjectHandler> ().RemoveObject (mousePointGameObject);
			//GetComponent<NodeHandler>().RemoveNode(mousePointGameObject);
			timer = 0;
		}
	}

	void FixedUpdate () {
		GetMouseCoords ();
	}

	void GetMouseCoords(){
		if (coordText != null) {


			Ray camRay = mainCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit floorhit;
			Debug.DrawRay (camRay.origin, camRay.direction * 100, Color.red); 
			if (Physics.Raycast (camRay, out floorhit, camRayLength, floorMask)) {

				//Debug.DrawRay (camRay.origin, camRay.direction * 8, Color.red); 

				mousePos = floorhit.point;


				mousePointGameObject = floorhit.transform.gameObject;


				UpdateMouseCoords (mousePos);
				//Debug (mousePosition.x, mousePosition.z);
				//jos hiiri on osunut floormaskiin.
			} else {
				coordText.GetComponent<Text> ().text = "no hit";
			}
		}
	}

	void UpdateMouseCoords(Vector3 mousePos){

		//pyöristä raycastin hitpointin sijainti
		mousePosx = Mathf.RoundToInt (mousePos.x);
		mousePosy = Mathf.RoundToInt (mousePos.y);
		mousePosz = Mathf.RoundToInt (mousePos.z);

		//Aseta osoitinteksti

		string mousePositionText = mousePosx + ", " + mousePosy + ", " + mousePosz;

		coordText.GetComponent<Text>().text = mousePositionText;



	}
}
