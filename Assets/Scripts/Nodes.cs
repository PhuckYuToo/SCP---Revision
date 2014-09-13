using UnityEngine;
using System.Collections;

public class Nodes : MonoBehaviour 
{
	public ArrayList nodes = new ArrayList();

	void Start()
	{
		GetNodes();
	}

	void GetNodes()
	{
		foreach(GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
		{
			if(obj.tag == "106") nodes.Add(obj);
		}
	}
}
