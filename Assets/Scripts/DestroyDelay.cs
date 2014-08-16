using UnityEngine;
using System.Collections;

public class DestroyDelay : MonoBehaviour
{
	public float time = 2f;

	void Update() 
	{
		Destroy(gameObject, time);
	}
}
