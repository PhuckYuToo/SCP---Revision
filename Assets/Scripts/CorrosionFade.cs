using UnityEngine;
using System.Collections;

public class CorrosionFade : MonoBehaviour
{
	private float size = 0f, lifeSpan = 6f;

	private int fadeInt = 0;

	void Update()
	{
		transform.localScale = new Vector3(size, size, size);
		if(size < 0.3f) size += 0.1f * Time.deltaTime;
		if(size >= 0.3f && fadeInt == 0) StartCoroutine(fading());

		if(fadeInt == 2 && size > 0f) size -= Time.deltaTime * 0.2f;
		if(fadeInt == 2 && size <= 0f) Destroy(gameObject);
	}

	IEnumerator fading()
	{
		fadeInt = 1;
		yield return new WaitForSeconds(lifeSpan);
		fadeInt = 2;
	}
}
