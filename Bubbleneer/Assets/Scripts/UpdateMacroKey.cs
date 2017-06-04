using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMacroKey : MonoBehaviour
{

	MacroManager macroManager;

	// Use this for initialization
	void Start ()
	{
		
		macroManager = GameObject.Find ("GameManager").GetComponent<MacroManager> ();
	
		switch (gameObject.transform.parent.name) 
		{
			case "ButtonBait":
				gameObject.GetComponent<Text> ().text = macroManager.BaitToolMacroKey;
				break;

			case "ButtonCut":
				gameObject.GetComponent<Text> ().text = macroManager.CutToolMacroKey;
				break;

			case "ButtonSell":
				gameObject.GetComponent<Text> ().text = macroManager.SellMacroKey;
				break;

			case "ButtonPipeStraight":
				gameObject.GetComponent<Text> ().text = macroManager.StraightPipeMacroKey;
				break;

			case "ButtonPipeCurve":
				gameObject.GetComponent<Text> ().text = macroManager.CurvePipeMacroKey;
				break;

			case "ButtonPipeUp":
				gameObject.GetComponent<Text> ().text = macroManager.UpPipeMacroKey;
				break;
		}
			
	}

}
