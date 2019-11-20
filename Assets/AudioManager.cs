using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public int customSlotLoaded;

	void Start () {
		DontDestroyOnLoad (this.gameObject);
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
		if (this.gameObject.GetComponents<AudioSource> ()[0].volume < 0.3f && SoundControl.music)
			this.gameObject.GetComponents<AudioSource> ()[0].volume += 0.003f;
	}
}
