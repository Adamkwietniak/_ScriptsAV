using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof(AudioSource))]

public class MovieDisplayScript : MonoBehaviour
{

	public MovieTexture movie;
	private AudioSource audio;
	TutorialScript ts;


	void Start ()
	{
		ts = FindObjectOfType<TutorialScript> ();
		GetComponent<RawImage> ().texture = movie as MovieTexture;
		audio = GetComponent<AudioSource> ();


	}

	void Update ()
	{
		
		if (Time.timeScale == 1) {
			movie.Stop ();
			movie.loop = false;
			audio.Stop ();
			movie.Stop ();
		} else if (Time.timeScale == 0) {
			audio.clip = movie.audioClip;
			audio.Play ();
			movie.Play ();
			movie.loop = true;
		}

	}
}