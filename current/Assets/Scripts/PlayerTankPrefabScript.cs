using UnityEngine;
using System.Collections;

public class PlayerTankPrefabScript : MonoBehaviour
{
	public GameObject BlueMissle;
	public GameObject RedMissle;
	private Transform Socket;
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
    	SelectedWeapon = "Missle";	
	}
	public Texture image;
     
    void OnGUI() {
		if(networkView.isMine){
    GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 10,10), image);}
    }		
	[RPC]
	void UpdatePlayerNameAcrossTheNetwork (string s)
	{
		this.gameObject.name = s;	
	}
		
	void Update ()
	{
		if (networkView.isMine) {
			//set playername in prefab and update playername across the network.
			PlayerPrefabName = MenuManager.GetComponent<MenuManagerC> ().playerName;
			if (namesent == false) {
				networkView.RPC ("UpdatePlayerNameAcrossTheNetwork", RPCMode.AllBuffered, PlayerPrefabName);
				namesent = true;
			}
			//check for our health and if 0, respawn
			for (int i = 0; i<GameObject.Find("MenuManager").GetComponent<PlayerDatabase>().OBplayer.Count; i++) {			
				if (GameObject.Find ("MenuManager").GetComponent<PlayerDatabase> ().OBplayer [i].PlayerName == this.gameObject.name) {
					if (GameObject.Find ("MenuManager").GetComponent<PlayerDatabase> ().OBplayer [i].Health <= 0) {
						GameObject.Find ("MenuManager").GetComponent<PlayerDatabase> ().setHealth = true;
						GameObject.Find ("MenuManager").GetComponent<PlayerDatabase> ().whosHealthToSet = this.gameObject.name;
						//GameObject.Find ("MenuManager").GetComponent<MenuManagerC>().iJustDied = true;
						Network.Destroy (this.gameObject.networkView.viewID);
						Network.Instantiate (playerDeathExplosion, this.transform.position, this.transform.rotation, 2);
					}
				}
				//Check for our health.	
				
					
				if (SelectedWeapon == "Missle") {
					if (Input.GetKeyDown (KeyCode.Mouse0) && PlayerPrefabBlueTeam == true) {
						Network.Instantiate (BlueMissle, Camera.main.transform.position, Camera.main.transform.rotation, 0);
					}
					if (Input.GetKeyDown (KeyCode.Mouse0) && PlayerPrefabRedTeam == true) {
						Network.Instantiate (RedMissle, Camera.main.transform.position, Camera.main.transform.rotation, 0);
					}
//				} else if (SelectedWeapon == "BulletRay") {			
//					if (Input.GetKeyDown (KeyCode.Mouse0)) {
//						RaycastHit hit;		
//						Debug.DrawRay (Camera.main.transform.position, Camera.main.transform.forward, Color.red, 4);
//						if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, 10)) {
//							if (hit.transform.tag == "BluePlayer" && this.transform.tag == "RedPlayer") {   
//								go.GetComponent<PlayerDatabase> ().iHit = true;
//								go.GetComponent<PlayerDatabase> ().WhoIHit = hit.transform.name;
//								go.GetComponent<PlayerDatabase> ().damage = 15;	
//								go.GetComponent<PlayerDatabase> ().WhoDoneTheHit = this.gameObject.name;	
//							}
//							if (hit.transform.tag == "RedPlayer" && this.transform.tag == "BluePlayer") {  
//								go.GetComponent<PlayerDatabase> ().iHit = true;
//								go.GetComponent<PlayerDatabase> ().WhoIHit = hit.transform.name;
//								go.GetComponent<PlayerDatabase> ().damage = 15;
//								go.GetComponent<PlayerDatabase> ().WhoDoneTheHit = this.gameObject.name;	
//							}	
//						}
//					}
				}
			}
		}
	}
}
	

