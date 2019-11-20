using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour {

	public static bool sceneFading = false;

	IEnumerator Start()
	{
		DontDestroyOnLoad (this.gameObject);
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex + 1 > PlayerPrefs.GetInt ("LevelsUnlocked")) {
			PlayerPrefs.SetInt ("LevelsUnlocked", UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex + 1);
			PlayerPrefs.Save ();
		}
		sceneFading = true;
		yield return new WaitForSeconds (0.5f);
		float fadeTime = GameObject.Find ("Game Manager").GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime + 1f);
		if (Random.Range (0.0f, 1.0f) <= AdFrequency (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex)) {
			AdManager.Instance.ShowInterstitial ();
			AdManager.Instance.StartLoadingInterstitial ();
		}
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex != SceneManager.sceneCountInBuildSettings - StartingScene.otherLevelCount) {
			PlayerPrefs.SetInt ("CurrentLevel", UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex + 1);
			UnityEngine.SceneManagement.SceneManager.LoadScene (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex + 1);
		}
		else {
			yield return new WaitForSeconds (1.5f);
			PlayerPrefs.SetInt ("CurrentLevel", 1);
			UnityEngine.SceneManagement.SceneManager.LoadScene (1);
		}
		yield return new WaitForSeconds (0.5f);
		sceneFading = false;
		Destroy (this.gameObject);
	}

	private double AdFrequency(int currentScene) {
		return (0.2 + currentScene / 150);
	}

}
