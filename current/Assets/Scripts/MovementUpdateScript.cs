using UnityEngine;
using System.Collections;




/// <summary>
/// This Script is attached to the player and ensures that palyers rotation, position and scale are kept upt to date across the network
/// </summary>

public class MovementUpdateScript : MonoBehaviour {
	private Vector3 lastPosition;
	private Quaternion lastRotation;
	private Transform myTransform;
	
	void Start () {
		if(networkView.isMine == true)
		{
	      myTransform = transform;
			//ensure that everyone sees the player at the correct location the moment they spawn
			
			networkView.RPC("updateMovement", RPCMode.OthersBuffered, 
				             myTransform.position, myTransform.rotation);
		}
		else { enabled = false; }
	}
	
	void Update () {
		//if the player has moved at all then fire off an RPC to update the players pos/rot across the network
		if(Vector3.Distance(myTransform.position, lastPosition) >= 0.1)
		{
		 lastPosition = myTransform.position;	
			networkView.RPC("updateMovement", RPCMode.OthersBuffered, 
				             myTransform.position, myTransform.rotation);
		}
		if(Quaternion.Angle(myTransform.rotation, lastRotation) >= 1)
		{
		  lastRotation = myTransform.rotation;
			networkView.RPC("updateMovement", RPCMode.OthersBuffered, 
				             myTransform.position, myTransform.rotation);
		}
		
	
	}
	
	[RPC]
	void updateMovement (Vector3 newPosition, Quaternion newRotation)
	{
	  transform.position = newPosition;
	  transform.rotation = newRotation;
	}
}

