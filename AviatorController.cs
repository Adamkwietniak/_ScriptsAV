using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[System.Serializable]
public class AviatorController : MonoBehaviour
{
	
	private int i;
	public float timerToOpenUp = 8.0f;
	WindScript ws;
	MotionBlur motionBlur;
	[SerializeField]
	private JointsPoseController posController;
	[SerializeField]
	private Transform root;

	[SerializeField]
	private JointsRandomAnimations[] joints;
	[SerializeField]
	private Transform
		hipSuit;
	private Transform[] hipSuitPoints;
	[SerializeField]
	private Transform[] armSuits;
	private Vector3[] hipSuitRotations;
	private Vector3[] armSuitRotations;
	[SerializeField]
	private Transform
		aviatorRoot;

	[HideInInspector]public float rotationY;
	[HideInInspector]public float velocityY;
	[HideInInspector]public float velocityZ;
	private float angle = 45f;

	private RigidbodyConstraints contrains;
	private Quaternion quaterion;

	public Vector3 velocity {
		get;
		private set;
	}

	[SerializeField]
	[HideInInspector]public float suitFrequency;
	[SerializeField]
	[HideInInspector]public float suitMagnitude;
	private float time;
	[System.NonSerialized]
	public bool parachuteIsOpened = false;

	[SerializeField]
	public Transform parachute;
	private Vector3 parachuteStrSqale;
	private bool isMobilePlatform;

	AviatorGUI avGUI;


	public void OnAwake ()
	{
		avGUI = FindObjectOfType<AviatorGUI> ();
		ws = FindObjectOfType <WindScript> ();
		motionBlur = FindObjectOfType<MotionBlur> ();
		parachuteStrSqale = parachute.localScale;
		parachute.localScale = 0.01f * Vector3.one;
		parachute.GetChild (0).GetComponent<SkinnedMeshRenderer> ().enabled = false;
		hipSuitPoints = new Transform[hipSuit.childCount];
		hipSuitRotations = new Vector3[hipSuit.childCount];
		isMobilePlatform = Application.isMobilePlatform;
		for (int i = 0; i < hipSuitRotations.Length; i++) {
			hipSuitPoints [i] = hipSuit.GetChild (i);
			hipSuitRotations [i] = hipSuitPoints [i].localRotation.eulerAngles;
		}
		armSuitRotations = new Vector3[armSuits.Length];
		for (int i = 0; i < armSuitRotations.Length; i++) {
			armSuitRotations [i] = armSuits [i].localRotation.eulerAngles;
		}
		foreach (var item in joints) {
			item.OnStart ();
		}
		posController.OnStartAnimation += (JointsPoseController controller) => {
			if (controller.NewPoseName == "Open parachute") {
				parachute.GetChild (0).GetComponent<SkinnedMeshRenderer> ().enabled = true;
			}
		};
		posController.OnAnimationComplete += (JointsPoseController controller) => {
			if (controller.NewPoseName == "Open parachute") {
				parachute.GetChild (0).GetComponent<SkinnedMeshRenderer> ().enabled = true;
			}
			SetCurrentState ();
		};
	}

	void SetCurrentState ()
	{
		foreach (var item in joints) {
			item.SetCurrentValue ();
		}
	}

