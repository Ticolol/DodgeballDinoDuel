using UnityEngine;
using System.Collections;

public class PlayerBody : MonoBehaviour {

	public float SLOWMODUR = 1f;
	public float DAMAGEDUR = .3f;
	public int LIFESMAX = 2;

	Player father;
	GameController gameController;
	LifeCounter lifeCounter;
	Animator dino;

	float time;
	int lifes;
	
	// Use this for initialization
	void Start () {
		father = transform.parent.gameObject.GetComponent<Player>();
		gameController = GameObject.Find("!GameController").GetComponent<GameController>();
		dino = father.transform.Find("RotDino/Dino").gameObject.GetComponent<Animator>();
		
		if(father.player == Player.PlayerN.Player1){
			lifeCounter = gameController.lifeCounter1.GetComponent<LifeCounter>();
		}else{
			lifeCounter = gameController.lifeCounter2.GetComponent<LifeCounter>();
		}

		Initiate();
	}

	public void Initiate(){
		lifes = LIFESMAX;
		time = 0;

		dino.SetBool("Atingido", false);
		dino.SetBool("Morto", false);

	}
	
	// Update is called once per frame
	void Update () {

			if(Time.timeScale == .2f){
				time += Time.deltaTime;

				if(time >= SLOWMODUR){
					Time.timeScale = 1;
					//Chamar o pai de todos
					gameController.FinishRound(father.player);
	            }
			}else if(father.allowMove){
				time += Time.deltaTime;
				if(time >= DAMAGEDUR){
					dino.SetBool("Atingido", false);
					time = 0;
				}
			}
	}

	void OnTriggerEnter(Collider c){
		if(c.gameObject.tag == "Meteor" && father.allowMove){
			//MORREU
			lifes -= 1;

			lifeCounter.LoseLife(); 

			if(lifes==0){
				//Bloquear todo mundo
				gameController.BlockEveryone();
				//
				dino.SetBool("Morto", true);
				//ZaWarudo
				time = 0;
				Time.timeScale = .2f;
				father.Vencedor = false;
			}else{
				time = 0;
				Debug.Log("Apanhou aqui");
				dino.SetBool("Atingido", true);


			}
		}
	}
}
