using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour {

	public GameObject straightPipe;
	public GameObject curvePipe;
	public GameObject upPipe;
	public GameObject cuttingTool;
	public GameObject baitTool;
	public GameObject clearGround;

	private ToggleManager toggleManager;

	void Start(){
		toggleManager = GameObject.Find ("Toggles").GetComponent<ToggleManager>();
	}

	public GameObject getCurrentModel()
	{
		GameObject tempObject = null;

		if (toggleManager.currentSelection != null) {
			switch (toggleManager.currentSelection.name) {

			case "ButtonBait":
				tempObject = baitTool;
				break;

			case "ButtonCut":
				tempObject = cuttingTool;
				break;

			case "ButtonPipeStraight":
				tempObject = straightPipe;
				break;

			case "ButtonPipeCurve":
				tempObject = curvePipe;
				break;

			case "ButtonPipeUp":
				tempObject = upPipe;
				break;

			
			default:
				break;

			}
		}
		return tempObject;
	}
}
