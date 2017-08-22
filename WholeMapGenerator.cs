using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WholeMapGenerator : MonoBehaviour{

	public int chunkGridSize = 2;
	public GameObject chunkPrefab;

	public string seed;
	public bool useRandomSeed;
	public static int seedHash;

	// Use this for initialization
	GameObject newChunk;

	public static int gridSizeInfo;


	List<GameObject> mapChunks = new List<GameObject>();

	void Start () {

		if (useRandomSeed){
			//seed = Time.time.ToString ();
			seed = (Random.Range (0,999999)).ToString();
		}

		seedHash = seed.GetHashCode ();
		Debug.Log (seed+": " + seed);

		GenerateGrid ();
		ChangeMapPos ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GenerateGrid(){
		gridSizeInfo = chunkGridSize;
		//id = 0;
		//mapChunks = new GameObject[chunkGridSize*chunkGridSize]();
		int chunkSize = ChunkGenerator.mapSize;

		//Debug.Log(chunkSize);

		//int chunkOffset = (chunkGridSize * chunkSize)/ 2;
		//int chunkRemoveExtra = chunkGridSize - 1;

//		for (int z = -chunkOffset+1; z < (chunkGridSize * chunkSize) /2 -1; z+=chunkSize-1){
//			
//			for (int x = -chunkOffset+1; x < (chunkGridSize * chunkSize) /2 -1; x+=chunkSize-1){
		for (int z = 0; z < (chunkGridSize * chunkSize) -1; z+=chunkSize){

			for (int x = 0; x < (chunkGridSize * chunkSize) -1; x+=chunkSize){
				
				Vector3 position = new Vector3 (x - (x/chunkSize), 0, z- (z/chunkSize));

				mapChunks.Add(Instantiate (chunkPrefab, position, Quaternion.Euler (Vector3.zero)));
				//ChunkGenerator.ChunkID.id = id;
				//id += 1;

				//id:
				//	6	7	8
				//	3	4	5
				//	0	1	2



			}
		}
	}

	void ChangeMapPos(){

		for (int i = 0; i < mapChunks.Count; i++){
			int xPosChange = Mathf.FloorToInt(-(chunkGridSize * ChunkGenerator.mapSize)/2);
			int zPosChange = Mathf.FloorToInt(-(chunkGridSize * ChunkGenerator.mapSize)/2);
			int xPos = Mathf.FloorToInt(mapChunks [i].transform.position.x);
			int yPos = Mathf.FloorToInt(mapChunks [i].transform.position.y);
			int zPos = Mathf.FloorToInt(mapChunks [i].transform.position.z);


			//mapChunks [i].transform.SetPositionAndRotation (new Vector3(xPos + xPosChange-(xPos/(ChunkGenerator.mapSize/2)+i), yPos, zPos + zPosChange - (zPos/(ChunkGenerator.mapSize/2)+i)), 
			//												Quaternion.Euler(Vector3.zero));

			mapChunks [i].transform.SetPositionAndRotation (new Vector3(xPos + xPosChange, yPos, zPos + zPosChange), 
				Quaternion.Euler(Vector3.zero));



		}

	}


}
