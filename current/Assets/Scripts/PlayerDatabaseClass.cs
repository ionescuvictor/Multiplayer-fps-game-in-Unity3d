using UnityEngine;
using System.Collections;

public class PlayerDatabaseClass
{
	public int playerIndex;   // the name field
	public int PlayerIndex {   // the Name property
		get {
			return playerIndex; 
		}
		set {
			playerIndex = value;	
		}
	}

	public string playerName;

	public string PlayerName {
		get {
			return playerName; 
		}
		set {
			playerName = value;	
		}
	}

	public int kills;

	public int Kills
	{ get { return kills; } set { kills = value; } }

	public int deaths;

	public int Deaths
	{ get { return deaths; } set { deaths = value; } }

	public int health = 100;

	public int Health
	{ get { return health; } set { health = value; } }
}
