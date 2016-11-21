using UnityEngine;
using System.Collections;

public class BlinkShader : MonoBehaviour
{
	public float fadeSpeed = 2f;
	public float highIntensity = 2f;
	public float lowIntensity = 0.5f;
	public float changeMargin = 0.2f;
	public bool blinkOn;
	private GameObject Aviator;

	float currentIntensity;

	private float targetIntensity;

	private MaterialPropertyBlock materialProperty;
	private Renderer renderer;


	void Awake ()
	{

		currentIntensity = 0f;
		targetIntensity = highIntensity;
		materialProperty = new MaterialPropertyBlock ();
		renderer = this.GetComponent<Renderer> ();
	}


	void Update ()
	{

	
		// If the light is on...
		if (blinkOn) {
			currentIntensity = Mathf.Lerp (currentIntensity, targetIntensity, fadeSpeed * Time.deltaTime);

			CheckTargetIntensity ();
		} else
			currentIntensity = Mathf.Lerp (currentIntensity, 0f, fadeSpeed * Time.deltaTime);

		SetMaterial ();

	}

	void SetMaterial ()
	{
		materialProperty.Clear ();
		materialProperty.SetFloat ("_Shininess", currentIntensity);
		renderer.SetPropertyBlock (materialProperty);
	}

	void CheckTargetIntensity ()
	{
		if (Mathf.Abs (targetIntensity - currentIntensity) < changeMargin) {
			if (targetIntensity == highIntensity)
				targetIntensity = lowIntensity;
			else
				targetIntensity = highIntensity;
		}
	}
}