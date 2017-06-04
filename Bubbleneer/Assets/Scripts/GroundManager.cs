using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{

    public class groundNode
    {
        public groundNode parent { get; set; }
        public bool active { get; set; }
        public bool isBait { get; set; }
        public Vector3 squarePosition { get; set; }
        public GameObject square { get; set; }
        public GameObject lerpNode { get; set; }
        public bool hasNeighbourN { get; set; }
        public bool hasNeighbourS { get; set; }
        public bool hasNeighbourE { get; set; }
        public bool hasNeighbourW { get; set; }
        public Vector3 neighbourN { get; set; }
        public Vector3 neighbourS { get; set; }
        public Vector3 neighbourE { get; set; }
        public Vector3 neighbourW { get; set; }
    }

    public groundNode[,,] groundArray;

    private groundNode blankNode;

    private void Awake()
    {
        groundArray = new groundNode[32, 32, 32];

        blankNode = new groundNode();
        blankNode.parent = null;
        blankNode.active = false;
        blankNode.isBait = false;
        blankNode.squarePosition = new Vector3(-1, -1, -1);
        blankNode.hasNeighbourN = false;
        blankNode.hasNeighbourS = false;
        blankNode.hasNeighbourE = false;
        blankNode.hasNeighbourW = false;
        blankNode.neighbourN = new Vector3(-1, -1, -1);
        blankNode.neighbourS = new Vector3(-1, -1, -1);
        blankNode.neighbourE = new Vector3(-1, -1, -1);
        blankNode.neighbourW = new Vector3(-1, -1, -1);

        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                for (int k = 0; k < 32; k++)
                {
                    groundArray[i, j, k] = blankNode;
                }
            }
        }
    }

    //inserts a ground piece into the 3D array of ground pieces
    public void insertGround(Transform pos, GameObject groundPiece)
    {
        Vector3 arrayPos = worldToArray(pos.position);

        groundNode newNode = new groundNode();

        newNode.parent = null;
        newNode.active = true;
        newNode.isBait = false;

        newNode.square = groundPiece;
        newNode.squarePosition = pos.position;
        newNode.lerpNode = groundPiece.transform.FindChild("Lerp Point").gameObject;

        newNode.hasNeighbourN = isConnected(arrayPos, new Vector3(arrayPos.x, arrayPos.y, arrayPos.z + 1));
        newNode.hasNeighbourS = isConnected(arrayPos, new Vector3(arrayPos.x, arrayPos.y, arrayPos.z - 1));
        newNode.hasNeighbourE = isConnected(arrayPos, new Vector3(arrayPos.x + 1, arrayPos.y, arrayPos.z));
        newNode.hasNeighbourW = isConnected(arrayPos, new Vector3(arrayPos.x - 1, arrayPos.y, arrayPos.z));

        if(newNode.hasNeighbourN)
        {
            newNode.neighbourN = new Vector3(arrayPos.x, arrayPos.y, arrayPos.z + 1);
            groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z + 1].neighbourS = arrayPos;
            groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z + 1].hasNeighbourS = true;
        }
        if (newNode.hasNeighbourS)
        {
            newNode.neighbourS = new Vector3(arrayPos.x, arrayPos.y, arrayPos.z- 1);
            groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z - 1].neighbourN = arrayPos;
            groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z - 1].hasNeighbourN = true;
        }
        if (newNode.hasNeighbourE)
        {
            newNode.neighbourE = new Vector3(arrayPos.x + 1, arrayPos.y, arrayPos.z);
            groundArray[(int)arrayPos.x + 1, (int)arrayPos.y, (int)arrayPos.z].neighbourW = arrayPos;
            groundArray[(int)arrayPos.x + 1, (int)arrayPos.y, (int)arrayPos.z].hasNeighbourW = true;
        }
        if (newNode.hasNeighbourW)
        {
            newNode.neighbourW = new Vector3(arrayPos.x - 1, arrayPos.y, arrayPos.z);
            groundArray[(int)arrayPos.x - 1, (int)arrayPos.y, (int)arrayPos.z].neighbourE = arrayPos;
            groundArray[(int)arrayPos.x - 1, (int)arrayPos.y, (int)arrayPos.z].hasNeighbourE = true;
        }

        groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z] = newNode;
    }

    //checks if there exists a neighbor for a given ground piece
    public bool isConnected(Vector3 groundPos, Vector3 neighbourPos)
    {
        groundNode testNode = null;
        if (neighbourPos.x >= 0 && neighbourPos.x <= groundArray.GetLength(0) &&
    neighbourPos.y >= 0 && neighbourPos.z <= groundArray.GetLength(1) &&
    neighbourPos.z >= 0 && neighbourPos.z <= groundArray.GetLength(2))
        {
            testNode = groundArray[(int)neighbourPos.x, (int)neighbourPos.y, (int)neighbourPos.z];
        }
        if (testNode != null)
        {
            if (testNode.active)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    //removes a ground piece from the array
    public void removeGround(Transform pos)
    {
        Vector3 arrayPos = worldToArray(pos.position);

        groundNode curNode = new groundNode();

        curNode = groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z];

        if(curNode.hasNeighbourN)
        {
            groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z + 1].hasNeighbourS = false;
            groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z + 1].neighbourS = new Vector3(-1,-1,-1);
        }
        if (curNode.hasNeighbourS)
        {
            groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z - 1].hasNeighbourN = false;
            groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z - 1].neighbourN = new Vector3(-1, -1, -1);
        }
        if (curNode.hasNeighbourE)
        {
            groundArray[(int)arrayPos.x + 1, (int)arrayPos.y, (int)arrayPos.z].hasNeighbourW = false;
            groundArray[(int)arrayPos.x + 1, (int)arrayPos.y, (int)arrayPos.z].neighbourW = new Vector3(-1, -1, -1);
        }
        if (curNode.hasNeighbourW)
        {
            groundArray[(int)arrayPos.x - 1, (int)arrayPos.y, (int)arrayPos.z].hasNeighbourE = false;
            groundArray[(int)arrayPos.x - 1, (int)arrayPos.y, (int)arrayPos.z].neighbourE = new Vector3(-1, -1, -1);
        }

        groundArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z] = blankNode;
    }

    public Vector3 worldToArray(Vector3 position)
    {
        Vector3 arrayIndex;

        if (position.y == 0)
        {
            arrayIndex = new Vector3(position.x / 3.2f, position.y / 3.2f, position.z / 3.2f);
        }
        else
        {
            arrayIndex = new Vector3(position.x / 3.2f, position.y / 3, position.z / 3.2f);
        }

        return arrayIndex;
    }

    public Vector3 arrayToWorld(Vector3 position)
    {
        Vector3 worldPos;

        if (position.y == 0)
        {
            worldPos = new Vector3(position.x * 3.2f, position.y * 3.2f, position.z * 3.2f);
        }
        else
        {
            worldPos = new Vector3(position.x * 3.2f, position.y * 3, position.z * 3.2f);
        }

        return worldPos;
    }
}
