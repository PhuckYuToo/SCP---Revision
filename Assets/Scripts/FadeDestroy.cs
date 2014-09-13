using UnityEngine;
using System.Collections;

public class FadeDestroy : MonoBehaviour
{
	public float time = 2f;
	public Vector2 randomTime;

	private int fadeStage = 0;
	private float i = 1f;
	private Shader shading;
	
	void Start() 
	{
		if(randomTime == Vector2.zero) StartCoroutine(delay(time));
		else StartCoroutine(delay(Random.Range(randomTime.x, randomTime.y)));
	}

	void Update()
	{
		if(transform.renderer.material.HasProperty("_Color"))
		{
			Color old = transform.renderer.material.color;
			transform.renderer.material.SetColor("_Color", new Color(old.r, old.g, old.b, i));
		}
		else if(transform.renderer.material.HasProperty("_SpecColor"))
		{
			transform.renderer.material.SetColor("_SpecColor", new Color(i, i, i, 1f));
		}
		if(fadeStage == 1)
		{
			if(i > 0f) i -= Time.deltaTime;
			else fadeStage = 2;
		}
		if(fadeStage == 2) Destroy (gameObject);
	}

	IEnumerator delay(float time)
	{
		yield return new WaitForSeconds(time);
		fadeStage = 1;
		transform.renderer.material.shader = Shader.Find("Transparent/Bumped Diffuse");
	}
}
