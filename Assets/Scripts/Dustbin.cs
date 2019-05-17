using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dustbin : MonoBehaviour, IInteractable {

	public void OnInteract(T_Player player){
		if(player.ThrowInDustbin()){
			player.IncrementScore(Constants.ANGRY_CUSTOMER_PENALTY);
		}
	}
}
