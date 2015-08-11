using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public MeteorSpawnerScript meteorSpawnerScript;
	public Player player1;
	public Player player2;
	public LifeCounter lifeCounter1;
	public LifeCounter lifeCounter2;

	public GameObject IntroductionGroup;
	public GameObject FaixaH1;
	public GameObject FaixaH2;
	public GameObject FaixaV;

	public StatisticsController StatisticsCanvas;


	// Use this for initialization
	void Start () {
		BlockEveryone();
		StartCoroutine("AnimateEntrance");


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BlockEveryone(){
		player1.allowMove = false;
		player2.allowMove = false;
		meteorSpawnerScript.allowSpawn = false;
	}
	
	public void FinishRound(Player.PlayerN loser){
		//		InitiateRound(); //Tirar depois. Ative as animacoes de placar ==========================================
		StatisticsCanvas.DisplayStatistics(); 
	}

	public void InitiateRound(){
		player1.Initiate();
		player2.Initiate();
		meteorSpawnerScript.Initiate();
		lifeCounter1.Initiate();
		lifeCounter2.Initiate();
	}

	IEnumerator AnimateEntrance() {
		float PositionModifierH = -16;
		float PositionModifierV = -9;//372/151
		
		while (PositionModifierH < -6.5){
			PositionModifierH += Time.deltaTime * 15;
			PositionModifierV += Time.deltaTime * 18f;
			FaixaH1.transform.position = new Vector2(PositionModifierH , PositionModifierH * 0.4f);
			FaixaH2.transform.position = new Vector2(-PositionModifierH , PositionModifierH * 0.4f);
			FaixaV.transform.position = new Vector2(0, PositionModifierV);
			yield return null;
			
		}
		while (PositionModifierH < -6){
			PositionModifierH += Time.deltaTime * 0.25f;
			PositionModifierV += Time.deltaTime * 0.15f;
			FaixaH1.transform.position = new Vector2(PositionModifierH , PositionModifierH * 0.4f);
			FaixaH2.transform.position = new Vector2(-PositionModifierH , PositionModifierH * 0.4f);
			FaixaV.transform.position = new Vector2(0, PositionModifierV);
			yield return null;
		}
		while (PositionModifierH < 16){
			PositionModifierH += Time.deltaTime * 15;
			PositionModifierV += Time.deltaTime * 10f;
			FaixaH1.transform.position = new Vector2(PositionModifierH , PositionModifierH * 0.4f);
			FaixaH2.transform.position = new Vector2(-PositionModifierH , PositionModifierH * 0.4f);
			FaixaV.transform.position = new Vector2(0, PositionModifierV);
			yield return null;
		}
		Destroy(IntroductionGroup);
		InitiateRound();
		
	}
}
