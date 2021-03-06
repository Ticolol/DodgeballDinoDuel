﻿ using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public enum PlayerN {Player1, Player2}
	public enum Side {Right, Left}

	const float SKIN = .1f;
	const float MINIMUMSPEEDX = .05f;

	public PlayerN player = PlayerN.Player1;

	public float GRAVITYACCEL = .7f;
	public float GRAVITYMAX = .5f;
	public float WALKSPEEDMAX = .2f;
	public float WALKACCEL = 1f;
	public float DRAG = .7f; //fator multiplicativo
	public float JUMPFORCE = .15f;
	public float JUMPACCEL = .7f;
	public float JUMPDURMAX = .3f; //em segundos
	public float JUMPSMAX = 2; //vezes
	public float CROUCHHEIGHT = .35f; // porcentagem
	public float CROUCHWIDTH = 2f; // porcentagem
	public float ATTACKDURMAX = .3f; // em segundos
	public float ATTACKCOOLDOWN = .2f; // em segundos

	public bool Vencedor;
	public int knockedMeteors = 0;

	public LayerMask scenario;

	public RuntimeAnimatorController animtP1;
	public RuntimeAnimatorController animtP2;

	public AudioClip jumpOne;
	public AudioClip jumpTwo;
	public AudioClip andando;

	Vector3 originalScale;
	GameObject tailWhipHitBox;
	Vector3 tailWhipPos;
	GameObject body;
	Animator dino;
	Vector3 dinoScale;

	Vector3 velocity;
	bool onGround;
	float jumpDur;
	bool jumpCtrlAllowed;
	bool crouched;
	bool canJump;
	int jumpTimes;
	Side side;
	bool attacking;
	float attackDur;
	bool attackCoolDown;

	public bool allowMove;

	string getUp, getDown, getLeft, getRight, getAttack;

	// Use this for initialization
	void Start () {
		tailWhipHitBox = transform.Find("TailWhipHitBox").gameObject;
		body = transform.Find("Body").gameObject;
		dino = transform.Find("RotDino/Dino").gameObject.GetComponent<Animator>();

		tailWhipPos = new Vector3(tailWhipHitBox.transform.localPosition.x, tailWhipHitBox.transform.localPosition.y, tailWhipHitBox.transform.localPosition.z);
		originalScale = new Vector3(body.transform.localScale.x * transform.localScale.x,
									body.transform.localScale.y * transform.localScale.y,
									body.transform.localScale.z * transform.localScale.z);
		print(originalScale);
		scenario = LayerMask.GetMask("Ground");

		if(player == PlayerN.Player1){
			dino.runtimeAnimatorController = animtP1;
		}else{
			dino.runtimeAnimatorController = animtP2;
		}

        //Setar keycode pros comandos do jogador
		if(player == PlayerN.Player2){
			getUp = "Jump2";
			getDown = "Crouch2";
			getLeft = "Horizontal2";
			getRight = "Horizontal2";
			getAttack = "Attack2";
		}else if(player == PlayerN.Player1){
			getUp = "Jump1";
			getDown = "Crouch1";
			getLeft = "Horizontal1";
			getRight = "Horizontal1";
			getAttack = "Attack1";
		}

		//Initiate();
	}

	public void Initiate(){
		Vencedor = true;
		allowMove = true;
		body.GetComponent<PlayerBody>().Initiate();
		tailWhipHitBox.SetActive(false);

		if(player == PlayerN.Player1){
			side = Side.Right;
		}else{
			side = Side.Left;
		}

		AnimSide();

		dino.SetBool("Andando", false);
		dino.SetBool("Atacando", false);
		dino.SetBool("Agachado", false);
		dino.SetBool("Pulando", false);
		dino.SetBool("Atingido", false);
		dino.SetBool("Morto", false);

		velocity = new Vector3(0,0,0);
		onGround = false;
		jumpDur = 0;
		jumpCtrlAllowed = false;
		crouched = false;
		canJump = false;
		jumpTimes = 0;
        attacking = false;
        attackDur = 0;
        attackCoolDown = false;

		if(player == PlayerN.Player2){
			transform.position = new Vector3(5,-3.1f,0);
		}else{
			transform.position = new Vector3(-5,-3.1f,0);
		}
	}

	// Update is called once per frame
	void Update () {

		//Agachamento e fast fall
		if(Input.GetAxis(getDown) < 0 && !attacking && allowMove){
			if(onGround){
				//Agachar
				dino.SetBool("Agachado", true);
				body.transform.localScale = new Vector3(originalScale.x * CROUCHWIDTH, originalScale.y * CROUCHHEIGHT, originalScale.z);
				if(!crouched){ //Se eh o 1o frame do agachamento, andar metade do tamanho para baixo pra encostar no chao
					transform.position = transform.position - Vector3.up * ((originalScale.y * (1 - CROUCHHEIGHT)/2) - SKIN - .4f);
					dino.transform.localPosition = dino.transform.localPosition + Vector3.up * originalScale.y * (1 - CROUCHHEIGHT)/2;
				}
				crouched = true;
			}else {
				//Fastfall
				velocity.y = -GRAVITYMAX * Time.deltaTime;
			}
		}else if (crouched && !attacking){//Voltar ao normal
			dino.SetBool("Agachado", false);
			transform.position = transform.position + Vector3.up * ((originalScale.y * (1 - CROUCHHEIGHT)/2) + SKIN + .4f);
			body.transform.localScale = originalScale;
			dino.transform.localPosition = dino.transform.localPosition - Vector3.up * originalScale.y * (1 - CROUCHHEIGHT)/2;
			crouched = false;
		}

		//Andar lateralmente
		if(Input.GetAxis(getRight) > 0 && !crouched && allowMove){
			//Tocar som
			if(onGround){
				if(!GetComponent<AudioSource>().isPlaying){
					SoundController.instance.PlayLoop(andando, GetComponent<AudioSource>());
				}
			}else{
				if(GetComponent<AudioSource>().isPlaying){
					SoundController.instance.StopLoop(GetComponent<AudioSource>());
				}
			}

			if(!attacking)//se atacar, nao virar personagem
				side = Side.Right;
			dino.SetBool("Andando", true);
			velocity.x += WALKACCEL * Time.deltaTime;
			if(velocity.x < 0)//Se velocidade eh oposta a aceleracao, aplicar drag para muda-la mais rapido
				velocity.x *= DRAG;
		}else if(Input.GetAxis(getLeft) < 0 && !crouched && allowMove){
			//Tocar som
			if(onGround){
				if(!GetComponent<AudioSource>().isPlaying){
					SoundController.instance.PlayLoop(andando, GetComponent<AudioSource>());
				}
			}else{
				if(GetComponent<AudioSource>().isPlaying){
					SoundController.instance.StopLoop(GetComponent<AudioSource>());
				}
			}

			if(!attacking)//se atacar, nao virar personagem
				side = Side.Left;
			dino.SetBool("Andando", true);
			velocity.x += -WALKACCEL * Time.deltaTime;
			if(velocity.x > 0)//Se velocidade eh oposta a aceleracao, aplicar drag para muda-la mais rapido
				velocity.x *= DRAG;
		}else {
			SoundController.instance.StopLoop(GetComponent<AudioSource>());
			dino.SetBool("Andando", false);
			if(Mathf.Abs(velocity.x) < MINIMUMSPEEDX){//Se velocidade menor que o minimo, zera-la
				velocity.x = 0;
			}else{//senao, aplicar arrasto
				velocity.x *= DRAG * Time.deltaTime;
			}
		}
		AnimSide();
		//Corrigir possiveis ruins
		if(Mathf.Abs (velocity.x) > WALKSPEEDMAX * Time.deltaTime){//Se andar rapido demais, abaixe pro limite de vel
			velocity.x = Mathf.Sign (velocity.x) * WALKSPEEDMAX * Time.deltaTime;
		}

		//Aplicando gravidade
		velocity.y += -GRAVITYACCEL * Time.deltaTime;

		if(Mathf.Abs(velocity.y) > GRAVITYMAX * Time.deltaTime){// se vel > vel terminal, corrigi-la
			velocity.y = Mathf.Sign(velocity.y) * GRAVITYMAX * Time.deltaTime;
		}
		//Checando colisao com o chao
		Ray r1 = new Ray(transform.position
		 					- Vector3.up * (body.GetComponent<Collider>().bounds.size.y/2 + .2f),
		                 	- Vector3.up);//Raycasting do pe do dinossauro pra baixo
		RaycastHit h1 = new RaycastHit();
		Debug.DrawLine(r1.origin, r1.origin + r1.direction * Mathf.Abs(velocity.y));
		print(body.GetComponent<Collider>().bounds.size.y);
		if(Physics.Raycast(r1, out h1, Mathf.Abs(velocity.y), scenario)){//Se relou no chao, esta tocando
			dino.SetBool("Pulando", false);
			onGround = true;
			canJump = true;
			jumpTimes = 0;
		}else{//nao relou no chao
			onGround = false;
		}
		//PULAR!!!
		if(Input.GetAxis(getUp) > 0 && !crouched && allowMove){
			dino.SetBool("Agachado", false);
			if(onGround){//Se no chao, aplicar 1o pulo
				SoundController.instance.PlaySingle(jumpOne);
				dino.SetBool("Pulando", true);
				velocity.y += JUMPFORCE;
				onGround = false;
				canJump = false;
				jumpTimes += 1;
				jumpDur = 0;
				jumpCtrlAllowed = true;
			}else if( canJump && jumpTimes < JUMPSMAX){//Se no ar, puder pular e tiver pulos sobrando, pular
				SoundController.instance.PlaySingle(jumpTwo);
				dino.SetBool("Pulando", true);
				dino.SetBool("Agachado", true);
				velocity.y = JUMPFORCE;
				onGround = false;
				canJump = false;
				jumpTimes += 1;
				jumpDur = 0;
				jumpCtrlAllowed = true;
			}else if(jumpCtrlAllowed){//se puder controlar, controlar a extensao do pulo
				if(jumpDur < JUMPDURMAX){
					velocity.y += JUMPACCEL * Time.deltaTime;
				}else{
					jumpCtrlAllowed = false;
				}
				jumpDur += Time.deltaTime;
			}
		}else{//Nao apertou o botao, entao pode pular novamente
			jumpCtrlAllowed = false;
			canJump = true;
			jumpDur = 0;

		}
		//Debug.Log(velocity.y);

		//Raycasting em Y
		r1 = new Ray(transform.position
		 					- Vector3.up * (body.GetComponent<Collider>().bounds.size.y/2 + .2f),
		                 	-Vector3.up);//Raycasting do pe do dinossauro pra baixo
		h1 = new RaycastHit();
		if(Physics.Raycast(r1, out h1, Mathf.Abs(velocity.y), scenario)){//checa se raycast bateu no chao
			float dist = Vector3.Distance(r1.origin, h1.point);//pega distancia pro chao
			if(velocity.y < 0)//Se estiver caindo
				if(dist < SKIN){//se dist < SKIN, ja estamos no chao, entao zeramos a vely
					velocity.y = 0;
				}else{//senao, percorra soh o bastante para relar no chao
					velocity.y = -dist + SKIN;
				}
		}

		//Aplicando velocidades
		transform.Translate(velocity);
        if(transform.position.x < -12)
            transform.position = new Vector3(-12f, transform.position.y, transform.position.z);
        if(transform.position.x > 12)
            transform.position = new Vector3(12f, transform.position.y, transform.position.z);

		//ATTACK
		if(Input.GetButton(getAttack) && !attacking && !attackCoolDown && allowMove){//Pegando o input, começa o ataque
			attacking = true;
			attackDur = 0;
			//Acertar anim
			dino.SetBool("Atacando", true);
			//Virar pro lado certo
			if(side == Side.Right){
				tailWhipHitBox.transform.localPosition = tailWhipPos;
			}else{
				tailWhipHitBox.transform.localPosition = new Vector3(-tailWhipPos.x, tailWhipPos.y, tailWhipPos.z);
			}
			tailWhipHitBox.SetActive(true);
			tailWhipHitBox.gameObject.GetComponent<TailWhip>().StartAttack();
		}
		if(attacking){//Se atacando, atualizar tempo de ataque
			attackDur += Time.deltaTime;
			if(attackDur >= ATTACKDURMAX){//Se tempo de ataque estourar, finaliza-lo
				dino.SetBool("Atacando", false);
				tailWhipHitBox.SetActive(false);
				tailWhipHitBox.gameObject.GetComponent<TailWhip>().FinishAttack();
				attacking = false;
				attackCoolDown = true;
			}
		}else if(attackCoolDown){
			attackDur += Time.deltaTime;
			if(attackDur >= ATTACKDURMAX + ATTACKCOOLDOWN){//Apos tempo de cooldown, liberar novamente o ataque
				attackCoolDown = false;
			}
		}
	}

	void AnimSide(){
		if(side == Side.Right){
			dino.transform.parent.localScale = new Vector3(Mathf.Abs(dino.transform.parent.localScale.x),dino.transform.parent.localScale.y,dino.transform.parent.localScale.z);
		}else{
			dino.transform.parent.localScale = new Vector3(-Mathf.Abs(dino.transform.parent.localScale.x),dino.transform.parent.localScale.y,dino.transform.parent.localScale.z);
		}
	}

//	void OnTriggerStay(Collider c){
//		Debug.Log ("AAAAAAAAAAAAAAAAHHHHHHHHHHHHH!!!!!!!!!!!!");
//
//
//
//	}
}
