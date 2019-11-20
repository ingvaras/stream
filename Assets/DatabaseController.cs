using UnityEngine;
using System.Collections;

public class DatabaseController : MonoBehaviour {

	private string temp;
	private string save_dataURL = "http://ingvarasgalinskas.000webhostapp.com/save_data.php";
	private string load_dataURL = "http://ingvarasgalinskas.000webhostapp.com/load_data.php";


	void Start(){
		StartCoroutine(LoadData());
	}


	IEnumerator SaveData(string name)
	{

		WWWForm form = new WWWForm();

		form.AddField("name", name);
		form.AddField ("structure", GameObject.FindGameObjectWithTag("DesignBattery").GetComponent<Instance>().GenerateString());

		WWW webRequest = new WWW(save_dataURL, form);

		yield return webRequest;
	}


	IEnumerator LoadData()
	{
		WWWForm form = new WWWForm ();
		WWW webRequest;
		
		while (true){
			form.AddField ("id", 12);
			webRequest = new WWW (load_dataURL, form);
			yield return webRequest;
			if(webRequest.text != "nothing found"){
				break;
			}
		}

		Debug.Log (webRequest.text);
	}


	public void Load(){
		StartCoroutine (LoadData ());
	}


	public void Save(string name){
		StartCoroutine (SaveData (name));
	}
}


