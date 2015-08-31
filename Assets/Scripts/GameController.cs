 using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public MeteorSpawnerScript meteorSpawnerScript;
	public Player player1;
	public Player player2;
	public LifeCounter lifeCounter1;
	public LifeCounter lifeCounter2;

	public GameObject IntroductionGroup;
	//public GameObject FaixaH1;
	//public GameObject FaixaH2;
	public GameObject dilofo;
	public GameObject f3;
	public GameObject f2;
	public GameObject f1;

	public StatisticsController StatisticsCanvas;

	public AudioClip musicIntro;
	public AudioClip musicLoop;
	bool start;

	// Use this for initialization
	void Start () {
		start = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(start){
			start = false;
			StartRound();
		}
        
    }

	public void BlockEveryone(){
		player1.allowMove = false;
		player2.allowMove = false;
		meteorSpawnerScript.allowSpawn = false;
	}
	
	public void FinishRound(Player.PlayerN loser){
		//InitiateRound(); //Tirar depois. Ative as animacoes de placar ==========================================
		StatisticsCanvas.DisplayStatistics(); 
	}

	public void StartRound(){
		SoundController.instance.PlayMusicWithIntro(musicIntro, musicLoop, GetComponent<AudioSource>());
		player1.Initiate();
		player2.Initiate();
		BlockEveryone();
        lifeCounter1.Initiate();
        lifeCounter2.Initiate();
		StartCoroutine("AnimateEntrance");
	}

	public void InitiateRound(){
		player1.Initiate();
		player2.Initiate();
		meteorSpawnerScript.Initiate();

	}

	IEnumerator AnimateEntrance() {
		float PositionModifierV;
		GameObject[] arr = {f3, f2, f1};
		IntroductionGroup.SetActive(true);

		foreach(GameObject f in arr){
			PositionModifierV = -9;	

			while (PositionModifierV < -0.2f){
				PositionModifierV += Time.deltaTime * 32f;
				if(PositionModifierV > -.2f)
					PositionModifierV = -.2f;
				f.transform.position = new Vector2(0, PositionModifierV);
				//Debug.Log("PARTE 1 ===============");
				yield return null;
				
			}
			while (PositionModifierV < .05f){
				PositionModifierV += Time.deltaTime * .6f;
				f.transform.position = new Vector2(0, PositionModifierV);
				//Debug.Log("PARTE 2 ===============");

				yield return null;
			}
			while (PositionModifierV < 10){
				PositionModifierV += Time.deltaTime * 32f;
				f.transform.position = new Vector2(0, PositionModifierV);
				//Debug.Log("PARTE 3 ===============");

				yield return null;
			}
			//Debug.Log(Time.time);
		}

		//DIFALOSSAUROOOO
		PositionModifierV = -9;	

		SoundController.instance.PlaySingle(dilofo.GetComponent<AudioSource>().clip);
		while (PositionModifierV < .5f){
			PositionModifierV += Time.deltaTime * 80f;
			if(PositionModifierV > .5f)
				PositionModifierV = .5f;
			dilofo.transform.position = new Vector2(0, PositionModifierV);
			//Debug.Log("PARTE 1 ===============");
			yield return null;
			
		}
		while (PositionModifierV < .65f){
			PositionModifierV += Time.deltaTime * .3f;
			dilofo.transform.position = new Vector2(0, PositionModifierV);
			//Debug.Log("PARTE 2 ===============");
			
			yield return null;
		}
		while (PositionModifierV < 12){
			PositionModifierV += Time.deltaTime * 40f;
			dilofo.transform.position = new Vector2(0, PositionModifierV);
			//Debug.Log("PARTE 3 ===============");
			
			yield return null;
		}

		IntroductionGroup.SetActive(false);
		InitiateRound();
		
	}

	public void Exit(){

		Application.Quit();
	}
}
