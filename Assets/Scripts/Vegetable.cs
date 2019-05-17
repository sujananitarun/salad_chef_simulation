using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegetable : MonoBehaviour, IInteractable{

	private string vegetableName;

	private void Awake() {
		vegetableName = GetComponentInChildren<TextMesh>().text;
	}

	public void OnInteract(T_Player player){
		player.AddVegetable(vegetableName);
	}
}
