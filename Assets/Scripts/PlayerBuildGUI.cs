using UnityEngine;
using System.Collections;

public class PlayerBuildGUI : MonoBehaviour 
{
	public Texture2D slot, slotSelect, labelRect;
	public GameObject[] SCPChambers, Walls, Doorways, Traps, Misc;
	private GameObject menu, prev, next, label, buy, desc;
	private GameObject[] buttons;
	private Transform GUICam, buildCam, snapper;

	private int selection = -1, index = 0, selectedIndex = 0, rotInt = 0;
	private GameObject oldObj, build;
	private GameObject[] chosenList;

	void Start()
	{
		buildCam = transform.FindChild("Camera");
		buttons = new GameObject[5];
		menu = new GameObject("Menu");
		prev = new GameObject("PrevMenu");
		next = new GameObject("NextMenu");
		label = new GameObject("Label");
		buy = new GameObject("BuyMenu");
		desc = new GameObject("DescMenu");
		prev.AddComponent("GUIText");
		next.AddComponent("GUIText");
		menu.AddComponent("GUITexture");
		next.AddComponent("GUITexture");
		prev.AddComponent("GUITexture");
		label.AddComponent("GUIText");
		label.AddComponent("GUITexture");
		buy.AddComponent("GUIText");
		buy.AddComponent("GUITexture");
		desc.AddComponent("GUIText");
		desc.AddComponent("GUITexture");
		menu.guiTexture.texture = prev.guiTexture.texture = next.guiTexture.texture = buy.guiTexture.texture = desc.guiTexture.texture = slot;
		menu.transform.parent = prev.transform.parent = next.transform.parent = buy.transform.parent = desc.transform.parent = label.transform.parent = GameObject.Find("HUD/Build").transform;
		menu.transform.localScale = prev.transform.localScale = next.transform.localScale = buy.transform.localScale = desc.transform.localScale = label.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
		menu.transform.position = prev.transform.position = next.transform.position = desc.transform.position = buy.transform.position = new Vector3(0.5f, 0.05f, 1.0f);
		label.transform.position = new Vector3(0.5f, 0.05f, 21.0f);
		label.guiTexture.texture = labelRect;
		buy.guiTexture.color = new Color(0.4f, 0.8f, 0.4f);

		prev.guiText.font = GameObject.Find("HUD/Mode/ModeSelection/GamemodeText").guiText.font;
		prev.guiText.anchor = TextAnchor.MiddleCenter;
		prev.guiText.alignment = TextAlignment.Center;
		next.guiText.font = GameObject.Find("HUD/Mode/ModeSelection/GamemodeText").guiText.font;
		next.guiText.anchor = TextAnchor.MiddleCenter;
		next.guiText.alignment = TextAlignment.Center;
		desc.guiText.font = GameObject.Find("HUD/Mode/ModeSelection/GamemodeText").guiText.font;
		desc.guiText.anchor = TextAnchor.MiddleCenter;
		desc.guiText.alignment = TextAlignment.Center;
		buy.guiText.font = GameObject.Find("HUD/Mode/ModeSelection/GamemodeText").guiText.font;
		buy.guiText.anchor = TextAnchor.MiddleCenter;
		buy.guiText.alignment = TextAlignment.Center;
		label.guiText.font = GameObject.Find("HUD/Mode/ModeSelection/GamemodeText").guiText.font;
		label.guiText.anchor = TextAnchor.MiddleCenter;
		label.guiText.alignment = TextAlignment.Center;
		next.guiText.text = ">";
		prev.guiText.text = "<";
		buy.guiText.text = "$";
		desc.guiText.text = "?";
		prev.AddComponent("ButtonMouseOver");
		next.AddComponent("ButtonMouseOver");
		menu.AddComponent("ButtonMouseOver");
		buy.AddComponent("ButtonMouseOver");
		desc.AddComponent("ButtonMouseOver");

		for(int i = 0; i < buttons.Length; i++)
		{
			buttons[i] = new GameObject("Slot");
			buttons[i].AddComponent("GUITexture");

			GameObject gText = new GameObject("SlotText"), sText = new GameObject("ShadowText");
			gText.AddComponent("GUIText");
			sText.AddComponent("GUIText");

			buttons[i].transform.parent = GameObject.Find("HUD/Build").transform;
			buttons[i].transform.position = new Vector3(0.5f, 0.05f, 1.0f);
			buttons[i].transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
			buttons[i].guiTexture.texture = slot;
			buttons[i].AddComponent("ButtonMouseOver");
			buttons[i].GetComponent<ButtonMouseOver>().hoverTexture = slotSelect;

			gText.guiText.font = GameObject.Find("HUD/Mode/ModeSelection/GamemodeText").guiText.font;
			gText.guiText.anchor = TextAnchor.MiddleCenter;
			gText.guiText.alignment = TextAlignment.Center;
			gText.transform.parent = buttons[i].guiTexture.transform;
			gText.transform.position = buttons[i].guiTexture.transform.position + new Vector3(0, 0, 44);

			sText.guiText.font = GameObject.Find("HUD/Mode/ModeSelection/GamemodeText").guiText.font;
			sText.guiText.anchor = TextAnchor.MiddleCenter;
			sText.guiText.alignment = TextAlignment.Center;
			sText.transform.parent = buttons[i].guiTexture.transform;
			sText.transform.position = buttons[i].guiTexture.transform.position + new Vector3(0, 0, 43);
			sText.guiText.color = Color.black;

			switch(i)
			{
			case 0:
				gText.guiText.text = sText.guiText.text = "SCP";
				break;
			case 1:
				gText.guiText.text = sText.guiText.text = "Walls";
				break;
			case 2:
				gText.guiText.text = sText.guiText.text = "Doors";
				break;
			case 3:
				gText.guiText.text = sText.guiText.text = "Traps";
				break;
			case 4:
				gText.guiText.text = sText.guiText.text = "Misc";
				break;
			}
		}
		GUICam = GameObject.Find("3DGUICamera").transform;
	}
	
