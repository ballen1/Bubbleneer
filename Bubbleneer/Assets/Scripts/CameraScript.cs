using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	private Camera Cam;

	public GameObject GM;
	private MapBuilder GM_Map;

    public float scrollSpeed = 1;
    public float scrollMax = 2;
	public float zoomSpeed = 2.0f;
	public float maxZoom = 4.0f;
	public float minZoom = 15.0f;

	public MacroManager macroManager;

    private float offscreenTimer = 0;

    private bool isOffscreen = false;

    private float x;
    private float y;

	private Bounds CameraBoundary;
	public float BoundsExtension = 2.0f;
	public float TopAdjustment = 5.0f;

	void Awake() {
		Cam = GetComponent<Camera> ();
	}

	void Start() {
		GM_Map = GM.GetComponent<MapBuilder> ();
		CalculateCameraBoundary ();
	}

    void Update()
    {
        if(isOffscreen)
        {
            offscreenTimer += Time.deltaTime;
        }

        x = Input.mousePosition.x;
        y = Input.mousePosition.y;

        //scroll up
		if (y >= Screen.height * 0.95 || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * scrollSpeed, Space.World);
            transform.Translate(Vector3.right * Time.deltaTime * scrollSpeed, Space.World);
            isOffscreen = true;
        }
        //scroll down
		else if (y <= Screen.height * 0.05 || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * -scrollSpeed, Space.World);
            transform.Translate(Vector3.right * Time.deltaTime * -scrollSpeed, Space.World);
            isOffscreen = true;
        }
        //scroll right
		if (x >= Screen.width * 0.95 || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * -scrollSpeed, Space.World);
            transform.Translate(Vector3.right * Time.deltaTime * scrollSpeed, Space.World);
            isOffscreen = true;
        }
        //scroll left
		else if (x <= Screen.width * 0.05 || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * scrollSpeed, Space.World);
            transform.Translate(Vector3.right * Time.deltaTime * -scrollSpeed, Space.World);
            isOffscreen = true;
        }

		// Keep the camera in the boundary
		if (!CameraBoundary.Contains (transform.position)) {
			transform.position = CameraBoundary.ClosestPoint (transform.position);
		}
			
		// Zoom In
		if (Input.GetKey(macroManager.ZoomInMacroKey)) {
			ZoomIn ();
		}
		// Zoom Out
		else if (Input.GetKey(macroManager.ZoomOutMacroKey)) {
			ZoomOut ();
		}


        //cursor on screen
        if (y < Screen.height * 0.95 && y > Screen.height * 0.05 && x < Screen.width * 0.95 && x > Screen.width * 0.05)
        {
            isOffscreen = false;
            offscreenTimer = 0;
        }
    }

	public void CalculateCameraBoundary() {

		CameraBoundary = new Bounds(new Vector3 (0 + (GM_Map.GetXDimension ()*MapBuilder.TILE_DIMENSION / 2.0f) - MapBuilder.TILE_DIMENSION/2.0f,
												 0 + (GM_Map.GetYDimension ()*MapBuilder.TILE_DIMENSION / 2.0f),
												 0 + (GM_Map.GetZDimension ()*MapBuilder.TILE_DIMENSION / 2.0f) - MapBuilder.TILE_DIMENSION/2.0f), 
									new Vector3 (GM_Map.GetXDimension()*MapBuilder.TILE_DIMENSION + BoundsExtension,
												 GM_Map.GetYDimension()*MapBuilder.TILE_DIMENSION + BoundsExtension, 
												 GM_Map.GetZDimension()*MapBuilder.TILE_DIMENSION + BoundsExtension));

		CameraBoundary.SetMinMax(CameraBoundary.min,
			new Vector3(CameraBoundary.max.x - TopAdjustment, CameraBoundary.max.y, CameraBoundary.max.z - TopAdjustment));
	}


	public void ZoomOut(){
		Cam.orthographicSize += zoomSpeed * Time.deltaTime;
		Cam.orthographicSize = Mathf.Clamp (Cam.orthographicSize, maxZoom, minZoom);
	}

	public void ZoomIn(){
		Cam.orthographicSize += -zoomSpeed * Time.deltaTime;
		Cam.orthographicSize = Mathf.Clamp (Cam.orthographicSize, maxZoom, minZoom);
	}

	public void ZoomOut(int amount){
		Cam.orthographicSize += amount;
		Cam.orthographicSize = Mathf.Clamp (Cam.orthographicSize, maxZoom, minZoom);
	}

	public void ZoomIn(int amount){
		Cam.orthographicSize += -amount;
		Cam.orthographicSize = Mathf.Clamp (Cam.orthographicSize, maxZoom, minZoom);
	}

}
