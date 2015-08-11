using UnityEngine;
using System.Collections;

public class MeteorSpawnerScript : MonoBehaviour {

	public int INITIALEXPECTEDMETEORS = 1;
	public float SpawnHeight;
	public float groundHeight;
	public float ScreenLimitSafeDistance;

	public float InitialMaxXSpeed;
	public float InitialMaxYSpeed;
	public float XDifficultyIncrease;
	public float YDifficultyIncrease;
	

	public Camera GameCamera;
	public GameObject MeteorPrefab;

	public float BeginTime;
//	private float LastSpawn;
//	private float LastDestroyed;

	private int CurrentMeteors;
	public int ExpectedMeteors;

	private float timeToNewExpectedMeteor;
	private float lastNewExpectedMeteor;

	public static GameObject SpawnerInstance;

	public bool allowSpawn;

	// Use this for initialization
	void Start () {
		SpawnerInstance = gameObject;

//		Initiate();
	}

	public void Initiate(){
		allowSpawn = true;
		BeginTime = Time.time;

		foreach (GameObject m in GameObject.FindGameObjectsWithTag("Meteor"))
			Destroy(m);
		
		CurrentMeteors = 0;
		ExpectedMeteors = INITIALEXPECTEDMETEORS;
		
		timeToNewExpectedMeteor = 3;
		lastNewExpectedMeteor = Time.time;
		
		SpawnMeteor ();
	}
	
	// Update is called once per frame
	void Update () {
		if(allowSpawn){
			//if ((Time.time - LastDestroyed > 8) && (Time.time - LastSpawn) > 5){
			if ((Time.time - lastNewExpectedMeteor) > timeToNewExpectedMeteor){
				ExpectedMeteors++;
				timeToNewExpectedMeteor = Random.Range(10,30);
				lastNewExpectedMeteor = Time.time;
			}
			if (CurrentMeteors < ExpectedMeteors){
				SpawnMeteor();
			}
		}
	}

	void SpawnMeteor() {
		//Conseguir os limites da tela:
		float ScreenLimitX = GameCamera.orthographicSize * Screen.width / Screen.height;
		float ScreenLimitY = GameCamera.orthographicSize;
		
		//SOrtear uma velocidade inicial:
		float ActualMaxXSpeed = InitialMaxXSpeed + XDifficultyIncrease*Mathf.Log(Time.time - BeginTime + 1);

		float VelX = Random.Range (-ActualMaxXSpeed,ActualMaxXSpeed);
		float VelY = Random.Range(-InitialMaxYSpeed, -YDifficultyIncrease*Mathf.Log(Time.time - BeginTime + 1));
		
		//Calcular tempo de voo do meteoro:
		float gravity = Physics.gravity.magnitude;
		float height = 2 * ScreenLimitY - groundHeight;

		float fallTimeFull = (VelY + Mathf.Sqrt(VelY * VelY + 2*gravity*(height + SpawnHeight)))/gravity;
		float fallTimeHalf = (VelY + Mathf.Sqrt(VelY * VelY +   gravity*(height + SpawnHeight)))/gravity;

		//Calcular a faixa de posicao X que o meteoro pode spawnar sem aterrisar fora da tela:
		float MinX, MaxX;
		if (VelX < 0){
			MinX = -ScreenLimitX + ScreenLimitSafeDistance - VelX * fallTimeFull;
			MaxX = ScreenLimitX - ScreenLimitSafeDistance - VelX * fallTimeHalf;
		}else{
			MinX = -ScreenLimitX + ScreenLimitSafeDistance - VelX * fallTimeHalf;
			MaxX = ScreenLimitX - ScreenLimitSafeDistance - VelX * fallTimeFull;
		}
		
		//Sortear uma posição X para spawnar o meteoro:
		float SpawnX = Random.Range(MinX,MaxX);
		
		GameObject newMeteor = (GameObject) Instantiate(MeteorPrefab,new Vector3 (SpawnX,ScreenLimitY + SpawnHeight,0), Quaternion.identity);
		newMeteor.GetComponent<Rigidbody>().velocity = new Vector3(VelX,VelY,0);
		
		CurrentMeteors++;
//		LastSpawn = Time.time;
	}

	public void MeteorDestroyed() {
		CurrentMeteors--;
//		LastDestroyed = Time.time;

	}

	public int getMaxMeteors() {
		return ExpectedMeteors;
	}
}
