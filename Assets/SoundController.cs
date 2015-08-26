using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	public bool mute = false;

	private static SoundController _instance;

	bool updateMute;

	public static SoundController instance {
		get {
			if(_instance == null){
				_instance = GameObject.FindObjectOfType<SoundController>();
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}

	void Awake(){
		if(_instance == null){
			_instance = this;
			DontDestroyOnLoad(this);
		}else{
			if(this != _instance){
				Destroy(this.gameObject);
			}
		}

		updateMute = true;
	}



	// Use this for initialization
	void OnLevelWasLoaded(int level){
		updateMute = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(updateMute){
			updateMute = false;
			ApplyMuteValue();
		}
	}

	public void ToggleMute(){
		mute = !mute;
		
		ApplyMuteValue();
	}

	public void ApplyMuteValue(){
		foreach( AudioSource a in UnityEngine.Object.FindObjectsOfType<AudioSource>())
			a.mute = mute;
	}

}
