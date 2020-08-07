using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	public GameObject game;
	public GameObject popupMenu;
	
	void Update()
    {
        if(Input.GetKeyDown (KeyCode.Escape)) 
        {
            if (!game.activeInHierarchy) 
            {
                PopUpMenuOn();
            }
            else
            {
                PopUpMenuOff();   
            }
        } 
     }
	 
	public void PlayButton()
	{
			Score.scoreInt = 0;
			SceneManager.LoadScene(1);
	}
	
	public void QuitButton()
	{
			Application.Quit();
	}
	
	public void MainMenu()
	{
			SceneManager.LoadScene(0);
	}
	
	public void PopUpMenuOn()
	{
			game.SetActive(true);
			popupMenu.SetActive(false);
	}
	
	public void PopUpMenuOff()
	{
			game.SetActive(false);
			popupMenu.SetActive(true);
	}
}
