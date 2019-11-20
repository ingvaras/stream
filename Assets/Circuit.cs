using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Circuit : MonoBehaviour {

	public Sprite actuatedBulb;
	public Sprite disengagedBulb;
	public Sprite activeOff;
	public Sprite activeLeft;
	public Sprite activeRight;
	public Sprite activeFull;
	public GameObject LevelChanger;
	public GameObject circleLine;
	private GameObject[] bulbs;
	private GameObject[] batteries;
	private GameObject[] lines;
	private GameObject[] corners;
	private GameObject[] crossroads;
	private GameObject[] splitways;
	private GameObject[] triangles;
	private GameObject menu;
	private List<GameObject> visitedObjects = new List<GameObject>();
	private float nextPosX;
	private float nextPosY;
	private bool recalculate = true;
	private bool[] activatedBulbs;


	void Update() {
		if (recalculate == true) {
			bulbs = GameObject.FindGameObjectsWithTag("Bulb");
			batteries = GameObject.FindGameObjectsWithTag("Battery");
			lines = GameObject.FindGameObjectsWithTag("Line");
			corners = GameObject.FindGameObjectsWithTag("Corner");
			crossroads = GameObject.FindGameObjectsWithTag("Crossroad");
			splitways = GameObject.FindGameObjectsWithTag("Splitway");
			triangles = GameObject.FindGameObjectsWithTag("Triangle");
			activatedBulbs = new bool[bulbs.Length];
			for (int i = 0; i < bulbs.Length; i++) {
				if (bulbs [i].GetComponent<SpriteRenderer> ().sprite == disengagedBulb)
					activatedBulbs [i] = false;
				else
					activatedBulbs [i] = true;
			}
			foreach (GameObject bulb in bulbs)
				bulb.GetComponent<SpriteRenderer> ().sprite = disengagedBulb;
			foreach (GameObject triangle in triangles) {
				triangle.GetComponent<Triangle> ().leftActive = false;
				triangle.GetComponent<Triangle> ().rightActive = false;
				triangle.GetComponent<SpriteRenderer> ().sprite = activeOff;
			}
			foreach (GameObject battery in batteries) {
				//CheckCircuit (batteries [i], batteries [i].transform.rotation.eulerAngles.z);
				CheckCircuit (battery, 0);
				CheckCircuit (battery, 180);
				visitedObjects.Clear ();
			}
			for(int i = 0; i < bulbs.Length; i++) {
				if (bulbs [i].GetComponent<SpriteRenderer> ().sprite == actuatedBulb) {
					if (i == bulbs.Length - 1 && GameObject.FindGameObjectsWithTag ("DesignBattery").Length == 0)
						Instantiate (LevelChanger);
				} else
					break;
			}
			recalculate = false;
		}
	}

	void CheckCircuit(GameObject obj, float rot) {
		if (visitedObjects.Contains(obj))
			return;
		if(obj.tag == "Line" || obj.tag == "Corner")
			visitedObjects.Add (obj);
		nextPosX = Mathf.Round((obj.transform.position.x + Mathf.Cos (rot * Mathf.PI / 180)) * 10) / 10;
		nextPosY = Mathf.Round((obj.transform.position.y + Mathf.Sin (rot * Mathf.PI / 180)) * 10) / 10;
		for (int i = 0; i < bulbs.Length; i++) {
			if (nextPosX == bulbs [i].transform.position.x && nextPosY == bulbs [i].transform.position.y && Mathf.Abs (rot) == 90) {
				bulbs [i].GetComponent<SpriteRenderer> ().sprite = actuatedBulb;
				if (activatedBulbs [i] == false && SoundControl.sound)
					this.GetComponents<AudioSource> () [0].Play ();
				return;
			}
		}
		foreach (GameObject corner in corners) {
			if (nextPosX == corner.transform.position.x && nextPosY == corner.transform.position.y && ((rot + 270) % 360 == corner.transform.rotation.eulerAngles.z || (rot + 180) % 360 == corner.transform.rotation.eulerAngles.z)) {
				if (Mathf.Abs (corner.transform.rotation.eulerAngles.z - rot) == 180)
					CheckCircuit (corner, (rot + 90) % 360);
				else
					CheckCircuit (corner, (rot + 270) % 360);
			}
		}
		nextPosX = Mathf.Round((obj.transform.position.x + Mathf.Cos (rot * Mathf.PI / 180)) * 10) / 10;
		nextPosY = Mathf.Round((obj.transform.position.y + Mathf.Sin (rot * Mathf.PI / 180)) * 10) / 10;
		foreach (GameObject line in lines) {
			if (nextPosX == line.transform.position.x && nextPosY == line.transform.position.y) {
				if (Mathf.Round (Mathf.Abs (Mathf.Sin (rot * Mathf.PI / 180))) == Mathf.Round (Mathf.Abs (Mathf.Sin (line.transform.rotation.eulerAngles.z * Mathf.PI / 180)))) {
					CheckCircuit (line, rot);
					return;
				}
			}
		}
		foreach (GameObject circle in crossroads) {
			if (nextPosX == circle.transform.position.x && nextPosY == circle.transform.position.y) {
				CheckCircuit (circle, rot);
				CheckCircuit (circle, (rot + 90) % 360);
				CheckCircuit (circle, (rot + 270) % 360);
				return;
			}
		}
		foreach (GameObject splitway in splitways) {
			if (nextPosX == splitway.transform.position.x && nextPosY == splitway.transform.position.y) {
				for(int j = 0; j < 4; j++)
					if((splitway.transform.rotation.eulerAngles.z + j * 90) % 360 != Mathf.Round(rot + 180) % 360 && j != 1 && (splitway.transform.rotation.eulerAngles.z + 270) % 360 != rot)
						CheckCircuit (splitway, (splitway.transform.rotation.eulerAngles.z + j * 90) % 360);
				return;
			}
		}
		foreach (GameObject triangle in triangles) {
			if (nextPosX == triangle.transform.position.x && nextPosY == triangle.transform.position.y) {
				if (!triangle.GetComponent<Triangle> ().leftActive || !triangle.GetComponent<Triangle> ().rightActive) {
					if (triangle.transform.rotation.eulerAngles.z == rot) {
						triangle.GetComponent<Triangle> ().rightActive = true;
						triangle.GetComponent<SpriteRenderer> ().sprite = activeRight;
					} else if((triangle.transform.rotation.eulerAngles.z + 180) % 360 == rot) {
						triangle.GetComponent<Triangle> ().leftActive = true;
						triangle.GetComponent<SpriteRenderer> ().sprite = activeLeft;
					}
				}
				if (triangle.GetComponent<Triangle> ().leftActive && triangle.GetComponent<Triangle> ().rightActive) {
					CheckCircuit (triangle, (triangle.transform.rotation.eulerAngles.z + 270) % 360);
					triangle.GetComponent<SpriteRenderer> ().sprite = activeFull;
				}
				return;
			}
		}
	}

	public void restartLevel() {
		if (!ChangeLevel.sceneFading) {
			if(SoundControl.sound)
				GameObject.Find("Audio Manager").GetComponents<AudioSource>()[1].Play ();
			if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.4f) {
				AdManager.Instance.ShowInterstitial ();
				AdManager.Instance.StartLoadingInterstitial ();
			}
			SceneManager.LoadScene (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex);
		}
	}

	public void loadMenu() {
		if (!ChangeLevel.sceneFading) {
			GameObject.FindGameObjectWithTag ("Menu").GetComponent<Image> ().enabled = false;
			if(GameObject.FindGameObjectWithTag ("Restart") != null)
				GameObject.FindGameObjectWithTag ("Restart").GetComponent<Image> ().enabled = false;
			Time.timeScale = 0;
			if(SoundControl.sound)
				GameObject.Find("Audio Manager").GetComponents<AudioSource>()[1].Play ();
			menu = GameObject.FindGameObjectWithTag ("menuScene");
			for(int i = 0; i < menu.transform.childCount; ++i)
				menu.transform.GetChild(i).gameObject.SetActive(true);
			if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.4f) {
				AdManager.Instance.ShowInterstitial ();
				AdManager.Instance.StartLoadingInterstitial ();
			}
		}
	}

	public void loadMenuCustom() {
		if (Instance.designState) {
			GameObject.FindGameObjectWithTag ("Menu").GetComponent<Image> ().enabled = false;
			if (SoundControl.sound)
				GameObject.Find ("Audio Manager").GetComponents<AudioSource> () [1].Play ();
			if (UnityEngine.Random.Range (0.0f, 1.0f) <= 0.4f) {
				AdManager.Instance.ShowInterstitial ();
				AdManager.Instance.StartLoadingInterstitial ();
			}
			SceneManager.LoadScene ("MenuCustom");
		} else
			GameObject.FindGameObjectWithTag("DesignBattery").GetComponent<Instance>().DesignMenu ();
	}

	public void resume()
	{
		GameObject.FindGameObjectWithTag ("Menu").GetComponent<Image> ().enabled = true;
		if(GameObject.FindGameObjectWithTag ("Restart") != null)
			GameObject.FindGameObjectWithTag ("Restart").GetComponent<Image> ().enabled = true;
		Time.timeScale = 1;
		if(SoundControl.sound)
			GameObject.Find("Audio Manager").GetComponents<AudioSource>()[1].Play ();
		//Destroy (GameObject.FindGameObjectWithTag("menuScene"));
		menu = GameObject.FindGameObjectWithTag ("menuScene");
		for(int i = 0; i < menu.transform.childCount; ++i)
			menu.transform.GetChild(i).gameObject.SetActive(false);
	}
		
	public void setRecalculateValue(bool value) {
		recalculate = value;
	}

}
