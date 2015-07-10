using UnityEngine;

public class EarthSphere : MonoBehaviour {
	int nbLong = 30;
	int nbLat = 30;

	private float[] impressions;

	void Start () {
		impressions = new float[(nbLong+1) * nbLat + 2];
	}

	void Update () {
		transform.Rotate (0.0f, (MidiJack.GetKnob(0, 0.5f) - 0.5f) * 5.0f, 0.0f);

		object[] fft = OscJack.OscMaster.MasterDirectory.GetData ("/audio/fft");
		if (fft != null && fft.Length > 0) {
			for (int k = 0; k < fft.Length; k++) {
				int impressedLong = Random.Range (0, nbLong);
				int impressedLat = Random.Range (0, nbLat);

				impressions [impressedLong + impressedLat * (nbLong + 1) + 1] = Mathf.Clamp01 ((float)fft [k] - 0.5f) * 2.5f;
				//impressions [k] = (float)fft[(int)Mathf.Lerp (0, fft.Length - 1, (float)k / (float)impressions.Length)];
			}

			//		float[] maImpressions = new float[impressions.Length];
			//		for (int j = 0; j < impressions.Length; j++) {
			//			if (j > 0 && j < impressions.Length - 1) {
			//				maImpressions[j] = (impressions[j - 1] + impressions[j] + impressions[j + 1]) / 3.0f;
			//			}
			//		}
			//		
			//		impressions = maImpressions;
		}

		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		mesh.Clear();
		
		float radius = 1.0f;

		Vector3[] vertices = new Vector3[(nbLong+1) * nbLat + 2];
		float _pi = Mathf.PI;
		float _2pi = _pi * 2f;
		
		vertices[0] = Vector3.up * radius;
		for( int lat = 0; lat < nbLat; lat++ )
		{
			float a1 = _pi * (float)(lat+1) / (nbLat+1);
			float sin1 = Mathf.Sin(a1);
			float cos1 = Mathf.Cos(a1);
			
			for( int lon = 0; lon <= nbLong; lon++ )
			{
				float a2 = _2pi * (float)(lon == nbLong ? 0 : lon) / nbLong;
				float sin2 = Mathf.Sin(a2);
				float cos2 = Mathf.Cos(a2);
				
				vertices[ lon + lat * (nbLong + 1) + 1] = new Vector3( sin1 * cos2, cos1, sin1 * sin2 ) * (1.0f + impressions[lon + lat * (nbLong + 1) + 1]) * ((float)fft [0] / 5.0f + 1.0f);
				impressions[lon + lat * (nbLong + 1) + 1] = Mathf.Clamp01(impressions[lon + lat * (nbLong + 1) + 1] - 0.02f);
			}
		}
		vertices[vertices.Length-1] = Vector3.up * -radius;

		Vector3[] normales = new Vector3[vertices.Length];
		for (int n = 0; n < vertices.Length; n++) {
			normales [n] = vertices [n].normalized;
		}

		Vector2[] uvs = new Vector2[vertices.Length];
		uvs[0] = Vector2.up;
		uvs[uvs.Length-1] = Vector2.zero;
		for( int lat = 0; lat < nbLat; lat++ ) {
			for( int lon = 0; lon <= nbLong; lon++ ) {
				uvs[lon + lat * (nbLong + 1) + 1] = new Vector2( (float)lon / nbLong, 1f - (float)(lat+1) / (nbLat+1) );
			}
		}

		int nbFaces = vertices.Length;
		int nbTriangles = nbFaces * 2;
		int nbIndexes = nbTriangles * 3;
		int[] triangles = new int[ nbIndexes ];
		
		int i = 0;
		for( int lon = 0; lon < nbLong; lon++ )
		{
			triangles[i++] = lon+2;
			triangles[i++] = lon+1;
			triangles[i++] = 0;
		}
		
		for( int lat = 0; lat < nbLat - 1; lat++ )
		{
			for( int lon = 0; lon < nbLong; lon++ )
			{
				int current = lon + lat * (nbLong + 1) + 1;
				int next = current + nbLong + 1;
				
				triangles[i++] = current;
				triangles[i++] = current + 1;
				triangles[i++] = next + 1;
				
				triangles[i++] = current;
				triangles[i++] = next + 1;
				triangles[i++] = next;
			}
		}
		
		for( int lon = 0; lon < nbLong; lon++ )
		{
			triangles[i++] = vertices.Length - 1;
			triangles[i++] = vertices.Length - (lon+2) - 1;
			triangles[i++] = vertices.Length - (lon+1) - 1;
		}

		mesh.vertices = vertices;
		mesh.normals = normales;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		
		mesh.RecalculateBounds();
		mesh.Optimize();

		GetComponent<MeshCollider> ().sharedMesh = mesh;
	}
}
