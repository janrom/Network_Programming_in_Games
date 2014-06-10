using UnityEngine;
using System.Collections;

public class PlasmaScript : MonoBehaviour {
	
	public float plasmaSpeed = 3000.0f;
	private float timeAlive = 0.0f;
	public float lifeSpan = 2.0f;
	

	void Start()
	{
		rigidbody.AddForce( transform.forward.normalized * -plasmaSpeed );
	}
	
	void FixedUpdate()
	{
		
		float aspectRatio = (float)Screen.width / (float)Screen.height;
		Vector3 pos = transform.position;
		
		// change ship position if moved off-screen to match "wrapping" of edges.
		// Quick and Dirty solution, but works well enough.
		if ( pos.x >  100*aspectRatio )	pos.x -= 200*aspectRatio; 
		if ( pos.x < -100*aspectRatio ) pos.x += 200*aspectRatio;
		if ( pos.y >  100 ) pos.y -= 200;
		if ( pos.y < -100 ) pos.y += 200;
		// make it so
		transform.position = pos;
		timeAlive += Time.deltaTime;
		
		if ( timeAlive >= lifeSpan )
		{
			GameObject.Destroy(this.gameObject);
		}
	}
	
	void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Ship" )
		{
			GameObject.Find("NetworkGUI").GetComponent<BattleScript>().SpawnExplosion(other.gameObject.transform.position);
			GameObject.Destroy(other.gameObject);
		}
		// destroy plasma despite what it hits.
		GameObject.Destroy(this.gameObject);
		
	}
}
