using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using admob;

public class AdManager : MonoBehaviour {

	public static AdManager Instance{set; get;}

	private void Start () {
		Instance = this;
		DontDestroyOnLoad (gameObject);

		Admob.Instance ().initAdmob ("", "ca-app-pub-6403892120774730/3203833404");
		//Admob.Instance ().setTesting (true);
		Admob.Instance ().loadInterstitial ();
	}

	public void ShowInterstitial() {
		if (Admob.Instance ().isInterstitialReady ())
			Admob.Instance ().showInterstitial ();
	}

	public void StartLoadingInterstitial () {
		Admob.Instance ().loadInterstitial ();
	}

}
