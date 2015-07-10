using UnityEngine;
using Exploder;

public class MidiExploder : MonoBehaviour {
	void Update () {
		if (MidiJack.GetKnob (6) > 0.0f) {
			GetComponent<ExploderObject> ().Explode();
		}
	}
}
