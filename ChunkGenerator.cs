using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimplexNoise;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
//using SimplexNoise;

public class ChunkGenerator : MonoBehaviour {


	// ei jaollinen kahdella
	public static int mapSize = 255;
	//public int mapGridSize = 2;
	//public string seed;
	public bool useRandomSeed;

	List<Vector3> vertices = new List<Vector3>();
	List<int> triangles = new List<int> ();
	//float[][] heightArray = new float[mapSize][];

	MeshFilter filter;
	MeshCollider coll;

	int coordXgrid = 0;
	int coordZgrid = 0;

	ChunkID chunkID;


	[Range(0,10)]
	public float heightScale1 = 2f; // 5
	[Range(0,1)]
	public float frequency1 = 0.2f; // 0.02 = 0.2f/10
	[Range(0,10)]
	public float heightScale2 = 3f; // 0.2 ROUGHNESS
	[Range(0,1)]
	public float frequency2 = 0.84f; // 0.42 = 0.84/2 ROUGHNESS
	[Range(0,15)]
	public float heightScale3 = 3f; // 3
	[Range(0,1)]
	public float frequency3 = 0.1f; // 0.001 = 0.1/100

	// HEIGHTSCALE MÄÄRÄYTYMINEN NOISENMUKAAN!!!!!!!!





