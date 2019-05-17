using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour, IInteractable, ISpawnable {

	public enum CustomerState{
		ENTERING = 0,
		WAITING = 1,
		ANGRY = 2,
		LEAVING = 3
	}

	public string[] availableIngredients = {
		"a", "b", "c", "d", "e", "f"
	};

	private Color WAITING_COLOR = new Color(0, 0, 1);
	private Color ANGRY_COLOR = new Color(1, 0, 0);

	private CustomerState state = CustomerState.ENTERING;
	private Vector2 velocity = new Vector2(0, -5);

	private bool hasEaten = false;
	private SpriteRenderer spriteRenderer;
	
	private float waitTimeCounter = 0;
	private float waitingDelay;
	private Vector2 progressScale = new Vector2(1, 1);

	private int tempSwapIndex;
	private string tempSwapIngredient;
	private const int maxIngredients = 3;
	private List<string> ingredients = new List<string>(maxIngredients);
	private int tempMumberOfIngredients;

	private TextMesh recipe;
	private string tempRecipe;

	private SpriteRenderer progressSprite;

	private T_Player failedServer;

	private PlayerManager playerManager;

	public GameObject waitingBar;
	public GameObject progress;
	public GameObject recipeHolder;

	private void Awake(){
		spriteRenderer = GetComponent<SpriteRenderer>();
		recipe = recipeHolder.GetComponentInChildren<TextMesh>();
		progressSprite = progress.GetComponentInChildren<SpriteRenderer>();
	}

	private void Start(){
		HasEaten = true;
		playerManager = GameObject.FindGameObjectWithTag(Tags.PLAYER_MANAGER).GetComponent<PlayerManager>();
	}

	private void Update(){
		if(HasEaten) return;
		CheckState();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == Tags.SERVING_COUNTER) State = CustomerState.WAITING;
		if(other.tag == Tags.WALL) HasEaten = true;
	}

	private void CheckState(){
		switch(State){
			case CustomerState.ENTERING:
				transform.Translate(velocity * Time.deltaTime);
				break;
			case CustomerState.WAITING:
				Wait(1);
				break;
			case CustomerState.ANGRY:
				Wait(3);
				break;
			case CustomerState.LEAVING:
				transform.Translate(-velocity * Time.deltaTime);
				break;
		}
	}

	private void Wait(int units){
		waitTimeCounter -= units * Time.deltaTime;
		progressScale.x = waitTimeCounter / waitingDelay;
		progress.transform.localScale = progressScale;

		if(waitTimeCounter <= 0) {
			if(failedServer != null) failedServer.IncrementScore(2 * Constants.ANGRY_CUSTOMER_PENALTY);
			else{
				playerManager.PenalizeAll();
			}
			State = CustomerState.LEAVING;
		}
	}

	private void UpdateUI(){
		tempRecipe = "";
		for(int i=0; i<ingredients.Count; i++){
			if(i < ingredients.Count - 1) tempRecipe += ingredients[i] + ", ";
			else tempRecipe += ingredients[i];
		}
	
		recipe.text = tempRecipe;
	}

	private void CreateNewRecipe(){
		for(int i=0; i<availableIngredients.Length; i++){
			tempSwapIndex = Random.Range(i, availableIngredients.Length);
			tempSwapIngredient = availableIngredients[i];
			availableIngredients[i] = availableIngredients[tempSwapIndex];
			availableIngredients[tempSwapIndex] = tempSwapIngredient;
		}

		tempMumberOfIngredients = Random.Range(1, maxIngredients + 1);
		ingredients.Clear();
		for(int i=0; i < tempMumberOfIngredients; i++) ingredients.Add(availableIngredients[i]);

		waitingDelay = Constants.WAIT_PER_INGREDIENT * tempMumberOfIngredients;

	}

	private void ShowRecipe(bool value){
		recipeHolder.SetActive(value);
	}

	private void ShowWaitingBar(bool value){
		waitingBar.SetActive(value);
	}


	public void OnInteract(T_Player player){
		if(HasEaten) return;
		if(player.ServeCustomer(ingredients)){
			player.IncrementScore(ingredients.Count * Constants.SATISFIED_CUSTOMER_REWARD);
			player.IncrementTime(ingredients.Count * Constants.SATISFIED_CUSTOMER_REWARD_TIME);
			State = CustomerState.LEAVING;
			player.ThrowInDustbin();
		}
		else{
			if(State != CustomerState.ANGRY) {
				failedServer = player;
				State = CustomerState.ANGRY;
			}
			else {
				if(player == failedServer) failedServer.IncrementScore(2 * Constants.ANGRY_CUSTOMER_PENALTY);
				else{
					playerManager.PenalizeAll();
				}
				State = CustomerState.LEAVING;
			}
		}
	}

	public bool IsAvailableToSpawn(){
		return HasEaten;
	}

	public void OnSpawn(Vector2 position){
		transform.position = position;
		HasEaten = false;
	} 

	#region Getters-Setters

	public CustomerState State{
		get{
			return state;
		}
		private set{
			state = value;
			switch(state){
				case CustomerState.ENTERING:
					failedServer = null;
					CreateNewRecipe();
					UpdateUI();
					ShowWaitingBar(false);
					ShowRecipe(false);
					break;
				case CustomerState.WAITING:
					progressSprite.color = WAITING_COLOR;
					waitTimeCounter = waitingDelay;
					ShowWaitingBar(true);
					ShowRecipe(true);
					break;
				case CustomerState.LEAVING:
					ShowWaitingBar(false);
					ShowRecipe(false);
					break;
				case CustomerState.ANGRY:
					progressSprite.color = ANGRY_COLOR;
					ShowWaitingBar(true);
					ShowRecipe(true);
					break;
			}
		}
	}

	public void ResetSpawn(){
		HasEaten = true;
	}

	public bool HasEaten{
		get{
			return hasEaten;
		}
		set{
			hasEaten = value;
			spriteRenderer.enabled = !value;

			waitingBar.SetActive(!value);
			progress.SetActive(!value);
			recipeHolder.SetActive(!value);

			if(value){
				failedServer = null;
			}
			else{
				State = CustomerState.ENTERING;
			}
		}
	}

	#endregion
}
