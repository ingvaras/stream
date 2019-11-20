using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Connect : MonoBehaviour {

	public GameObject line;
	public Sprite usedCircle;
	private GameObject circleOfThisLine;
	private GameObject gameManager;
	private GameObject[] circleLines;
	private GameObject[] circles;
	private Vector3 offset;
	private Vector3 curPosition;
	private Vector3 startingPosition;
	private float angle;
	private float radius = 1.5f;
	private float extensionRate = 1.1f;
	private bool dragging = false;

	void Start(){
		gameManager = GameObject.FindGameObjectWithTag("GameManager");
		circles = GameObject.FindGameObjectsWithTag("Crossroad");
		foreach (GameObject circle in circles)
			if (transform.position == circle.transform.position)
				circleOfThisLine = circle;
	}

	void OnMouseDown() {
		if (!ChangeLevel.sceneFading && Time.timeScale != 0) {
			dragging = true;
			GetComponent<SpriteRenderer> ().sortingOrder = 2;
			startingPosition = gameObject.transform.position;
			circleOfThisLine.transform.localScale *= extensionRate;
			offset = startingPosition - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
			if(SoundControl.sound)
				this.GetComponent<AudioSource>().Play();
		}
	}

	void OnMouseDrag() {
		if (!ChangeLevel.sceneFading && dragging && Time.timeScale != 0) {
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
			curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
			float xDistance = curPosition.x - startingPosition.x;
			float yDistance = curPosition.y - startingPosition.y;
			float Distance = Mathf.Sqrt (Mathf.Pow (xDistance, 2) + Mathf.Pow (yDistance, 2));
			if (xDistance != 0)
				angle = Mathf.Atan (yDistance / xDistance);
			else if (yDistance >= 0)
				angle = Mathf.PI / 2;
			else
				angle = -Mathf.PI / 2;
			if (Distance > radius * 2) {
				if (xDistance < 0)
					transform.position = new Vector2 (-Mathf.Cos (angle) * radius + startingPosition.x, -Mathf.Sin (angle) * radius + startingPosition.y);
				else
					transform.position = new Vector2 (Mathf.Cos (angle) * radius + startingPosition.x, Mathf.Sin (angle) * radius + startingPosition.y);
			} else {
				transform.localScale = new Vector2 (0.02f * Distance, transform.localScale.y);
				transform.position = (curPosition + startingPosition) / 2;
			}
			transform.rotation = Quaternion.Euler (0, 0, angle * 180 / Mathf.PI);
		}
	}

	void OnMouseUp(){
		if (!ChangeLevel.sceneFading && dragging && Time.timeScale != 0) {
			circleLines = GameObject.FindGameObjectsWithTag("CircleLine");
			circles = GameObject.FindGameObjectsWithTag("Crossroad");
			circleOfThisLine.transform.localScale /= extensionRate;
			for (int i = 0; i < circles.Length; i++) {
				if (((circles[i].transform.position.x == startingPosition.x) != (circles[i].transform.position.y == startingPosition.y)) && Mathf.Abs (circles[i].transform.position.x - (2 * transform.position.x - startingPosition.x)) < 0.7f && Mathf.Abs (circles[i].transform.position.y - (2 * transform.position.y - startingPosition.y)) < 0.7f && Mathf.Abs(10 * circles[i].transform.position.x) % 1 == 0 && Mathf.Abs(10 * circles[i].transform.position.y) % 1 == 0 && (Mathf.Abs(circles[i].transform.position.x - startingPosition.x) > 2.0f || Mathf.Abs(circles[i].transform.position.y - startingPosition.y) > 2.0f)) {
					Instantiate (line, new Vector2 (startingPosition.x + (circles[i].transform.position.x - startingPosition.x) / 3, startingPosition.y + (circles[i].transform.position.y - startingPosition.y) / 3), Quaternion.Euler (0, 0, Mathf.Round (Mathf.Abs (circles[i].transform.position.y - startingPosition.y) * 30)));
					Instantiate (line, new Vector2 (startingPosition.x + (circles[i].transform.position.x - startingPosition.x) / 3 * 2, startingPosition.y + (circles[i].transform.position.y - startingPosition.y) / 3 * 2), Quaternion.Euler (0, 0, Mathf.Round (Mathf.Abs (circles[i].transform.position.y - startingPosition.y) * 30)));
					gameManager.GetComponent<Circuit> ().setRecalculateValue (true);
					circles[i].GetComponent<Movement> ().setConnectedValue (true);
					foreach(GameObject circleLine in circleLines)
						if(circleLine.transform.position == circles[i].transform.position)
							Destroy (circleLine);
					circleOfThisLine.GetComponent<Movement> ().setConnectedValue (true);
					circles[i].GetComponent<SpriteRenderer> ().sprite = usedCircle;
					circleOfThisLine.GetComponent<SpriteRenderer> ().sprite = usedCircle;
					if(SoundControl.sound)
						gameManager.GetComponents<AudioSource>()[1].Play();
					Destroy (this.gameObject);
				} else if (i == circles.Length - 1) {
					transform.position = startingPosition;
					transform.rotation = Quaternion.Euler (0, 0, 0);
					transform.localScale = new Vector2 (0.02f, transform.localScale.y);
				}
			}
			GetComponent<SpriteRenderer> ().sortingOrder = 0;
			dragging = false;
		}
	}

}
