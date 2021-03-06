﻿using System.Collections;
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


	// korkeuden tunnistus apuja - dictionary nopeampi kuin lista

	Dictionary<Vector2, Vector3> vertexID = new Dictionary<Vector2, Vector3>();
	//Vector3 vertexPos;

	List<Vector3> vertices = new List<Vector3>();
	List<int> triangles = new List<int> ();


	List<Square> squares = new List<Square>();
	Dictionary<WorldPos, Square> squaresDict = new Dictionary<WorldPos, Square> ();
	//float[][] heightArray = new float[mapSize][];

	Mesh mesh;
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

//	float heightScale1;
//	float frequency1;
//	float heightScale2;
//	float frequency2;
//	float heightScale3;
//	float frequency3;


	private float[][] heightArray1;
	private float[][] heightArray2;
	private float[][] heightArray3;
	//

	private float timer;

	public struct WorldPos{
		public int x;
		public int z;
		//public int y;

		public WorldPos(int x, int z){
			this.x = x;
			this.z = z;
			//this.y = y;
		}
	}

	public class Square{

		public int coordX;
		public int coordZ;
		public float coordY;

	}



	// Use this for initialization
	void Start () {

		//GetNoiseValues ();

		mesh = new Mesh ();

		SeedToScale ();

		//float[][] heightArray1 = new float[mapSize][];
		heightArray1 = new float[mapSize][];
		GenerateHeightArray (heightArray1);

		//float[][] heightArray2 = new float[mapSize][];
		heightArray2 = new float[mapSize][];
		GenerateHeightArray (heightArray2);

		//float[][] heightArray3 = new float[mapSize][];
		heightArray3 = new float[mapSize][];
		GenerateHeightArray (heightArray3);


		filter = gameObject.GetComponent<MeshFilter> ();
		coll = gameObject.GetComponent<MeshCollider> ();


		//TestFunction ();
		UpdateChunkID ();
		SetUItext ();
		//GetWorldPos ();



		GenerateVertices (heightArray1, heightArray2, heightArray3, false);
		TriangulateSquare ();
		GenerateMesh ();


		AddVecticesToDict ();

		MapMeshGenerator meshGenerator = GetComponent<MapMeshGenerator> ();

		//Debug.Log (mesh.vertices [0]);

	}

	
	// Update is called once per frame
	void Update () {
		
		timer += Time.deltaTime;

		//update y values if timer is greater than 1 AND a change has happened in frequencies or heightscales

		//if (Input.GetKeyDown(KeyCode.R) && timer > 10){
		if (timer > .5f){
			
			//GetNoiseValues ();
			GenerateVertices (heightArray1, heightArray2, heightArray3, true);
			UpdateMesh ();
			timer = 0;
		}


	}

	//get noise values from wholemapgenerator script -- something WEIRD happening when using these values. Everything fills up with noise
	//looks like heightscale2 and freq2 causing that?
	void GetNoiseValues(){
		float[] noiseInput = WholeMapGenerator.noiseInput;

//		for (int i = 0; i < noiseInput.Length; i++){
//			Debug.Log (noiseInput [i]);
//		}

		heightScale1 = noiseInput[0];
		frequency1 = noiseInput[1];

		heightScale2 = noiseInput[2];
		frequency2 = noiseInput[3];

		heightScale3 = noiseInput[4];
		frequency3 = noiseInput[5];
	}

	//check if variables  have changed
	bool ChangeHappened(){
		return true;
	}

	void GenerateVertices(float[][] heightArray1, float[][] heightArray2, float[][] heightArray3, bool update){
		

//		if (useRandomSeed){
//			seed = Time.time.ToString ();
//		}
		float freq1 = frequency1 / 10;
		float freq2 = frequency2 / 2;
		float freq3 = frequency3 / 100;

//		float freq1 = frequency1;
//		float freq2 = frequency2;
//		float freq3 = frequency3;

		heightArray1 = FillHeightArray (heightArray1, freq1);

		heightArray2 = FillHeightArray (heightArray2, freq2);

		heightArray3 = FillHeightArray (heightArray3, freq3);


		float y = 0f;
		int i = 0;

		if (update){
			vertices.Clear ();
		}

		for (int z = 0; z < mapSize; z++){
			for (int x = 0; x < mapSize; x++){


				// heightArray1 pienetkukkulat
				// heightArray2 ihan pienet epätasaisuudet
				// heightArray3 suuret muodot
				y = 0f;
				y = heightArray1 [x] [z] * heightScale1;


				y += heightArray3 [x] [z] * heightScale3;
//				if (y < 0.5f){
//					y = 0.5f;
//				}
				y += heightArray2 [x] [z] * heightScale2;


				vertices.Add (new Vector3 (x, y, z));

//				if (!update) {
//					
//					vertices.Add (new Vector3 (x, y, z));
//
//				} else {
//					
//					//unity crashes with this
//					//mesh.vertices [i].y = y;
//
//
//				}

				i++;
			}
		}
	}


