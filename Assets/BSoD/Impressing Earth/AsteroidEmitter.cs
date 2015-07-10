using UnityEngine;
using System.Collections;

public class AsteroidEmitter : MonoBehaviour {
	public Transform target;
	public float initialDistanceFromTarget = 30.0f;
	public float speed = 30.0f;
	public Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);
	public PhysicMaterial physicMaterial;
	public GameObject[] asteroidPrefabs;

	void Update () {
		if (Random.value < MidiJack.GetKnob(1)) {
			GameObject asteroid = Instantiate (asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)]);

			float t =  Random.value * 2 * Mathf.PI;
			float f =  Random.value * 2 * Mathf.PI;

			Vector3 position = new Vector3(Mathf.Sin (t) * Mathf.Cos (f), Mathf.Sin (t) * Mathf.Sin (f), Mathf.Cos (t));
			position *= initialDistanceFromTarget;
			position += target.position;

			asteroid.transform.position = position;
			asteroid.transform.localScale = scale;

			Rigidbody rigidbody = asteroid.AddComponent<Rigidbody> ();
			rigidbody.useGravity = false;
			rigidbody.AddForce ((target.position - position) * speed);

			Collider collider = asteroid.AddComponent<SphereCollider> ();
			collider.material = physicMaterial;
		}
	}
}
