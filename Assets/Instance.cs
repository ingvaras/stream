using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Instance : MonoBehaviour {

	public static bool designState = true;
	public static GameObject selected;
	public static bool submitWindowActive = false;
	public GameObject bulb;
	private GameObject[] bulbs;
	public GameObject battery;
	private GameObject[] batteries;
	public GameObject circle;
	private GameObject[] circles;
	public GameObject framework;
	private GameObject[] frameworks;
	public GameObject triangle;
	private GameObject[] triangles;
	public GameObject line;
	private GameObject[] lines;
	public GameObject corner;
	private GameObject[] corners;
	public GameObject splitway;
	private GameObject[] splitways;
	public GameObject square;
	private GameObject[] squares;
	public GameObject railway;
	private GameObject[] railways;
	public GameObject bulb2;
	public GameObject battery2;
	public GameObject circle2;
	public GameObject framework2;
	public GameObject triangle2;
	public GameObject line2;
	public GameObject corner2;
	public GameObject splitway2;
	public GameObject square2;
	public GameObject railway2;
	public GameObject gameManager;
	public GameObject modalWindow;
	public InputField txt;
	private GameObject[][] list;
	private Dictionary<string, GameObject> objectList;
	private GameObject[] gameObjs2;
	private GameObject[] gameObjs;
	private String[] splitedCode;


	void Start() {
		objectList = new Dictionary<string, GameObject> ()
		{
			{"DesignBulb", bulb},
			{"DesignBattery", battery},
			{"DesignCircle", circle},
			{"DesignFramework", framework},
			{"DesignTriangle", triangle},
			{"DesignLine", line},
			{"DesignCorner", corner},
			{"DesignSplitway", splitway},
			{"DesignSquare", square},
			{"DesignRailway", railway},
		};
		gameObjs = new GameObject[] {
			bulb,
			battery,
			circle,
			framework,
			triangle,
			line,
			corner,
			splitway,
			square,
			railway
		};
		gameObjs2 = new GameObject[] {
			bulb2,
			battery2,
			circle2,
			framework2,
			triangle2,
			line2,
			corner2,
			splitway2,
			square2,
			railway2
		};
		if (this.tag == "DesignBattery") {
			int slot = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ().customSlotLoaded;
			switch (slot) {
			case 1:
				InstantiateObjects (PlayerPrefs.GetString ("Custom1").Split(','), gameObjs, true);
				break;
			case 2:
				InstantiateObjects (PlayerPrefs.GetString ("Custom2").Split(','), gameObjs, true);
				break;
			case 3:
				InstantiateObjects (PlayerPrefs.GetString ("Custom3").Split(','), gameObjs, true);
				break;
			case 4:
				InstantiateObjects (PlayerPrefs.GetString ("Custom4").Split(','), gameObjs, true);
				break;
			case 5:
				InstantiateObjects (PlayerPrefs.GetString ("Custom5").Split(','), gameObjs, true);
				break;
			}
		}
	}


	void OnMouseDown() {
		if(!submitWindowActive) {
			if (selected != null)
				selected.GetComponent<Blinking> ().ChangeBlinking ();
			selected = Instantiate (objectList [this.tag], new Vector2 (transform.position.x, transform.position.y), Quaternion.identity);
		}
	}


	void OnMouseUp() {
		if (!submitWindowActive) {
			selected.GetComponent<DragAndDrop> ().OnMouseUp ();
			if (selected != null && !selected.GetComponent<Blinking> ().GetBlinking ())
				selected.GetComponent<Blinking> ().ChangeBlinking ();
		}
	}


	public void Rotate() {
		if(SoundControl.sound)
			GameObject.Find("Audio Manager").GetComponents<AudioSource>()[1].Play ();
		if(selected != null) {
			if(selected.tag != "Bulb" && selected.tag != "Battery" && selected.tag != "Square" && selected.tag != "Framework" && selected.tag != "Crossroad")
				selected.transform.Rotate (0, 0, -90);
			if (selected.tag == "Battery")
				selected.transform.Rotate (180, 0, 180);
		}
	}


	public void Delete() {
		if(SoundControl.sound)
			GameObject.Find("Audio Manager").GetComponents<AudioSource>()[1].Play ();
		if(selected != null)
			Destroy (selected);
	}


	public void Save() {
		int slot = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ().customSlotLoaded;
		switch (slot) {
		case 1:
			PlayerPrefs.SetString ("Custom1", GenerateString());
			break;
		case 2:
			PlayerPrefs.SetString ("Custom2", GenerateString());
			break;
		case 3:
			PlayerPrefs.SetString ("Custom3", GenerateString());
			break;
		case 4:
			PlayerPrefs.SetString ("Custom4", GenerateString());
			break;
		case 5:
			PlayerPrefs.SetString ("Custom5", GenerateString());
			break;
		}
		if(SoundControl.sound)
			GameObject.Find("Audio Manager").GetComponents<AudioSource>()[1].Play ();
	}


	public void Submit() {
		GameObject.FindGameObjectWithTag ("DatabaseController").GetComponent<DatabaseController> ().Save (txt.text.ToString());
		submitWindowActive = false;
		modalWindow.SetActive (false);
	}


	public void SubmitWindowOn() {
		submitWindowActive = true;
		modalWindow.SetActive (true);
	}


	public void SubmitWindowOff() {
		submitWindowActive = false;
		modalWindow.SetActive (false);
	}


	public void Erase() {
		DestroyObjs ();
		if(SoundControl.sound)
			GameObject.Find("Audio Manager").GetComponents<AudioSource>()[1].Play ();
	}


	public void Restart() {
		DestroyObjs ();
		int index = 10;
		for (int i = 0; i < 10; i++)
			for (int j = 0; j < Int32.Parse(splitedCode [i]); j++) {
				Instantiate (gameObjs2 [i], new Vector2 (Int32.Parse(splitedCode[index]) - 6.5f, Int32.Parse(splitedCode[index + 1]) - 4.0f), Quaternion.Euler(0, Convert.ToInt32(i == 0) * Int32.Parse(splitedCode[index + 2]) * 90, Convert.ToInt32(i != 0) * Int32.Parse(splitedCode[index + 2]) * 90));
				index += 3;
			}
		if(SoundControl.sound)
			GameObject.Find("Audio Manager").GetComponents<AudioSource>()[1].Play ();
	}


	public String GenerateString() {
		bulbs = GameObject.FindGameObjectsWithTag("Bulb");
		batteries = GameObject.FindGameObjectsWithTag("Battery");
		circles = GameObject.FindGameObjectsWithTag("Crossroad");
		frameworks = GameObject.FindGameObjectsWithTag("Framework");
		triangles = GameObject.FindGameObjectsWithTag("Triangle");
		lines = GameObject.FindGameObjectsWithTag("Line");
		corners = GameObject.FindGameObjectsWithTag("Corner");
		splitways = GameObject.FindGameObjectsWithTag("Splitway");
		squares = GameObject.FindGameObjectsWithTag("Square");
		railways = GameObject.FindGameObjectsWithTag("Railway");
		list = new GameObject[][] {bulbs, batteries, circles, frameworks, triangles, lines, corners, splitways, squares, railways};
		String code = "";
		foreach (GameObject[] element in list) {
			code += element.Length;
			code += ',';
		}
		foreach (GameObject[] element in list) {
			foreach (GameObject obj in element) {
				code += (int)Math.Round (obj.transform.position.x + 6.5, MidpointRounding.AwayFromZero);
				code += ',';
				code += obj.transform.position.y + 4;
				code += ',';
				if (obj.tag == "Battery")
					code += Math.Round (obj.transform.rotation.eulerAngles.y / 90, MidpointRounding.AwayFromZero);
				else
					code += Math.Round (obj.transform.rotation.eulerAngles.z / 90, MidpointRounding.AwayFromZero);
				code += ',';
			}
		}
		return code;
	}


	public void Play() {
		GameObject.FindGameObjectWithTag("Web").GetComponent<SpriteRenderer> ().enabled = false;
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ().orthographicSize = 4.5f;
		SwitchState ();
		DestroyObjs ();
		RemoveObjectsFromTheScreen(true);
		splitedCode = GenerateString ().Split(',');
		//splitedCode = "".Split(',');
		splitedCode = "3,5,20,7,14,7,1,4,1,1,12,2,0,10,3,0,2,1,0,10,0,0,9,2,0,5,3,0,6,3,0,7,3,0,9,0,0,8,0,0,12,0,0,7,0,0,6,0,0,5,0,0,4,0,0,3,0,0,1,0,0,2,0,0,0,0,0,7,1,0,7,2,0,0,2,0,1,2,0,3,2,0,2,2,0,4,2,0,5,2,0,6,2,0,11,0,0,13,0,0,9,1,0,0,3,0,1,3,0,2,3,0,0,4,0,7,1,0,9,1,0,4,1,0,1,1,0,0,1,0,3,1,0,9,1,0,5,1,0,6,1,0,8,1,0,10,1,0,11,1,0,9,2,0,7,2,0,12,0,0,11,0,0,10,0,0,9,0,0,10,2,0,11,2,0,4,3,0,12,0,0,12,1,0,3,3,0,7,2,0,13,2,0,12,2,0,11,0,0".Split(',');
		InstantiateObjects (splitedCode, gameObjs2, false);
		designState = false;
		Instantiate (gameManager, new Vector2 (0.0f, 0.0f), Quaternion.identity);
		if(SoundControl.sound)
			GameObject.Find("Audio Manager").GetComponents<AudioSource>()[1].Play ();
	}


	public void DesignMenu() {
		GameObject.FindGameObjectWithTag("Web").GetComponent<Grid>().TurnOnOffGrid ();
		GameObject.FindGameObjectWithTag("Web").GetComponent<Grid>().TurnOnOffGrid ();
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ().orthographicSize = 5.5f;
		SwitchState ();
		DestroyObjs ();
		Destroy(GameObject.FindGameObjectWithTag("GameManager"));
		RemoveObjectsFromTheScreen(false);
		InstantiateObjects (splitedCode, gameObjs, true);
		designState = true;
	}


	void RemoveObjectsFromTheScreen(bool yes) {
		float direction = -1.0f;
		if (yes)
			direction = 1.0f;
		String[] tags = {"DesignBulb", "DesignBattery", "DesignCircle", "DesignFramework", "DesignTriangle", "DesignLine", "DesignCorner", "DesignSplitway", "DesignSquare", "DesignRailway"};
		foreach (String str in tags)
			GameObject.FindGameObjectWithTag (str).transform.position = new Vector2 (GameObject.FindGameObjectWithTag(str).transform.position.x * (float) Math.Pow(1.79f, direction), GameObject.FindGameObjectWithTag(str).transform.position.y); 
	}


	void DestroyWithTag(String tag) {
		GameObject[] objs = GameObject.FindGameObjectsWithTag (tag);
		foreach (GameObject obj in objs)
			Destroy (obj);
	}

	void DestroyObjs() {
		DestroyWithTag ("Bulb");
		DestroyWithTag ("Battery");
		DestroyWithTag ("Crossroad");
		DestroyWithTag ("Framework");
		DestroyWithTag ("Triangle");
		DestroyWithTag ("Line");
		DestroyWithTag ("Corner");
		DestroyWithTag ("Splitway");
		DestroyWithTag ("Square");
		DestroyWithTag ("Railway");
		DestroyWithTag ("CircleLine");
		DestroyWithTag ("Rotor");
	}


	void SwitchState() {
		GameObject.FindGameObjectWithTag ("Load").GetComponent<Image> ().enabled = !GameObject.FindGameObjectWithTag ("Load").GetComponent<Image> ().enabled;
		GameObject.FindGameObjectWithTag ("Save").GetComponent<Image> ().enabled = !GameObject.FindGameObjectWithTag ("Save").GetComponent<Image> ().enabled;
		GameObject.FindGameObjectWithTag ("Submit").GetComponent<Image> ().enabled = !GameObject.FindGameObjectWithTag ("Submit").GetComponent<Image> ().enabled;
		GameObject.FindGameObjectWithTag ("Grid").GetComponent<Image> ().enabled = !GameObject.FindGameObjectWithTag ("Grid").GetComponent<Image> ().enabled;
		GameObject.FindGameObjectWithTag ("Rotate").GetComponent<Image> ().enabled = !GameObject.FindGameObjectWithTag ("Rotate").GetComponent<Image> ().enabled;
		GameObject.FindGameObjectWithTag ("Delete").GetComponent<Image> ().enabled = !GameObject.FindGameObjectWithTag ("Delete").GetComponent<Image> ().enabled;
		GameObject.FindGameObjectWithTag ("Erase").GetComponent<Image> ().enabled = !GameObject.FindGameObjectWithTag ("Erase").GetComponent<Image> ().enabled;
		GameObject.FindGameObjectWithTag ("Restart").GetComponent<Image> ().enabled = !GameObject.FindGameObjectWithTag ("Restart").GetComponent<Image> ().enabled;
	}


	void InstantiateObjects(String[] code, GameObject[] objList, bool setOutdated) {
		int index = 10;
		for (int i = 0; i < 10; i++)
			for (int j = 0; j < Int32.Parse(code [i]); j++) {
				GameObject obj = Instantiate (objList [i], new Vector2 (Int32.Parse(code[index]) - 6.5f, Int32.Parse(code[index + 1]) - 4.0f), Quaternion.Euler(0, Convert.ToInt32(i == 0) * Int32.Parse(code[index + 2]) * 90, Convert.ToInt32(i != 0) * Int32.Parse(code[index + 2]) * 90));
				if(setOutdated)
					obj.GetComponent<DragAndDrop> ().outdated = true;
				index += 3;
			}
	}
}
