using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour {

	public Sprite level_on;
	public Sprite level_unlocked;


	void Start() {
		if (this.tag == "Custom" && PlayerPrefs.GetString ("Custom" + name.Remove (0, 5)) != "" && PlayerPrefs.GetString ("Custom" + name.Remove (0, 5)) != "0,0,0,0,0,0,0,0,0,0,")
			ChangeSprite ();
	}


	void OnMouseDown() {
		if(!CameraMovement.left && !CameraMovement.right)
			transform.localScale = new Vector2 (transform.localScale.x * 0.8f, transform.localScale.y * 0.8f);
	}


	void OnMouseUp(){
		if (!CameraMovement.left && !CameraMovement.right) {
			Time.timeScale = 1;
			if (this.GetComponent<SpriteRenderer> ().sprite.name == "custom_lvls") {
				transform.localScale = new Vector2 (0.9f, 0.9f);
				if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.4f) {
					AdManager.Instance.ShowInterstitial ();
					AdManager.Instance.StartLoadingInterstitial ();
				}
				UnityEngine.SceneManagement.SceneManager.LoadScene ("MenuCustom");
			} else if(this.tag == "Custom") {
				transform.localScale = new Vector2 (0.2f, 0.2f);
				GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager>().customSlotLoaded = int.Parse(name.Remove(0, 5));
				if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.4f) {
					AdManager.Instance.ShowInterstitial ();
					AdManager.Instance.StartLoadingInterstitial ();
				}
				UnityEngine.SceneManagement.SceneManager.LoadScene ("LevelDesign");
			} else {
				transform.localScale = new Vector2 (0.2f, 0.2f);
				if (this.GetComponent<SpriteRenderer> ().sprite.name != "level_off") {
					String name = this.gameObject.name;
					PlayerPrefs.SetInt ("CurrentLevel", int.Parse (name.Remove (0, 4)));
					if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.4f) {
						AdManager.Instance.ShowInterstitial ();
						AdManager.Instance.StartLoadingInterstitial ();
					}
					UnityEngine.SceneManagement.SceneManager.LoadScene (int.Parse (name.Remove (0, 4)));
				}
			}
			if(SoundControl.sound)
				GameObject.Find("Audio Manager").GetComponents<AudioSource>()[1].Play ();
		}
	}

	public void ChangeSprite() {
		this.GetComponent<SpriteRenderer> ().sprite = level_on;
	}

	public void ChangeUnlockedSprite() {
		this.GetComponent<SpriteRenderer> ().sprite = level_unlocked;
	}
}
