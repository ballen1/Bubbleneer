using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour
{
    public int crabType = 1;
    private GroundManager groundManager;
    private CrabManager crabManager;

    private List<GroundManager.groundNode> visited = new List<GroundManager.groundNode>();
    private Queue<GroundManager.groundNode> nodes = new Queue<GroundManager.groundNode>();

    private List<GroundManager.groundNode> traversal = new List<GroundManager.groundNode>();

    public GameObject parentSquare;

    GroundManager.groundNode root = new GroundManager.groundNode();
    GroundManager.groundNode nextNode = new GroundManager.groundNode();

    public GroundManager.groundNode[,,] groundArrayCopy;

    public Transform startPoint;
    public Transform endPoint;

    private float lerpTime = 0;

    public float crabSpeed = 1.0f;

    public bool isLerping = false;

    public bool onBait = false;

    public int sightRange = 3;

    public void Start()
    {
        groundArrayCopy = new GroundManager.groundNode[32, 32, 32];

        groundManager = GameObject.Find("GameManager").GetComponent<GroundManager>();
        crabManager = GameObject.Find("GameManager").GetComponent<CrabManager>();

        //get a copy of ground array for each crab
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                for (int k = 0; k < 32; k++)
                {
                    groundArrayCopy[i, j, k] = groundManager.groundArray[i, j, k];
                }
            }
        }
        //addthis crab to the queue of crabs
        crabManager.crabs.Add(this);

        //mark current square as occupied
        Vector3 arrayPos = groundManager.worldToArray(parentSquare.transform.position);
        root = groundArrayCopy[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z];
        root.square.tag = "Occupied";
    }

    public Transform CrabTraverse(List<Transform> priorityTrans)
    {
        //reset parents in ground array for next crabs search
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                for (int k = 0; k < 32; k++)
                {
                    groundArrayCopy[i, j, k] = groundManager.groundArray[i, j, k];
                    groundArrayCopy[i, j, k].parent = null;
                }
            }
        }

        //search for bait if not moving or on bait
        if (!isLerping && !onBait)
        {
            //prepare for BFS
            visited.Clear();
            nodes.Clear();
            traversal.Clear();
            visited.Add(root);
            nodes.Enqueue(root);
      
            //perfrom BFS
            GroundManager.groundNode goal = new GroundManager.groundNode();
            goal = BFS();

            //if bait is found
            if (goal != null)
            {
                //build path to bait
                while (goal.parent != null)
                {
                    traversal.Insert(0, goal);
                    goal = goal.parent;
                }
                
                //set up start and end nodes
                if (traversal.Count > 0)
                {
                    nextNode = traversal[0];
                    startPoint = this.transform;
                    endPoint = nextNode.lerpNode.transform;
                }

                //check if next square is open
                if(!priorityTrans.Contains(endPoint) && nextNode.square.tag != "Occupied")
                {
                    //check if next square is bait and claim it
                    if (nextNode.isBait)
                    {
                        nextNode.isBait = false;
                        onBait = true;
                        groundManager.removeGround(nextNode.square.transform);
                    }
                    isLerping = true;
                }

                return endPoint;
            }
        }
        return null;
    }

    public void Update()
    {
        
        if (isLerping)
        {
            root.square.tag = "Occupied";
            nextNode.square.tag = "Occupied";

            transform.LookAt(endPoint);

            lerpTime += crabSpeed * Time.deltaTime;

            if (lerpTime < 1)
            {
                transform.position = Vector3.Lerp(startPoint.position, endPoint.position, lerpTime);
            }
            else
            {
                root.square.tag = "Buildable";
                Vector3 curPos = groundManager.worldToArray(new Vector3(nextNode.squarePosition.x, nextNode.squarePosition.y, nextNode.squarePosition.z));
                root = groundArrayCopy[(int)curPos.x, (int)curPos.y, (int)curPos.z];
                lerpTime = 0;
                isLerping = false;
            }
        }
    }

    //performs a breadth first search for bait, breaking only if bait is found or after i levels of the search tree
    public GroundManager.groundNode BFS()
    {
        GroundManager.groundNode current = new GroundManager.groundNode();
        GroundManager.groundNode n = new GroundManager.groundNode();
        int i = -1;
        Queue<int> indices = new Queue<int>();
        while (nodes.Count > 0 && i < sightRange)
        {
           
            current = nodes.Dequeue();

            //Debug.Log(i + " : " + current.square.transform.position);

            if (current.isBait)
            {   
                return current;
            }
            if (current.hasNeighbourN)
            {
                n = groundArrayCopy[(int)current.neighbourN.x, (int)current.neighbourN.y, (int)current.neighbourN.z];

                if (!visited.Contains(n))
                {
                    indices.Enqueue(i + 1);
                    visited.Add(n);
                    n.parent = current;
                    nodes.Enqueue(n);
                }
            }
            if (current.hasNeighbourS)
            {
                n = groundArrayCopy[(int)current.neighbourS.x, (int)current.neighbourS.y, (int)current.neighbourS.z];

                if (!visited.Contains(n))
                {
                    indices.Enqueue(i + 1);
                    visited.Add(n);
                    n.parent = current;
                    nodes.Enqueue(n);
                }
            }
            if (current.hasNeighbourE)
            {
                n = groundArrayCopy[(int)current.neighbourE.x, (int)current.neighbourE.y, (int)current.neighbourE.z];

                if (!visited.Contains(n))
                {
                    indices.Enqueue(i + 1);
                    visited.Add(n);
                    n.parent = current;
                    nodes.Enqueue(n);
                }
            }
            if (current.hasNeighbourW)
            {
                n = groundArrayCopy[(int)current.neighbourW.x, (int)current.neighbourW.y, (int)current.neighbourW.z];

                if (!visited.Contains(n))
                {
                    indices.Enqueue(i + 1);
                    visited.Add(n);
                    n.parent = current;
                    nodes.Enqueue(n);
                }
            }
            if (indices.Count > 0)
            {
                i = indices.Dequeue();
            }
        }
        return null;
    }
}
