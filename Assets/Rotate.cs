using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	private GameObject gameManager;
	private GameObject[] lines;
	private GameObject[] corners;
	private GameObject[] splitways;
	private GameObject[] triangles;
	private float extensionRate = 1.1f;
	private bool expanded = false;
	public GameObject rotor;

	void Start() {
		gameManager = GameObject.FindGameObjectWithTag("GameManager");
		Instantiate (rotor, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.Euler (0, 0, 0));
	}

	void OnMouseDown() {
		if (!ChangeLevel.sceneFading && Time.timeScale != 0) {
			lines = GameObject.FindGameObjectsWithTag ("Line");
			corners = GameObject.FindGameObjectsWithTag ("Corner");
			splitways = GameObject.FindGameObjectsWithTag ("Splitway");
			triangles = GameObject.FindGameObjectsWithTag ("Triangle");
			RotateObject (lines);
			RotateObject (corners);
			RotateObject (splitways);
			RotateObject (triangles);
			transform.localScale *= extensionRate;
			expanded = true;
			gameManager.GetComponent<Circuit> ().setRecalculateValue (true);
		}
	}

	void OnMouseUp() {
		if (expanded) {
			transform.localScale /= extensionRate;
			expanded = false;
		}
	}

	void RotateObject(GameObject[] array)
	{
		foreach (GameObject element in array)
			if (gameObject.transform.position.x == element.transform.position.x && gameObject.transform.position.y == element.transform.position.y)
				element.transform.Rotate (0, 0, -90);
		if(SoundControl.sound)
			this.GetComponent<AudioSource>().Play();
	}
}
