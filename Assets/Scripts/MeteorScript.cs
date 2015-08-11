using UnityEngine;
using System.Collections;

public class MeteorScript : MonoBehaviour {

	public int CollisionsBeforeDiscarding;

	public GameObject MeteorSpawner;

	private int CollisionCount;

	Vector3 lastPos;


	// Use this for initialization
	void Start () {
		MeteorSpawner = MeteorSpawnerScript.SpawnerInstance;
		lastPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.position.y < -Camera.main.orthographicSize - 1){
			if ((transform.position.x > Camera.main.orthographicSize * Screen.width / Screen.height) ||
			    (transform.position.x < -Camera.main.orthographicSize * Screen.width / Screen.height)){
				Destroy(gameObject);
				MeteorSpawner.GetComponent<MeteorSpawnerScript>().MeteorDestroyed();
			}
		}
		//lastPos
	}

	void OnCollisionEnter(Collision obj){
		if(obj.gameObject.tag == "Floor")
		{
			CollisionCount++;
			if (CollisionCount >= CollisionsBeforeDiscarding){
				StartCoroutine("Fade");
			}
		}
	}

	IEnumerator Fade() {
		for (float f = 1f; f >= 0; f -= 0.04f) {
			Color c = GetComponent<Renderer>().material.color;
			c.a = f;
			GetComponent<Renderer>().material.color = c;
			yield return null;
		}
		Destroy(gameObject);
		MeteorSpawner.GetComponent<MeteorSpawnerScript>().MeteorDestroyed();
	}

	public void Restore(){
		CollisionCount = 0;
	}
}
