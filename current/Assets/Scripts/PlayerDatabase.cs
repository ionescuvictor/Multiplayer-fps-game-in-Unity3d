using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//attached to menumanager
public class PlayerDatabase : MonoBehaviour {
	private Rect listWindowRect = new Rect(20, 20, 100, 100);
	
	public List<PlayerDatabaseClass> OBplayer = new List<PlayerDatabaseClass>();
	private NetworkPlayer ntplayer;
	
	
	public bool setName;
	
	
	public string setPlayerName;
	
	//variables for doing damage and scoring either a death or a kill
	
	public bool iHit;
	public string WhoIHit;
	public string WhoDoneTheHit;
	public int damage;
	
	//Variables to respawn and set health back to 100
	
	public string whosHealthToSet;
	public bool setHealth = false;
	
	
	
	void Start(){
	 iHit = false;	
	 setName = false;	
	 
	}
	void Update(){
	  	if(iHit == true) //set true from misslescript
		{ 
			networkView.RPC("UpdateHealthInList",RPCMode.AllBuffered,WhoIHit,damage,WhoDoneTheHit);
			iHit = false;
		}
		if(setName == true) //set true from the MenuManagerC script
		{
		    networkView.RPC("AddPlayerNameToList",RPCMode.AllBuffered,ntplayer); //netplayer gets set once player connects.
			setName = false;
		}
		if(setHealth == true) //set true from playerprefab script
		{
		  networkView.RPC("SetHealthBackTo100", RPCMode.AllBuffered, whosHealthToSet);	
		  setHealth = false;	
		}
	}
	
    
	
	
	void OnPlayerConnected(NetworkPlayer netPlayer){
		 ntplayer = netPlayer;
		 networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, netPlayer); //Updates the list for everybody
	}
	void OnPlayerDisconnected(NetworkPlayer netplayer){
		networkView.RPC("RemovePlayerFromList",RPCMode.AllBuffered, netplayer);
	}
	[RPC]
	void AddPlayerToList(NetworkPlayer rpcNetplayer){
		 PlayerDatabaseClass capture = new PlayerDatabaseClass();
		 capture.PlayerIndex = int.Parse(rpcNetplayer.ToString());
		 OBplayer.Add(capture);			
	}
	[RPC]
	void RemovePlayerFromList(NetworkPlayer rpcNetPlayer)
	{
		for(int i = 0; i<OBplayer.Count; i++)
		{
		   if(OBplayer[i].PlayerIndex == int.Parse (rpcNetPlayer.ToString()))
			{
				OBplayer.RemoveAt(i); 
			}
		}
	}
	[RPC]
	void AddPlayerNameToList(NetworkPlayer rpcNetPlayer)//takes the onplayerconncted(netplayer.tostring) ID. 
	{
		for(int i = 0; i<OBplayer.Count; i++)
		{
		   if(OBplayer[i].PlayerIndex == int.Parse (rpcNetPlayer.ToString()))
			{
				OBplayer[i].PlayerName = setPlayerName; //this setPlayerName was set globally from menumanager RPC also nameset = true so it fires
			}
		}
	}
	[RPC]
	void UpdateKillsInList(string rpcWhoKilledMe)
	{
		for(int i = 0; i<OBplayer.Count; i++)
		{
		   if(OBplayer[i].PlayerName == rpcWhoKilledMe)
			{
				OBplayer[i].Kills = OBplayer[i].Kills + 1; 
			}
		}
	}
	[RPC]
	void UpdateDeathsInList(string rpcWhoDied)
	{
		for(int i = 0; i<OBplayer.Count; i++)
		{
		   if(OBplayer[i].PlayerName == rpcWhoDied)
			{
				OBplayer[i].Deaths = OBplayer[i].Deaths + 1; 
			}
		}
	}
	[RPC]
	void UpdateHealthInList(string rpcWhoGotHit, int rpcDamage, string rpcWhoDoneTheHit)
	{
		for(int i = 0; i<OBplayer.Count; i++)
		{
		   if(OBplayer[i].PlayerName == rpcWhoGotHit)
			{
				OBplayer[i].Health = OBplayer[i].Health - rpcDamage;
				if(OBplayer[i].health <= 0)
				{
				OBplayer[i].Deaths+=1;
				 for(int j = 0; j<OBplayer.Count; j++){
				   if(OBplayer[j].PlayerName == rpcWhoDoneTheHit)
						{
							OBplayer[j].Kills+=1;
						}
					}
				}
			}
		}
	}
	[RPC]
	void SetHealthBackTo100(string rpcPlayerName)
	{
		for(int i = 0; i<OBplayer.Count; i++)
		{
		   if(OBplayer[i].PlayerName == rpcPlayerName)
			{
				OBplayer[i].Health = 100;
			}
		}
	}
	
	
	void OnGUI()
	{
	   if(Input.GetKey(KeyCode.Tab)){
	   listWindowRect = GUILayout.Window(1, listWindowRect, DoListWindow, "List Window",GUILayout.Width(250), GUILayout.Height(225));	
	   }
	}
		
	void DoListWindow(int windowID)
	{
		foreach(PlayerDatabaseClass db in OBplayer)
		{
		  GUILayout.Label(db.PlayerIndex.ToString() + db.PlayerName + db.Kills + db.Deaths + db.Health);	
		}
	}
}