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




		Vector3 pos = new Vector3(x, y, z);
		Vector3 posUp = new Vector3 (x, y + 1, z);

		if (!occupiedSpaces.ContainsKey(pos)){
			
			//occupiedSpaces.Add (pos, true);
			//occupiedSpaces.Add (posUp, true); //added to counter stacking objects
			int sizeX = selectionObject.GetComponent<ObjectProperties> ().sizeX;
			int sizeZ = selectionObject.GetComponent<ObjectProperties> ().sizeZ;

			//logic for bigger than 1x1 object occupying space
			for (int zn = 0; zn < sizeZ; zn++){
				for (int xn = 0; xn < sizeX; xn++){

					pos = new Vector3 (x+xn, y, z+zn);
					posUp = new Vector3 (x + xn, y+1, z + zn); //added +1 to heigh to counter stacking objects


					occupiedSpaces.Add (pos, true);
					occupiedSpaces.Add (posUp, true);
				}
			}

			Instantiate (selectionObject, pos, Quaternion.identity);

		}
			
	}

	public void RemoveObject(GameObject gameObject){
		
		if (gameObject.GetComponent<ObjectProperties>().Destructable) {
			Destroy (gameObject);
		}
	}
}
