using UnityEngine;
using System.Collections;

public class ImpressingEarth : MonoBehaviour {
	public GameObject earthSphere;
	public GameObject prefab;

	void Update () {
		if (earthSphere == null) {
			earthSphere = Instantiate (prefab);
			earthSphere.SetActive(false);
			earthSphere.transform.parent = transform;

			Invoke("ResurrectEarthSphere", 3.0f);
		}
	}

	void ResurrectEarthSphere() {
		earthSphere.SetActive(true);
	}
}
