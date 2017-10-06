using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatisticsController : MonoBehaviour {

	public GameObject resultPanel;
	public Sprite firstSprite;
	public Sprite secondSprite;

	public Text DurationTextDesc;
	public Text MaxMeteorTextDesc;
	public Text KnockedMeteor1TextDesc;
	public Text KnockedMeteor2TextDesc;

	public Text DurationText;
	public Text MaxMeteorText;
	public Text KnockedMeteor1Text;
	public Text KnockedMeteor2Text;
	public GameObject btnRestart;
	public GameObject btnExit;


	Player Player1;
	Player Player2;
	public MeteorSpawnerScript MeteorSpawner;

	private float Duracao;
	private int MeteorosRebatidos;
	private int MaximoMeteorosSimultaneos;

	private bool AlreadyChanged;
	private int phase;
	
	bool restart;
	int DestinationY;

	// Use this for initialization
	void Start() {
		phase = 0;
		resultPanel.transform.position = new Vector2(Screen.width/2f, -Screen.height * 0.5f);
		restart = false;

		//Setar sprites padrão da linguagem PT
		Sprite[] sprites = new Sprite[2];
		sprites[0] = firstSprite;
		sprites[1] = secondSprite;
		GetComponent<LanguageUpdater>().SetAssistSprites(sprites, LanguageController.Language.Portuguese);
		//Atualizar lingua dos assets
		sprites = GetComponent<LanguageUpdater>().GetAssistSprites(LanguageController.instance.language);
		firstSprite = sprites[0];
		secondSprite = sprites[1];


		Player1 = GameObject.Find("Player 1").GetComponent<Player>();
		Player2 = GameObject.Find("Player 2").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void DisplayStatistics (){
		//Debug.Log(DurationTextDesc.transform.position);
		

		if (Player2.Vencedor == true){
			transform.Find("ResultPanel").GetComponent<Image>().sprite = secondSprite;
			btnRestart.GetComponent<RectTransform>().anchoredPosition = new Vector3(-24,-143,0);
			btnExit.GetComponent<RectTransform>().anchoredPosition = new Vector3(354,167,0);
			DurationTextDesc.rectTransform.localPosition = new Vector3(-225,68,0);
			MaxMeteorTextDesc.rectTransform.localPosition = new Vector3(-180,12,0);
			KnockedMeteor1TextDesc.rectTransform.localPosition = new Vector3(-180,-38,0);
			KnockedMeteor2TextDesc.rectTransform.localPosition = new Vector3(-180,-90,0);
			
		}else {
			transform.Find("ResultPanel").GetComponent<Image>().sprite = firstSprite;
			btnRestart.GetComponent<RectTransform>().anchoredPosition = new Vector3(37,-153,0);
			btnExit.GetComponent<RectTransform>().anchoredPosition = new Vector3(-356,167,0);
			DurationTextDesc.rectTransform.localPosition = new Vector3(50,68,0);
			MaxMeteorTextDesc.rectTransform.localPosition = new Vector3(80,12,0);
			KnockedMeteor1TextDesc.rectTransform.localPosition = new Vector3(80,-38,0);
			KnockedMeteor2TextDesc.rectTransform.localPosition = new Vector3(80,-90,0);
		}
		DurationText.GetComponent<Text>().text = System.Math.Round((Time.time - MeteorSpawner.BeginTime),2).ToString();

		KnockedMeteor1Text.GetComponent<Text>().text = Player1.knockedMeteors.ToString();
		KnockedMeteor2Text.GetComponent<Text>().text = Player2.knockedMeteors.ToString();

		MaxMeteorText.GetComponent<Text>().text = MeteorSpawner.ExpectedMeteors.ToString();

		btnExit.GetComponent<UnityEngine.UI.Button>().interactable = true;

		StartCoroutine("StatisticRoutine");



	}
	
	IEnumerator StatisticRoutine () {
		float EndSlide;

		restart = false;
		//Primeira vez que a tela de estatisticas sobe:
		EndSlide = Screen.height * 0.5f;
		gameObject.SetActive(true);
		while(resultPanel.transform.position.y < EndSlide){
			resultPanel.transform.Translate(new Vector2(0,EndSlide * 2.0f * Time.deltaTime));
			yield return null;
		}

		while (!Input.GetKeyDown(KeyCode.Return) && !restart){
			yield return null;
		}

		//Descer a tela:
		EndSlide = -Screen.height * 0.5f;
		while(resultPanel.transform.position.y > EndSlide){
			resultPanel.transform.Translate(new Vector2(0, EndSlide * 2.0f * Time.deltaTime));
			yield return null;
		}

		GameObject.Find("!GameController").GetComponent<GameController>().StartRound();

		btnExit.GetComponent<UnityEngine.UI.Button>().interactable = false;
	}

	public void Restart(){
		restart = true;
	}

}
