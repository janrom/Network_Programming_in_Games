using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class BattleScript : MonoBehaviour {

	/// Player object prefab.
	public GameObject playerModel = null;
	public GameObject enemyModel = null;
	public GameObject explosion = null;
	
	/// Current player reference.
	private GameObject clientPlayer = null;
	/// Contains all player objects (including client player)
	private List<GameObject> players = new List<GameObject>();
	public Texture2D[] textures;
	
	void SpawnPlayer( Vector3 pos) 
	{
		GameObject player = (GameObject)Instantiate(playerModel, pos, Quaternion.AngleAxis(-90.0f, Vector3.right));	
		
		players.Add(player);
		// Load random texture for player.
		int texId = new System.Random().Next(0,textures.Length);
		player.transform.GetChild(0).renderer.materials[0].mainTexture = textures[texId];	
	}
	
	void SpawnDummy( Vector3 pos) 
	{
		GameObject player = (GameObject)Instantiate(playerModel, pos, Quaternion.AngleAxis(-90.0f, Vector3.right));	
		
		// Load random texture for player.
		int texId = new System.Random().Next(0,textures.Length);
		player.transform.GetChild(0).renderer.materials[0].mainTexture = textures[texId];	
		player.GetComponent<ShipController>().isControlledLocally = false;
	}
	
	public void SpawnExplosion( Vector3 pos)
	{
		Instantiate(explosion, pos, Quaternion.AngleAxis( -90, Vector3.right));	
	}
				
}
