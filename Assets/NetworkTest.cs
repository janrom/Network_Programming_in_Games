using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageQueue : Queue<string>
{
	private int m_iMaxMessagesInQueue;
	const int iDefaultMaxMessagesInQueue = 15;

	// initializes message queue with desired length
	public MessageQueue( int maxMessages = MessageQueue.iDefaultMaxMessagesInQueue )
	{
		m_iMaxMessagesInQueue = maxMessages;
	}

	// enqueues a new message, removing oldest if new one doesn't fit in
	public new void Enqueue( string msg )
	{
		if ( Count > m_iMaxMessagesInQueue ) Dequeue();
		base.Enqueue( msg );
	}
}

/// <summary>
/// NetworkTest
/// This class handles all networking stuff for client and server
/// </summary>
public class NetworkTest : MonoBehaviour 
{
	// server port. ports under 1025 are reserved for system use
	string m_strPort = "1025";
	MessageQueue msgQueue = new MessageQueue();

	void Start()
	{
		// for debugging
		Debug.developerConsoleVisible = true;
		msgQueue.Enqueue ("Starting application...");	
	}

	/// <summary>
	/// LaunchServer()
	/// Initialize server with given connection amount, port number, nat option and password
	/// then try to launch it
	/// </summary>
	void LaunchServer()
	{
		msgQueue.Enqueue ( "Starting a server..." );

		// client must pass this password as parameter when connecting
		Network.incomingPassword = "test";
		// true if private address
		bool useNat = !Network.HavePublicAddress();

		// if parsing to int fails it throws FormatException
		try
		{
			int port = int.Parse ( m_strPort );
			NetworkConnectionError error = Network.InitializeServer ( 32, port, useNat );
			switch ( error )
			{
				case NetworkConnectionError.NoError:
					break;
				default:
					msgQueue.Enqueue("Could not launch server: '" + error + "'");
				break;
			}
		}
		catch ( System.FormatException )
		{
			msgQueue.Enqueue ( "Invalid port number: " + m_strPort );
		}
	}

	/// <summary>
	/// OnServerInitialized() 
	/// Send message on succesful server launch
	/// </summary>
	void OnServerInitialized() 
	{
		msgQueue.Enqueue ( "Server started at port " + m_strPort );
	}

	/// <summary>
	/// OnDisconnectedFromServer(NetworkDisconnection info)
	/// Send message on client or server disconnection
	/// </summary>
	void OnDisconnectedFromServer(NetworkDisconnection info) 
	{
		if (Network.isServer)
			msgQueue.Enqueue ( "Server disconnected" );
		else 
		{
			if (info == NetworkDisconnection.LostConnection)
				msgQueue.Enqueue ( "Lost connection to the server" );
			else
				msgQueue.Enqueue ( "Successfully diconnected from the server" );
		}
	}

	/// <summary>
	/// OnPlayerConnected(NetworkPlayer player)
	/// Add message to message queue when new player succesfully connects to server
	/// </summary>
	void OnPlayerConnected(NetworkPlayer player) 
	{
		msgQueue.Enqueue( "Player connected from " + player.ipAddress + ":" + player.port );

	}

	/// <summary>
	/// OnPlayerDisconnected(NetworkPlayer player)
	/// Handles player disconnection from server
	/// </summary>
	void OnPlayerDisconnected(NetworkPlayer player) 
	{
		msgQueue.Enqueue ( "Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	/// <summary>
	/// ConnectToServer() 
	/// Connect client to server using given IP, port and optional password
	/// </summary>
	void ConnectToServer() 
	{
		msgQueue.Enqueue ( "Connecting to a server..." );

		try
		{
			int port = int.Parse ( m_strPort );
			Network.Connect ( "127.0.0.1", port, "test" );
		}
		catch ( System.FormatException )
		{
			msgQueue.Enqueue ( "Invalid port number: " + m_strPort );
		}
	}

	/// <summary>
	/// OnConnectedToServer() 
	/// Send message when connected to server succesfully
	/// </summary>
	void OnConnectedToServer() 
	{
		msgQueue.Enqueue ( "Connected to server" );
	}

	/// <summary>
	/// OnFailedToConnect(NetworkConnectionError error)
	/// Send message when connecting to server have failed
	/// </summary>
	void OnFailedToConnect(NetworkConnectionError error) 
	{
		msgQueue.Enqueue ( "Could not connect to server: " + error );
	}

	/// <summary>
	/// OnGUI()
	/// User interface for server and client connections
	/// </summary>
	void OnGUI()
	{
		GUILayout.BeginArea ( new Rect ( Screen.width / 2 - 250, 50, 500, Screen.height - 100 ) );

		GUILayout.BeginVertical ();

		GUILayout.BeginHorizontal();

		// start server button
		if ( GUILayout.Button ( "Start Server" ) )
		{
			LaunchServer ();
		}

		GUILayout.FlexibleSpace();

		// server port number
		GUILayout.Label ( "Port number:" );
		m_strPort = GUILayout.TextField ( m_strPort, GUILayout.Width ( 100 ) ); 

		GUILayout.FlexibleSpace();

		// client connect-button
		if ( GUILayout.Button ( "Connect to Server" ) )
		{
			ConnectToServer();
		}

		// disconnect-button for client and server
		if ( GUILayout.Button ( "Disconnect" ) )
		{
			Network.Disconnect();
			MasterServer.UnregisterHost();
		}

		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		GUILayout.Label ("Message queue:");

		GUILayout.FlexibleSpace();

		// display message queue content
		foreach ( string s in msgQueue )
		{
			GUILayout.Label ( s );
		}
		GUILayout.EndVertical ();

		GUILayout.EndArea ();
	}
}
