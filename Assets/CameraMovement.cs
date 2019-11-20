using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour {

	private float speed = 50;
	public GameObject leftArrow;
	public GameObject rightArrow;
	private GameObject[] levels;
	public static bool left = false;
	public static bool right = false;
	private Vector3 nextCameraPosition = new Vector3(0f, 0f, -10f);

	void Start() {
		levels = GameObject.FindGameObjectsWithTag("Level");
		int UnlockedLevels = PlayerPrefs.GetInt ("LevelsUnlocked");
		for (int i = 0; i < levels.Length; i++) {
			if (int.Parse (levels [i].name.Remove (0, 4)) < UnlockedLevels)
				levels [i].GetComponent<LoadLevel> ().ChangeSprite ();
			else if(int.Parse (levels [i].name.Remove (0, 4)) == UnlockedLevels)
				levels [i].GetComponent<LoadLevel> ().ChangeUnlockedSprite ();
		}
		if (PlayerPrefs.GetInt ("CurrentLevel") <= 10)
			leftArrow.GetComponent<Image> ().enabled = false;
		else if (PlayerPrefs.GetInt ("CurrentLevel") <= 25) {
			nextCameraPosition = new Vector3 (15f, 0f, -10f);
			transform.position = nextCameraPosition;
		} else if (PlayerPrefs.GetInt ("CurrentLevel") <= 40) {
			nextCameraPosition = new Vector3 (30f, 0f, -10f);
			transform.position = nextCameraPosition;
		} else if (PlayerPrefs.GetInt ("CurrentLevel") <= 55) {
			nextCameraPosition = new Vector3 (45f, 0f, -10f);
			transform.position = nextCameraPosition;
			rightArrow.GetComponent<Image> ().enabled = false;
		}
	}

	void Update() {
		if (left || right) {
			transform.position = Vector3.MoveTowards (transform.position, nextCameraPosition, speed * Time.deltaTime);
			speed *= 0.948f;
			if (transform.position == nextCameraPosition) {
				right = false;
				left = false;
				speed = 50;
				Time.timeScale = 0;
			}
		}
	}

	public void Right()
	{
		if (!right && !left) {
			Time.timeScale = 1;
			nextCameraPosition.x += 15f;
			right = true;
			if (nextCameraPosition.x == 45.0f)
				rightArrow.GetComponent<Image> ().enabled = false;
			else if (!leftArrow.GetComponent<Image> ().enabled)
				leftArrow.GetComponent<Image> ().enabled = true;
		}
		if(SoundControl.sound)
			GameObject.Find("Audio Manager").GetComponents<AudioSource>()[1].Play ();
	}

	public void Left()
	{
		if (!left && !right) {
			Time.timeScale = 1;
			nextCameraPosition.x -= 15f;
			left = true;
			if (nextCameraPosition.x == 0.0f)
				leftArrow.GetComponent<Image> ().enabled = false;
			else if (!rightArrow.GetComponent<Image> ().enabled)
				rightArrow.GetComponent<Image> ().enabled = true;
		}
		if(SoundControl.sound)
			GameObject.Find("Audio Manager").GetComponents<AudioSource>()[1].Play ();
	}

}
