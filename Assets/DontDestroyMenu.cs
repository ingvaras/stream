using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMenu : MonoBehaviour {

	void Start () {
		DontDestroyOnLoad (this.gameObject);
	}
}
