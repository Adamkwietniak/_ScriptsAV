using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof(AudioSource))]

public class MovieDisplayScript : MonoBehaviour
{

	public MovieTexture movie;
	private AudioSource audio;
	TutorialScript ts;
	public bool TurnOnMovie;


	void Start ()
	{
		TurnOnMovie = false;
		ts = FindObjectOfType<TutorialScript> ();
		GetComponent<RawImage> ().texture = movie as MovieTexture;
		audio = GetComponent<AudioSource> ();


	}

	void Update ()
	{
		
		if (TurnOnMovie == false) {
			movie.Stop ();
			movie.loop = false;
			audio.Stop ();
			movie.Stop ();
		} else if (TurnOnMovie == true) {
			audio.clip = movie.audioClip;
			audio.Play ();
			movie.Play ();
			movie.loop = true;
		}

	}
}