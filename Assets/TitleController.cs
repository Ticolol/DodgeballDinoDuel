using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

	public GameObject recado;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame(){
		Application.LoadLevel("Game");
	}

	public void GoToCred(){
		Application.LoadLevel("Credits");
	}

	public void ToggleMute(){
		SoundController s = GameObject.FindObjectOfType<SoundController>();
		s.ToggleMute();

		if(s.mute){
			//Mandar mensagem do coleguinha
			recado.transform.position = new Vector3(0,0,-1);

			foreach( Button a in UnityEngine.Object.FindObjectsOfType<Button>())
				if(a.gameObject != recado)
					a.enabled = false;
		}
	}

	public void Exit(){
		Application.Quit();
	}

	public void HideRecado(){
		//Esconder mensagem do coleguinha
		recado.transform.position = new Vector3(0,10,-1);

		foreach( Button a in UnityEngine.Object.FindObjectsOfType<Button>())
				a.enabled = true;
	}
}
