using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

	public int maxPlayerSpeed;
	public GameObject playerOnePrefab, playerTwoPrefab;

	private T_Player playerOne, playerTwo;
	private Vector2 playerOneDirection, playerTwoDirection;
	private bool playerOneTryingToInteract, playerTwoTryingToInteract;
	public Text playerOneScore, playerTwoScore;
	public Text playerOneTime, playerTwoTime;

	private GameObject tempCreatedObject;

	private void Awake () {
		Initialize();
		UpdateUI();	
	}
	
	private void Update () {

		playerOneDirection = playerTwoDirection = Vector2.zero;
		playerOneTryingToInteract = playerTwoTryingToInteract = false;

		if(Input.GetKey(KeyCode.A)) playerOneDirection.x -= 1;
		if(Input.GetKey(KeyCode.D)) playerOneDirection.x += 1;
		if(Input.GetKey(KeyCode.W)) playerOneDirection.y += 1;
		if(Input.GetKey(KeyCode.S)) playerOneDirection.y -= 1;
		if(Input.GetKey(KeyCode.LeftShift)) playerOneTryingToInteract = true;

		if(Input.GetKey(KeyCode.LeftArrow)) playerTwoDirection.x -= 1;
		if(Input.GetKey(KeyCode.RightArrow)) playerTwoDirection.x += 1;
		if(Input.GetKey(KeyCode.UpArrow)) playerTwoDirection.y += 1;
		if(Input.GetKey(KeyCode.DownArrow)) playerTwoDirection.y -= 1;
		if(Input.GetKey(KeyCode.RightShift)) playerTwoTryingToInteract = true;

		playerOne.Velocity = playerOneDirection * maxPlayerSpeed;
		playerTwo.Velocity = playerTwoDirection * maxPlayerSpeed;

		playerOne.IsTryingToInteract = playerOneTryingToInteract;
		playerTwo.IsTryingToInteract = playerTwoTryingToInteract;

	}

	private T_Player InitializePlayer(GameObject playerPrefab, Vector2 position, string name){
		tempCreatedObject = Instantiate(playerPrefab, position, Quaternion.identity);
		tempCreatedObject.name = name;
		return tempCreatedObject.GetComponent<T_Player>();
	}

	public void UpdateScoreUI(){
		playerOneScore.text = playerOne.Score.ToString();
		playerTwoScore.text = playerTwo.Score.ToString();
	}

	public void UpdateTimeUI(){
		playerOneTime.text = playerOne.GameTime.ToString();
		playerTwoTime.text = playerTwo.GameTime.ToString();
	}

	public void Initialize(){
		playerOne = InitializePlayer(playerOnePrefab, new Vector2(6.63f, -3.25f), "Player 1");
		playerTwo = InitializePlayer(playerTwoPrefab, new Vector2(-7.19f, -3.25f), "Player 2");

		playerOne.Initialize(this);
		playerTwo.Initialize(this);
	}

	public void UpdateUI(){
		UpdateScoreUI();
		UpdateTimeUI();
	}

	public void Reset(){
		playerOne.Reset();
		playerTwo.Reset();
		UpdateUI();
	}

	public void PenalizeAll(){
		playerOne.IncrementScore(Constants.ANGRY_CUSTOMER_PENALTY);
		playerTwo.IncrementScore(Constants.ANGRY_CUSTOMER_PENALTY);
	}

	public void StopPlayers(bool value){
		playerOne.Deactivate(value);
		playerTwo.Deactivate(value);
	}

	public bool CanPlayersMove{
		get{
			return playerOne.CanMove || playerTwo.CanMove;
		}
	}

	public int PlayerOneScore{
		get{
			return playerOne.Score;
		}
	}

	public int PlayerTwoScore{
		get{
			return playerTwo.Score;
		}
	}
}
