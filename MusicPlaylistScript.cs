using UnityEngine;
using System.Collections;

public class MusicPlaylistScript : MonoBehaviour
{

	private AudioSource audSrc;
	public AudioClip[] songs;

	// Use this for initialization
	void Start ()
	{
		audSrc = GetComponent<AudioSource> ();
		PlaySong ();

	}

	void PlaySong ()
	{
		AudioClip clip = songs [Random.Range (0, songs.Length)];
		audSrc.clip = clip;
		audSrc.Play ();
		Invoke ("PlaySong", audSrc.clip.length);
	}


	

}