	void Update ()
	{
		if (parachuteIsOpened) {
			if (parachute.localScale.magnitude < parachuteStrSqale.magnitude) {
				parachute.localScale *= 1.0f + 5.0f * Time.deltaTime;
				for (i = 5; i < 11; i++) {
					joints [i].frequency = 0f;
				}
			} else {
				parachute.localScale = parachuteStrSqale;
			}
		}
		time += Time.deltaTime;
		if (time > 100000.0f) {
			time = 0.0f;
		}
		for (int i = 0; i < hipSuitPoints.Length; i++) {
			hipSuitPoints [i].localRotation = Quaternion.Euler (hipSuitRotations [i] + suitMagnitude * Mathf.Sin (suitFrequency * time + ((float)i)) * (Mathf.PI / 2.0f) * Vector3.right);
		}
		for (int i = 0; i < armSuits.Length; i++) {
			armSuits [i].localRotation = Quaternion.Euler (armSuitRotations [i] + 0.75f * suitMagnitude * Mathf.Sin (suitFrequency * time + ((float)i)) * (Mathf.PI / 2.0f) * Vector3.right);
		}
		if (!posController.inAnimate) {
			foreach (var item in joints) {
				item.AnimateJoint ();
			}
		} 
		VelocityControl ();
		if (posController.NewPoseName == "Salto" || posController.NewPoseName == "From Salto") {
			velocity = velocityY * Vector3.up + velocityZ * VectorOperator.getProjectXZ (velocity.normalized, true);
		} else if (posController.NewPoseName == "Open parachute" || (parachuteIsOpened && posController.NewPoseName == "ParachuteDown") || (parachuteIsOpened && posController.NewPoseName == "ParachuteUp") || (parachuteIsOpened && posController.NewPoseName == "Parachute right") || (parachuteIsOpened && posController.NewPoseName == "Parachute left")) {
			velocity = velocityY * Vector3.up - velocityZ * root.up;
			if (posController.NewPoseName == "ParachuteDown") {
				velocity = velocityY * Vector3.up - velocityZ * root.up * 2f;
			}
		} else {
			velocity = velocityY * Vector3.up + velocityZ * root.forward;
		}
		velocity *= 9.0f;
		transform.position += velocity * Time.deltaTime;
		transform.Rotate (rotationY * Time.deltaTime * Vector3.up);
	}

	public void TimerRecovery ()
	{
		if (timerToOpenUp < 8) {
			timerToOpenUp += Time.deltaTime;
		} 
	}

