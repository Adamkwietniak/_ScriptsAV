using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeCameraScript : MonoBehaviour
{

	public Camera[] cameras;
	public GameObject[] dustParticle;
	[HideInInspector] public int dustParticleIndex;
	[HideInInspector]public int cameraIndex;
	public Camera photoCameraView;
	public GameObject backButton;
	public Canvas brokenGlass;
	public Canvas bloodOnScreen;
	[HideInInspector]public bool changeCameraPossible;
	// bool umożliwiający lawirowanie między kamerami, będziemy go w pewnych momentach blokować.
	private bool onAndOffScreenshotCamera = false;
	// bool ułatwiające zmiane między trybem kamery screenshot, a powrotem do gry pod klawiszem "P".
	//CameraShake cmShake;
	public float defaultTimeScale;
	private bool NormalCameraBool = true;




	// Use this for initialization
	void Start ()
	{
		
		Time.timeScale = defaultTimeScale;
		cameraIndex = 0;
		dustParticleIndex = 0;
		photoCameraView.enabled = false;
		backButton.SetActive (false);
		changeCameraPossible = true;



		for (int o = 1; o < dustParticle.Length; o++) {
			dustParticle [o].SetActive (false);
		}
		if (dustParticle.Length > 0) {
			dustParticle [0].SetActive (true);
		}
		for (int i = 1; i < cameras.Length; i++) {
			cameras [i].enabled = false; // tablica kamer. Po indexie wybiera, która z nich ma być aktywna.
		}

		if (cameras.Length > 0) {
			cameras [0].enabled = true; // ustawia pierwszą kamerę jako domyślną.
		}


	}
	
	// Update is called once per frame
	void Update ()
	{


	
		if (Input.GetKeyDown (KeyCode.C) && changeCameraPossible == true) { // po wciśnięciu "C", lawirujemy między kamerami.
			cameraIndex++; // indeksy służą do przypisania kamer.
			dustParticleIndex++;
			if (cameraIndex < cameras.Length) {
				cameras [cameraIndex - 1].enabled = false;
				dustParticle [dustParticleIndex - 1].SetActive (false);
				cameras [cameraIndex].enabled = true;
				dustParticle [dustParticleIndex].SetActive (true);
			} else {
			
				cameras [cameraIndex - 1].enabled = false;
				dustParticle [dustParticleIndex - 1].SetActive (false);
				cameraIndex = 0;
				dustParticleIndex = 0;
				cameras [cameraIndex].enabled = true;
				dustParticle [dustParticleIndex].SetActive (true);
			}		
		}
		if (Input.GetKeyDown (KeyCode.P)) {
			onAndOffScreenshotCamera = !onAndOffScreenshotCamera;
		}

		if (onAndOffScreenshotCamera == true) {
			if (NormalCameraBool == true) {
				if (changeCameraPossible == true) {
					PhotoCameraButton ();
					NormalCameraBool = false;
				}
			}
		} else {
			if (NormalCameraBool == false) {
				BackFromPhotoView ();
				NormalCameraBool = true;
			}
		}

	}


	public void PhotoCameraButton ()
	{
		
		onAndOffScreenshotCamera = true;
		Time.timeScale = 0;
		photoCameraView.enabled = true;
		changeCameraPossible = false;
		for (int i = 1; i < cameras.Length; i++) {
			cameras [i].enabled = false; // tablica kamer. Po indexie wybiera, która z nich ma być aktywna.
		}
		backButton.SetActive (true);


	}

	public void BackFromPhotoView ()
	{
		
		onAndOffScreenshotCamera = false;
		Time.timeScale = defaultTimeScale;
		photoCameraView.enabled = false;
		changeCameraPossible = true;
		if (cameras.Length > 0) {
			cameras [0].enabled = true; // ustawia pierwszą kamerę jako domyślną.
		}
		backButton.SetActive (false);

	}


		


}
