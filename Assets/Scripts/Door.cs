using UnityEngine;
using System.Collections;

public class Door : OnShot
{
	public AnimationClip open;

	private int state = 0;
	
	void Update()
	{
		if(state == 1)
		{
			animation[open.name].speed = 1f;
			animation[open.name].time = 0;
			animation.Play(open.name);
			state = 2;
		}
	    if(state == 4)
		{
			animation[open.name].speed = -1f;
			animation[open.name].time = animation[open.name].length;
			animation.Play(open.name);
			state = 5;
		}
		if(state == 2 && !animation.IsPlaying(open.name)) state = 3;
		if(state == 5 && !animation.IsPlaying(open.name)) state = 0;
	}

	public override void OnShoot(float damage, RaycastHit hit)
	{
		if(state == 0) state = 1;
		else if(state == 3) state = 4;
	}
}
