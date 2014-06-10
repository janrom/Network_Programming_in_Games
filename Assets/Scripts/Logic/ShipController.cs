using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipController : MonoBehaviour {
	
	public float 				turningSpeedFactor = 5.0f;
	public float 				accelerationSpeedFactor = 10.0f;
	public GameObject 			plasmaModel = null;
	public Weapon  				weapon = new Weapon();
	public bool 				isControlledLocally = false; ///< Whether object is controlled locally or not.
	public float 				turningSpeed;
	
	private float				rotationCompensationAngleSpeed;
	private float               positionCompensationSpeed;
	public Vector3 				positionOffset;
	
	void Awake()
	{
		// we remain until explicitly destroyed (lasts even when new level is loaded!)
		GameObject.DontDestroyOnLoad(transform.gameObject);
		
		isControlledLocally = true;
		turningSpeed = 0.0f;
		rotationCompensationAngleSpeed = 360.0f;
		positionCompensationSpeed = 15.0f;
		positionOffset = new Vector3();
		
		
	}
	
	void Update()
	{
		if ( isControlledLocally )
		{	
			if ( Input.GetKeyDown(KeyCode.Space) && weapon.IsCharged )
			{
				Fire(); // fire local weapon
			}
		}
		
		weapon.Update(Time.deltaTime);
	}
	
	
	
	/// Firing method.  
	void Fire()
	{
		
		weapon.Fire( 	
						plasmaModel, 
					 	transform.position-transform.forward*3.0f, 
						transform.rotation);
		audio.Play();
		
	}
	
	void FixedUpdate()
	{
		
		if ( isControlledLocally )
		{
			// model is reversed, thus is our forward axis. There is no negative acceleration (See Project Settings->Input), 
			// ship must be turned and accelerated in order to change movement direction.
			rigidbody.AddRelativeForce(  Input.GetAxis("Acceleration")*-accelerationSpeedFactor * Vector3.forward );
			
			// Rotation will be applied discretely.
			// Smoother turning could be accomplished by using Torque. 
			//turningSpeed = Input.GetAxis("Rotation");
			if ( Input.GetKey(KeyCode.LeftArrow)) turningSpeed = -1.0f;
			else if ( Input.GetKey(KeyCode.RightArrow)) turningSpeed = 1.0f;
			else turningSpeed = 0.0f;
			rigidbody.MoveRotation( rigidbody.rotation * Quaternion.AngleAxis( turningSpeedFactor * turningSpeed, Vector3.up) ); 
			
		}
		else
		{
			
			// Dead reckoning? Check module 
				
		}
		
		float aspectRatio = (float)Screen.width / (float)Screen.height;
		Vector3 pos = rigidbody.position;
		
		// change ship position if moved off-screen to match "wrapping" of edges.
		if ( pos.x >  100*aspectRatio )	pos.x -= 200*aspectRatio; 
		if ( pos.x < -100*aspectRatio ) pos.x += 200*aspectRatio;
		if ( pos.y >  100 ) pos.y -= 200;
		if ( pos.y < -100 ) pos.y += 200;
		
		// Finally, make it so on screen.
		rigidbody.position = pos;
		//transform.position = rigidbody.position;
		
	}

	
}