	void VelocityControl ()
	{
		if (Input.GetKey (KeyCode.E) && parachuteIsOpened == false) {
			posController.SetPose ("RightDramatic", 1.0f);
			posController.UpdateSpeed = 2.0f;
		} else if (Input.GetKeyUp (KeyCode.E) && parachuteIsOpened == false) {
			posController.SetPose ("Stop n drop", 1.0f);
			posController.UpdateSpeed = 2.0f;
		}

		if (Input.GetKey (KeyCode.Q) && parachuteIsOpened == false) {
			posController.SetPose ("LeftDramatic", 1.0f);
			posController.UpdateSpeed = 2.0f;


		} else if (Input.GetKeyUp (KeyCode.Q) && parachuteIsOpened == false) {
			posController.SetPose ("Stop n drop", 1.0f);
			posController.UpdateSpeed = 2.0f;

		}
		if (posController.NewPoseName == "Squeeze") {
			suitFrequency = 220;
			suitMagnitude = 100;
			motionBlur.blurAmount = 0.83f;

		} else {
			suitFrequency = 50f;
			suitMagnitude = 30f;
			motionBlur.blurAmount = 0.65f;
		}
		if (posController.NewPoseName == "Stop n drop") {
			rotationY = 0.0f;
			velocityY = -3.4f;
			velocityZ = 5.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "RightDramatic") {
			rotationY = 16.0f;
			velocityY = -4.4f;
			velocityZ = 8.0f;
			suitFrequency = 150;
			TimerRecovery ();
		} else if (posController.NewPoseName == "LeftDramatic") {
			rotationY = -16.0f;
			velocityY = -4.4f;
			velocityZ = 8.0f;
			suitFrequency = 150;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Slow n hold") {
			rotationY = 0.0f;
			velocityY = -3.0f;
			velocityZ = 12.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Energency stop") {
			rotationY = 0.0f;
			velocityY = -10.0f;
			velocityZ = 2.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Open up") {
			rotationY = 0.0f;
			velocityY = -4.0f;
			velocityZ = 9.0f;
			timerToOpenUp -= Time.deltaTime;
			while (true) {
				if (timerToOpenUp < 0) {
					timerToOpenUp = 8;
				}
				if (timerToOpenUp < 7.9 && timerToOpenUp > 6.5) {
					velocityY = 3f;
				} else {
					velocityY = -3f;
				}

				break;
			}
			if (avGUI.changeStandardPose == true) {
				velocityZ = 11.0f;
			}

		} else if (posController.NewPoseName == "Down&right") {
			rotationY = 0.0f;
			velocityY = -8.0f;
			velocityZ = 10.0f;
			TimerRecovery ();
			transform.Translate (Vector3.right * 38.0f * Time.deltaTime);
		} else if (posController.NewPoseName == "Down&left") {
			rotationY = 0.0f;
			velocityY = -8.0f;
			velocityZ = 10.0f;
			TimerRecovery ();
			transform.Translate (Vector3.right * -38.0f * Time.deltaTime);
		} else if (posController.NewPoseName == "Up&right") {
			rotationY = 0.0f /** posController.LerpTime*/;
			velocityY = -2.4f;
			velocityZ = 10.0f;
			timerToOpenUp -= Time.deltaTime;
			transform.Translate (Vector3.right * 38.0f * Time.deltaTime);
			while (true) {
				if (timerToOpenUp < 0) {
					timerToOpenUp = 8;
				}
				if (timerToOpenUp < 7.9 && timerToOpenUp > 6.5) {
					velocityY = 2.4f;
				} else {
					velocityY = -3f;
				}

				break;
			}
	
		} else if (posController.NewPoseName == "Up&left") {
			rotationY = 0.0f /** posController.LerpTime*/;
			velocityY = -2.4f;
			velocityZ = 10.0f;
			timerToOpenUp -= Time.deltaTime;
			transform.Translate (Vector3.right * -38.0f * Time.deltaTime);
			while (true) {
				if (timerToOpenUp < 0) {
					timerToOpenUp = 8;
				}
				if (timerToOpenUp < 7.9 && timerToOpenUp > 6.5) {
					velocityY = 2.4f;
				} else {
					velocityY = -3f;
				}

				break;
			}


		} else if (posController.NewPoseName == "Squeeze") {
			rotationY = 0.0f;
			velocityY = -8.0f;
			velocityZ = 10.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Proper kinesthetic") {
			rotationY = 0.0f;
			velocityY = -4.0f;
			velocityZ = 7.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Backfly position 1") {
			rotationY = 0.0f;
			velocityY = -15.0f;
			velocityZ = 2.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Backfly position 2") {
			rotationY = 0.0f;
			velocityY = -8.0f;
			velocityZ = 2.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Backfly position 3") {
			rotationY = 0.0f;
			velocityY = -7.0f;
			velocityZ = 2.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Right turn") {
			rotationY = 1.0f /** posController.LerpTime*/;
			velocityY = -2.4f;
			velocityZ = 12.0f;
			TimerRecovery ();
			transform.Translate (Vector3.right * 45.0f * Time.deltaTime);
			if (avGUI.changeStandardPose == false) {
				velocityZ = 4.0f;
			}
		} else if (posController.NewPoseName == "Left turn") {
			rotationY = -1.0f /** posController.LerpTime*/;
			velocityY = -2.5f;
			velocityZ = 7.5f;
			TimerRecovery ();
			transform.Translate (Vector3.right * -45.0f * Time.deltaTime);
			if (avGUI.changeStandardPose == false) {
				velocityZ = 4.0f;
			}
		} else if (posController.NewPoseName == "From Salto") {
			velocityY = -12.6f;
		} else if (posController.NewPoseName == "Salto") {
			rotationY = 0.0f;
			velocityY = -8.0f;
			velocityZ = 10.0f;
			TimerRecovery ();
		} else if (posController.NewPoseName == "Rotate left") {
			rotationY = -1.5f;
			velocityY = -4.0f;
			velocityZ = 10.0f;
			transform.Translate (Vector3.right * -25.0f * Time.deltaTime);
			TimerRecovery ();
		} else if (posController.NewPoseName == "Rotate right") {
			rotationY = 1.5f;
			velocityY = -4.0f;
			velocityZ = 10.0f;
			transform.Translate (Vector3.right * 25.0f * Time.deltaTime);
			TimerRecovery ();
		} else if (posController.NewPoseName == "Parachute right") {
			transform.Translate (Vector3.right * 18.0f * Time.deltaTime);
		} else if (posController.NewPoseName == "Parachute left") {
			transform.Translate (Vector3.right * -18.0f * Time.deltaTime);
		} else if (posController.NewPoseName == "Open parachute" || (parachuteIsOpened && posController.NewPoseName == "ParachuteDown") || (parachuteIsOpened && posController.NewPoseName == "ParachuteUp") || (parachuteIsOpened && posController.NewPoseName == "Parachute right") || (parachuteIsOpened && posController.NewPoseName == "Parachute left")) {
			float horizontal = 0.0f;
			if (isMobilePlatform) {
				horizontal = Mathf.Clamp (3.0f * Input.gyro.gravity.x, -1.0f, 1.0f);

				if (Mathf.Abs (horizontal) < 0.3f) {
					horizontal = 0.0f;
				}
			} else {
				horizontal = Input.GetAxis ("Horizontal");
			}
			rotationY = 10.0f * horizontal;
			velocityY = -1.5f;
			velocityZ = 2.0f;
		}
		if (posController.NewPoseName == "ParachuteUp") { // rotacja spadochronu
			angle += Input.GetAxis ("Vertical") * 32 * Time.deltaTime;
			angle = Mathf.Clamp (angle, 0f, 15f);
			parachute.transform.localRotation = Quaternion.AngleAxis (angle + Time.deltaTime, Vector3.right);


		} else if (posController.NewPoseName == "ParachuteDown" || posController.NewPoseName == "Open parachute") {
			angle -= 22 * Time.deltaTime;
			angle = Mathf.Clamp (angle, 0f, 15f);
			parachute.transform.localRotation = Quaternion.AngleAxis (-angle + Time.deltaTime, Vector3.left);
		}
	}


