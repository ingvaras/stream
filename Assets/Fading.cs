using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour {

	public Texture2D fadeInTexture;
	public Texture2D fadeOutTexture;
	public float fadeInSpeed = 1.0f;
	public float fadeOutSpeed = 1.0f;

	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;			//FadeIn is -1, FadeOut is 1

	void OnGUI()
	{
		if(fadeDir == -1)
			alpha += fadeDir * fadeInSpeed * Time.deltaTime;
		else
			alpha += fadeDir * fadeOutSpeed * Time.deltaTime;
		
		alpha = Mathf.Clamp01 (alpha);
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;

		if(fadeDir == -1)
			GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), fadeInTexture);
		else
			GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
	}

	public float BeginFade(int direction)
	{
		fadeDir = direction;
		if(fadeDir == -1)
			return(fadeInSpeed);
		else
			return(fadeOutSpeed);
	}
		
}
