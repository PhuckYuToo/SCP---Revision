using UnityEngine;
using System.Collections;

public class GameMode : MonoBehaviour 
{
	[HideInInspector]
	public bool locked = false;

	private Color col;

	public Transform player;
	private int selection, gameMode;
	private Transform modes;

	private ArrayList children = new ArrayList();
	private Color defaultCol;

	public Texture2D def, select;

	void Start()
	{
		selection = gameMode = transform.parent.parent.GetComponent<GameHUD>().gameMode;
		col = guiTexture.color;
		modes = transform.FindChild("GameOptions");
		for(int i = 0; i < modes.childCount; i++) if(modes.GetChild(i).name.Contains("GM_"))
		{
			children.Add(modes.GetChild(i));
			defaultCol = modes.GetChild(i).guiTexture.color;
			int number = int.Parse(modes.GetChild(i).name.Substring(3));
			if(number == gameMode)
			{
				modes.GetChild(i).guiTexture.color = new Color(.7f, .7f, .7f);
				modes.GetChild(i).guiTexture.texture = select;
			}
			else
			{
				modes.GetChild(i).guiTexture.color = defaultCol;
				modes.GetChild(i).guiTexture.texture = def;
			}
		}
	}
	
	void Update()
	{
		gameMode = transform.parent.parent.GetComponent<GameHUD>().gameMode;
		if(locked)
		{
			guiTexture.color = new Color(.2f, .2f, .2f, .2f);
			modes.gameObject.SetActive(false);
		}
		else 
		{
			if(Input.GetButton("GameMode"))
			{
				modes.gameObject.SetActive(true);
				guiTexture.color = new Color(.3f, .3f, .3f);

				for(int i = 0; i < children.Count; i++)
				{
					Transform child = modes.GetChild(i);
					int number = int.Parse(modes.GetChild(i).name.Substring(3));
					if(number == selection)
					{
						child.guiTexture.color = new Color(.7f, .7f, .7f);
						child.guiTexture.texture = select;
						if(child.FindChild("Label").guiTexture.pixelInset.x < 30) 
						{
							Vector2 lerp = Vector2.Lerp(child.FindChild("Label").guiTexture.pixelInset.position, new Vector2(30, child.FindChild("Label").guiTexture.pixelInset.y), Time.deltaTime * 4f);
							child.FindChild("Label").guiTexture.pixelInset = new Rect(lerp.x, child.FindChild("Label").guiTexture.pixelInset.y, child.FindChild("Label").guiTexture.pixelInset.width, child.FindChild("Label").guiTexture.pixelInset.height);
							Vector2 lerp2 = Vector2.Lerp(child.FindChild("Label").FindChild("Text").guiText.pixelOffset, new Vector2(58, child.FindChild("Label").FindChild("Text").guiText.pixelOffset.y), Time.deltaTime * 4f);
							child.FindChild("Label").FindChild("Text").guiText.pixelOffset = new Vector2(lerp2.x, child.FindChild("Label").FindChild("Text").guiText.pixelOffset.y);
						}
					}
					else
					{
						child.guiTexture.color = defaultCol;
						child.guiTexture.texture = def;
						if(child.FindChild("Label").guiTexture.pixelInset.x > 0) 
						{
							Vector2 lerp = Vector2.Lerp(child.FindChild("Label").guiTexture.pixelInset.position, new Vector2(0, child.FindChild("Label").guiTexture.pixelInset.y), Time.deltaTime * 4f);
							child.FindChild("Label").guiTexture.pixelInset = new Rect(lerp.x, child.FindChild("Label").guiTexture.pixelInset.y, child.FindChild("Label").guiTexture.pixelInset.width, child.FindChild("Label").guiTexture.pixelInset.height);
							Vector2 lerp2 = Vector2.Lerp(child.FindChild("Label").FindChild("Text").guiText.pixelOffset, new Vector2(28, child.FindChild("Label").FindChild("Text").guiText.pixelOffset.y), Time.deltaTime * 4f);
							child.FindChild("Label").FindChild("Text").guiText.pixelOffset = new Vector2(lerp2.x, child.FindChild("Label").FindChild("Text").guiText.pixelOffset.y);
						}
					}
				}
				if(Input.GetKey(KeyCode.DownArrow) && selection < children.Count) selection++;
				if(Input.GetKey(KeyCode.UpArrow) && selection > 1) selection--;
			}
			else 
			{
				guiTexture.color = col;
				modes.gameObject.SetActive(false);
				if(transform.parent.parent.GetComponent<GameHUD>().gameMode != selection) transform.parent.parent.GetComponent<GameHUD>().OnChange();
				transform.parent.parent.GetComponent<GameHUD>().gameMode = selection;
			}
		}
	}
}
