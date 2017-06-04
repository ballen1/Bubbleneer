using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MacroManager : MonoBehaviour {

	public string SellMacroKey;
	public string BaitToolMacroKey;
	public string CutToolMacroKey;
	public string StraightPipeMacroKey;
	public string CurvePipeMacroKey;
	public string UpPipeMacroKey;
	public string ZoomInMacroKey;
	public string ZoomOutMacroKey;

	public InputField sellField;
	public InputField baitField;
	public InputField cutField;
	public InputField straightPipeField;
	public InputField curvePipeField;
	public InputField zoomInField;
	public InputField zoomOutField;

	public Text sellButton;
	public Text baitButton;
	public Text cutButton;
	public Text straightPipeButton;
	public Text curvePipeButton;
	public Text zoomInButton;
	public Text zoomOutButton;

	public List<string> macroCharacters; 

	private string previousSell;
	private string previousBait;
	private string previousCut;
	private string previousSPB;
	private string previousCPB;

	void Awake(){
		macroCharacters = new List<string> ();
	}

	void Start(){
		
		macroCharacters.Add (SellMacroKey);
		macroCharacters.Add (BaitToolMacroKey);
		macroCharacters.Add (CutToolMacroKey);
		macroCharacters.Add (StraightPipeMacroKey);
		macroCharacters.Add (CurvePipeMacroKey);
		macroCharacters.Add (ZoomInMacroKey);
		macroCharacters.Add (ZoomOutMacroKey);

		sellField.text = SellMacroKey;
		baitField.text = BaitToolMacroKey;
		cutField.text = CutToolMacroKey;
		straightPipeField.text = StraightPipeMacroKey;
		curvePipeField.text = CurvePipeMacroKey;
		zoomInField.text = ZoomInMacroKey;
		zoomOutField.text = ZoomOutMacroKey;

		sellField.onEndEdit.AddListener(SubmitSellKey);
		baitField.onEndEdit.AddListener(SubmitBaitToolKey);
		cutField.onEndEdit.AddListener (SubmitCutToolKey);
		straightPipeField.onEndEdit.AddListener (SubmitStraightPipeKey);
		curvePipeField.onEndEdit.AddListener (SubmitCurvePipeKey);
		zoomInField.onEndEdit.AddListener (SubmitZoomInKey);
		zoomOutField.onEndEdit.AddListener (SubmitZoomOutKey);

	}

	void Update(){

		// + Updates buttons in scene
		sellButton.text = SellMacroKey;
		baitButton.text = BaitToolMacroKey;
		cutButton.text = CutToolMacroKey;
		straightPipeButton.text = StraightPipeMacroKey;
		curvePipeButton.text = CurvePipeMacroKey;
		zoomInButton.text = ZoomInMacroKey;
		zoomOutButton.text = ZoomOutMacroKey;

		sellButton.text = sellButton.text.ToUpper ();
		baitButton.text = baitButton.text.ToUpper ();
		cutButton.text = cutButton.text.ToUpper ();
		straightPipeButton.text = straightPipeButton.text.ToUpper ();
		curvePipeButton.text = curvePipeButton.text.ToUpper ();
		zoomInButton.text = zoomInButton.text.ToUpper ();
		zoomOutButton.text = zoomOutButton.text.ToUpper ();
	
	}

	private void SubmitSellKey(string arg0){

		// + Handles the user putting a captial letter in that
		//   would screw everything up
		arg0 = arg0.ToLower ();

		if (!arg0.Equals ("") && !macroCharacters.Contains (arg0)) {

			macroCharacters.Remove (sellButton.text.ToLower());

			SellMacroKey = arg0;
			macroCharacters.Add (SellMacroKey);
		}

		else if(arg0.Equals ("") || macroCharacters.Contains (arg0)){
			sellField.text = SellMacroKey;
		}

	}

	private void SubmitBaitToolKey(string arg0){

		arg0 = arg0.ToLower ();

		if (!arg0.Equals ("") && !macroCharacters.Contains (arg0)) {

			macroCharacters.Remove (baitButton.text.ToLower());

			BaitToolMacroKey = arg0;
			macroCharacters.Add (BaitToolMacroKey);
		}

		else if(arg0.Equals ("") || macroCharacters.Contains (arg0)){
			baitField.text = BaitToolMacroKey;
		}
	}

	private void SubmitCutToolKey(string arg0){

		arg0 = arg0.ToLower ();

		if (!arg0.Equals ("") && !macroCharacters.Contains (arg0)) {

			macroCharacters.Remove (cutButton.text.ToLower());

			CutToolMacroKey= arg0;
			macroCharacters.Add (CutToolMacroKey);
		}

		else if(arg0.Equals ("") || macroCharacters.Contains (arg0)){
			cutField.text = CutToolMacroKey;
		}

	}

	private void SubmitStraightPipeKey(string arg0){

		arg0 = arg0.ToLower ();

		if (!arg0.Equals ("") && !macroCharacters.Contains (arg0)) {

			macroCharacters.Remove (straightPipeButton.text.ToLower ());

			StraightPipeMacroKey = arg0;
			macroCharacters.Add (StraightPipeMacroKey);
		} else if (arg0.Equals ("") || macroCharacters.Contains (arg0)) {
			straightPipeField.text = StraightPipeMacroKey;
		}
	}

	private void SubmitCurvePipeKey(string arg0){

		arg0 = arg0.ToLower ();

		if (!arg0.Equals ("") && !macroCharacters.Contains (arg0)) {

			macroCharacters.Remove (curvePipeButton.text.ToLower ());

			CurvePipeMacroKey = arg0;
			macroCharacters.Add (CurvePipeMacroKey);
		} else if (arg0.Equals ("") || macroCharacters.Contains (arg0)) {
			curvePipeField.text = CurvePipeMacroKey;
		}
	}

	private void SubmitZoomInKey(string arg0){

		arg0 = arg0.ToLower ();

		if (!arg0.Equals ("") && !macroCharacters.Contains (arg0)) {

			macroCharacters.Remove (zoomInButton.text.ToLower ());

			ZoomInMacroKey = arg0;
			macroCharacters.Add (ZoomInMacroKey);
		} else if (arg0.Equals ("") || macroCharacters.Contains (arg0)) {
			zoomInField.text = ZoomInMacroKey;
		}
	}

	private void SubmitZoomOutKey(string arg0){

		arg0 = arg0.ToLower ();

		if (!arg0.Equals ("") && !macroCharacters.Contains (arg0)) {

			macroCharacters.Remove (zoomOutButton.text.ToLower ());

			ZoomOutMacroKey = arg0;
			macroCharacters.Add (ZoomOutMacroKey);
		} else if (arg0.Equals ("") || macroCharacters.Contains (arg0)) {
			zoomOutField.text = ZoomOutMacroKey;
		}
	}

}