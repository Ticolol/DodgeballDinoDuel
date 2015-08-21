using UnityEngine;
using System.Collections;

public class TailWhip : MonoBehaviour {

	public float FORCE = 1f;

	Player father;

	// Use this for initialization
	void Start () {
		father = transform.parent.gameObject.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c){
		if(c.gameObject.tag == "Meteor"){
			//Rebater!!!!
			c.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(c.transform.position - father.transform.position)
			                                                * FORCE , ForceMode.Impulse);
			c.gameObject.GetComponent<MeteorScript>().ShowTail();
			c.GetComponent<MeteorScript>().Restore();
			father.knockedMeteors++;
		}
	}
}