	// Use this for initialization
	void Start () {
		float[][] heightArray1 = new float[mapSize][];
		GenerateHeightArray (heightArray1);

		float[][] heightArray2 = new float[mapSize][];
		GenerateHeightArray (heightArray2);

		float[][] heightArray3 = new float[mapSize][];
		GenerateHeightArray (heightArray3);


		filter = gameObject.GetComponent<MeshFilter> ();
		coll = gameObject.GetComponent<MeshCollider> ();

		GetNoiseCoordMultiplier ();
		//UpdateChunkID ();
		SetUItext ();
		GetWorldPos ();



		GenerateVertices (heightArray1, heightArray2, heightArray3);
		TriangulateSquare ();
		GenerateMesh ();

		MapMeshGenerator meshGenerator = GetComponent<MapMeshGenerator> ();


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GenerateVertices(float[][] heightArray1, float[][] heightArray2, float[][] heightArray3){
		

//		if (useRandomSeed){
//			seed = Time.time.ToString ();
//		}
		float freq1 = frequency1 / 10;
		float freq2 = frequency2 / 2;
		float freq3 = frequency3 / 100;

		heightArray1 = FillHeightArray (heightArray1, freq1);

		heightArray2 = FillHeightArray (heightArray2, freq2);

		heightArray3 = FillHeightArray (heightArray3, freq3);


		float y = 0f;

		for (int z = 0; z < mapSize; z++){
			for (int x = 0; x < mapSize; x++){

//				//Height values - random Noise
//				randomHeight = Random.Range (0, 51);
//				if (randomHeight <= 30){
//					y = 0.0f;
//				} else if(randomHeight > 30 && randomHeight <= 45) {
//					y = 0.33f;
//				} else if (randomHeight > 45 && randomHeight <= 48){
//					y = 0.66f;
//				} else if (randomHeight > 48 && randomHeight <= 50){
//					y = 1.0f;
//				}
//


				y = heightArray1 [x] [z] * heightScale1;
				//if (heightArray2 [x] [y] > 0f) {
				y += heightArray2 [x] [z] * heightScale2;
				//}
				y += heightArray3 [x] [z] * heightScale3;


				vertices.Add(new Vector3(x, y, z));

			}
		}
	}

	void GenerateMesh(){


		Mesh mesh = new Mesh ();
		filter.mesh = mesh;


		mesh.vertices = vertices.ToArray ();
		mesh.triangles = triangles.ToArray ();
		mesh.RecalculateNormals ();
		coll.sharedMesh = mesh;
		
	

	}

	void TriangulateSquare(){
		//int y = 0;
		int step = -1;
		for (int z = 0 ; z < mapSize-1; z++){

			step += 1;

			for (int x = 0; x < mapSize-1; x++){


				AddTrianglesToList (0+step, step+mapSize, 1+step+mapSize);
				AddTrianglesToList (0+step, 1+step+mapSize, 1+step);

				step += 1;



			}						
			
		}
	}

	void AddTrianglesToList(int vertexAIndex, int vertexBIndex, int vertexCIndex){

		triangles.Add (vertexAIndex);
		triangles.Add (vertexBIndex);
		triangles.Add (vertexCIndex);
		
	}

	void GenerateTriangle(){


	}

//	void OnDrawGizmos(){
//
//		for (int i = 0; i < vertices.Count; i++) {
//			Gizmos.DrawCube (vertices [i], Vector3.one);
//		}
//	}


//	public static int GetNoise1(int x, int y, int z, float scale, int max){
//		return Mathf.FloorToInt ((Noise.Generate (x * scale, y * scale, z * scale) + 1f) * (max / 2f));
//	}
//
//	public static int GetNoise(float y, float scale, int max){
//		return Mathf.FloorToInt ((Noise.Generate (y * scale + 1f)) * (max / 2f));
//	}

	float[][] GenerateHeightArray(float[][] heightArray){
		for (int i = 0; i < mapSize; i++){
			heightArray [i] = new float[mapSize];
		}
		return heightArray;
	}

	float[][] FillHeightArray(float[][] heightArray, float frequency){

		int gridSize = 0;

		for (int z = 0; z < mapSize; z++ ){
			
			for (int x = 0; x < mapSize; x++){

				Vector3 position = GetComponent<Transform>().position;

//				float xn = position.x * x;
//				float zn = position.z * z;

				float xn = (x + (this.chunkID.chunkX * mapSize));
				float zn = (z + (this.chunkID.chunkZ * mapSize));

//				float xn = (x + (coordXgrid * mapSize));
//				float zn = (z + (coordZgrid * mapSize));

				//float xn = (x + (gridSize * mapSize));
				//float zn = (z + (gridSize * mapSize));
				//Debug.Log (position);


				//heightArray [x] [z] = Mathf.PerlinNoise (xn * frequency, zn * frequency);

				heightArray [x] [z] = Noise.Generate (xn * frequency, zn * frequency);


			}
		}


		return heightArray;
	}


	void UpdateMap(){



		//Vector3[] meshVertices = mesh.vertices;




	}

	int GetGridSize(){

		int gridSize;

		Vector3 currentPos = GetComponent<Transform> ().position;

		//oletettavasti negatiivinen
		gridSize = ((Mathf.FloorToInt(currentPos.x)-(mapSize/2))*-1)/mapSize;
		Debug.Log (gridSize);


		return gridSize;
		
	}

	void GetWorldPos(){
		Vector3 position = GetComponent<Transform> ().position;


		//jos gridsize = 2 niin kaukaisin vertex on x= -50, z = -50 -jos chunkSize = 50

		//jos gridsize = 3 niin kaukaisin vertex on x= -75, z = -75 -jos chunkSize = 50

		//  pitää plussata  gridSize*chunkSize/2

		//  
		// 					(	2	*50/2) = 50
		//					(	3	*50/2) = 75

		coordXgrid = Mathf.FloorToInt(position.x) / mapSize;
		if(coordXgrid < 0){
			coordXgrid *= -1;
		}
		coordZgrid = Mathf.FloorToInt(position.z) / mapSize;
		if(coordZgrid < 0){
			coordZgrid *= -1;
		}




	}

	void ChangeVertexCoordsToPositive(){
		


	}

	void UpdateChunkID(){

		Vector3 position = GetComponent<Transform> ().position;
		chunkID.chunkPos = position;
//		chunkID.posX = Mathf.FloorToInt (position.x);
//		chunkID.posY = Mathf.FloorToInt(position.y);
//		chunkID.id = WholeMapGenerator.id;

		if (position.x < 0){
			position.x *= -1;
		}

		//chunkID.chunkX = position.x * 2; --------------------------wat



	}


	public struct ChunkID{

		public Vector3 chunkPos;
		public int posX;
		public int posZ;
		public int id;

		public int chunkX;
		public int chunkZ;

		public ChunkID(int x, int z, Vector3 pos, int identity, int idx, int idy){
			chunkPos = pos;
			posX = x;
			posZ = z;
			id = identity;

			chunkX = idx;
			chunkZ = idy;
		}




	}

	void SetUItext(){

		GetComponentInChildren<TextMesh> ().text = this.chunkID.chunkX.ToString () + ", " + this.chunkID.chunkZ.ToString();

	}

	void GetNoiseCoordMultiplier(){

		int gridSize = WholeMapGenerator.gridSizeInfo;

		int multiplierX = (int)(this.transform.position.x - (mapSize / 2) - 1) / (mapSize-1);
		int multiplierZ = (int)(this.transform.position.z - (mapSize / 2) - 1) / (mapSize-1);

		multiplierX += 1;
		multiplierZ += 1;

		chunkID.chunkX = multiplierX;
		chunkID.chunkZ = multiplierZ;

		Debug.Log (multiplierX +" "+multiplierZ);

//		for (int z = mapSize/2; z < gridSize*mapSize; z+=mapSize){
//			if (this.transform.position.z == z){
//				chunkID.chunkZ = z;
//			}
//			for (int x = mapSize/2; x < gridSize*mapSize; z +=mapSize){
//				if (this.transform.position.x == x){
//					chunkID.chunkX = x;
//				}
//
//			}
//			
//		}

	}

}
