using UnityEngine;
using System.Collections;

public class Wall : OnShot
{
	public GameObject wallFrame, cubePrefab;

	private Object destroyedWall;
	public string material;

	void Start() 
	{
		
	}

	void Update()
	{
		if(health > 0) transform.renderer.enabled = transform.collider.enabled = true;
		else transform.renderer.enabled = transform.collider.enabled = false;
	}

	protected override void OnDeath()
	{
		base.OnDeath();
		for(int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(false);
		destroyedWall = Instantiate(wallFrame, transform.position, transform.rotation);
		for(int j = 0; j < 10; j++)
			for(int i = 0; i < 27; i++)
			{
				GameObject cube = (GameObject)Instantiate(cubePrefab, transform.position, transform.rotation);
				cube.transform.parent = transform;
				cube.transform.localPosition = new Vector3(5.2f - (i / 2.5f), 0, 4 - (j / 2.5f));
				cube.transform.renderer.material = transform.renderer.materials[3];
				Vector2 offs = new Vector2(i * 0.06f, j * 0.06f);
				cube.transform.renderer.material.mainTextureScale = new Vector2(0.06f, 0.06f);
				cube.transform.renderer.material.mainTextureOffset = offs;
				cube.transform.renderer.material.SetTextureScale("_BumpMap", new Vector2(0.06f, 0.06f));
				cube.transform.renderer.material.SetTextureOffset("_BumpMap", offs);
				cube.transform.renderer.material.SetTextureScale("_SpecularTex", new Vector2(0.06f, 0.06f));
				cube.transform.renderer.material.SetTextureOffset("_SpecularTex", offs);
				cube.transform.renderer.material.SetTextureScale("_RampTex", new Vector2(0.06f, 0.06f));
				cube.transform.renderer.material.SetTextureOffset("_RampTex", offs);
			}
	}

	public void repair()
	{
		for(int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(true);
	}
}
