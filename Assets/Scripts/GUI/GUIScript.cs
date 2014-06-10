using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class GUIScript : MonoBehaviour {
	
	private float numPlayers = 2.0f; ///< Max number of players in game.
	public string playerName = ""; ///< Name of player in field.
	public int    numberOfKills = 0;
	
	
	// areas for starting game.
	public Rect connectRect = new Rect(Screen.width/2-60-160, Screen.height/2-25, 120, 150);
	public Rect hostRect    = new Rect(Screen.width/2-60+10, Screen.height/2, 220, 150);
	// Areas for in-game gui.
	public Rect controlRect = new Rect(Screen.width-25.0f,10.0f,50.0f,25.0f);
	public Rect statsRect  = new Rect(10.0f, Screen.height - 160.0f, 150.0f, 50.0f);
	// high score rect
	private Rect highScoreRect = new Rect(Screen.width-160,150,160, 250);
	private bool gameIsOn = false;
	private string chatMessage = "";
	private bool showHighscores = false;
	
	void Awake()
	{
		// we remain until explicitly destroyed.
		GameObject.DontDestroyOnLoad(transform.gameObject);
		Debug.Log("Hello there!");
	}
	
	
	void Start()
	{
		
   		
		
		
		
	}
	
	void OnGUI()
	{
		
		
		if ( gameIsOn )
		{
			controlRect = GUILayout.Window(0, controlRect,HandleControls,"Controls");
		}
		else
		{
			GUILayout.BeginVertical();
		
			if ( GUILayout.Button("Start") )
			{
				BroadcastMessage("SpawnPlayer", Vector3.zero);
				BroadcastMessage("SpawnDummy", new Vector3(10.0f, 0.0f, 0.0f));
				gameIsOn = true;
				Application.LoadLevel(1);
			}
			GUILayout.EndVertical();
		
		}
			
	}
	
	void HandleControls( int windowId )
	{
		GUILayout.BeginVertical();
		if ( GUILayout.Button("Quit game", GUILayout.ExpandWidth(false)))
		{
			// Won't work from editor! only from standalone.
			Application.Quit();
		}
		
		if ( GUILayout.Button("Spawn Dummy", GUILayout.ExpandWidth(false)))
		{
			BroadcastMessage("SpawnDummy", new Vector3(10.0f, 0.0f, 0.0f));	
		}
		GUILayout.EndVertical();
	}
	
		
}
