using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


//	This script handles flickering for lights
//	It is expected the light contains a true Unity Light source so will turn it on and off and will change the model texture for better effect


[RequireComponent(typeof(MeshRenderer))]
public class FlickeringLight : MonoBehaviour {

	[SerializeField] private Light light;
	[SerializeField]
	private float minWaitTime	= 0.1f;
	[SerializeField]
	private float maxWaitTime	= 0.5f;
	[SerializeField]
	private int materialIdx ;

	[SerializeField]
	private Material	onMaterial;
	[SerializeField]
	private Material	offMaterial;

	private MeshRenderer	meshRenderer;
	private Material []	materials;
	private bool isOn = true;
	private float duration = 0.2f;

	// Use this for initialization
	void Start () {
		
		if (light == null) {
			light = GetComponentInChildren <Light>();
		}
		if (light != null) {
			StartCoroutine (FlickerMaterial ());
		}
		meshRenderer	= GetComponent<MeshRenderer> ();
		materials	= meshRenderer.materials;
	}
	
	private IEnumerator FlickerMaterial()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range (minWaitTime, maxWaitTime));
			duration = Random.Range (minWaitTime, maxWaitTime/2);
			
			float time = 0;
			Material startValue = isOn ? onMaterial : offMaterial;
			Material endValue = isOn ? offMaterial : onMaterial;
			StartCoroutine(FlickerLight ());

			while (time < duration)
			{
				meshRenderer.materials[materialIdx].Lerp(startValue, endValue, time / duration);
				time += Time.deltaTime;
				yield return null;
			}

			materials[materialIdx] = endValue;
			isOn = !isOn;

			meshRenderer.materials = materials;
		}
	}

	//	Turn on and off the light
	private IEnumerator FlickerLight () {
			float time = 0;

			float startIntensity = isOn ? 3 : 2;
			float endIntensity = isOn ? 2 : 3;
        
			while (time < duration)
			{
				light.intensity = Mathf.Lerp(startIntensity, endIntensity, time / duration);
				time += Time.deltaTime;
				yield return null;
			}
			light.intensity = endIntensity;
			isOn = !isOn;
			
			meshRenderer.materials	= materials;
	}
	
}
