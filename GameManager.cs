using UnityEngine;
using System.Collections;
using TMPro; 

public class GameManager : MonoBehaviour
{
	public enum GameState
	{
		Start,
		CountDown,
		Play,
		GameOver,
		Scoreboard, 
	};

	public struct HighScore
	{
		string name;
		float score; 
	};

	public GameState gameState = GameState.Start;

	public int totalPresents;
	public int foundPresents;
	public float playTime;	

	public float playTimeStart = 60; // one minute max play time
	public float presentBonus = 2.0f; // each present worth 2 seconds  

	PlatformFighterController playerController; 

	HighScore [] highScores;

	public Canvas mainMenu;
	public Canvas playMenu;
	public Canvas scoreMenu;

    public TMPro.TextMeshProUGUI timeTxt;
    public TMPro.TextMeshProUGUI presentsTxt;

    public TMPro.TextMeshProUGUI scoreTimeTxt;
    public TMPro.TextMeshProUGUI scorePresentsTxt;
	public TMPro.TextMeshProUGUI scoreTotalTxt;

	public TMPro.TextMeshProUGUI highScoreTxt;
	int highScore; 

    private void Start()
    {
		playerController = FindObjectOfType<PlatformFighterController>();

		// disable player on start
		mainMenu.enabled = true;
		playMenu.enabled = false;
		scoreMenu.enabled = false;
		highScore = 0; 
    }

    public void LoadScores()
	{


	}

	public void SaveScores()
	{


	}

	public void AddScore( string name, float time )
	{

	}

	public void GotToStart()
	{
        gameState = GameState.Start;
        playerController.playMode = false;
        mainMenu.enabled = true;
        playMenu.enabled = false;
        scoreMenu.enabled = false;
    }

    private void Update()
    {

		switch ( gameState )
		{
			case GameState.Start:

				playerController.playMode = false; 
                // display base ui and controls

                // on play start count down
				if (Input.GetKeyDown(KeyCode.Space))
				{
					gameState = GameState.Play;
					playerController.ResetGame(); 
                    playerController.playMode = true;
					playTime = playTimeStart;
					mainMenu.enabled = false;
					playMenu.enabled = true;
					scoreMenu.enabled = false; 

					timeTxt.text = "Time Remaining - 60";
					presentsTxt.text = playerController.GetPresentsString(); 				
                }
                break;
			case GameState.CountDown:
                
                // once countdown is over star the play clock
                // enable player control 
                break;
			case GameState.Play:
				// allow player to move

				// Level clock counts down from maxPlayTime
				playTime -= Time.deltaTime;

				// update time
				timeTxt.text = "Time Remaining " + (int)playTime;

				// update presents
				presentsTxt.text = playerController.GetPresentsString();

                // player hits spike reset

                // player reaches end disable and stop the play clock

                // go to game Over
				if ( playerController.atEnd )
				{
					gameState = GameState.Scoreboard; 
					scorePresentsTxt.text = playerController.GetPresentsString();
					int presentCount = playerController.presentCount; 
					scoreTimeTxt.text = "Time Left " + (int)playTime;
					int score = ((int)playTime * presentCount);
                    scoreTotalTxt.text = " " + score;

					if (highScore < score)
						highScore = score;

					highScoreTxt.text = "High Score " + highScore;

                    mainMenu.enabled = false;
                    playMenu.enabled = false;
                    scoreMenu.enabled = true;

                }
                break;
			case GameState.GameOver:
                // compute the score, maybe presents * bonus value + max time - timeScore
                // load scoreboard
                // if new score is better add to list and remove worst

		

                break;
			case GameState.Scoreboard:
                // show players score 
                // display the score board
                // highlight the players score if in list

                // play again ?

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    mainMenu.enabled = true;
                    playMenu.enabled = false;
                    scoreMenu.enabled = false;
                    gameState = GameState.Start;
                }

                break; 
		}


    }
};

