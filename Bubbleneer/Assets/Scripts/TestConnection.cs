using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConnection : MonoBehaviour
{

    public NodeManager nodeManager;

	private RoundSystem roundManager;

    public GameObject bubblePrefab;
    public GameObject squidPrefab;

    public float spawnRate;

    private float spawnTimer;

    public Transform spawnPoint;

    public bool spawnSquid = false;

    private void Start()
    {
        spawnTimer = spawnRate;
        nodeManager = GameObject.Find("GameManager").GetComponent<NodeManager>();
		roundManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<RoundSystem> ();
    }

    void Update()
    {
        //check if the round has started
		if (roundManager.IsSimulationStarted ()) {
			List<Vector3> visited = new List<Vector3> ();
			bool test = nodeManager.isComplete (nodeManager.worldToArray (transform.position), visited);

            //spawn a bubble or squid at a fixed rate (squids spawn if a boolean is flipped)
			spawnTimer -= Time.deltaTime;
			if (test && spawnTimer <= 0) {
				spawnTimer = spawnRate;
				if (!spawnSquid) {
					GameObject newBubble = Instantiate (bubblePrefab, spawnPoint.position, Quaternion.identity);
					newBubble.GetComponent<BubbleController> ().pipes = visited;
				} else {
					GameObject newSquid = Instantiate (squidPrefab, spawnPoint.position, Quaternion.Euler (90, 180, 0));
					newSquid.GetComponent<SquidController> ().pipes = visited;
					spawnSquid = false;
				}
			}
		}
    }
}
