using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabManager : MonoBehaviour
{

    public List<CrabController> crabs = new List<CrabController>();

    public List<Transform> endPoints = new List<Transform>();

    //executes crabs traversals one at a time as to not have them conflict with one another
    public void Update()
    {
        foreach (CrabController crab in crabs)
        {
            endPoints.Add(crab.CrabTraverse(endPoints));
        }
        endPoints.Clear();
    }
}
