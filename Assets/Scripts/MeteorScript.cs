using UnityEngine;
using System.Collections;

public class MeteorScript : MonoBehaviour {

	public int CollisionsBeforeDiscarding;

	public GameObject meteorSprite;

	public GameObject explodingMeteor;

	MeteorSpawnerScript MeteorSpawner;

	public RuntimeAnimatorController tailCtrl;
	public RuntimeAnimatorController slowTailCtrl;

	private int CollisionCount;
	bool onFire = true;

	GameObject cauda;

	Vector3 lastPos;
	bool rebatido;


	// Use this for initialization
	void Start () {
		//Carrega MeteorSpawner e inicializa variaveis
		MeteorSpawner = GameObject.Find("MeteorSpawner").GetComponent<MeteorSpawnerScript>();
		lastPos = transform.position;
		onFire = true;
		cauda = transform.Find("cauda").gameObject;

	}
	
	// Update is called once per frame
	void Update () {

		//Roda cauda
		RotateTail();

		//Se sair muito da tela, destruir meteoro
		if (transform.position.y < -Camera.main.orthographicSize - 1){
			if ((transform.position.x > Camera.main.orthographicSize * Screen.width / Screen.height) ||
			    (transform.position.x < -Camera.main.orthographicSize * Screen.width / Screen.height)){
				Destroy(gameObject);
				MeteorSpawner.GetComponent<MeteorSpawnerScript>().MeteorDestroyed();
			}
		}
	}

	void OnCollisionEnter(Collision obj){
		//Checa colisao com o chao
		if(obj.gameObject.tag == "Floor")
		{
			//Caso estiver com cauda, se nao foi rebatida, tira causa, senao coloca a cauda menor
			if(onFire){
				if(!rebatido)
					HideTail();
				else if(rebatido){
					Debug.Log("vem berg");
					cauda.GetComponentInChildren<Animator>().runtimeAnimatorController = slowTailCtrl;
				}
			}

			CollisionCount++;
			//Se colidiu demais, aplica fade no meteoro
			if (CollisionCount >= CollisionsBeforeDiscarding){
				StartCoroutine("Fade");
			}
		}
	}

	IEnumerator Fade() {
		//Aplica fade e mata meteoro
		for (float f = 1f; f >= 0; f -= 0.04f) { 
			Color c = meteorSprite.GetComponent<SpriteRenderer>().material.color;
			c.a = f;
			meteorSprite.GetComponent<SpriteRenderer>().material.color = c;
			yield return null;
		}
		MeteorSpawner.MeteorDestroyed();
		Destroy(gameObject);
	}

	public void Restore(){
		//Restaura meteoro e sua cauda apos rabada
		CollisionCount = 0;
		if(rebatido)
			cauda.GetComponentInChildren<Animator>().runtimeAnimatorController = tailCtrl;
		rebatido = true;
	}


	public void HideTail(){
		//Esconde cauda do meteoro (ou aplica a cauda menor em caso de rabada)
		onFire = false;
		if(rebatido)
			cauda.GetComponentInChildren<Animator>().runtimeAnimatorController = slowTailCtrl;
		else
			cauda.SetActive(false);
	}
	public void ShowTail(){
		//Faz cauda reaparecer apos rabada
		onFire = true;
		cauda.SetActive(true);
		rebatido = true;
	}
	public void RotateTail(){
		//Rotaciona cauda
		Vector3 velNorm = GetComponent<Rigidbody>().velocity.normalized;
		transform.rotation = Quaternion.Euler(0,0,Mathf.Rad2Deg * (Mathf.Atan2(velNorm.y, velNorm.x) + Mathf.PI/2) - transform.rotation.z);
	}

	public void Explode(){
		//Instancia meteoro explodindo, o mataa e se mata
		SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
		foreach(SpriteRenderer s in sprites){
			s.enabled = false;
		}
		GameObject g = Instantiate(explodingMeteor, transform.position, Quaternion.identity) as GameObject;
		Destroy (g,1f);
		MeteorSpawner.MeteorDestroyed();
		Destroy(gameObject);
	}

}
