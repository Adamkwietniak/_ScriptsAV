using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TutorialHintScript : MonoBehaviour
{

	[HideInInspector]public int i = 0;
	public int SetAmountOfTriggers = 2;
	// tablica colliderów
	public GameObject[] trigger = new GameObject[1];
	// tablica obiektów
	public GameObject[] hints = new GameObject[1];
	[HideInInspector]public int y = 0;
	//public GameObject[] movies;
	MovieDisplayScript mds;
	List<GameObject> hintsList = new List<GameObject> ();
	private int IndexHint = 0;


	void Start ()
	{
		foreach (GameObject hint in hints) {
			hintsList.Add (hint);
		}
		mds = FindObjectOfType<MovieDisplayScript> ();
		//for (int z = 0; z == SetAmountOfTriggers; z++) {
		trigger [i] = GameObject.FindGameObjectWithTag ("Trigger");
		//}
		Hints (y);
		ChangeTrigger (i);
	}

	void Update ()
	{

		if (Time.timeScale == 0) { 

			if (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Escape)) { //pozbywamy się hintów
	
				DisableHint ();


			}

		}

	}


	void Hints (int y)
	{
		//for (int z = 0; z < hintsList.Count; z++) {
		//if (z == y) {

		hintsList [i].SetActive (true);	
		Time.timeScale = 0;
		//} 
		//}
	}

	public void DisableHint ()
	{

		Time.timeScale = 1;
		Destroy (hints [IndexHint], 0.1f);

	}

	void OnTriggerEnter (Collider other) // gracz wbija w trigger
	{

		if (other.tag == "Trigger") {
			i++;
			if (Missions (i) == true) {
				
				ChangeTrigger (i);

			}
		}

	}

	void ChangeTrigger (int i) // Gdy wejdziemy w jeden trigger, ten znika i uruchamia się drugi.
	{
		for (int z = 0; z < SetAmountOfTriggers; z++) {
			if (i == z)
				trigger [z].SetActive (true);
			else
				trigger [z].SetActive (false);

		}
	}

	bool Missions (int i) // tutaj wybieramy przy którym szanownym triggerze ma nam opokazać hinta.
	{
		switch (i) {
		case 0:
			Hints (y);
			return true;
			break;
		case 1:
			Hints (y);
			return true;
			break;
		case 2:
			Hints (y);
			return true;
			break;

		default:
			return true;
			break;

		}
		return false;
	}
}