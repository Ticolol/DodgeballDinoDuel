using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	public Sprite normalSprite;
	public Sprite overSprite;
	public Sprite pressedSprite;

	public GameObject objectOwner;
    public string scriptName;
	public string functionName;
	public bool enabled = true;
	public bool temporaryBlockOnClick = false;
	public AudioClip clickFX;
	
	float time = 0;
	AudioSource AS;
	bool pressed;
	
	//Fazer uma booleana "pressed"
	void Awake(){
		if(normalSprite == null)
			normalSprite = GetComponent<SpriteRenderer>().sprite;
		if(overSprite == null)
			overSprite = normalSprite;
		if(pressedSprite == null)
			pressedSprite = normalSprite;
    }
	
	void Start () {
		time = 0;

		if(clickFX == null)
			clickFX = (AudioClip)Resources.Load("Botoes", typeof(AudioClip));
		
		GetComponent<SpriteRenderer>().sprite = normalSprite;

		AS = null;
		AS = GetComponent<AudioSource>();
		if(AS == null){
			gameObject.AddComponent<AudioSource>();
			AS = GetComponent<AudioSource>();
			AS.playOnAwake = false;
			AS.loop = false;
			AS.clip = clickFX;
		}
		pressed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(temporaryBlockOnClick){
			if(time > 5){
				enabled = true;
				GetComponent<SpriteRenderer>().sprite = normalSprite;
                
            }else{
				time += Time.deltaTime;
			}
		}
		//Debug.Log(pressed);
	}
    
	void OnMouseDown(){
		if(enabled){
			//Apertou
			GetComponent<SpriteRenderer>().sprite = pressedSprite;
			pressed = true;
            if(scriptName.Length != 0){
				MonoBehaviour temp = (MonoBehaviour) objectOwner.GetComponent(scriptName);
				if(functionName != null){
					temp.Invoke(functionName, 0);
				}

				GetComponent<AudioSource>().PlayOneShot(clickFX);
				
				if(temporaryBlockOnClick){
					enabled = false;
					time = 0;
				}
				
				
			}
			
		}	
	}

	void OnMouseEnter(){
		if(!pressed && enabled){
			GetComponent<SpriteRenderer>().sprite = overSprite;
        }
	}

	void OnMouseExit(){
		if(enabled)
			if(!pressed){
				GetComponent<SpriteRenderer>().sprite = normalSprite;
	        }else{
				GetComponent<SpriteRenderer>().sprite = overSprite;
	        }
    }
    
    void OnMouseUp(){
		if(enabled){
			pressed = false;
			GetComponent<SpriteRenderer>().sprite = normalSprite;
		}
    }
    
    
}
