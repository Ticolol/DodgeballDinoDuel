using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LanguageController : MonoBehaviour {
	public enum Language {Portuguese, English}

	public static LanguageController instance = null;

	public Language language;

	bool updateLang;



	void Awake(){
		//Singleton
		if(instance == null)
			instance = this;
		else if(this != instance)
				Destroy(gameObject);
		DontDestroyOnLoad(this);

		updateLang = true;

	}

	void Update(){
		if(updateLang){
			updateLang = false;
			UpdateLang();
		}
		
	}


	void UpdateLang(){
		foreach(LanguageUpdater s in Object.FindObjectsOfType<LanguageUpdater>()){
			s.UpdateLang(language);
		}
	}

	void OnLevelWasLoaded(int level){
		updateLang = true;
	}

	public void ChangeLang(){
		if(language == Language.Portuguese)
			language = Language.English;
		else
			language = Language.Portuguese;

		updateLang = true;
	}
		/*
		string assetString = "";
		SpriteRenderer spriteRenderer;
		Image image;

		//Carregar string
		spriteRenderer = g.GetComponent<SpriteRenderer>();
		image = g.GetComponent<Image>();
		if(spriteRenderer != null)
			assetString = AssetDatabase.GetAssetPath(spriteRenderer.sprite);
		if(image != null)
			assetString = AssetDatabase.GetAssetPath(image.sprite);

		assetString = assetString.Remove(0, 17); //Remover "Assets/Resources/"
		assetString = assetString.Remove(assetString.Length - 4, 4); // Remover ".png"

		//Checar linguagem
		if(language == Language.Portuguese){
			//Checar se é de outra linguagem
			//EN
			if(assetString.EndsWith("_en.png")){
				Debug.Log("poe em portugues");//===============================================
			}
		}else if(language == Language.English){
			//Checar se é de outra linguagem
			//PT
			if(!assetString.EndsWith("_en.png")){
				//Recarregar assets em EN
				Translate(g, assetString, "_en");
			}
		}
	}

	public void Translate(GameObject g, string s, string langEnding){
		//CARREGAR ASSET
		Sprite[] sprites = new Sprite[3];
		sprites = Resources.LoadAll<Sprite>(
			s + langEnding);
		Debug.Log(sprites.Length);
		if(sprites.Length == 1){
			Debug.Log(s + langEnding);//, typeof(Sprite)) as Sprite;
			Sprite spriteAux = sprites[0];
			sprites = new Sprite[3];
			sprites[0] = spriteAux;
			sprites[1] = spriteAux;
			sprites[2] = spriteAux;
		}

		//Mudar SpriteRenderer
		SpriteRenderer spriteRenderer = g.GetComponent<SpriteRenderer>();
		if(spriteRenderer != null){
			spriteRenderer.sprite = sprites[0];
		}

		//Mudar UI Image
		Image image = g.GetComponent<Image>();
		if(image != null){
			image.sprite = sprites[0];
		}
		
		//Mudar Button
		Button button = g.GetComponent<Button>();
		if(button != null)
			button.UpdateSprites(sprites[0], sprites[1], sprites[2]);

		//Mudar UI Button
		UnityEngine.UI.Button uibutton = g.GetComponent<UnityEngine.UI.Button>();
		if(uibutton != null){
			SpriteState ss = new SpriteState();
			ss.disabledSprite = sprites[0];
			ss.highlightedSprite = sprites[1];
			ss.pressedSprite = sprites[2];
			uibutton.spriteState = ss;
		}
	}

	public Sprite UpdateMySprite(Sprite sprite){
		string s = AssetDatabase.GetAssetPath(sprite);

		s = s.Remove(0, 17); //Remover "Assets/Resources/"
		s = s.Remove(s.Length - 4, 4); // Remover ".png"

		string langEnding = "";
		if(language == Language.Portuguese){
			if(s.EndsWith("_en.png")){
				s.Remove(s.Length - 3, 3);
			}
		}else if(language == Language.English){
			langEnding = "_en";
		}

		sprite = Resources.Load(s + langEnding, typeof(Sprite)) as Sprite;
		return sprite;
	}*/

	/*
	[MenuItem("Custom/Find Sprite")]
	public static void FindSprite(){
	    var selected = Selection.activeGameObject;
	    if (selected == null) return;
	    var renderer = selected.GetComponent<SpriteRenderer>();
	    if (renderer == null) return;
	    Debug.Log(AssetDatabase.GetAssetPath(renderer.sprite));
	}
	*/


}
