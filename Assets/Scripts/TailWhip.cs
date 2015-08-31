using UnityEngine;
using System.Collections;


public class TailWhip : MonoBehaviour {

	public float FORCE = 25f;
	public AudioClip rabada;
	public AudioClip rabadaNoMeteoro;

	Player father;
	ArrayList meteorsTailed;
	bool tailed;

	// Use this for initialization
	void Start () {
		father = transform.parent.gameObject.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c){
		//Se bater num meteoro
		if(c.gameObject.tag == "Meteor"){
			tailed = false;
			//Checar se ja foi rebatido por esta rabada
			foreach(GameObject g in meteorsTailed)
				if(g == c.gameObject)
					tailed = true;
			if(!tailed){
				//Rebater!!!!
				meteorsTailed.Add(c.gameObject);
				//Tocar som do meteoro no rabo
				SoundController.instance.PlaySingle(rabadaNoMeteoro);
				//Impulsionar meteoro
				c.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;//Zerar forças
				c.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(c.transform.position - father.transform.position)
				                                               * FORCE , ForceMode.Impulse);//Aplica Impulso
				//Mostrar cauda
				c.gameObject.GetComponent<MeteorScript>().ShowTail();
				//Restaurar meteoro
				c.GetComponent<MeteorScript>().Restore();
				father.knockedMeteors++;

			}
		}
	}

	public void StartAttack(){
		meteorsTailed = new ArrayList();
		SoundController.instance.RandomizeSfx(rabada);
	}

	public void FinishAttack(){

	}
}
