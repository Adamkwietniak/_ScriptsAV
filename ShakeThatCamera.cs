using UnityEngine;
using System.Collections;

public class ShakeThatCamera : MonoBehaviour
{

	[Range (0, 3)]
	public float shake;
	public GameObject camShakeOne, camShakeTwo;
	[HideInInspector]public float px = 0f, py = 0f, pz = 0f;
	ChangeCameraScript cCamS;

	void Start ()
	{
		cCamS = FindObjectOfType <ChangeCameraScript> ();
	}

	void LateUpdate ()
	{
		camShakeOne.transform.localPosition = new Vector3 (px, py, pz);
		camShakeTwo.transform.localPosition = new Vector3 (px, py, pz);
		if (cCamS.cameraIndex == 1) {
			px = Random.Range (-shake, shake);
			pz = Random.Range (-shake, shake);
		} else if (cCamS.cameraIndex == 2) {
			py = Random.Range (-shake, shake);
			pz = Random.Range (-shake, shake);

		}
	}



}