//	void UpdateHeightVertices(float[][] heightArray1, float[][] heightArray2, float[][] heightArray3){
//		Vector3 newVertex = new Vector3 ();
//		for (int i = 0; i < vertices.Count; i++){
//			newVertex = vertices [i];
//
//			//määritä y (korkeus) kyseiselle vertexille
//
//			newVertex.y = 0; //placeholder
//
//			vertices [i] = newVertex;
//		}
//
//		for (int i = 0; i < mesh.vertices.Length; i++){
//
//			newVertex = mesh.vertices [i];
//
//			//määritä y (korkeus) kyseiselle vertexille
//
//			newVertex.y = 0; 
//
//			mesh.vertices [i] = newVertex;
//		}
//
//
//	}

	void GenerateMesh(){

		//mesh = new Mesh ();
		//Mesh mesh = new Mesh ();
		filter.mesh = mesh;


		mesh.vertices = vertices.ToArray ();
		mesh.triangles = triangles.ToArray ();
		mesh.RecalculateNormals ();
		coll.sharedMesh = mesh;
		
	

	}

	void UpdateMesh(){
		mesh.vertices = vertices.ToArray ();
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
				
				//Vector3 position = GetComponent<Transform>().position;

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
		//public int posX;
		//public int posZ;
		public int id;

		public int chunkX;
		public int chunkZ;

		public ChunkID(int x, int z, Vector3 pos, int identity, int idx, int idy){
			chunkPos = pos;
			//posX = x;
			//posZ = z;
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


	//en oo varma toimiiko, tartteeko tätä edes? Jos mouse RayCasthit:lla tekis mieluummin
	void GenerateSquareGridIDs(){

		int minXSquare = this.chunkID.chunkX * (mapSize - 1);
		int maxXSquare = this.chunkID.chunkX * (mapSize - 1) + (mapSize -1);

		int minZSquare = this.chunkID.chunkZ * (mapSize - 1);
		int maxZSquare = this.chunkID.chunkZ * (mapSize - 1) + (mapSize -1);

		for (int x = minXSquare; x < maxXSquare; x++){
			for (int z = minZSquare; z < maxZSquare; z++){
				
				WorldPos pos = new WorldPos ();
				Square squ = new Square ();

				pos.x = x;
				pos.z = z;

				squ.coordX = pos.x;
				squ.coordZ = pos.z;
				squ.coordY = GetHighestPoint (x,z);

				squaresDict.Add (pos, squ );

			}

		}
			
	}

	//korkein piste neljän vertexin joukossa eli neliössä
	//parametrina vasen ala vertexin sijainti
	float GetHighestPoint(int x, int z){
		
		float highestPoint;

		List<float> points = new List<float> ();
		Vector2 key = new Vector2 (x, z);
		Vector2 key1 = new Vector2 (x+1, z);
		Vector2 key2 = new Vector2 (x, z+1);
		Vector2 key3 = new Vector2 (x+1, z+1);

		points.Add (vertexID [key].y);
		points.Add (vertexID [key1].y);
		points.Add (vertexID [key2].y);
		points.Add (vertexID [key3].y);


		highestPoint = Mathf.Max(Mathf.Max(Mathf.Max (points[0],points[1]),points[2]),points[3]);

		return highestPoint;
	}


	//lisaa vertexit dictiin, joista on helppo hakea x,z koordinaateilla korkein korkeus
	void AddVecticesToDict(){

		for (int i = 0; i < vertices.Count;i++){
			
			float x = vertices [i].x;
			float z = vertices [i].z;
			float y = vertices [i].y;

			vertexID.Add (new Vector2(x, z), new Vector3(x, y, z));
		}



	}


}
