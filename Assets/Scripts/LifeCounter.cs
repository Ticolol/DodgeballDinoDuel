using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeCounter : MonoBehaviour {
	
	public Player.PlayerN player;
	public Sprite lifeRemaining;
	public Sprite lifeLost;
	
	GameController gameController;
	PlayerBody playerBody;

	// Use this for initialization
	void Start () {
		gameController = GameObject.Find("!GameController").GetComponent<GameController>();

//		if(player == Player.PlayerN.Player1){
//			playerBody = gameController.player1.transform.Find("Body").GetComponent<PlayerBody>();
//		}else{
//			playerBody = gameController.player2.transform.Find("Body").GetComponent<PlayerBody>();
//		}

		Initiate();
	}

	public void Initiate(){
		//Setar valores iniciais
		foreach(Image i in GetComponentsInChildren<Image>()){
			i.sprite = lifeRemaining;
		}
	}

	public void LoseLife(){
		foreach(Image i in GetComponentsInChildren<Image>()){
			if(i.sprite == lifeRemaining){
				i.sprite = lifeLost;
				break;
			}
		}
	}

}
