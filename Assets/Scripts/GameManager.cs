using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public PlayerManager playerManager;
    public Spawner spawner;
    public AudioSource musicSource;
    public AudioClip musicClip;

    private UIManager uIManager;

	private List<Plate> plates = new List<Plate>();
	private List<ChoppingBoard> choppingBoards = new List<ChoppingBoard>();

	private void Awake(){
		uIManager = GetComponent<UIManager>();
	}

	private void Start() {
		GameObject[] plateObjects = GameObject.FindGameObjectsWithTag(Tags.PLATE);
		foreach(GameObject obj in plateObjects){
			plates.Add(obj.GetComponent<Plate>());
		}

		GameObject[] choppingBoardObjects = GameObject.FindGameObjectsWithTag(Tags.CHOPPING_BOARD);
			foreach(GameObject obj in choppingBoardObjects){
			choppingBoards.Add(obj.GetComponent<ChoppingBoard>());
		}

		StopGame();
		uIManager.DisplayMainMenu();
	}

	private void Update() {
		if(uIManager.State == UIManager.MenuState.GAME_PANEL && !playerManager.CanPlayersMove){
			StopGame();
			uIManager.FinalPlayerOneScore = playerManager.PlayerOneScore;
			uIManager.FinalPlayerTwoScore = playerManager.PlayerTwoScore;
			uIManager.DisplayEnd();
		}
	}

	private void ResetGame(){
		playerManager.Reset();
		spawner.Reset();
		foreach(ChoppingBoard board in choppingBoards) board.Reset();
		foreach(Plate plate in plates) plate.Reset();
	}

	private void StopGame(){
		playerManager.StopPlayers(true);
		spawner.ToSpawn = false;
        musicSource.Stop();
	}

	public void StartGame(){
		ResetGame();
        musicSource.clip = musicClip;
        musicSource.Play();
        playerManager.StopPlayers(false);
		spawner.ToSpawn = true;
	}
}
