using UnityEngine;
using System.Collections;


public class CameraWithShakeScript : MonoBehaviour
{
	public GameObject objectToFollow, cameraParent, cameraGO;

	[Range (0, 25)]
	public float minDistance, maxDistance;
	public float scrollSpeed, rayBackToPositionSpeed;

	public bool shakeBool = false;
	[Range (0, 5)]
	public float shake;
	public float horVerSpeed, maxVerticalAngle;
	public float px = 0f, py = 0f, pz = 0f, pzRay, rx = 0f, ry = 125f;

	private int layerMaskPlayer = ~(1 << 9);
	public GameObject camTwo;
	AviatorController controller;


	void Start ()
	{
		controller = FindObjectOfType<AviatorController> ();
		ry = objectToFollow.transform.rotation.eulerAngles.y;
		cameraParent.transform.position = objectToFollow.transform.position;

	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		//CameraControlXYAxis ();
		CameraControlZAxis ();
		CameraRotation ();

		if (controller.parachuteIsOpened == true) {
			maxDistance = 15.0f;
		}
	}

	/*void CameraControlXYAxis ()
	{
		if (Input.GetAxis ("Mouse Y") > 0 && rx > -maxVerticalAngle) {
			rx -= Time.deltaTime * (horVerSpeed / 2);
		} else if (Input.GetAxis ("Mouse Y") < 0 && rx < maxVerticalAngle) {				//kontrola myszką rotacji kamery.
			rx += Time.deltaTime * (horVerSpeed / 2);
		}
		if (Input.GetAxis ("Mouse X") > 0) {
			ry += Time.deltaTime * horVerSpeed;
		} else if (Input.GetAxis ("Mouse X") < 0) {
			ry -= Time.deltaTime * horVerSpeed;
		}
		cameraParent.transform.rotation = Quaternion.Euler (rx, ry, 0f);
	}*/

	void CameraRotation ()
	{
		cameraParent.transform.rotation = objectToFollow.transform.rotation; // kamera podąża za 
	}

	void CameraControlZAxis ()
	{
		cameraParent.transform.position = objectToFollow.transform.position;
		if (shakeBool == true) {
			px = Random.Range (-shake, shake);
			py = Random.Range (-shake, shake);
		} else {
			px = 0f;
			py = 0f;
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			ry = objectToFollow.transform.rotation.eulerAngles.y; // reset kamery do startu
		}

		Ray distanceRay = new Ray (cameraParent.transform.position, -cameraParent.transform.forward);
		RaycastHit hitInfo;
		if (Physics.Raycast (distanceRay, out hitInfo, -pz, layerMaskPlayer)) {
			float dist = Vector3.Distance (cameraParent.transform.position, hitInfo.point);
			if (pz <= -dist) {
				pzRay = -dist;
			} else if (pz > -dist) {
				pzRay -= Time.deltaTime * rayBackToPositionSpeed;
			}
			cameraGO.transform.localPosition = new Vector3 (px, py, pzRay);
			camTwo.transform.localPosition = new Vector3 (0f, 0f, pzRay);
		} else if (pz < pzRay) {
			pzRay -= Time.deltaTime * rayBackToPositionSpeed;
			cameraGO.transform.localPosition = new Vector3 (px, py, pzRay);
			camTwo.transform.localPosition = new Vector3 (0f, 0f, pzRay);
		} else {
			cameraGO.transform.localPosition = new Vector3 (px, py, pz);
			camTwo.transform.localPosition = new Vector3 (0f, 0f, pz);
		}

		if (pz <= -minDistance && pz >= -maxDistance) {
			if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
				pz += 0.1f * scrollSpeed;
			} else if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
				pz -= 0.1f * scrollSpeed;
			}
		}
		if (pz > -minDistance) {
			pz = -minDistance;
		} else if (pz < -maxDistance) {
			pz = -maxDistance;
		}
	}
}
