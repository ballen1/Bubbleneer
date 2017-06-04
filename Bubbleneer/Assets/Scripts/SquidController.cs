using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidController : MonoBehaviour
{

    public NodeManager nodeManager;


    public List<Vector3> pipes = new List<Vector3>();

    public Transform startPoint;
    public Transform endPoint;

    private int index = 0;

    private float lerpTime = 0;

    public float squidSpeed = 1.02f;

    private void Start()
    {
        nodeManager = GameObject.Find("GameManager").GetComponent<NodeManager>();

        startPoint = this.transform;
        endPoint = nodeManager.pipeArray[(int)pipes[index].x, (int)pipes[index].y, (int)pipes[index].z].pipe.transform.Find("Lerp Point").transform;
    }

    void Update()
    {
        //look at the end point
        if (index != 0 && index != 1)
        {
            transform.LookAt(endPoint);
            transform.Rotate(90, 0, 0);
        }

        lerpTime += squidSpeed * Time.deltaTime;

        //lerp through the pipe
        if (lerpTime < 1)
        {
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, lerpTime);
        }
        //find next lerp point if it exists
        else if (lerpTime >= 1)
        {
            index++;
            if (index == pipes.Count)
            {
                startPoint = endPoint;
                endPoint = nodeManager.pipeArray[(int)pipes[index - 1].x, (int)pipes[index - 1].y, (int)pipes[index - 1].z].pipe.transform.Find("End Point").transform;
                lerpTime = 0;
            }
            else if (index < pipes.Count)
            {
                startPoint = endPoint;
                endPoint = nodeManager.pipeArray[(int)pipes[index].x, (int)pipes[index].y, (int)pipes[index].z].pipe.transform.Find("Lerp Point").transform;
                lerpTime = 0;
            }
            //destroy self at the end of a pipe
            else if (index > pipes.Count)
            {
                Destroy(gameObject);
            }
        }
    }

    //destroy any bubble encountered
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Bubble")
        {
            Destroy(col.gameObject);
        }
    }
}
