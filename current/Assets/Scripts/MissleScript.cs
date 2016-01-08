using UnityEngine;
using System.Collections;

public class MissleScript : MonoBehaviour
{
	private bool namesent = false;
	private string missleName;
	public Transform mytransform;
	float dtime = 5; /// 5 seconds
	GameObject MM;
	private float nextGrenadeDmg = 0;
	private bool moving;
	public GameObject missleDeathExplosion;

	void Start ()
	{
		moving = true;
		MM = GameObject.Find ("MenuManager");
		mytransform = transform;
		StartCoroutine (DestroyMyselfAfterSomeTime ());
	}
	
	void Update ()
	{
		if      (moving == true) {this.gameObject.transform.Translate (Vector3.up * Time.deltaTime * 10);}
		else if (moving == false) {this.gameObject.transform.Translate (Vector3.zero);}
		
		RaycastHit hit;
		if (networkView.isMine) {
			if (namesent == false) {
				missleName = GameObject.FindGameObjectWithTag ("MenuManager").GetComponent<MenuManagerC> ().playerName;
				networkView.RPC ("SetMissleNameAcrossTheNetwork", RPCMode.AllBuffered, missleName);
				namesent = true;
			}
	
		
			if (Physics.Raycast (mytransform.position, mytransform.up, out hit, 0.2f)) { //0.2 is magic number
				this.gameObject.collider.enabled = false;
					this.gameObject.renderer.enabled = false;
				if (hit.transform.tag == "envr" || hit.transform.tag == "BluePlayer" || hit.transform.tag == "RedPlayer"){//!! hit.transform.tag = "redTank -> dmg = 20;
			
					
					//if(Time.time > oneExplosionOnly){
					//Network.Instantiate(missleDeathExplosion,this.transform.position, this.transform.rotation, 9);
					//oneExplosionOnly = Time.time + 2;}
					moving = false;
					StartCoroutine (DestroyMyselfAfterHit ());
					GameObject[] gored  =  GameObject.FindGameObjectsWithTag ("RedPlayer");
					GameObject[] goblue =  GameObject.FindGameObjectsWithTag ("BluePlayer");
					
					if (this.gameObject.transform.tag == "RedMissleTag") {//if im on red team
						for (int i = 0; i<goblue.Length; i++) {
							Debug.Log(goblue[0].transform.name);
							RaycastHit hitr;
							if (Vector3.Distance (this.transform.position, goblue [i].gameObject.transform.position) < 6) {
								if (!Physics.Linecast (this.transform.position, goblue [i].gameObject.transform.position, out hitr))Debug.Log("working!!"); { //if theres nothing in the way
									if (Time.time > nextGrenadeDmg) {
										nextGrenadeDmg = Time.time + 2;
										MM.GetComponent<PlayerDatabase> ().iHit = true;
										MM.GetComponent<PlayerDatabase> ().WhoIHit = hitr.transform.name;
										MM.GetComponent<PlayerDatabase> ().damage = 35;	
										MM.GetComponent<PlayerDatabase> ().WhoDoneTheHit = this.gameObject.name;
									}
						
								}
			
							}
						}
					}
				
					if (this.gameObject.transform.tag == "BlueMissleTag") {// if im on blue team
						for (int i = 0; i<gored.Length; i++) { 
							RaycastHit hitb;
							if (Vector3.Distance (this.transform.position, gored [i].gameObject.transform.position) < 6) {			
								if (!Physics.Linecast (this.transform.position, gored [i].gameObject.transform.position, out hitb))Debug.Log("working!!"); { //if theres nothing in the way
									if (Time.time > nextGrenadeDmg) {
										nextGrenadeDmg = Time.time + 2;
										Debug.Log(hit.transform.tag);
										MM.GetComponent<PlayerDatabase> ().iHit = true;
										MM.GetComponent<PlayerDatabase> ().WhoIHit = hitb.transform.name;
										MM.GetComponent<PlayerDatabase> ().damage = 35;	
										MM.GetComponent<PlayerDatabase> ().WhoDoneTheHit = this.gameObject.name;
									}
						
								}
			
							}
						}
					}
				}
			}
		}
	}

	[RPC]
	void SetMissleNameAcrossTheNetwork (string s)
	{
		this.gameObject.name = s;
	}
	
	IEnumerator DestroyMyselfAfterSomeTime ()
	{
		yield return new WaitForSeconds(dtime);
		
		Destroy (this.gameObject);
	}

	IEnumerator DestroyMyselfAfterHit ()
	{
		yield return new WaitForSeconds(1);
		Destroy (this.gameObject);
	}

	
}
