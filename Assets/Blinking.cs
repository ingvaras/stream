using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinking : MonoBehaviour {

	private bool blink = false;
	private bool fade = true;


	void Update () {
		if (blink) {
			Color tmp = gameObject.GetComponent<SpriteRenderer> ().color;
			if(fade) {
				tmp.a *= 0.97f;
				if (tmp.a <= 0.4f)
					fade = false;
			} else {
				tmp.a /= 0.97f;
				if (tmp.a >= 1.0f)
					fade = true;
			}
			gameObject.GetComponent<SpriteRenderer> ().color = tmp;
		}
	}

	public void ChangeBlinking() {
		blink = !blink;
		fade = true;
		Color tmp = gameObject.GetComponent<SpriteRenderer> ().color;
		tmp.a = 1.0f;
		gameObject.GetComponent<SpriteRenderer> ().color = tmp;
	}

	public bool GetBlinking() {
		return blink;
	}
}
