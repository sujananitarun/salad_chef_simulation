using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public enum MenuState{
		MAIN_PANEL = 0,
		GAME_PANEL = 1,
		END_PANEL = 2
	}
        public GameObject level, gamePanel, mainPanel, endPanel;

	    public Text finalPlayerOneScore, finalPlayerTwoScore;

	private MenuState menuState;

	public void DisplayMainMenu()
	{
		mainPanel.SetActive(true);
		endPanel.SetActive(false);
		gamePanel.SetActive(false);
		level.SetActive(false);
		menuState = MenuState.MAIN_PANEL;
	}

	public void DisplayGame()
	{
		mainPanel.SetActive(false);
		endPanel.SetActive(false);
		gamePanel.SetActive(true);
		level.SetActive(true);
		menuState = MenuState.GAME_PANEL;
	}

	public void DisplayEnd()
	{
		mainPanel.SetActive(false);
		endPanel.SetActive(true);
		gamePanel.SetActive(false);
		level.SetActive(false);
		menuState = MenuState.END_PANEL;
	}

	public MenuState State{
		get{
			return menuState;
		}
	}

	public int FinalPlayerOneScore{
		set{
			finalPlayerOneScore.text = value.ToString();
		}
	}


	public int FinalPlayerTwoScore{
		set{
			finalPlayerTwoScore.text = value.ToString();
		}
	}
}
