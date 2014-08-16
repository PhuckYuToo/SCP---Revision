using UnityEngine;
using System.Collections;

public class Mag : Pickable
{
	public int bullets = 30, capacity = 30;
	public string weapon;

	void Start()
	{
		if(bullets <= 0) this.itemName = "Empty " + itemName;
		else this.itemName = itemName + " (" + bullets + "/" + capacity + ")";
	}

	public override void OnPickup(Transform player)
	{
		base.OnPickup(player);
		Transform currentWeapon = player.FindChild("Camera").FindChild("Main Camera").FindChild("Gun Camera");
		if(bullets == capacity)
		{
			for(int i = 0; i < currentWeapon.childCount; i++)
				if(currentWeapon.GetChild(i).tag.Equals(weapon))
				{
					currentWeapon.GetChild(i).GetComponent<Weapon>().AddMag();
					break;
				}
		}
	}
}
