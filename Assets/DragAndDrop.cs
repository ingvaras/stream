using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]

public class DragAndDrop : MonoBehaviour {

	private Dictionary<string, GameObject> objectList;
	private Vector3 screenPoint;
	private Vector3 offset;
	private bool onDrag = false;
	private float min;
	private int j;
	private Vector3 curPosition;
	private bool firstTime;
	public bool outdated = false;

	private float[] xList = new float[112] {-6.5f, -5.5f, -4.5f, -3.5f, -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f, 3.5f, 4.5f, 5.5f, 6.5f,
		-6.5f, -5.5f, -4.5f, -3.5f, -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f, 3.5f, 4.5f, 5.5f, 6.5f, -6.5f, -5.5f, -4.5f, -3.5f, -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f, 3.5f, 4.5f, 5.5f, 6.5f,
		-6.5f, -5.5f, -4.5f, -3.5f, -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f, 3.5f, 4.5f, 5.5f, 6.5f, -6.5f, -5.5f, -4.5f, -3.5f, -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f, 3.5f, 4.5f, 5.5f, 6.5f,
		-6.5f, -5.5f, -4.5f, -3.5f, -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f, 3.5f, 4.5f, 5.5f, 6.5f, -6.5f, -5.5f, -4.5f, -3.5f, -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f, 3.5f, 4.5f, 5.5f, 6.5f,
		-6.5f, -5.5f, -4.5f, -3.5f, -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f, 3.5f, 4.5f, 5.5f, 6.5f};
	private float[] yList = new float[112] {3.0f, 3.0f, 3.0f, 3.0f, 3.0f, 3.0f, 3.0f, 3.0f, 3.0f, 3.0f, 3.0f, 3.0f, 3.0f, 3.0f, 
		2.0f, 2.0f, 2.0f, 2.0f, 2.0f, 2.0f, 2.0f, 2.0f, 2.0f, 2.0f, 2.0f, 2.0f, 2.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 
		0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, 
		-2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -3.0f, -3.0f, -3.0f, -3.0f, -3.0f, -3.0f, -3.0f, -3.0f, -3.0f, -3.0f, -3.0f, -3.0f, -3.0f, -3.0f, 
		-4.0f, -4.0f, -4.0f, -4.0f, -4.0f, -4.0f, -4.0f, -4.0f, -4.0f, -4.0f, -4.0f, -4.0f, -4.0f, -4.0f};


	void Start() {
		offset = transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
		if(!outdated)
			firstTime = true;
	}

	void Update() {
		if (firstTime) {
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
			curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
			transform.position = curPosition;
			onDrag = true;
		}
	}


	void OnMouseDown() {
		if (!Instance.submitWindowActive) {
			if (Instance.selected != null && Instance.selected.GetComponent<Blinking> ().GetBlinking ())
				Instance.selected.GetComponent<Blinking> ().ChangeBlinking ();
			Instance.selected = gameObject;
			offset = transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
		}
	}


	void OnMouseDrag() {
		if (!Instance.submitWindowActive) {
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
			curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
			transform.position = curPosition;
			onDrag = true;
		}
	}


	public void OnMouseUp() {
		if (!Instance.submitWindowActive) {
			if (Instance.selected != null && !Instance.selected.GetComponent<Blinking> ().GetBlinking ())
				Instance.selected.GetComponent<Blinking> ().ChangeBlinking ();
			if (firstTime)
				firstTime = false;
			if (onDrag == true) {
				j = 0;
				min = Mathf.Infinity;
				for (int i = 0; i < 112; i++) {
					if (Mathf.Abs (xList [i] - curPosition.x) + Mathf.Abs (yList [i] - curPosition.y) < min) {
						min = Mathf.Abs (xList [i] - curPosition.x) + Mathf.Abs (yList [i] - curPosition.y);
						j = i;
					}
				}
				curPosition.x = xList [j];
				curPosition.y = yList [j];
				transform.position = curPosition;
				onDrag = false;
			}
		}
	}
}
