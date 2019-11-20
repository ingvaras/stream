using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	public float speed = 1;
	public GameObject circleLine;
	private Object cl;
	private int rot = 0;
	private int directionToSquare, directionToRailway;
	private GameObject[] squares;
	private GameObject[] railways;
	private Vector2 nextPosition;
	private bool atSpot = true;
	private bool connected = false;

	IEnumerator Start() {
		squares = GameObject.FindGameObjectsWithTag("Square");
		railways = GameObject.FindGameObjectsWithTag("Railway");
		if(!StandingOn(squares, railways))
			Instantiate (circleLine, new Vector2 (transform.position.x, transform.position.y), Quaternion.Euler (0, 0, 0));
		if(CheckForAdjacent (squares) != -1 || CheckForAdjacent (railways) != -1)
			atSpot = StandingOn (squares, railways);
		while (true) {
			if (atSpot) {
				squares = GameObject.FindGameObjectsWithTag("Square");
				railways = GameObject.FindGameObjectsWithTag("Railway");
				if (StandingOn (squares) && nextPosition.x == transform.position.x && nextPosition.y == transform.position.y) {
					cl = Instantiate (circleLine, new Vector2 (transform.position.x, transform.position.y), Quaternion.Euler (0, 0, 0));
					yield return new WaitForSeconds (1.5f);
					if (connected)
						break;
					Destroy (cl);
					this.transform.localScale = new Vector3(0.026f, 0.026f, 1f);
				}
				directionToSquare = CheckForAdjacent (squares);
				directionToRailway = CheckForAdjacent (railways);
				if (directionToSquare != -1 && directionToRailway != -1) {
					if (directionToSquare != (rot + 2) % 4)
						LookForNewPosition (directionToSquare);
					else
						LookForNewPosition (directionToRailway);
				} else if (directionToSquare != -1)
					LookForNewPosition (directionToSquare);
				else if (directionToRailway != -1)
					LookForNewPosition (directionToRailway);
				else
					break;
			} else
				yield return null;
		}
	}

	void Update() {
		if (!atSpot) {
			transform.position = Vector3.MoveTowards (transform.position, nextPosition, speed * Time.deltaTime);
			atSpot = (nextPosition.x == transform.position.x && nextPosition.y == transform.position.y);
		}
	}
		
	int CheckForAdjacent(GameObject[] objs) {
		for (int i = (rot + 3) % 4; i < 4 + (rot + 3) % 4; i++)
			foreach (GameObject obj in objs)
				if (Mathf.Round ((transform.position.x + Mathf.Cos ((i % 4) * Mathf.PI / 2)) * 10) / 10 == obj.transform.position.x && Mathf.Round ((transform.position.y + Mathf.Sin ((i % 4) * Mathf.PI / 2)) * 10) / 10 == obj.transform.position.y)
					return i % 4;
		return -1;
	}

	bool StandingOn(GameObject[] objs) {
		foreach (GameObject obj in objs)
			if (transform.position == obj.transform.position)
				return true;
		return false;
	}

	bool StandingOn(GameObject[] objs1, GameObject[] objs2) {
		foreach (GameObject obj1 in objs1)
			if (transform.position == obj1.transform.position)
				return true;
		foreach (GameObject obj2 in objs2)
			if (transform.position == obj2.transform.position)
				return true;
		return false;
	}
		
	void LookForNewPosition(int rotation) {
		rot = rotation;
		nextPosition = new Vector2 (Mathf.Round ((transform.position.x + Mathf.Cos (rot * Mathf.PI / 2)) * 10) / 10, Mathf.Round ((transform.position.y + Mathf.Sin (rot * Mathf.PI / 2)) * 10) / 10);
		atSpot = false;
	}

	public void setConnectedValue(bool value) {
		connected = value;
	}

}