	void Update()
	{
		float aspect = (Screen.width * Screen.height) / (Screen.width * 200f);
		for(int i = 0; i < buttons.Length; i++)
		{
			buttons[i].guiTexture.pixelInset = new Rect(i * (40 * aspect) - (buttons.Length * 20 * aspect), 0, 40 * aspect, 40 * aspect);
			buttons[i].transform.FindChild("SlotText").guiText.pixelOffset = new Vector2(buttons[i].guiTexture.pixelInset.x, buttons[i].guiTexture.pixelInset.y) + new Vector2(20 * aspect, 20 * aspect);
			buttons[i].transform.FindChild("SlotText").guiText.fontSize = (int)(aspect * 10);
			buttons[i].transform.FindChild("ShadowText").guiText.pixelOffset = new Vector2(buttons[i].guiTexture.pixelInset.x, buttons[i].guiTexture.pixelInset.y - 2) + new Vector2(20 * aspect, 20 * aspect);
			buttons[i].transform.FindChild("ShadowText").guiText.fontSize = (int)(aspect * 10);
	
			if(buttons[i].GetComponent<ButtonMouseOver>().isHover() && Input.GetMouseButtonDown(0))
			{
				if(selection != i)
				{
					index = 0;
					selection = i;
					buttons[i].GetComponent<ButtonMouseOver>().isSelected = true;
					Destroy(build);
					if(index < getCurList().Length - 1) spawnObj();
				}
				else selection = -1;
			}
			if(selection != i) buttons[i].GetComponent<ButtonMouseOver>().isSelected = false;
			else
			{
				menu.gameObject.SetActive(true);
				next.gameObject.SetActive(true);
				prev.gameObject.SetActive(true);
				buy.gameObject.SetActive(true);
				desc.gameObject.SetActive(true);
				label.gameObject.SetActive(true);
				GUICam.camera.enabled = true;
			}
			if(selection < 0)
			{
				menu.gameObject.SetActive(false);
				next.gameObject.SetActive(false);
				prev.gameObject.SetActive(false);
				buy.gameObject.SetActive(false);
				desc.gameObject.SetActive(false);
				label.gameObject.SetActive(false);
				GUICam.camera.enabled = false;
				Destroy(oldObj);
				index = 0;
			}
		}

		if(selection >= 0)
		{
			if(next.GetComponent<ButtonMouseOver>().isHover() && Input.GetMouseButtonDown(0) && index < getCurList().Length - 1)
			{
				index++;
				spawnObj();
			}
			if(prev.GetComponent<ButtonMouseOver>().isHover() && Input.GetMouseButtonDown(0) && index > 0)
			{
				index--;
				spawnObj();
			}
		}

		float x;
		if(selection < 0) x = 0f;
		else x = buttons[selection].guiTexture.pixelInset.x;
		menu.guiTexture.pixelInset = new Rect(x, 40 * aspect, 40 * aspect, 40 * aspect);
		prev.guiTexture.pixelInset = new Rect(x - 18 * aspect, 42 * aspect, 20 * aspect, 20 * aspect);
		next.guiTexture.pixelInset = new Rect(x + 38 * aspect, 42 * aspect, 20 * aspect, 20 * aspect);
		prev.guiText.pixelOffset = new Vector2(x - 8 * aspect, 52 * aspect);
		next.guiText.pixelOffset = new Vector2(x + 48 * aspect, 52 * aspect);

		desc.guiTexture.pixelInset = new Rect(x - 18 * aspect, 62 * aspect, 20 * aspect, 20 * aspect);
		buy.guiTexture.pixelInset = new Rect(x + 38 * aspect, 62 * aspect, 20 * aspect, 20 * aspect);
		desc.guiText.pixelOffset = new Vector2(x - (8 * aspect), (72 * aspect) - 1 / aspect);
		buy.guiText.pixelOffset = new Vector2(x + (48 * aspect), (72 * aspect) - 1 / aspect);
		prev.guiText.fontSize = (int)(aspect * 15);
		next.guiText.fontSize = (int)(aspect * 15);
		buy.guiText.fontSize = (int)(aspect * 15);
		desc.guiText.fontSize = (int)(aspect * 15);

		GUIStyle style = new GUIStyle();
		style.font = label.guiText.font;
		label.guiText.fontSize = style.fontSize = (int)(aspect * 10);
		float fontX = style.CalcSize(new GUIContent(label.guiText.text)).x + 16;
		label.guiTexture.pixelInset = new Rect((x + (20 * aspect)) - fontX / 2, 80 * aspect, fontX, 20 * aspect);
		label.guiText.pixelOffset = new Vector2(x + (20 * aspect), 90 * aspect);
		if(getCurList() != null && index < getCurList().Length) label.guiText.text = getCurList()[index].name;
		else label.guiText.text = "";

		if(selection >= 0) GUICam.camera.pixelRect = new Rect(Screen.width / 2 + x + (6 * aspect), 55.9f * aspect, 28 * aspect, 28 * aspect);

		if(oldObj) GUICam.RotateAround(oldObj.transform.position, new Vector3(0.0f, 0.0f, 1.0f), 20f * Time.deltaTime * 4f);
	
		if(buy.GetComponent<ButtonMouseOver>().isHover() && Input.GetMouseButtonUp(0) && getCurList() != null)
		{
			chosenList = getCurList();
			selectedIndex = index;
			Destroy(build);
			build = Instantiate(getCurList()[index], new Vector3(200, 200, 200), getCurList()[index].transform.rotation) as GameObject;
			build.transform.Rotate(-90, 0, 0);
			build.layer = 14;
			SetShaderRecursively(build, "Custom/GlassShader");
			selection = -1;
		}

		if(build != null)
		{
			if(Input.GetMouseButtonDown(1))
			{
				if(Input.GetKey(KeyCode.LeftControl)) //TODO
				{
					rotInt--;
					if(rotInt < 0) rotInt = 6;
					rotateBuild(rotInt);
				}
				else
				{
					rotInt++;
					if(rotInt > 6) rotInt = 0;
					rotateBuild(rotInt);
				}
			}

			RaycastHit mouseHit;
			int layerMask = 1 << 14;
			layerMask = ~layerMask;
			if(Physics.Raycast(buildCam.camera.ScreenPointToRay(Input.mousePosition), out mouseHit, Mathf.Infinity, layerMask))
			{
				snapper = canSnap(mouseHit.transform, build.transform);
				if(snapper != null) fixRotation(mouseHit);
			}
			else snapper = null;

			if(snapper != null)
			{
				SetColorRecursively(build, new Color(0.4f, 0.4f, 0.4f));
				if(Input.GetMouseButtonDown(0) && chosenList != null && selectedIndex < chosenList.Length && canPlace())
				{
					GameObject structure;
					structure = Instantiate(chosenList[selectedIndex], build.transform.position, build.transform.rotation) as GameObject;
					structure.transform.parent = GameObject.Find("Terrain").transform;
				}
			}
			else 
			{
				SetColorRecursively(build, new Color(1f, 0.4f, 0.4f));
				RaycastHit hit;
				Ray ray = buildCam.camera.ScreenPointToRay(Input.mousePosition);
				if(Physics.Raycast(ray, out hit)) build.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
			}
		}
	}

