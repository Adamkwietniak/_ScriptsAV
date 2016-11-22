using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class WindScript : MonoBehaviour
{

	public float hoverForce;
	public Rigidbody rbWingman;
	//private AudioSource source;
	//public AudioClip windSound;
	//private float randomNumber;
	public bool wingDirectionUp, wingDirectionRight, wingDirectionOpenUp, changeRotation, changePose = false;
	public Image windWarningImage;
	private float valueOfAlpha = 0f;
	private bool upColor = false;
	private float blinkingSpeed = 3f;
	[SerializeField]
	private AviatorController controller;
	[SerializeField]
	private JointsPoseController posController;

	public Transform wingmanTransform;
	private float smooth = 1f;
	private Quaternion targetRotation;
	private bool isTurning = false;
	public float axisToRotate;
	AviatorGUI avGUI;



	void Start ()
	{
		//source = GetComponent <AudioSource> ();
		targetRotation = wingmanTransform.transform.localRotation;
		avGUI = FindObjectOfType<AviatorGUI> ();
	}

	void Update ()
	{
		
		if (isTurning == true)
			wingmanTransform.transform.localRotation = Quaternion.Lerp (wingmanTransform.transform.localRotation, targetRotation, 5 * smooth * Time.deltaTime);
	}


	void OnTriggerEnter (Collider other)
	{
		
		if (other.tag == "Player") {
			if (wingDirectionUp == true) {
				wingmanTransform.Rotate (axisToRotate, 0, 0);
				windWarningImage.enabled = true;
			} else if (wingDirectionRight == true) {
				rbWingman.AddForce (Vector3.back * hoverForce, ForceMode.Acceleration);
				windWarningImage.enabled = true;
			} else if (wingDirectionOpenUp == true) {
				controller.transform.Translate (Vector3.up * 50.0f * Time.deltaTime);
				windWarningImage.enabled = true;
			} else if (changeRotation == true) {
				isTurning = true;
				targetRotation *= Quaternion.AngleAxis (axisToRotate, Vector3.right); 
			} else if (changePose == true) {
				avGUI.changeStandardPose = true;
				posController.SetPose ("Slow n hold", 1.0f);
			}

			//source.PlayOneShot (windSound);

		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.tag == "Player") {
			BlinkingImage ();
		}
	}


	void OnTriggerExit (Collider other)
	{
		if (rbWingman != null) {
			rbWingman.velocity = Vector3.zero;  /// powrót do normalnej prędkości
			rbWingman.angularVelocity = Vector3.zero;
			controller.transform.Translate (Vector3.zero);
			windWarningImage.enabled = false;
			isTurning = false;
			Destroy (gameObject, 1.0f);
		}

	}

	public void BlinkingImage () // metoda odpowiedzialna za mryganie znaczka "uważaj wiater se wieje"
	{

		windWarningImage.enabled = true;
		if (valueOfAlpha <= 1 && upColor == true) {
			valueOfAlpha += Time.deltaTime * blinkingSpeed;
			if (valueOfAlpha >= 1) {
				upColor = false;
				valueOfAlpha = 1;
			}
		} else if (valueOfAlpha >= 0 && upColor == false) {
			valueOfAlpha -= Time.deltaTime * blinkingSpeed;
			if (valueOfAlpha <= 0) {
				upColor = true;
				valueOfAlpha = 0;
			}
		}
		windWarningImage.color = new Color (255f, 255f, 255f, valueOfAlpha);
	}


}

