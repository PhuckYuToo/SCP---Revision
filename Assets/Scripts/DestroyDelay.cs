using UnityEngine;
using System.Collections;

public class DestroyDelay : MonoBehaviour
{
	public float time = 2f;
	public Vector2 randomTime;

	void Update() 
	{
		if(randomTime == Vector2.zero) Destroy(gameObject, time);
		else Destroy(gameObject, Random.Range(randomTime.x, randomTime.y));
	}
}