	private void spawnObj()
	{
		Destroy(oldObj);
		GameObject obj = oldObj = (GameObject)Instantiate(getCurList()[index].gameObject, new Vector3(100, 100, 100), getCurList()[index].gameObject.transform.rotation);
		SetLayerRecursively(obj, 13);
		SetShadowRecursively(obj, false);
		GUICam.position = obj.transform.position + new Vector3(0, 15, 4);
		GUICam.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
	}

	private GameObject[] getCurList()
	{
		switch(selection)
		{
		case 0:
			return SCPChambers;
		case 1:
			return Walls;
		case 2:
			return Doorways;
		case 3:
			return Traps;
		case 4:
			return Misc;
		default: return null;
		}
	}

	void SetLayerRecursively(GameObject obj, int newLayer)
	{
		obj.layer = newLayer;
		foreach(Transform child in obj.transform) SetLayerRecursively(child.gameObject, newLayer);
	}

	void SetShadowRecursively(GameObject obj, bool value)
	{
		if(obj.GetComponent<MeshRenderer>()) obj.GetComponent<MeshRenderer>().receiveShadows = false;
		if(obj.GetComponent<SkinnedMeshRenderer>()) obj.GetComponent<SkinnedMeshRenderer>().receiveShadows = false;
		foreach(Transform child in obj.transform) SetShadowRecursively(child.gameObject, value);
	}

