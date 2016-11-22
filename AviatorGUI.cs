using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AviatorGUI : MonoBehaviour
{
	CameraWithShakeScript cWSS;
	private bool doParachuteUp, doParachuteDown, doParachuteRight, doParachuteLeft, doUpAndright, doUpAndLeft, doDownAndRight, doDownAndLeft;
	[SerializeField]
	private JointsPoseController
		posControlle;
	[SerializeField]
	private AviatorController controller;
	[SerializeField]
	private Texture2D[] suits;
	[SerializeField]
	private Material suitMat;
	[SerializeField]
	private Material helmetMat;
	private int suitNumber;
	private bool inTurn = false;
	private bool inJump = false;
	private LayerMask mask;
	private bool isMobilePlatform;
	[SerializeField]
	private Dropdown poses;
	private List<string> posesName;
	[SerializeField]
	private Gyro gyro;
	[HideInInspector]public bool changeStandardPose;

	void Awake ()
	{
		changeStandardPose = false;
		doParachuteUp = false;
		doParachuteDown = false;
		doParachuteLeft = false;
		doParachuteRight = false;
		doUpAndright = false;
		doUpAndLeft = false;
		doDownAndLeft = false;
		doDownAndRight = false;
		poses.options = new List<Dropdown.OptionData> (0);
		posesName = new List<string> (0);
		foreach (JointPose pose in posControlle.Poses) {
			if (pose.name == "Open parachute" || pose.name == "Left turn" || pose.name == "Right turn" || pose.name == "Squeeze" || pose.name == "Open up" || pose.name == "Up&right" || pose.name == "Up&Left" || pose.name == "Down&right" || pose.name == "Down&left") {
				continue;
			}
			bool incompatible0 = posControlle.NewPoseName == pose.name;
			bool incompatible1 = (posControlle.NewPoseName == "Backfly position 1" || posControlle.NewPoseName == "Backfly position 2" || posControlle.NewPoseName == "Backfly position 3") &&
			                     (pose.name == "Salto" || pose.name == "Rotate left" || pose.name == "Rotate right");
			bool incompatible2 = posControlle.NewPoseName == "Rotate left" || posControlle.NewPoseName == "Rotate right" || posControlle.NewPoseName == "Salto" ||
			                     posControlle.NewPoseName == "From Salto" || posControlle.NewPoseName == "From Rotate left" || posControlle.NewPoseName == "From Rotate right";

			bool compatible = pose.name != "T_Pose" && !incompatible0 && !incompatible1 && !incompatible2;


			if (compatible && pose.name != "From Salto" && pose.name != "From Rotate left" && pose.name != "From Rotate right") {
				Dropdown.OptionData date = new Dropdown.OptionData (pose.name);
				poses.options.Add (date);
				posesName.Add (pose.name);
				poses.onValueChanged.AddListener (OnPoseChanged);
			}
		}
       
		mask = 1 << LayerMask.NameToLayer ("Terrain");
		isMobilePlatform = Application.isMobilePlatform;
		if (isMobilePlatform) {
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			Input.gyro.enabled = true;
		} else {
			Destroy (gyro.gameObject);
			Destroy (GameObject.Find ("ControlText"));
		}
	}

	void Start ()
	{
		poses.value = 0;
		cWSS = FindObjectOfType <CameraWithShakeScript> ();
		//poseButton.onClick.AddListener(SetRandomPose);
		posControlle.OnAnimationComplete += HandleOnAnimationComplete;
	}

	void OnPoseChanged (int pose)
	{
		gyro.isActive = false;
		string poseName = posesName [pose];
		if (poseName == "Salto" || poseName == "Rotate left" || poseName == "Rotate right") {
			posControlle.UpdateSpeed = 1.5f;//3.5
		} else {
			posControlle.UpdateSpeed = 3.0f;//2.0
		}

		posControlle.SetPose (poseName, 2.0f);//1.0
	}

	void HandleOnAnimationComplete (JointsPoseController controller)
	{
		if (posControlle.NewPoseName == "T_Pose") {
			posControlle.SetPose ("Stop n drop", 1.0f);
			posControlle.UpdateSpeed = 3.0f;
			if (changeStandardPose == true) {
				posControlle.SetPose ("Slow n hold", 1.0f);
				posControlle.UpdateSpeed = 3.0f;
			}
		} else if (posControlle.NewPoseName == "Salto") {
			posControlle.SetPose ("From Salto", 1.0f);
			posControlle.UpdateSpeed = 3.2f;
			if (changeStandardPose == true) {
				posControlle.SetPose ("Slow n hold", 1.0f);
				posControlle.UpdateSpeed = 3.0f;
			}
		} else if (posControlle.NewPoseName == "From Salto") {
			posControlle.SetPose ("Stop n drop", 1.0f);//1
			posControlle.UpdateSpeed = 2.9f;//4
			if (changeStandardPose == true) {
				posControlle.SetPose ("Slow n hold", 1.0f);
				posControlle.UpdateSpeed = 3.0f;
			}
		} else if (posControlle.NewPoseName == "Rotate left") {
			posControlle.SetPose ("From Rotate left", 1.0f);
			posControlle.UpdateSpeed = 3.5f;
			if (changeStandardPose == true) {
				posControlle.SetPose ("Slow n hold", 1.0f);
				posControlle.UpdateSpeed = 3.0f;
			}
		} else if (posControlle.NewPoseName == "From Rotate left") {
			posControlle.SetPose ("Stop n drop", 1.0f);
			posControlle.UpdateSpeed = 3.0f;//4
			if (changeStandardPose == true) {
				posControlle.SetPose ("Slow n hold", 1.0f);
				posControlle.UpdateSpeed = 3.0f;
			}
		} else if (posControlle.NewPoseName == "Rotate right") {
			posControlle.SetPose ("From Rotate right", 1.0f);
			posControlle.UpdateSpeed = 2.5f;//3.5
			if (changeStandardPose == true) {
				posControlle.SetPose ("Slow n hold", 1.0f);
				posControlle.UpdateSpeed = 3.0f;
			}
		} else if (posControlle.NewPoseName == "From Rotate right") {
			posControlle.SetPose ("Stop n drop", 1.0f);
			posControlle.UpdateSpeed = 2.0f;//4.0

		}
		/*if (changeStandardPose == true) {
			posControlle.SetPose ("Slow n hold", 1.0f);
			posControlle.UpdateSpeed = 2.0f;
			Debug.Log ("Jestem!");
		}*/

	}


	void OnGUI ()
	{
		//GUILayout.Label ("\n Email: vagho.srapyan@gmail.com ");
		//if (controller) {
		/*if (!isMobilePlatform) {
				GUILayout.Label ("\n  Space Down - stop camera control ,\n  MouseOrbit - run camera control,\n  Mouse ScrollWheel - camera zoom.  ");
			} 
			GUILayout.Label ("     Horizontal velocity m/s: " + Vector3.ProjectOnPlane (controller.velocity, Vector3.up).magnitude.ToString ("0") +
			"\n     Vertical velocity m/s: " + Vector3.Project (controller.velocity, Vector3.up).magnitude.ToString ("0"));

			RaycastHit hit;
			Ray ray = new Ray (controller.transform.position, Vector3.down);
			if (Physics.Raycast (ray, out hit, 100000.0f, mask)) {
				GUILayout.Label ("     Ground height m: " + Vector3.Distance (controller.transform.position, hit.point).ToString ("0") +
				"\n     Height m: " + Mathf.Clamp (controller.transform.position.y, 0.0f, 100000.0f).ToString ("0"));
			}
		}
		//float height = isMobilePlatform ? 50.0f : 20.0f;
		/*if (GUILayout.Button ("Reload", GUILayout.Height (height))) {
			Application.LoadLevel (Application.loadedLevel);
		}*/
		if (!controller) {
			return;
		}
		//GUILayout.Label ("");
       
		//int nextSuitNumber = suitNumber + 1 < suits.Length ? suitNumber + 1 : 0;
		/*if (GUILayout.Button ("Suit: " + suits [nextSuitNumber].name, GUILayout.Height (height))) {
			suitMat.mainTexture = suits [nextSuitNumber];
			helmetMat.mainTexture = suits [nextSuitNumber];
			suitNumber = nextSuitNumber;
		}*/
       
		if (!controller.parachuteIsOpened) {
			
            
			//if (GUILayout.Button ("Open parachute", GUILayout.Height (height))) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				Destroy (poses.gameObject);
				cWSS.shake = 0.01f;
				controller.parachuteIsOpened = true;
				posControlle.SetPose ("Open parachute", 1.0f);
				if (isMobilePlatform) {
					MouseOrbit.FindObjectOfType<MouseOrbit> ().enabled = true;
					MouseOrbit.FindObjectOfType<MouseOrbit> ().distance = 7.5f;
				}
			}
		} else {
			if (posControlle.NewPoseName == "Open parachute" && doParachuteUp == true) {
				posControlle.SetPose ("ParachuteUp", 1.0f);
			} else if (posControlle.NewPoseName == "ParachuteUp" && doParachuteUp == false) {
				posControlle.SetPose ("Open parachute", 1.0f);
			} 
			if (posControlle.NewPoseName == "Open parachute" && doParachuteDown == true) {
				posControlle.SetPose ("ParachuteDown", 1.0f);
			} else if (posControlle.NewPoseName == "ParachuteDown" && doParachuteDown == false) {
				posControlle.SetPose ("Open parachute", 1.0f);
			} 
			if (posControlle.NewPoseName == "Open parachute" && doParachuteLeft == true) {
				posControlle.SetPose ("Parachute left", 1.0f);
			} else if (posControlle.NewPoseName == "Parachute left" && doParachuteLeft == false) {
				posControlle.SetPose ("Open parachute", 1.0f);
			}
			if (posControlle.NewPoseName == "Open parachute" && doParachuteRight == true) {
				posControlle.SetPose ("Parachute right", 1.0f);
			} else if (posControlle.NewPoseName == "Parachute right" && doParachuteRight == false) {
				posControlle.SetPose ("Open parachute", 1.0f);
			}


		}
	}

	void Update ()
	{
		if (controller.parachuteIsOpened) {
			if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
				doParachuteUp = true;
			} else if (Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.UpArrow)) {
				doParachuteUp = false;
			}
			if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
				doParachuteDown = true;
			} else if (Input.GetKeyUp (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
				doParachuteDown = false;
			}

			if (Input.GetKey (KeyCode.LeftShift)) {
				doParachuteLeft = true;
			} else if (Input.GetKeyUp (KeyCode.LeftShift)) {
				doParachuteLeft = false;
			}

			if (Input.GetKey (KeyCode.RightShift)) {
				doParachuteRight = true;
			} else if (Input.GetKeyUp (KeyCode.RightShift)) {
				doParachuteRight = false;
			}
	
			return;
		}

		float vertical = 0.0f;
		float horizontal = 0.0f;

		if (isMobilePlatform) {
			if (gyro.isActive) {
				vertical = -Mathf.Clamp (3.0f * (gyro.controlStartPosition - Input.gyro.gravity.y), -1.0f, 1.0f);
				horizontal = Mathf.Clamp (3.0f * Input.gyro.gravity.x, -1.0f, 1.0f);
				if (Mathf.Abs (vertical) < 0.3f) {
					vertical = 0.0f;
				}
				if (Mathf.Abs (horizontal) < 0.3f) {
					horizontal = 0.0f;
				}
			} else {
				vertical = 0.0f;
				horizontal = 0.0f;
			}
		} else {
			vertical = Input.GetAxis ("Vertical");
			horizontal = Input.GetAxis ("Horizontal");
		}
		if (vertical > 0.0f) {
			inJump = true;
			posControlle.SetPose ("Squeeze", vertical);
		} else if (vertical < 0.0f) {
			inJump = true;
			posControlle.SetPose ("Open up", -vertical);
		} else if (inJump) {
			inJump = false;
			posControlle.SetPose ("Stop n drop", 3.0f);
			if (changeStandardPose == true) {
				posControlle.SetPose ("Slow n hold", 1.0f);
				posControlle.UpdateSpeed = 2.0f;
			}
		}

		if (horizontal > 0.0f) {
			inTurn = true;
			posControlle.SetPose ("Right turn", horizontal);
			posControlle.UpdateSpeed = 4.0f;
		} else if (horizontal < 0.0f) {
			inTurn = true;
			posControlle.SetPose ("Left turn", -horizontal);
			posControlle.UpdateSpeed = 4.0f;
		} else if (inTurn) {
			inTurn = false;
			posControlle.SetPose ("Stop n drop", 1.0f);
			if (changeStandardPose == true) {
				posControlle.SetPose ("Slow n hold", 1.0f);
				posControlle.UpdateSpeed = 2.0f;
			}
		}
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			posControlle.SetPose ("Salto", 1.0f);
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			posControlle.SetPose ("Backfly position 1", 1.0f);
		}

		if (Input.GetKeyDown (KeyCode.Tab)) {
			posControlle.SetPose ("Open up", 1.0f);
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			posControlle.SetPose ("Squeeze", 1.0f);
		}
		if (Input.GetKey (KeyCode.LeftShift)) {
			posControlle.SetPose ("Rotate left", 2.0f);
		}
		if (Input.GetKey (KeyCode.RightShift)) {
			posControlle.SetPose ("Rotate right", 2.0f);
		}

		/*if (Input.GetKeyDown (KeyCode.Z)) {
			posControlle.SetPose ("ParachuteDown", 1.5f);
		} */

		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.LeftArrow)) {
			doDownAndLeft = true;
		} else if (Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.A) || Input.GetKeyUp (KeyCode.UpArrow) || Input.GetKeyUp (KeyCode.LeftArrow)) {
			doDownAndLeft = false;
		}
		if (doDownAndLeft == true) {
			posControlle.SetPose ("Down&left", 2.0f);
		}

		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.RightArrow)) {
			doDownAndRight = true;
		} else if (Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp (KeyCode.UpArrow) || Input.GetKeyUp (KeyCode.RightArrow)) {
			doDownAndRight = false;
		}
		if (doDownAndRight == true) {
			posControlle.SetPose ("Down&right", 2.0f);
		}

		if (Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.DownArrow) && Input.GetKey (KeyCode.RightArrow)) {
			doUpAndright = true;
		} else if (Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp (KeyCode.DownArrow) || Input.GetKeyUp (KeyCode.RightArrow)) {
			doUpAndright = false;
		}

		if (doUpAndright == true) {
			posControlle.SetPose ("Up&right", 2.0f);
		}

		if (Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.DownArrow) && Input.GetKey (KeyCode.LeftArrow)) {
			doUpAndLeft = true;
		} else if (Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp (KeyCode.A) || Input.GetKeyUp (KeyCode.DownArrow) || Input.GetKeyUp (KeyCode.LeftArrow)) {
			doUpAndLeft = false;
		}
		if (doUpAndLeft == true) {
			posControlle.SetPose ("Up&left", 2.0f);
		}

	}

	/*void SetRandomPose ()
	{
		JointPose pose = posControlle.Poses [Random.Range (0, posControlle.Poses.Count)];
		int i = 0;
		while (i < 100 && (pose == posControlle.newPose || pose.name == "From Salto" || pose.name == "From Rotate left" || pose.name == "Rotate right" || pose.name == "T_Pose")) {
			Debug.Log (posControlle.newPose.name + "    " + pose.name);
			Debug.Log ((pose == posControlle.newPose).ToString ());
			pose = posControlle.Poses [Random.Range (0, posControlle.Poses.Count)];

			i++;
		}
		if (pose.name == "Salto" || pose.name == "Rotate left" || pose.name == "Rotate right") {
			posControlle.UpdateSpeed = 3.5f;
		} else {
			posControlle.UpdateSpeed = 2.0f;
		}

		posControlle.SetPose (pose.name, 1.0f);
	}*/
}
