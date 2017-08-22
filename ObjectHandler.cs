using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : MonoBehaviour {

	// Use this for initialization


	//tulevaisuudessa UI:sta vaihdettava gameObject
	public GameObject selectionObject;

	Dictionary<Vector3, bool> occupiedSpaces = new Dictionary<Vector3, bool>();

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnObject(int x, int y, int z){

		//Instantiate (selectionObject, (new Vector3 (x, y, z), Quaternion.Euler(Vector3.zero)));

		Vector3 pos = new Vector3(x, y, z);
		if (!occupiedSpaces.ContainsKey(pos)){
			
			occupiedSpaces.Add (pos, true);
			Instantiate (selectionObject, pos, Quaternion.identity);

		}
			
	}

	public void RemoveObject(GameObject gameObject){
		
		if (gameObject.GetComponent<ObjectProperties>().Destructable) {
			Destroy (gameObject);
		}
	}
}
