using UnityEngine;
using System.Collections;

public class PlayerPrefabScript : MonoBehaviour
{   public GameObject RedTank;
	public GameObject BlueTank;
	public GameObject BlueMissle;
	public GameObject RedMissle;
	public Transform Socket;
	public bool namesent = false;
	public GameObject playerDeathExplosion;
	GameObject MenuManager;
	public string PlayerPrefabName;
	private bool PlayerPrefabRedTeam;
	private bool PlayerPrefabBlueTeam;
	public GameObject go;
	private string SelectedWeapon;
	
	void Start ()
	{
		SelectedWeapon = "BulletRay";	
		go = GameObject.Find ("MenuManager");
		Socket = transform.FindChild ("Socket");
		MenuManager = GameObject.FindGameObjectWithTag ("MenuManager");
	
		PlayerPrefabRedTeam = MenuManager.GetComponent<MenuManagerC> ().RedTeam;
		PlayerPrefabBlueTeam = MenuManager.GetComponent<MenuManagerC> ().BlueTeam;	
    		
	}
	 	
	[RPC]
	void UpdatePlayerNameAcrossTheNetwork (string s)
	{
		this.gameObject.name = s;	
	}
	[RPC]
	void DestroyNeutralTank(string d)
	{
	  Destroy(GameObject.Find(d));	
	}
	
	void Update ()
	{	
		if (networkView.isMine) {
			if(Input.GetKeyDown(KeyCode.E)){
		    RaycastHit hitd;
		    if(Physics.Raycast(this.transform.position,this.transform.forward, out hitd, 3)){	
		    if(this.gameObject.tag == "BluePlayer"){Network.Instantiate(BlueTank, hitd.transform.position, hitd.transform.rotation,1);}	
		    if(this.gameObject.tag == "RedPlayer"){Network.Instantiate(RedTank, hitd.transform.position, hitd.transform.rotation,1);}	
            networkView.RPC("DestroyNeutralTank", RPCMode.AllBuffered, hitd.transform.name);}
		    Destroy(this.gameObject);	
		    }
			//set playername in prefab and update playername across the network.
			PlayerPrefabName = MenuManager.GetComponent<MenuManagerC> ().playerName;
			if (namesent == false) {
				networkView.RPC ("UpdatePlayerNameAcrossTheNetwork", RPCMode.AllBuffered, PlayerPrefabName);
				namesent = true;
			}
			//check for our health
			for (int i = 0; i<GameObject.Find("MenuManager").GetComponent<PlayerDatabase>().OBplayer.Count; i++) {			
				if (GameObject.Find ("MenuManager").GetComponent<PlayerDatabase> ().OBplayer [i].PlayerName == this.gameObject.name) {
					if (GameObject.Find ("MenuManager").GetComponent<PlayerDatabase> ().OBplayer [i].Health <= 0) {
						GameObject.Find ("MenuManager").GetComponent<MenuManagerC>().comeBack2Life = true;
						GameObject.Find ("MenuManager").GetComponent<PlayerDatabase> ().setHealth = true;
						GameObject.Find ("MenuManager").GetComponent<PlayerDatabase> ().whosHealthToSet = this.gameObject.name;
						Debug.Log("hp is 0");
						Network.Destroy (this.gameObject.networkView.viewID);
						Network.Instantiate (playerDeathExplosion, this.transform.position, this.transform.rotation, 2);
					}
				}
				//Check for our health.	
				//SelectedWeapon
				if (Input.GetKeyDown (KeyCode.Alpha1)) {
					SelectedWeapon = "BulletRay";
				}
				if (Input.GetKeyDown (KeyCode.Alpha2)) {
					SelectedWeapon = "Missle";
				}
					
				if (SelectedWeapon == "Missle") {
					if (Input.GetKeyDown (KeyCode.Mouse0) && PlayerPrefabBlueTeam == true) {
						Network.Instantiate (BlueMissle, Socket.transform.position, Quaternion.Euler (Socket.eulerAngles.x + 90, Socket.eulerAngles.y, 0), 0);
					}
					if (Input.GetKeyDown (KeyCode.Mouse0) && PlayerPrefabRedTeam == true) {
						Network.Instantiate (RedMissle, Socket.transform.position, Quaternion.Euler (Socket.eulerAngles.x + 90, Socket.eulerAngles.y, 0), 0);
					}
				} else if (SelectedWeapon == "BulletRay") {			
					if (Input.GetKeyDown (KeyCode.Mouse0)) {
						RaycastHit hit;		
						Debug.DrawRay (Camera.main.transform.position, Camera.main.transform.forward, Color.red, 4);
						if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, 10)) {
							if (hit.transform.tag == "BluePlayer" && this.transform.tag == "RedPlayer") {   
								go.GetComponent<PlayerDatabase> ().iHit = true;
								go.GetComponent<PlayerDatabase> ().WhoIHit = hit.transform.name;
								go.GetComponent<PlayerDatabase> ().damage = 15;	
								go.GetComponent<PlayerDatabase> ().WhoDoneTheHit = this.gameObject.name;	
							}
							if (hit.transform.tag == "RedPlayer" && this.transform.tag == "BluePlayer") {  
								go.GetComponent<PlayerDatabase> ().iHit = true;
								go.GetComponent<PlayerDatabase> ().WhoIHit = hit.transform.name;
								go.GetComponent<PlayerDatabase> ().damage = 15;
								go.GetComponent<PlayerDatabase> ().WhoDoneTheHit = this.gameObject.name;	
							}	
						}
					}
				}
			}
		}
	}
}
	

