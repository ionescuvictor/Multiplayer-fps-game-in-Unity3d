using UnityEngine;
using System.Collections;

public class DPPlayerDeath : MonoBehaviour {
	float dtime;
	void Start () {
	dtime = 3;
	StartCoroutine(DestroyMyselfAfterSomeTime());
	}
	
	void Update () {
	
	}
	IEnumerator DestroyMyselfAfterSomeTime()
	{
		yield return new WaitForSeconds(dtime);
		Destroy(this.gameObject);
	}
		
}