	public void SetDefaultRotations ()
	{
		foreach (var item in joints) {
			item.minValue = item.joint.localRotation.eulerAngles;
			item.maxValue = item.joint.localRotation.eulerAngles;
		}
	}
}

[System.Serializable]
public class JointsRandomAnimations
{
	public Transform joint;
	[System.NonSerialized]
	public float time;
	[System.NonSerialized]
	public Vector3 startValue;
	public Vector3 minValue;
	public Vector3 maxValue;
	private Vector3 currentValue;
	public float frequency = 1.0f;
	private Vector3 minValueDelta;
	private Vector3 maxValueDelta;

	public void OnStart ()
	{
		startValue = joint.localRotation.eulerAngles;
		minValueDelta = minValue - startValue;
		maxValueDelta = maxValue - startValue;
		time = Random.Range (0.0f, 0.5f * Mathf.PI / frequency);
	}

	public void SetCurrentValue ()
	{
		startValue = joint.localRotation.eulerAngles;
		currentValue = startValue;
		minValue = startValue + minValueDelta;
		maxValue = startValue + maxValueDelta;
	}

	public void AnimateJoint ()
	{
		time += Time.deltaTime;
		if (time > 100000.0f) {
			time = 0.0f;
		}
		currentValue = Vector3.Lerp (minValue, maxValue, 0.5f + 0.5f * Mathf.Sin (frequency * time));
		joint.localRotation = Quaternion.Lerp (joint.localRotation, Quaternion.Euler (currentValue), 10.0f * Time.deltaTime);
	}

}
