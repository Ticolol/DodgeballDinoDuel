using UnityEngine;
using System.Collections;

public class MeteorScript : MonoBehaviour {

	public int CollisionsBeforeDiscarding;

	GameObject meteorSprite;

	public GameObject explodingMeteor;
	public GameObject attackedEffect;

	MeteorSpawnerScript MeteorSpawner;

	public RuntimeAnimatorController tailCtrl;
	public RuntimeAnimatorController slowTailCtrl;

    public AudioClip explosao;

	private int CollisionCount;


	bool onFire = true;

	GameObject cauda;

	Vector3 lastPos;
	bool rebatido;


	// Use this for initialization
	void Start () {
		//Carrega MeteorSpawner e inicializa variaveis
		MeteorSpawner = GameObject.Find("MeteorSpawner").GetComponent<MeteorSpawnerScript>();
		meteorSprite = transform.Find("sprite").gameObject;
        cauda = transform.Find("cauda").gameObject;
        lastPos = transform.position;
		onFire = true;
		

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
	public void ShowTail(float n){
		//Faz cauda reaparecer apos rabada
		onFire = true;
		cauda.SetActive(true);
		rebatido = true;

		RotateTail();

		GameObject g = Instantiate(attackedEffect, transform.position, 
			Quaternion.identity) as GameObject;
		if(n<0)
			g.transform.localScale = new Vector3(-g.transform.localScale.x,g.transform.localScale.y,g.transform.localScale.z);
		Destroy (g,1f);
	}
	public void RotateTail(){
		//Rotaciona cauda
		Vector3 velNorm = GetComponent<Rigidbody>().velocity.normalized;
		cauda.transform.rotation = Quaternion.Euler(0,0,Mathf.Rad2Deg * (Mathf.Atan2(velNorm.y, velNorm.x) + Mathf.PI/2) - transform.rotation.z);
	}

	public void Explode(){
        //Explode meteoro
        SoundController.instance.RandomizeSfx(explosao);
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
