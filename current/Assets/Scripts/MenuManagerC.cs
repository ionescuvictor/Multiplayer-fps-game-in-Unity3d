using UnityEngine;
using System.Collections;
//attached to menumanager
public class MenuManagerC : MonoBehaviour {
	//private Rect mainWindowRect;
	private Rect serverWindowRect;
	private Rect discWindowRect;
	private Rect failWindowRect;
	NetworkConnectionError conerror;
	
	private bool connectedMainMenu;
	
	private bool clientwin = false;
	private bool serverwin = false;
	private bool disconwin;
	private bool failwin;
	
	public string playerName = "l";
	private int ConnectionPort = 26500;
	private string IP = "127.0.0.1";
	private int numberOfPlayers = 10;
	private string serverName = "MyServer";
	
	public bool RedTeam;
	public bool BlueTeam;
	//!!! SPAWMING VARIABLEse
	
	private Rect spawmWindowRect;
	private bool areWeSpawming;
	
	public GameObject BluePlayer;
	public GameObject BlueSpawm;
	public bool comeBack2Life;
	
	public GameObject RedPlayer;
	public GameObject RedSpawm;
	
	//!!! SPAWMING VARIABLES
	
	
	void Start () {
		connectedMainMenu = true;
		disconwin = false;
		failwin = false;
		areWeSpawming = false;
			
	}

	void OnServerInitialized(){
		connectedMainMenu = false;
	}
	void OnConnectedToServer() {
        connectedMainMenu = false;
		areWeSpawming = true;
	}
	void OnFailedToConnect(NetworkConnectionError error){
		connectedMainMenu = false;
		failwin = true;
		conerror = error;
	}
	void OnDisconnectedFromServer(){
		Application.LoadLevel(Application.loadedLevel);
	}
	void OnPlayerDisconnected(NetworkPlayer player) {
        if(Network.isServer){
		Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
		}
	}
	
	[RPC]
	void UpdatePlayerNameEverywhere(string s)
	{
	  this.GetComponent<PlayerDatabase>().setPlayerName = s; //SET PLAYERNAME IN PLAYERDATABASE
	  this.GetComponent<PlayerDatabase>().setName = true;
	} 
	
	void Update(){
		
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			disconwin= !disconwin;	
		}
		if(comeBack2Life == true){
			areWeSpawming = true;
			comeBack2Life = false;
     	}
			
	}
	void OnGUI() 
	{ 
		
					
				
		if(areWeSpawming == true){
		spawmWindowRect = GUILayout.Window(7, spawmWindowRect, SpawmWindow, "Spawm Menu", GUILayout.Width(250), GUILayout.Height(225)); 
		}
		
		if(connectedMainMenu == true)
		{
		serverWindowRect = GUILayout.Window(0, serverWindowRect, MainWindow, "Menu", GUILayout.Width(250), GUILayout.Height(225));
		}
		if(disconwin == true)
		{
		discWindowRect = GUILayout.Window(0, discWindowRect, DiscWindow, "Menu", GUILayout.Width(250), GUILayout.Height(225));
		}
		if(failwin == true)
		{
		failWindowRect = GUILayout.Window(0, failWindowRect, FailWindow, "Menu", GUILayout.Width(250), GUILayout.Height(225));
		}
		
		
		
	}
   
	void MainWindow(int windowID)
	{
		   	if(clientwin==false && serverwin==false)
			{
				if(GUILayout.Button ("Start A Server"))
					{
						serverwin = true;
					}
				    if(GUILayout.Button ("Connect To A Server"))
					{
				     	clientwin = true;
					}
						if(GUILayout.Button ("Exit"))
					{
				        Application.Quit();	
					}
				}
				
				
				if(serverwin==true && clientwin==false)
				{
//					GUILayout.Label("Enter Your Name");
//					playerName = GUILayout.TextField(playerName);
					GUILayout.Label("Enter the server name");
					serverName = GUILayout.TextField(serverName);
					GUILayout.Label("Enter the port number");
					ConnectionPort = int.Parse(GUILayout.TextField(ConnectionPort.ToString ()));
				    GUILayout.Label("Enter Maximum Number Of Players");
			        numberOfPlayers = int.Parse(GUILayout.TextField(numberOfPlayers.ToString ()));
					
			        if(GUILayout.Button("Start A Server"))
					{
						Network.InitializeServer(numberOfPlayers, ConnectionPort, false);
				    }
			        if(GUILayout.Button("Go back"))
					{
						clientwin = false;
				        serverwin = false;
				        Network.Disconnect ();
					}
				}
				if(clientwin==true && serverwin==false)
				{
					GUILayout.Label("Enter Your Name");
					playerName = GUILayout.TextField(playerName);
					GUILayout.Label("Enter The IP");
					IP = GUILayout.TextField(IP);
					GUILayout.Label("Enter the port number");
					ConnectionPort = int.Parse(GUILayout.TextField(ConnectionPort.ToString ()));
					
					if(GUILayout.Button("Connect To Server"))
					{
					    Network.Connect(IP, ConnectionPort);
				    }
			        if(GUILayout.Button("Go back"))
					{
						clientwin = false;
				        serverwin = false;
				        Network.Disconnect ();
					}
			    }
	}
	
	void DiscWindow(int windowID)
	{

        if(Network.isClient){
			if(GUILayout.Button("Join Blue Team")&& areWeSpawming== true)
			{ 
			  	Network.Instantiate(BluePlayer, BlueSpawm.transform.position, BlueSpawm.transform.rotation, 0);
			    areWeSpawming = false;
			}
			if(GUILayout.Button("Join Red Team") && areWeSpawming== true)
			{ 
			  	Network.Instantiate(RedPlayer, RedSpawm.transform.position, RedSpawm.transform.rotation, 0);
			    areWeSpawming = false;

			}
			GUILayout.Space (10);
			
			if(GUILayout.Button ("Quit to Windows"))
			{
				Application.Quit();
			}
	        if(GUILayout.Button ("Quit Main Menu"))
			{
				disconwin = false;
		        Network.Disconnect();
			}
		}
		else{
			if(GUILayout.Button ("Quit to Windows"))
			{
				Application.Quit();
			}
	        if(GUILayout.Button ("Quit Main Menu"))
			{
				disconwin = false;
		        Network.Disconnect();
			}
		}
					
	}
	void FailWindow(int windowID)
	{
	   GUILayout.Label("Connection Error Is:" + conerror.ToString());
	
	   if(GUILayout.Button ("Quit to Windows"))
		{
			Application.Quit();
			
		}
        if(GUILayout.Button ("Quit Main Menu"))
		{
			disconwin = false;
	        Network.Disconnect();
			connectedMainMenu = true;
			failwin = false;
		}	
	}
	void SpawmWindow(int windowID)
	{
	    if(GUILayout.Button("Join Blue Team"))
		{   networkView.RPC ("UpdatePlayerNameEverywhere", RPCMode.AllBuffered, playerName); //puts the playername and playerset for all the same in database
		  	Network.Instantiate(BluePlayer, BlueSpawm.transform.position, BlueSpawm.transform.rotation, 0);
		    areWeSpawming = false;
			BlueTeam = true;
			RedTeam = false;
		}
		if(GUILayout.Button("Join Red Team"))
		{   networkView.RPC ("UpdatePlayerNameEverywhere", RPCMode.AllBuffered, playerName); //puts the playername and playerset for all the same in database
		  	Network.Instantiate(RedPlayer, RedSpawm.transform.position, RedSpawm.transform.rotation, 0);
		    areWeSpawming = false;
			RedTeam = true;
			BlueTeam = false;
		}
    }
}


