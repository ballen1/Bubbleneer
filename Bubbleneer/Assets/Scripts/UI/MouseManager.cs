using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class MouseManager : MonoBehaviour
{
	private NodeManager nodeManager;
    private GroundManager groundManager;
    private PipeManager pipeManager;
	private Highlight highLight;
	private CashManager Bank;
	private RoundSystem roundManager;
	private ToggleManager toggleManager;
	private ToolManager toolManager;
	private ModelManager modelManager;
	private Sounds sounds;

	private string currentTag;

	public GameObject quitMenu;

	// Use this for initialization
	void Awake ()
	{
		pipeManager = GameObject.Find ("GameManager").GetComponent<PipeManager> ();
		highLight = GameObject.Find ("GameManager").GetComponent<Highlight> ();
		toolManager = GameObject.Find ("GameManager").GetComponent<ToolManager> ();

		modelManager = GetComponent<ModelManager> ();
		roundManager = GetComponent<RoundSystem> ();
		nodeManager = GetComponent<NodeManager> ();
        groundManager = GetComponent<GroundManager>();
        toggleManager = GameObject.Find ("Toggles").GetComponent<ToggleManager> ();

		sounds = GameObject.Find ("AudioManager").GetComponent<Sounds> ();

		Bank = GetComponent<CashManager> ();
	}

	// Update is called once per frame
	void Update ()
	{
		
			// + The player left clicks on the mouse to build a pipe

			if (Input.GetMouseButtonDown (0) && toggleManager.currentSelection != null) {
				

			if (!roundManager.IsSimulationStarted ()) {
				// + Gets the tag from a button
				try {
					currentTag = EventSystem.current.currentSelectedGameObject.tag;
				} catch (NullReferenceException ex) {
					currentTag = "";
				}
					
				if (!currentTag.Equals ("Button") && highLight.getCurrentObject () != null) {
					// + A switch used to change the function of a left mouse click

					switch (toggleManager.currentSelection.name) {

					case "ButtonSell":
						
							// + Sells a selected pipe
						if (highLight.getCurrentObject ().tag.Equals ("Sellable")) {
							sellObject (highLight.getCurrentObject ());
						}
								
						break;

					case "ButtonBait":
						if (highLight.getCurrentObject ().tag.Equals ("Buildable")) {
							if (Bank.CanAfford(Bank.GetPipePrice("Bait"))) {
								Bank.RemoveCash (Bank.GetPipePrice ("Bait"));
								sounds.PlayPlaceBait ();
								toolManager.spawnBait (highLight.getCurrentObject ());
								if (!roundManager.IsRoundStarted ()) {
									roundManager.StartRound ();
								}
							}
						}
		
						break;


					case "ButtonCut":
						if (highLight.getCurrentObject ().tag.Equals ("Clearable")) {
							if (Bank.CanAfford(Bank.GetPipePrice("ScissorCut"))) {
								Bank.RemoveCash (Bank.GetPipePrice ("ScissorCut"));
								sounds.PlaySnip ();
								toolManager.trimSeaweed (highLight.getCurrentObject ());
								if (!roundManager.IsRoundStarted ()) {
									roundManager.StartRound ();
								}
							}
						}
						break;

					case "ButtonPipeStraight":

						if (highLight.getCurrentObject ().tag.Equals ("Buildable")) {
							buildPipe ();
						}
						break;

					case "ButtonPipeCurve":

						if (highLight.getCurrentObject ().tag.Equals ("Buildable")) {
							buildPipe ();
						}
						break;

					case "ButtonPipeUp":

						if (highLight.getCurrentObject ().tag.Equals ("Buildable")) {
							buildPipe ();
						}
						break;

					case "ButtonZoomIn":

						Camera.main.GetComponent<CameraScript> ().ZoomIn (2);

						break;

					case "ButtonZoomOut":
						Camera.main.GetComponent<CameraScript> ().ZoomOut (2);
						break;

					
					}
						
				}
			}
			} else if(Input.GetMouseButtonDown (0) && toggleManager.currentSelection == null){

				try {
					currentTag = EventSystem.current.currentSelectedGameObject.tag;
				} catch (NullReferenceException ex) {
					currentTag = "";
				}
				
			if (!currentTag.Equals ("QuitMenuButton") && (highLight.getCurrentObject () != null)) {
				if (highLight.getCurrentObject ().tag.Equals ("Buildable")) {
					sounds.PlayTouchedSand ();
				} else if (quitMenu.activeInHierarchy && highLight.getCurrentObject ().tag.Equals ("QuitMenuSoak")) {
					roundManager.UnpauseGame ();
				}
				} else {
					//print (currentTag);
				}



			}
			// + The player right clicks on the mouse to rotate a the preview pipe
			else if (Input.GetMouseButtonDown (1)) {
				pipeManager.rotateAmount += 90;
				pipeManager.dirIndex += 1;
				pipeManager.dirIndex %= 4;
			}


			
	}

	/// <summary>
	/// Handles the building. 
	/// </summary>
	private void buildPipe(){
	
		int PipePrice = Bank.GetPipePrice (toggleManager.currentSelection.name);

		if (Bank.CanAfford (PipePrice)) {
			pipeManager.spawnPipe (highLight.getCurrentObject ());

			sounds.PlayPipeConfirm ();

			Bank.RemoveCash (PipePrice);

			if (!roundManager.IsRoundStarted ()) {
				roundManager.StartRound ();
			}
		}

	}

	/// <summary>
	/// Sells the object.
	/// </summary>
	/// <param name="hit">Hit.</param>
	public void sellObject (Transform hit)
	{

		if (hit.name.Contains ("Straight")) {
			Bank.AddCash(Bank.GetPipePrice ("ButtonPipeStraight"));
		} else if (hit.name.Contains ("PipeC_Base")) {
			Bank.AddCash(Bank.GetPipePrice ("ButtonPipeCurve"));
		} else if (hit.name.Contains ("Up")){
			Bank.AddCash(Bank.GetPipePrice ("ButtonPipeUp"));
		}
			
		sounds.PlaySellChaChing ();

		NodeManager.node curNode = nodeManager.pipeArray [(int)nodeManager.worldToArray (hit.position).x, (int)nodeManager.worldToArray (hit.position).y, (int)nodeManager.worldToArray (hit.position).z];

		if (curNode.connectedA)
		{
			NodeManager.node nodeA = nodeManager.pipeArray [(int)curNode.openA.x, (int)curNode.openA.y, (int)curNode.openA.z];
			nodeA.connectedA = false;
		}
		if (curNode.connectedB)
		{
			NodeManager.node nodeB = nodeManager.pipeArray [(int)curNode.openB.x, (int)curNode.openB.y, (int)curNode.openB.z];
			nodeB.connectedB = false;
		}
		curNode = null;
		Destroy (hit.gameObject);
		GameObject ground = Instantiate (modelManager.clearGround, hit.position, Quaternion.Euler (0, 0, 0));

        groundManager.insertGround(hit,ground);
	
	}
		

}