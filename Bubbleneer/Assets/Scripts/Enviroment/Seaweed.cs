using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seaweed : MonoBehaviour {

    public MeshRenderer[] seaweed;

    public float changeCoolDown = 1.5f;

    private float changeTimer = 0;
	
    public int activeIndex = 0;
    public int prevIndex = 0;

    private void Start()
    {
        activeIndex = Random.Range(0,4);
    }

    void Update ()
    {
        changeTimer += Time.deltaTime;

        if(changeTimer >= changeCoolDown)
        {
            changeTimer = 0;
            prevIndex = activeIndex;
            activeIndex += 1;
            activeIndex %= 4;
            seaweed[activeIndex].enabled = true;
            seaweed[prevIndex].enabled = false;
        }
	}
}
