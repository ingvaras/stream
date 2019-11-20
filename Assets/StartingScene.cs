using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingScene : MonoBehaviour {

	public static int otherLevelCount = 3;

	void Start () {
		if (PlayerPrefs.GetInt ("MusicState") == 0)
			SoundControl.music = true;
		if (PlayerPrefs.GetInt ("SoundState") == 0)
			SoundControl.sound = true;
		if (PlayerPrefs.GetInt ("CurrentLevel") != 0 && PlayerPrefs.GetInt ("CurrentLevel") != SceneManager.sceneCountInBuildSettings - otherLevelCount) {
			UnityEngine.SceneManagement.SceneManager.LoadScene (PlayerPrefs.GetInt ("CurrentLevel"));
		}
		else {
			PlayerPrefs.SetInt ("CurrentLevel", 1);
			UnityEngine.SceneManagement.SceneManager.LoadScene (1);
		}
	}
}
