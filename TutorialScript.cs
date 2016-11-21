using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour
{

	public GameObject hint;
	[HideInInspector]public bool SaveFPSBool;
	MovieDisplayScript mds;
	[HideInInspector]public bool TurnOnMovie;

	void Start ()
	{
		mds = FindObjectOfType<MovieDisplayScript> ();
		SaveFPSBool = false;
		TurnOnMovie = false;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player") {
			TurnOnMovie = true;
			hint.SetActive (true);
			Time.timeScale = 0;
		}
			
	}

	void Update ()
	{

		if (TurnOnMovie == true) {
			if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.KeypadEnter)) {
				Time.timeScale = 1;
				TurnOnMovie = false;
				SaveFPSBool = true;
				Destroy (hint, 0.1f);
				Destroy (this.gameObject, 1.0f);

			}
		}
	
	}

}
