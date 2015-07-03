using UnityEngine;

public class EqualizerField : MonoBehaviour
{
	private float[,] heightMap;

	void Start ()
	{
		int resolution = GetComponent<Terrain> ().terrainData.heightmapResolution;;
		heightMap = new float[resolution, resolution]; 
	}
	
	void Update ()
	{
		if (!OscJack.OscMaster.MasterDirectory.HasData ("/audio/fft")) return;
		object[] fft = OscJack.OscMaster.MasterDirectory.GetData ("/audio/fft");

		for (int h = 127; h >= 0; --h) {
			for (int w = 0; w < 128; w++) {
				if (h > 0) {
					heightMap [h, w] = heightMap [h - 1, w];
				} else {
					float b = w / 128.0f * (fft.Length - 1);
					int ib = Mathf.FloorToInt (b);
					float pb = b - ib;
					heightMap [0, w] = Mathf.Clamp01((float)fft[ib] * (1 - pb) + (float)fft[ib + 1] * pb);
				}
			}
		}
		
		GetComponent<Terrain> ().terrainData.SetHeights (0, 0, heightMap);
	}
}
