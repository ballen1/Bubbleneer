using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{

    private NodeManager nodeManager;
	private RoundSystem roundManager;

    public List<Vector3> pipes = new List<Vector3>();

    public Transform startPoint;
    public Transform endPoint;

    private int index = 0;

    private float lerpTime = 0;

    private void Start()
    {
		GameObject GM = GameObject.FindGameObjectWithTag ("GameManager");
		nodeManager = GM.GetComponent<NodeManager> ();
		roundManager = GM.GetComponent<RoundSystem> ();

        //gets the first two lerp points
        startPoint = this.transform;
        endPoint = nodeManager.pipeArray[(int)pipes[index].x, (int)pipes[index].y, (int)pipes[index].z].pipe.transform.Find("Lerp Point").transform;
    }

    void Update()
    {
        lerpTime += Time.deltaTime;

        //lerp to next position
        if (lerpTime < 1)
        {
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, lerpTime);
        }

        //get next nodes if they exist
        else if (lerpTime >= 1)
        {
            index++;
            if (index == pipes.Count)
            {
                startPoint = endPoint;
                endPoint = nodeManager.pipeArray[(int)pipes[index-1].x, (int)pipes[index-1].y, (int)pipes[index-1].z].pipe.transform.Find("End Point").transform;
                lerpTime = 0;
            }
            else if (index < pipes.Count)
            {
                startPoint = endPoint;
                endPoint = nodeManager.pipeArray[(int)pipes[index].x, (int)pipes[index].y, (int)pipes[index].z].pipe.transform.Find("Lerp Point").transform;
                lerpTime = 0;
            }

            //add to score and destory bubble
            else if(index > pipes.Count)
            {
				roundManager.AddBubbleScore (endPoint);
                Destroy(gameObject);
            }
        }
    }
}
