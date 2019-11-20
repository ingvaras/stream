using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Grid : MonoBehaviour {

	public static bool grid = true;
	Image gridButtonImage;
	public Sprite gridOn;
	public Sprite gridOff;

	void Start () {
		gridButtonImage = GameObject.FindGameObjectWithTag("Grid").GetComponent<Image>();
		if (grid) {
			gridButtonImage.sprite = gridOn;
			gameObject.GetComponent<SpriteRenderer> ().enabled = true;
		} else {
			gridButtonImage.sprite = gridOff;
			gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		}
	}

	public void TurnOnOffGrid()
	{
		grid = !grid;
		if (grid) {
			gridButtonImage.sprite = gridOn;
			gameObject.GetComponent<SpriteRenderer> ().enabled = true;
		} else {
			gridButtonImage.sprite = gridOff;
			gameObject.GetComponent<SpriteRenderer> ().enabled = false;

		}
		if(SoundControl.sound)
			GameObject.Find("Audio Manager").GetComponents<AudioSource>()[1].Play ();
	}
}
