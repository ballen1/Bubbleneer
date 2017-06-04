using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidDropController : MonoBehaviour
{
    public bool isFinalSquid;
    public float amplitude = 1;
    public float frequency = 1;
    public float hoverSpeed = 1;

    public TestConnection pumpStation;

    public RoundSystem roundSystem;

    public float dropMin = 1;
    public float dropMax = 10;

    public float dropTimer = 0;
    public bool drop = false;
    public float dropValue;

    public bool rise = true;
    public Transform lerpPoint;
    private Transform startPoint;
    public float lerpTime;

    public bool foundBait = false;
    public GroundManager.groundNode baitNode;

    private float x = 0;

    private NodeManager nodeManager;
    public GroundManager groundManager;
    Vector3 arrayPos;
    private void Start()
    {

        groundManager = GameObject.Find("GameManager").GetComponent<GroundManager>();

        startPoint = this.transform;
        nodeManager = GameObject.Find("GameManager").GetComponent<NodeManager>();
        roundSystem = GameObject.Find("GameManager").GetComponent<RoundSystem>();

        arrayPos = nodeManager.worldToArray(this.transform.parent.position + new Vector3(0, 3, 0));
        checkPipe(arrayPos);

        dropValue = Random.Range(dropMin, dropMax);
        x = Random.Range(1.0f, 5.0f);
    }

    void Update()
    {
        //rise if simulation has started
        if (roundSystem.IsSimulationStarted())
        {
            if (rise)
            {
                lerpTime += 0.01f * Time.deltaTime;

                if (lerpTime < 0.035f)
                {
                    transform.position = Vector3.Lerp(startPoint.position, lerpPoint.position, lerpTime);
                }
                else
                {
                    baitNode = checkBait(arrayPos);
                    lerpTime = 0;
                    startPoint = this.transform;
                    rise = false;
                }
            }
            //move to bait if found
            else
            {
                if (foundBait)
                {
                    baitNode.square.tag = "Occupied";

                    lerpTime += 0.01f * Time.deltaTime;

                    if (lerpTime < 0.04f)
                    {
                        transform.position = Vector3.Lerp(startPoint.position, baitNode.lerpNode.transform.position, lerpTime);
                    }
                }
                //drop into pipe
                else
                {
                    dropTimer += Time.deltaTime;
                    if (dropTimer >= dropValue)
                    {
                        dropTimer = 0;
                        drop = true;
                    }
                }
            }
            //if hover until told to drop
            if (!drop)
            {
                transform.Translate(new Vector3(0, (Mathf.Sin(x) * amplitude) / frequency, 0) * Time.deltaTime);
                x += hoverSpeed;
            }
            else if (drop && dropTimer >= 5)
            {
                pumpStation.spawnSquid = true;
                Destroy(gameObject);
            }
            else
            {
                transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime);
            }
        }
    }

    //check for bait in all adjacent square and return their node if found
    public GroundManager.groundNode checkBait(Vector3 arrayPos)
    {
        if (groundManager.groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z + 1].isBait == true)
        {
            foundBait = true;
            if (isFinalSquid)
            {
                groundManager.groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z + 1].isBait = false;
            }
            return groundManager.groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z + 1];
        }
        if (groundManager.groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z - 1].isBait == true)
        {
            foundBait = true;
            if (isFinalSquid)
            {
                groundManager.groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z - 1].isBait = false;
            }
            return groundManager.groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z - 1];
        }
        else if (groundManager.groundArray[(int)arrayPos.x + 1, (int)arrayPos.y, (int)arrayPos.z].isBait == true)
        {
            foundBait = true;
            if (isFinalSquid)
            {
                groundManager.groundArray[(int)arrayPos.x + 1, (int)arrayPos.y, (int)arrayPos.z].isBait = false;
            }
            return groundManager.groundArray[(int)arrayPos.x + 1, (int)arrayPos.y, (int)arrayPos.z];
        }
        else if (groundManager.groundArray[(int)arrayPos.x - 1, (int)arrayPos.y, (int)arrayPos.z].isBait == true)
        {
            foundBait = true;
            if (isFinalSquid)
            {
                groundManager.groundArray[(int)arrayPos.x - 1, (int)arrayPos.y, (int)arrayPos.z].isBait = false;
            }
            return groundManager.groundArray[(int)arrayPos.x - 1, (int)arrayPos.y, (int)arrayPos.z];
        }
        return null;
    }

    //look for a terminal in all adjacent squares
    public void checkPipe(Vector3 arrayPos)
    {
        if (nodeManager.pipeArray[(int)arrayPos.x + 1, (int)arrayPos.y, (int)arrayPos.z] != null && nodeManager.pipeArray[(int)arrayPos.x + 1, (int)arrayPos.y, (int)arrayPos.z].pieceName.Equals("pumpStation"))
        {
            pumpStation = nodeManager.pipeArray[(int)arrayPos.x + 1, (int)arrayPos.y, (int)arrayPos.z].pipe.GetComponent<TestConnection>();
        }
        else if (nodeManager.pipeArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z + 1] != null && nodeManager.pipeArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z + 1].pieceName.Equals("pumpStation"))
        {
            pumpStation = nodeManager.pipeArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z + 1].pipe.GetComponent<TestConnection>();
        }
        else if (nodeManager.pipeArray[(int)arrayPos.x - 1, (int)arrayPos.y, (int)arrayPos.z] != null && nodeManager.pipeArray[(int)arrayPos.x - 1, (int)arrayPos.y, (int)arrayPos.z].pieceName.Equals("pumpStation"))
        {
            pumpStation = nodeManager.pipeArray[(int)arrayPos.x - 1, (int)arrayPos.y, (int)arrayPos.z].pipe.GetComponent<TestConnection>();
        }
        else if (nodeManager.pipeArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z - 1] != null && nodeManager.pipeArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z - 1].pieceName.Equals("pumpStation"))
        {
            pumpStation = nodeManager.pipeArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z - 1].pipe.GetComponent<TestConnection>();
        }
    }
}