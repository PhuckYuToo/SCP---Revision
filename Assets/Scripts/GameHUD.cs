using UnityEngine;
using System.Collections;

public class GameHUD : MonoBehaviour
{
	public Transform playerMTF, playerBuild;
	public int gameMode;

	public bool isBreach = false;

	public Texture2D cursor;

	void Start()
	{
		Cursor.SetCursor(this.cursor, Vector2.zero, CursorMode.Auto);
	}

	void Update()
	{
		if(isBreach)
		{
			gameMode = 1;
			transform.FindChild("Mode/ModeSelection").GetComponent<GameMode>().locked = true;
		}
		else transform.FindChild("Mode/ModeSelection").GetComponent<GameMode>().locked = false;
		switch(gameMode)
		{
		case 1: //MTF
			playerMTF.GetComponent<Player>().lockMouseMovement = false;
			transform.FindChild("MTF").gameObject.SetActive(true); //HUD
			transform.FindChild("Build").gameObject.SetActive(false); //HUD
			playerBuild.gameObject.SetActive(false);
			Screen.showCursor = false;
			playerMTF.gameObject.SetActive(true);
			break;
		case 2: //Build
			playerMTF.GetComponent<Player>().lockMouseMovement = true;
			transform.FindChild("MTF").gameObject.SetActive(false); //HUD
			transform.FindChild("Build").gameObject.SetActive(true); //HUD
			playerBuild.gameObject.SetActive(true);
			Screen.showCursor = true;
			playerMTF.gameObject.SetActive(false);
			break;
		}
	}

	public void OnChange()
	{
		switch(gameMode)
		{
		case 1: //MTF

			break;
		case 2: //Build
			playerBuild.GetComponent<PlayerBuildGUI>().onChangeGameMode();
			break;
		}
	}
}
