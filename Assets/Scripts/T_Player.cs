using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Player : MonoBehaviour {

	private const int MAX_HELD_VEGETABLES = 2;

	private Vector2 velocity = Vector2.zero;

	public TextMesh vegetablesUI, choppedVegetablesUI;
	public GameObject choppingIndicatorUI;

	private Rigidbody2D body;

	private PlayerManager playerManager;


	private Queue<string> vegetables = new Queue<string>();
	private List<string> choppedVegetables = new List<string>();

	private bool isTryingToInteract = false;
	private bool canInteract = true;
	private const float interactionDistance = 2.7f;
	private const float interactionDelay = 0.3f;
	private float interactionFrameCounter = 0;
	private Collider2D[] interactedColliders;
	private Collider2D closestCollider;
	private float minDistance = 0;
	private float tempMinDistance = 0;
	private int layerMask;

	private bool isChopping = false;
	private bool canMove = true;
	
	private string tempVegetableText;
	private string tempVegetableUIText;

	private int score = 0;
	private int gameTime = Constants.PLAYER_START_TIME;

	private float gameTimeFrameCounter = 0;

	private Vector2 oldPosition;
	private SpriteRenderer spriteRenderer;
    

	private void Awake(){
		body = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Start() {
		oldPosition = transform.position;
	}

	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, interactionDistance);
	}

	private void FixedUpdate() {
		CheckGameTimeFrameDelay();
		if(canMove){
			if(!isChopping){
				Move();
				CheckInteractionDelay();
				CheckForInteractions();
			}
		}
	}

	private void CheckGameTimeFrameDelay(){
		if(gameTime <= 0) return;
		gameTimeFrameCounter += Time.fixedDeltaTime;
		if(gameTimeFrameCounter >= 1){
			gameTimeFrameCounter = 0;
			IncrementTime(-1);
			if(gameTime <= 0){
				canMove = false;
				body.velocity = velocity = Vector2.zero;
			}
		}
	}

	private void CheckInteractionDelay(){
		if(!this.canInteract){
			interactionFrameCounter += Time.fixedDeltaTime;
			if(interactionFrameCounter >= interactionDelay) {
				interactionFrameCounter = 0;
				this.canInteract = true;
			}
		}
	}

	private void CheckForInteractions(){
		if(this.canInteract && isTryingToInteract){
			this.canInteract = false;
			layerMask = LayerMask.GetMask(Layers.INTERACTABLE);
			interactedColliders = Physics2D.OverlapCircleAll(transform.position, interactionDistance, layerMask);

			if(interactedColliders.Length > 0) {
				closestCollider = interactedColliders[0];
				minDistance = Vector2.Distance(closestCollider.transform.position, transform.position);
			}

			for(int i= 1; i<interactedColliders.Length; i++){
				tempMinDistance = Vector2.Distance(interactedColliders[i].transform.position, transform.position);
				if(tempMinDistance < minDistance){
					minDistance = tempMinDistance;
					closestCollider = interactedColliders[i];
				}
			}

			if(closestCollider != null){
				closestCollider.GetComponent<IInteractable>().OnInteract(this);
				closestCollider = null;
			}
		}
	}

	private void Move(){
		body.velocity = velocity;
	}

	private void UpdateUI(){
		if(vegetables.Count <= 0) {
			vegetablesUI.text = "---";
		}

		if(choppedVegetables.Count <= 0){
			choppedVegetablesUI.text = "---";
		}

		tempVegetableUIText = "";
		foreach(string vegetable in vegetables){
			tempVegetableUIText += vegetable + ", ";
            
		}
		
		if(tempVegetableUIText != "") vegetablesUI.text = tempVegetableUIText;

		tempVegetableUIText = "";
		for(int i=0; i < choppedVegetables.Count; i++){
			if(i < choppedVegetables.Count- 1) {
				tempVegetableUIText += choppedVegetables[i] + ", ";
				if(i % 3 == 2) tempVegetableUIText += "\n";
			}
			else{
				tempVegetableUIText += choppedVegetables[i];
			}
		}
		if(tempVegetableUIText != "") choppedVegetablesUI.text = tempVegetableUIText;
	}

	public void Initialize(PlayerManager playerManager){
		this.playerManager = playerManager;
	}

	public string RemoveVegetable(){
		if(vegetables.Count <= 0) return null;
		tempVegetableText = vegetables.Dequeue();
		UpdateUI();

		return tempVegetableText;
	}

	public bool AddVegetable(string name){
		if(vegetables.Count >= MAX_HELD_VEGETABLES) return false;

		vegetables.Enqueue(name);
		UpdateUI();

		return true;
	}

	public bool IsChoppingReduntant(string name){
		return choppedVegetables.Contains(name);
	}

	public bool ServeCustomer(List<string> recipe){
		if(choppedVegetables.Count != recipe.Count) return false;
		foreach(string ingredient in recipe){
			if(!choppedVegetables.Contains(ingredient)) return false;
		}
		return true;
	}

	public bool AddChoppedVegetable(string name){
		if(choppedVegetables.Contains(name)) return false;

		choppedVegetables.Add(name);
		UpdateUI();

		return true;
	}

	public bool ThrowInDustbin(){
		if(choppedVegetables.Count <= 0) return false;
		choppedVegetables.Clear();
		UpdateUI();
		return true;
	}

	public void IncrementScore(int value){
		score += value;
		playerManager.UpdateScoreUI();
	}

	public void IncrementTime(int value){
		gameTime += value;
		playerManager.UpdateTimeUI();
	}

	public void Deactivate(bool value){
		canMove = !value;
		spriteRenderer.enabled = !value;
		vegetablesUI.gameObject.SetActive(!value);
		choppedVegetablesUI.gameObject.SetActive(!value);
		if(!value) choppingIndicatorUI.SetActive(false);
	}

	public void Reset(){
		score = 0;
		gameTime = Constants.PLAYER_START_TIME;
		gameTimeFrameCounter = 0;
		isChopping = false;
		canMove = true;
		isTryingToInteract = false;
		canInteract =true;
		
		choppedVegetables.Clear();
		vegetables.Clear();

		body.velocity = velocity = Vector2.zero;
		transform.position = oldPosition;

		UpdateUI();
	}

	#region Getters-Setters

	public Vector2 Velocity{
		set{
			this.velocity = value;
		}
	}

	public bool IsTryingToInteract{
		set{
			this.isTryingToInteract = value;
		}
	}

	public bool IsChopping{
		set{
			this.isChopping = value;
			if(value) this.body.velocity = Vector2.zero;
			choppingIndicatorUI.SetActive(value);
		}
	}

	public int Score{
		get{
			return score;
		}
	}

	public int GameTime{
		get{
			return gameTime;
		}
	}

	public bool CanMove{
		get{
			return canMove;
		}
	}

	#endregion
}
