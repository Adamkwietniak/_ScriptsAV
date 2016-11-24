using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour
{

	public GameObject hint;
	MovieDisplayScript mds;
	[HideInInspector]public bool turnedClip;

	void Start ()
	{
		mds = FindObjectOfType<MovieDisplayScript> ();
		turnedClip = false;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player") {
			turnedClip = true;
			hint.SetActive (true);
			Time.timeScale = 0;
		}
			
	}

	void Update ()
	{
		if (Time.timeScale == 0) {
			mds.TurnOnMovie = true;
		} else if (Time.timeScale == 1) {
			mds.TurnOnMovie = false;
		}

		if (turnedClip == true) {
			if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.KeypadEnter)) {
				Time.timeScale = 1;
				turnedClip = false;
				Destroy (hint, 0.1f);
				Destroy (this.gameObject, 1.0f);

			}
		}
	
	}

}
