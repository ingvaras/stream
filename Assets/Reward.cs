using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour {

	private float extensionRate = 1.1f;
	private bool expanded = false;
	public GameObject LevelChanger;


	void OnMouseDown() {
		if (!ChangeLevel.sceneFading) {
			if (tag == "Transparent")
				Instantiate (LevelChanger);
			transform.localScale *= extensionRate;
			expanded = true;
			if(SoundControl.sound)
				GameObject.Find("Audio Manager").GetComponents<AudioSource>()[1].Play ();
		}
	}

	void OnMouseUp() {
		if (expanded) {
			if (tag == "Google-plus")
				Application.OpenURL ("https://plus.google.com/u/3/115405599447736159675");
			else if (tag == "Linkedin")
				Application.OpenURL ("https://www.linkedin.com/in/ingvaras-g-82a193138/");
			else if (tag == "Twitter")
				Application.OpenURL("https://twitter.com/ingvaras_");
			transform.localScale /= extensionRate;
			expanded = false;
		}
	}
}