	void SetShaderRecursively(GameObject obj, string shader)
	{
		if(obj.renderer) foreach(Material mat in obj.renderer.materials) mat.shader = Shader.Find(shader);
		foreach(Transform child in obj.transform) SetShaderRecursively(child.gameObject, shader);
	}

	void SetColorRecursively(GameObject obj, Color color)
	{
		if(obj.renderer) foreach(Material mat in obj.renderer.materials) mat.SetColor("_ReflectColor", color);
		foreach(Transform child in obj.transform) SetColorRecursively(child.gameObject, color);
	}

	public void onChangeGameMode()
	{
		Destroy(build);
	}	

	private bool canPlace()
	{
		if(selection == -1 && !isHovering()) return true;
		else return false;
	}

	private void rotateBuild(int rott) //TODO
	{
		//build.transform.RotateAround(build.transform.position, new Vector3(0, 1, 0), rott);
		rotInt = rott;
		build.transform.rotation = Quaternion.Euler(build.transform.rotation.eulerAngles.x, build.transform.rotation.eulerAngles.y, rotInt * 45f);
	}

	private bool isHovering()
	{
		for(int i = 0; i < buttons.Length; i++) if(buttons[i].GetComponent<ButtonMouseOver>().isHover()) return true;
		return false;
	}

	private Transform canSnap(Transform mouseHit, Transform build)
	{
		ArrayList snaps = new ArrayList();
		int index = -1;
		float dist = 0f;
		for(int i = 0; i < mouseHit.childCount; i++)
		{
			Transform child = mouseHit.GetChild(i);
			if(child.name.Equals("Snap"))
			{
				if(index == -1)
				{
					index = i;
					dist = (build.position - child.position).magnitude;
				}
				else
				{
					float newDist = (build.position - child.position).magnitude;
					if(dist > newDist)
					{
						index = i;
						dist = newDist;
					}
				}
			}
		}
		if(index == -1) return null;
		else return mouseHit.GetChild(index);
	}

	private void fixRotation(RaycastHit mouseHit)
	{
		int rot = (int)build.transform.rotation.eulerAngles.y;
		int fin;
		if(rot >= 0 && rot < 45) fin = 0;
		else if(rot >= 45 && rot < 90) fin = 45;
		else if(rot >= 90 && rot < 135) fin = 90;
		else if(rot >= 135 && rot < 180) fin = 135;
		else if(rot >= 180 && rot < 225) fin = 180;
		else if(rot >= 225 && rot < 270) fin = 225;
		else if(rot >= 270 && rot < 315) fin = 270;
		else fin = 0;
		//build.transform.rotation = Quaternion.Euler(build.transform.rotation.eulerAngles.x, fin, build.transform.rotation.eulerAngles.z);
	
		Transform end = canSnap(mouseHit.transform, snapper);
		if(fin == 0 || fin == 180) build.transform.position = snapper.position + end.localPosition;
		else if(fin == 90) build.transform.position = snapper.position + new Vector3(0, 0, 7);
		else if(fin == 270) build.transform.position = snapper.position + new Vector3(0, 0, -7);

		else if(fin == 45) build.transform.position = snapper.position + new Vector3(4.5f, 0, -4.5f);
		else if(fin == 135) build.transform.position = snapper.position + new Vector3(-4.5f, 0, -4.5f);
	
		else if(fin == 225) build.transform.position = snapper.position + new Vector3(-4.5f, 0, 4.5f);
		else if(fin == 315) build.transform.position = snapper.position + new Vector3(4.5f, 0, 4.5f);
	}
}
