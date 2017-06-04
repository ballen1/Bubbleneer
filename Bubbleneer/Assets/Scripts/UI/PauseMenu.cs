using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	public GameObject[] Menus;

	public void OpenOptionsMenu()
	{
		CloseMenus ();
		Menus [1].SetActive (true);
	}

	public void OpenConResOpQuit()
	{
		CloseMenus ();
		Menus [0].SetActive (true);
	}

	private void CloseMenus()
	{
		foreach (GameObject element in Menus) {
			element.SetActive (false);
		}
	}


}
