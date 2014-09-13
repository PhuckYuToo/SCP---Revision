using UnityEngine;
using System.Collections;

public class SCP2300 : MonoBehaviour
{
	private AnimationClip still, idle1, idle2;

	[HideInInspector]
	public float density;
	
	public TextMesh number;
	public int elementNumber;
	public Material[] materials;

	private bool idle = true;
	private int glowEffect = 0;

	private float lifeTick = 0;

	void Start()
	{
		if(elementNumber == 92)
		{
			glowEffect = 1;
			density = 20.2f;
		}
		if(elementNumber == 26) density = 7.87f;
		transform.FindChild("Cube").renderer.material = materials[elementNumber];
		number.text = elementNumber.ToString();
		number.renderer.material.color = transform.FindChild("Cube").renderer.material.GetColor("_Color");
	
		still = animation.GetClip("2300_Still");
		idle1 = animation.GetClip("2300_Idle1");
		idle2 = animation.GetClip("2300_Idle2");
	}

	void Update()
	{
		if(idle)
		{
			if(Random.Range(0, 220) == 0 && !animation.IsPlaying(idle1.name) && !animation.IsPlaying(idle2.name))
			{
				if(Random.Range(0, 2) == 0) animation.CrossFade(idle1.name);	
				else animation.CrossFade(idle2.name);	
			}
			else if(!animation.IsPlaying(idle1.name) && !animation.IsPlaying(idle2.name)) animation.CrossFade(still.name);
		}

		lifeTick += Time.deltaTime;
		if(glowEffect == 1)
		{
			float shininess = Mathf.Clamp(Mathf.Abs(Mathf.Cos(lifeTick)), 0.3f, 1f);
			transform.FindChild("Cube").renderer.material.SetColor("_Color", new Color(0.07f, shininess, 0, 1f));
		}
	}
}
