using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float knob = MidiJack.GetKnob (7);
		int speed = (int)Mathf.Lerp(30.0f, 2.0f, knob);

		if (knob != 0) { 
			if (Time.frameCount % speed == 0) {
				Camera.main.backgroundColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);	

				return;
			}
		}

		Camera.main.backgroundColor = new Color (0.0f, 0.0f, 0.0f, 1.0f);	
	}
}
