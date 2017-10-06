using UnityEngine;
using System.Collections;

public class PlayerBody : MonoBehaviour {

	public float SLOWMODUR = 1f;
	public float DAMAGEDUR = .3f;
	public int LIFESMAX = 3;
	public float BLINKINTERVAL = 0.1f;
	public float INVINCINTERVAL = 1.5f;

	public AudioClip morrendo;

	Player father;
	GameController gameController;
	LifeCounter lifeCounter;
	Animator dino;
	SpriteRenderer dinoSprite;

	float time;
	int lifes;
	bool blinking;
	float intervalTime;
	float invincibleTime;
	
	// Use this for initialization
	void Start () {
		father = transform.parent.gameObject.GetComponent<Player>();
		gameController = GameObject.Find("!GameController").GetComponent<GameController>();
		dino = father.transform.Find("RotDino/Dino").gameObject.GetComponent<Animator>();
		dinoSprite = dino.gameObject.GetComponent<SpriteRenderer>();
		
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
		blinking = false;
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

		//BLink do dino
		if(blinking){
			intervalTime += Time.deltaTime;
			invincibleTime += Time.deltaTime;

			if(intervalTime >= BLINKINTERVAL){
				intervalTime = 0;
				if(dinoSprite.color == new Color(1,1,1,1))
					dinoSprite.color = new Color(1,1,1,0.2f);
				else
					dinoSprite.color = new Color(1,1,1,1);
			}
			if(invincibleTime >= INVINCINTERVAL){
				blinking = false;
				dinoSprite.color = new Color(1,1,1,1);
			}

		}

	}

	void OnTriggerEnter(Collider c){
		if(c.gameObject.tag == "Meteor" && father.allowMove && !blinking){
			//PERDEU VIDA
			lifes -= 1;
			lifeCounter.LoseLife(); 

			if(lifes==0){
				//Bloquear todo mundo
				gameController.BlockEveryone();
				//Atualizando animaçao
				dino.SetBool("Morto", true);
				//Tocar somzim
				SoundController.instance.PlaySingle(morrendo);
				//ZaWarudo
				time = 0;
				Time.timeScale = .2f;
				father.Vencedor = false;
			}else{
				time = 0;
				dino.SetBool("Atingido", true);
				blinking = true;
				intervalTime = 0;
				invincibleTime = 0;

			}

			c.GetComponent<MeteorScript>().Explode();
		}
	}
}
