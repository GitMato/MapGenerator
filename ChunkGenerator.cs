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
	//toimii 101:lla
	public static int mapSize = 255;
	//public int mapGridSize = 2;


	List<Vector3> vertices = new List<Vector3>();
	List<int> triangles = new List<int> ();
	//float[][] heightArray = new float[mapSize][];

	MeshFilter filter;
	MeshCollider coll;

	//int coordXgrid = 0;
	//int coordZgrid = 0;

	ChunkID chunkID;


	//Noise controllers
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

	//





	// Use this for initialization
	void Start () {



		SeedToScale ();

		float[][] heightArray1 = new float[mapSize][];
		GenerateHeightArray (heightArray1);

		float[][] heightArray2 = new float[mapSize][];
		GenerateHeightArray (heightArray2);

		float[][] heightArray3 = new float[mapSize][];
		GenerateHeightArray (heightArray3);


		filter = gameObject.GetComponent<MeshFilter> ();
		coll = gameObject.GetComponent<MeshCollider> ();


		//TestFunction ();
		UpdateChunkID ();
		SetUItext ();
		//GetWorldPos ();



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


				// heightArray1 pienetkukkulat
				// heightArray2 ihan pienet epätasaisuudet
				// heightArray3 suuret muodot

				y = heightArray1 [x] [z] * heightScale1;


				y += heightArray3 [x] [z] * heightScale3;
//				if (y < 0.5f){
//					y = 0.5f;
//				}
				y += heightArray2 [x] [z] * heightScale2;

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

		//int countX = 0;
		//int countZ = 0;

		for (int z = 0; z < mapSize; z++ ){
			

			for (int x = 0; x < mapSize; x++){
				
				Vector3 position = GetComponent<Transform>().position;

//				float xn = position.x * x;
//				float zn = position.z * z;

				float xn = (x + (this.chunkID.chunkX * mapSize) - chunkID.chunkX);
				float zn = (z + (this.chunkID.chunkZ * mapSize) - chunkID.chunkZ);



//				float xn = (x + (coordXgrid * mapSize));
//				float zn = (z + (coordZgrid * mapSize));

				//float xn = (x + (gridSize * mapSize));
				//float zn = (z + (gridSize * mapSize));
				//Debug.Log (position);


				//heightArray [x] [z] = Mathf.PerlinNoise (xn * frequency, zn * frequency);

				heightArray [x] [z] = Noise.Generate (xn * frequency , zn * frequency);


			}
		}


		return heightArray;
	}
		

	int GetGridSize(){

		int gridSize;

		Vector3 currentPos = GetComponent<Transform> ().position;

		//oletettavasti negatiivinen
		gridSize = ((Mathf.FloorToInt(currentPos.x)-(mapSize/2))*-1)/mapSize;
		Debug.Log (gridSize);


		return gridSize;
		
	}


	//useless
//	void GetWorldPos(){
//		Vector3 position = GetComponent<Transform> ().position;
//
//
//		//jos gridsize = 2 niin kaukaisin vertex on x= -50, z = -50 -jos chunkSize = 50
//
//		//jos gridsize = 3 niin kaukaisin vertex on x= -75, z = -75 -jos chunkSize = 50
//
//		//  pitää plussata  gridSize*chunkSize/2
//
//		//  
//		// 					(	2	*50/2) = 50
//		//					(	3	*50/2) = 75
//
//		coordXgrid = Mathf.FloorToInt(position.x) / mapSize;
//		if(coordXgrid < 0){
//			coordXgrid *= -1;
//		}
//		coordZgrid = Mathf.FloorToInt(position.z) / mapSize;
//		if(coordZgrid < 0){
//			coordZgrid *= -1;
//		}
//
//	}
//

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

	void UpdateChunkID(){

		int gridSize = WholeMapGenerator.gridSizeInfo;

		//int multiplierX = (int)(this.transform.position.x - (mapSize / 2) - 1) / (mapSize-1);

		//int multiplierZ = (int)(this.transform.position.z - (mapSize / 2) - 1) / (mapSize-1);

		float posX = this.transform.position.x;
		float posZ = this.transform.position.z;


		int idX = Mathf.RoundToInt((posX-(mapSize/2))/(mapSize-1));
		//idX = Mathf.RoundToInt(idX);
		int idZ = Mathf.RoundToInt((posZ-(mapSize/2))/(mapSize-1));
		//idZ = Mathf.RoundToInt(idZ);

		//int multiplierX = (int)(posX-(mapSize/2))/(mapSize-1); //nnäissä vika
		//int multiplierZ = (int)(posZ-(mapSize/2))/(mapSize-1);

		// gridSize 8 eli multiplieriin lisätään 4
		//multiplierX += Mathf.FloorToInt(gridSize/2);
		//multiplierZ += Mathf.FloorToInt(gridSize/2);

		idX += Mathf.FloorToInt(gridSize/2);
		idZ += Mathf.FloorToInt(gridSize/2);

		if ((gridSize) % 2 != 0){
			
			//multiplierX += 1;
			//multiplierZ += 1;

			idX += 1;
			idZ += 1;

		} 

		//debuggausta
//		if (multiplierX == 4 || multiplierZ == 0){
//			Debug.Log ("id "+ multiplierX +","+ multiplierZ +" pos ="+this.transform.position.x);
//			Debug.Log ("XZ "+ idX +","+ idZ +" pos ="+this.transform.position.x);
//		}


		chunkID.chunkX = idX;
		chunkID.chunkZ = idZ;

		//Debug.Log (multiplierX +" "+multiplierZ);



	}

	void SeedToScale(){

		int seed = WholeMapGenerator.seedHash;

//		heightScale1 = 0 * seed;
//		frequency1 = 0 * seed;
//		heightScale2 = 0 * seed;
//		frequency2 = 0 * seed;
//		heightScale3 = 0 * seed;
//		frequency3 = 0 * seed;

//		int offset = Mathf.RoundToInt(seed*0.01);




	}

	void TestFunction(){

		float test1 = 25.5f;
		float test2 = 25.6f;
		float test3 = 25.4f;
		float test4 = -1 / 50;
		float test5 = -0.555f;

		int testi1 = (int)test1;
		int testi2 = (int)test2;
		int testi3 = (int)test3;
		int testi4 = (int)test4;
		int testi5 = (int)test5;

		//Debug.Log ("25.5 = "+testi1+", 25.6 = "+testi2+", 25.4 = "+testi3+", -1/50 = "+test4+", -0.555 = "+testi5);

	}



}
