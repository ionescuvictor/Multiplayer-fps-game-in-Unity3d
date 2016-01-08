using UnityEngine;
using System.Collections;

public class TestRPC : MonoBehaviour {
	public string fname;
	void Start () {
		fname = "lolo";
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.I))
		{
		  
		  networkView.RPC("test", RPCMode.AllBuffered, "called RPC test"); // i call this RPC on everybodys computer.
		}
		if(Input.GetKeyDown(KeyCode.O))
		{
		  Debug.Log (fname);	
		}
	
	}
	[RPC]
	void test(string name)
	{
	  	print(name);
		fname = "lala"; //fname turns to lala ACROSS the network
	}
}


//BASICALLY I NEED TO GET THE SPAWM RELOAD SYSTEM GOING AND TEAMSCORE GOING FOR WIN ROUND