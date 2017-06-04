using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
	private NodeManager nodeManager;
    private GroundManager groundManager;
	private ModelManager modelManager;

	void Start()
	{
		modelManager = gameObject.GetComponent<ModelManager> ();
        groundManager = gameObject.GetComponent<GroundManager>();
    }

	/// <summary>
	/// The player has toggled on the ability to drop bait
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	public void spawnBait (Transform hit)
	{
		// + Destory the gameobject below when the pipe is being built
		Destroy (hit.gameObject);

		// + Put the pipe down in the same place as the destory piece of land
		GameObject bait = Instantiate(modelManager.getCurrentModel(), hit.position, Quaternion.Euler (0, 0, 0));
        groundManager.removeGround(hit);
        groundManager.insertGround(hit, bait);
        Vector3 arrayPos = groundManager.worldToArray(hit.position);
        groundManager.groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z].isBait = true;
    }

	public void trimSeaweed(Transform hit)
	{
		Destroy (hit.gameObject);
		GameObject ground = Instantiate(modelManager.clearGround, hit.position, Quaternion.Euler (0, 0, 0));
        groundManager.insertGround(hit, ground);
    }
		
}
