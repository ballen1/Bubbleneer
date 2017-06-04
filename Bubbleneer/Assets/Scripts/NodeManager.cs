using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    //class for store information about pipes on the map
    public class node
    {
        public string pieceName { get; set; }
        public GameObject pipe { get; set; }
        public Vector3 openA { get; set; }
        public Vector3 openB { get; set; }
        public bool connectedA { get; set; }
        public bool connectedB { get; set; }
    }

    enum terminalA { N = 1, S = -1 }; //maps to z
    enum terminalB { E = 1, W = -1 }; //maps to x

    enum curveA { N = 1, S = -1, E = 1, W = -1 }; //maps to z
    enum curveB { N = -1, S = 1, E = 1, W = -1 }; //maps to x

    enum straightA { N = 1, S = 1 }; //maps to z
    enum straightB { E = 1, W = 1 }; //maps to x

    public node[,,] pipeArray;

    //list of complete pipe runs
    public List<List<Vector3>> completeRuns = new List<List<Vector3>>();

    void Awake()
    {
        pipeArray = new node[32, 32, 32];
    }

    //function to add pipes into the 3D array
    public void insertPipe(GameObject piece, string type, string direction)
    {
        node newPipe = new node();
        newPipe.pipe = piece;

        if(type.Equals("curve"))
        {
            type = "C";
        }
        else if(type.Equals("straight"))
        {
            type = "D";
        }

        switch (type)
        {
            case "A":
   
                newPipe.pieceName = "pumpStation";
                if (Enum.IsDefined(typeof(terminalA), direction))
                {
                    Vector3 openA = worldToArray(piece.transform.position);
                    int modifierA = (int)Enum.Parse(typeof(terminalA), direction);
                    newPipe.openA = new Vector3(openA.x, openA.y, openA.z + modifierA);
                    newPipe.connectedA = isConnected(worldToArray(piece.transform.position), newPipe.openA);
                    newPipe.openB = new Vector3(-1, -1, -1);
                    newPipe.connectedB = false;
                }
                else
                {
                    Vector3 openB = worldToArray(piece.transform.position);
                    int modifierB = (int)Enum.Parse(typeof(terminalB), direction);
                    newPipe.openB = new Vector3(openB.x + modifierB, openB.y, openB.z);
                    newPipe.connectedA = isConnected(worldToArray(piece.transform.position), newPipe.openB);
                    newPipe.openA = new Vector3(-1, -1, -1);
                    newPipe.connectedA = false;
                }
                break;
            case "B":
                newPipe.pieceName = "pumpTerminal";
                if (Enum.IsDefined(typeof(terminalA), direction))
                {
                    Vector3 openA = worldToArray(piece.transform.position);
                    int modifierA = (int)Enum.Parse(typeof(terminalA), direction);
                    newPipe.openA = new Vector3(openA.x, openA.y, openA.z + modifierA);
                    newPipe.connectedA = isConnected(worldToArray(piece.transform.position), newPipe.openA);
                    newPipe.openB = new Vector3(-1, -1, -1);
                    newPipe.connectedB = false;
                }
                else
                {
                    Vector3 openB = worldToArray(piece.transform.position);
                    int modifierB = (int)Enum.Parse(typeof(terminalB), direction);
                    newPipe.openB = new Vector3(openB.x + modifierB, openB.y, openB.z);
                    newPipe.connectedB = isConnected(worldToArray(piece.transform.position), newPipe.openB);
                    newPipe.openA = new Vector3(-1, -1, -1);
                    newPipe.connectedA = false;
                }
                break;
            case "C":
                newPipe.pieceName = "curve";
                if (Enum.IsDefined(typeof(curveA), direction))
                {
                    Vector3 openA = worldToArray(piece.transform.position);
                    Vector3 openB = worldToArray(piece.transform.position);
                    int modifierA = (int)Enum.Parse(typeof(curveA), direction);
                    newPipe.openA = new Vector3(openA.x, openA.y, openA.z + modifierA);
                    int modifierB = (int)Enum.Parse(typeof(curveB), direction);
                    newPipe.openB = new Vector3(openB.x + modifierB, openB.y, openB.z);
                    newPipe.connectedA = isConnected(worldToArray(piece.transform.position), newPipe.openA);
                    newPipe.connectedB = isConnected(worldToArray(piece.transform.position), newPipe.openB);
                }
                break;
            case "D":
                newPipe.pieceName = "straight";
                if (Enum.IsDefined(typeof(straightA), direction))
                {
                    Vector3 openA = worldToArray(piece.transform.position);
                    int modifierA = (int)Enum.Parse(typeof(straightA), direction);
                    newPipe.openA = new Vector3(openA.x, openA.y, openA.z + modifierA);
                    newPipe.connectedA = isConnected(worldToArray(piece.transform.position), newPipe.openA);

                    Vector3 openB = worldToArray(piece.transform.position);
                    int modifierB = (int)Enum.Parse(typeof(straightA), direction);
                    newPipe.openB = new Vector3(openB.x, openB.y, openB.z - modifierB);
                    newPipe.connectedB = isConnected(worldToArray(piece.transform.position), newPipe.openB);
                }
                else
                {
                    Vector3 openA = worldToArray(piece.transform.position);
                    int modifierA = (int)Enum.Parse(typeof(straightB), direction);
                    newPipe.openA = new Vector3(openA.x + modifierA, openA.y, openA.z);
                    newPipe.connectedA = isConnected(worldToArray(piece.transform.position), newPipe.openA);

                    Vector3 openB = worldToArray(piece.transform.position);
                    int modifierB = (int)Enum.Parse(typeof(straightB), direction);
                    newPipe.openB = new Vector3(openB.x - modifierB, openB.y, openB.z);
                    newPipe.connectedB = isConnected(worldToArray(piece.transform.position), newPipe.openB);
                }
                break;
            default:
                break;
        }
        Vector3 arrayPos = worldToArray(piece.transform.position);
        pipeArray[(int)arrayPos.x, (int)arrayPos.y, (int)arrayPos.z] = newPipe;
    }

    //help function to check if two pipes are connected
    public bool isConnected(Vector3 pipePos, Vector3 mouthPos)
    {
        node testNode = null;
        if (mouthPos.x >= 0 && mouthPos.x <= pipeArray.GetLength(0) &&
            mouthPos.y >= 0 && mouthPos.z <= pipeArray.GetLength(1) &&
            mouthPos.z >= 0 && mouthPos.z <= pipeArray.GetLength(2) )
        {
            testNode = pipeArray[(int)mouthPos.x, (int)mouthPos.y, (int)mouthPos.z];
        }

        if (testNode != null)
        {
            if (testNode.openA == pipePos)
            {
                testNode.connectedA = true;
                return true;
            }
            else if (testNode.openB == pipePos)
            {
                testNode.connectedB = true;
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

    //perfroms a traversal from each pump terminal to check if there is a complete run
    public bool isComplete(Vector3 startPos, List<Vector3>visited)
    {

        visited.Add(startPos);
        node curNode = pipeArray[(int)startPos.x, (int)startPos.y, (int)startPos.z];

        if (curNode.connectedA)
        {
            node nextA = pipeArray[(int)curNode.openA.x, (int)curNode.openA.y, (int)curNode.openA.z];
            if (!visited.Contains(nextA.openA) && nextA.openA != new Vector3(-1,-1,-1) && nextA.connectedA || !visited.Contains(nextA.openB) && nextA.openB != new Vector3(-1, -1, -1) && nextA.connectedB)
            {
                return isComplete(new Vector3((int)curNode.openA.x, (int)curNode.openA.y, (int)curNode.openA.z),visited);
            }
            else if (nextA.pieceName.Equals("pumpTerminal"))
            {
                visited.Add(new Vector3((int)curNode.openA.x, (int)curNode.openA.y, (int)curNode.openA.z));
                return true;
            }
        }

        if (curNode.connectedB)
        {
            node nextB = pipeArray[(int)curNode.openB.x, (int)curNode.openB.y, (int)curNode.openB.z];
            if (!visited.Contains(nextB.openA) && nextB.openA != new Vector3(-1, -1, -1) && nextB.connectedA || !visited.Contains(nextB.openB) && nextB.openB != new Vector3(-1, -1, -1) && nextB.connectedB)
            {
                return isComplete(new Vector3((int)curNode.openB.x, (int)curNode.openB.y, (int)curNode.openB.z),visited);
            }
            else if (nextB.pieceName.Equals("pumpTerminal"))
            {
                visited.Add(new Vector3((int)curNode.openB.x, (int)curNode.openB.y, (int)curNode.openB.z));
                return true;
            }
        }

        return false;
    }

    //converts world coordinates to array coordinates
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

    //converts array coordinates to world coordinates
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
