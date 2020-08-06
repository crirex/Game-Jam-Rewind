using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	public static int scoreInt = 0;
	public Text scoreText;
	public Text highText;
	public static int scoreHigh;
	
	void Start()
	{
		scoreHigh = PlayerPrefs.GetInt("High Score", 0);
		highText.text = scoreHigh.ToString();
	}
	
    void Update()
    {
		scoreText.text = scoreInt.ToString();
    }
	
	public static void HighScore()
	{
			if( scoreInt > PlayerPrefs.GetInt("High Score", 0))
			{
				PlayerPrefs.SetInt("High Score", scoreInt);	
				scoreHigh = scoreInt;
			}
			
	}
	
	
}
