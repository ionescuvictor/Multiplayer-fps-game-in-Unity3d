using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	//Variables Start___________________________________
	
	private Camera myCamera;
	
	private Transform cameraHeadTransform;
	
	//Variables End_____________________________________
	
	void Start () 
	{
		if(networkView.isMine == true)
		{
		myCamera = Camera.main;
		
		cameraHeadTransform = transform.FindChild("Socket");
		}
		else
		{
			enabled = false;
		}
	}
	
	void Update () 
	{
		//Make the camera follow the player's cameraHeadTransform.
		myCamera.transform.position = cameraHeadTransform.position;
		myCamera.transform.rotation = cameraHeadTransform.rotation;
	}
}
