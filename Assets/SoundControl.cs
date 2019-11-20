using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour {

	public static bool music = false;
	public static bool sound = false;
	Image musicButtonImage;
	Image soundButtonImage;
	public Sprite musicOn;
	public Sprite musicOff;
	public Sprite soundOn;
	public Sprite soundOff;

	void Start () {
		if (PlayerPrefs.GetInt ("MusicState") == 0)
			music = true;
		if (PlayerPrefs.GetInt ("SoundState") == 0)
			sound = true;
		musicButtonImage = GameObject.FindGameObjectWithTag("Music").GetComponent<Image>();
		soundButtonImage = GameObject.FindGameObjectWithTag("Sound").GetComponent<Image>();

		if (music)
			musicButtonImage.sprite = musicOn;
		else
			musicButtonImage.sprite = musicOff;
		if (sound)
			soundButtonImage.sprite = soundOn;
		else
			soundButtonImage.sprite = soundOff;
	}

	public void TurnOnOffMusic()
	{
		music = !music;
		if (music) {
			musicButtonImage.sprite = musicOn;
			GameObject.Find("Audio Manager").GetComponents<AudioSource>()[0].mute = false;
			PlayerPrefs.SetInt ("MusicState", 0);
		} else {
			musicButtonImage.sprite = musicOff;
			GameObject.Find("Audio Manager").GetComponents<AudioSource>()[0].mute = true;
			PlayerPrefs.SetInt ("MusicState", 1);
		}
		if(SoundControl.sound)
			GameObject.Find("Audio Manager").GetComponents<AudioSource>()[1].Play ();
	}

	public void TurnOnOffSound()
	{
		sound = !sound;
		if (sound) {
			soundButtonImage.sprite = soundOn;
			PlayerPrefs.SetInt ("SoundState", 0);
		} else {
			soundButtonImage.sprite = soundOff;
			PlayerPrefs.SetInt ("SoundState", 1);
		}
		if(SoundControl.sound)
			GameObject.Find("Audio Manager").GetComponents<AudioSource>()[1].Play ();
	}
}
