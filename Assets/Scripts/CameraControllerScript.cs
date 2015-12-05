using UnityEngine;

public class CameraControllerScript : MonoBehaviour {

	float minCamX;
	float maxCamX;
	float minCamY;
	float maxCamY;
	public float minMapX = -38f;
	public float maxMapX = 38f;
	public float minMapY = -2.6f;
	public float maxMapY = 11f;

	public Camera mainCamera;
	public float camSpeed = 10f;
	public float minZoom = 2.4f;
	public float maxZoom = 10f;

	void Start()
	{
		//Maximum zoom should be the minimum value between half the X and Y amplitude of your map or a smaller value
		maxZoom = Mathf.Min((Mathf.Abs(minMapX) + Mathf.Abs(maxMapX))/2,
		                    (Mathf.Abs(minMapY) + Mathf.Abs(maxMapY))/2,
		                    maxZoom);

		float vertExtent = mainCamera.orthographicSize; // Half the size the camera sees vertically
		float horzExtent = vertExtent * Screen.width / Screen.height; // Half the size the camera sees horizontally

		// Calculate camera bounds
		minCamX = minMapX + horzExtent;
		maxCamX = maxMapX - horzExtent;
		minCamY = minMapY + vertExtent;
		maxCamY = maxMapY - vertExtent;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float horizontalMovement = Input.GetAxis ("Horizontal");
		float verticalMovement = Input.GetAxis ("Vertical");

		//Deal with zooming
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			ZoomOrthoCamera(mainCamera.ScreenToWorldPoint(Input.mousePosition), 0.1f);
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			ZoomOrthoCamera(mainCamera.ScreenToWorldPoint(Input.mousePosition), -0.1f);
		}
		else if(Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.KeypadPlus)) //European keyboard
		{
			ZoomOrthoCamera(mainCamera.ScreenToWorldPoint(Input.mousePosition), 0.1f);
		}
		else if(Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
		{
			ZoomOrthoCamera(mainCamera.ScreenToWorldPoint(Input.mousePosition), -0.1f);
		}

		//Stop movement at border
		if (mainCamera.transform.position.y <= minCamY && verticalMovement < 0)
			verticalMovement = 0;
		if (mainCamera.transform.position.y >= maxCamY && verticalMovement > 0)
			verticalMovement = 0;
		if (mainCamera.transform.position.x <= minCamX && horizontalMovement < 0)
			horizontalMovement = 0;
		if (mainCamera.transform.position.x >= maxCamX && horizontalMovement > 0)
			horizontalMovement = 0;
		if(horizontalMovement != 0 || verticalMovement != 0)
		{
			mainCamera.transform.Translate(new Vector3(horizontalMovement * camSpeed * Time.deltaTime,
			                                           verticalMovement * camSpeed * Time.deltaTime,
			                                           0.0f));
		}

		//Don't allow the camera to go outside the border
		if (mainCamera.transform.position.y < minCamY)
			mainCamera.transform.Translate(new Vector3(0.0f,
			                                           minCamY - mainCamera.transform.position.y,
			                                           0.0f));
		if (mainCamera.transform.position.y > maxCamY)
			mainCamera.transform.Translate(new Vector3(0.0f,
			                                           maxCamY - mainCamera.transform.position.y,
			                                           0.0f));

		if (mainCamera.transform.position.x < minCamX)
			mainCamera.transform.Translate(new Vector3(minCamX - mainCamera.transform.position.x,
			                                           0.0f,
			                                           0.0f));
		if (mainCamera.transform.position.x > maxCamX)
			mainCamera.transform.Translate(new Vector3(maxCamX - mainCamera.transform.position.x,
			                                           0.0f,
			                                           0.0f));
	}

	void ZoomOrthoCamera(Vector3 zoomTowards, float amount)
	{
		// Calculate how much we will have to move towards the zoomTowards position
		float multiplier = (1.0f / mainCamera.orthographicSize * amount);
		
		// Zoom camera
		mainCamera.orthographicSize -= amount;

		if(mainCamera.orthographicSize >= minZoom && mainCamera.orthographicSize <= maxZoom)
		{
			float vertExtent = mainCamera.orthographicSize; // Half the size the camera sees vertically
			float horzExtent = vertExtent * Screen.width / Screen.height; // Half the size the camera sees horizontally
			
			// Calculate camera bounds
			minCamX = minMapX + horzExtent;
			maxCamX = maxMapX - horzExtent;
			minCamY = minMapY + vertExtent;
			maxCamY = maxMapY - vertExtent;

			// Move camera
			transform.position += (zoomTowards - transform.position) * multiplier;
		}
		else
		{
			// Limit zoom
			mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minZoom, maxZoom);
		}
	}
}
