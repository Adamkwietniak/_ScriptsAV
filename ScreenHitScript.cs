using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class ScreenHitScript : MonoBehaviour
{
	
	public GameObject[] buttonsToHide;
	private int screenshotCount = 0;
	// liczba, która pojawia się po nazwie screenshot

	// Update is called once per frame
	void Update ()
	{


		if (Input.GetKeyDown (KeyCode.F9)) { // jak przyciskamy F9 to robi screena
			
			for (int i = 1; i < buttonsToHide.Length; i++) {
				buttonsToHide [i].gameObject.SetActive (false);
			}

			string screenshotFilename;
			do {
				screenshotCount++;

				//string path = /*Environment.GetFolderPath (Environment.SpecialFolder.MyPictures) + "/Dev4Play/Base Jump/";*/ System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyPictures);
				//string folder = AssetDatabase.CreateFolder ("Dev4play", "Wingsuit Aviator VR");
				screenshotFilename = "screenshot" + screenshotCount + ".png";

			} while (System.IO.File.Exists (screenshotFilename));
			Application.CaptureScreenshot (screenshotFilename);
			for (int i = 1; i < buttonsToHide.Length; i++) {

				buttonsToHide [i].gameObject.SetActive (true);

			}
		}

	}
}


