using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LanguageUpdater : MonoBehaviour {

	public Sprite EN_Sprite;
	Sprite PT_Sprite;

	public Sprite EN_Highlighted;
	public Sprite EN_Pressed;
	Sprite PT_Highlighted;
	Sprite PT_Pressed;

	public string EN_Text;
	string PT_Text;

	public Sprite[] EN_AssistSprites;
	Sprite[] PT_AssistSprites;

	SpriteRenderer spriteRenderer;
	Image image;

	Button button;
	UnityEngine.UI.Button uibutton;

	Text text;

	void Awake(){
		//Pegar o component que detem o sprite
		spriteRenderer = GetComponent<SpriteRenderer>();
		image = GetComponent<Image>();
		if(spriteRenderer != null){
			//Usar SpriteRenderer
			PT_Sprite = spriteRenderer.sprite;
		}else if(image != null){
			//Usar UI Image
			PT_Sprite = image.sprite;
		}
		
		//Pegar button, se existir
		button = GetComponent<Button>();
		uibutton = GetComponent<UnityEngine.UI.Button>();
		if(button != null){
			//Usar Button
			Sprite[] btnSprites = button.GetSprites();
			PT_Sprite = btnSprites[0];
			PT_Highlighted = btnSprites[1];
			PT_Pressed = btnSprites[2];
		}else if(uibutton != null){
			//Usar UI Button
			SpriteState ss = uibutton.spriteState;
			PT_Sprite = ss.disabledSprite;
			PT_Highlighted = ss.highlightedSprite;
			PT_Pressed = ss.pressedSprite;
		}
		
		//Pegar texto, se existir
		text = GetComponent<Text>();
		if(text != null){
			PT_Text = text.text;
		}
	}

	public void UpdateLang(LanguageController.Language lang){
		//Mudar Sprite
		if(lang == LanguageController.Language.Portuguese){
			ChangeSprite(PT_Sprite);
		}else if(lang == LanguageController.Language.English){
			ChangeSprite(EN_Sprite);
		}

		//Mudar buttons
		if(button != null || uibutton != null){
			if(lang == LanguageController.Language.Portuguese){
				ChangeButton(PT_Sprite, PT_Highlighted, PT_Pressed);
			}else if(lang == LanguageController.Language.English){
				ChangeButton(EN_Sprite, EN_Highlighted, EN_Pressed);
			}
		}

		//Mudar Texts
		if(text != null){
			if(lang == LanguageController.Language.Portuguese){
				text.text = PT_Text;
			}else if(lang == LanguageController.Language.English){
				text.text = EN_Text;
			}		
		}
	}
	
	void ChangeSprite(Sprite sprite){
		if(spriteRenderer != null){
			//Mudar SpriteRenderer
			spriteRenderer.sprite = sprite;
		}else if(image != null){
			//Mudar UI Image
			image.sprite = sprite;
		}
	}

	void ChangeButton(Sprite disabled, Sprite highlighted, Sprite pressed){
		if(button != null){
			//Mudar Button
			button.UpdateSprites(disabled, highlighted, pressed);
		}else if(uibutton != null){
			//Mudar UI Button
			SpriteState ss = new SpriteState();
			ss.disabledSprite = disabled;
			ss.highlightedSprite = highlighted;
			ss.pressedSprite = pressed;
			uibutton.spriteState = ss;
		}
	}


	public void SetAssistSprites(Sprite[] sprites, LanguageController.Language lang){
		if(lang == LanguageController.Language.Portuguese){
			PT_AssistSprites = sprites;
		}else if(lang == LanguageController.Language.English){
			EN_AssistSprites = sprites;
		}
	}

	public Sprite[] GetAssistSprites(LanguageController.Language lang){
		if(lang == LanguageController.Language.Portuguese){
			return PT_AssistSprites;
		}else if(lang == LanguageController.Language.English){
			return EN_AssistSprites;
		}
		return null;
	}
}
