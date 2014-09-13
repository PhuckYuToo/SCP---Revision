using UnityEngine;
using System.Collections;

public class ButtonMouseOver : MonoBehaviour
{
	public Texture2D hoverTexture;
	private Texture2D normalTexture;

	private Color normalCol;

	[HideInInspector]
	public bool isSelected = false;

	void Start()
	{
		normalTexture = (guiTexture.texture as Texture2D);
		normalCol = guiTexture.color;
	}

	void Update()
	{
		if(isHover()) guiTexture.color = normalCol + new Color(.2f, .2f, .2f);
		else guiTexture.color = normalCol;

		if(isSelected) guiTexture.texture = hoverTexture;
		else guiTexture.texture = normalTexture;
	}

	public bool isHover()
	{
		return guiTexture.GetScreenRect().Contains(Input.mousePosition);
	}
}
