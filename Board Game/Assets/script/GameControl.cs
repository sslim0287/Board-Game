using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {
	
	public GameObject dice; 
	
	class Quiz {
		public string Question;
		public string Answer1;
		public string Answer2;
		public int ValidAnswerId;
	}
	
	Quiz []GameQuizes = new Quiz[3] {
		new Quiz() 
		{
			Question = "Q1. 543 + 1100",
			Answer1 = "1223",
			Answer2 = "1643",
			ValidAnswerId = 1
		},
		new Quiz()
		{
			Question = "Q2. 2/3 x 4/5",
			Answer1 = "9/10",
			Answer2 = "8/15",
			ValidAnswerId = 1
		},
		new Quiz()
		{
			Question = "Q3. 300/15",
			Answer1 = "42",
			Answer2 = "20",
			ValidAnswerId = 1
		}
	};
	
    private static GameObject winnerText, player1Turn, player2Turn;

    private static GameObject player1, player2;

    public static int diceNumber = 0;
    public static int player1StartingPoint = 0;
    public static int player2StartingPoint = 0;

    public static bool gameOver = false;
	
	// Variables for the Quiz
	public GameObject QuizPanel;
	public Text QuestionText, ButtonText1, ButtonText2;
	int []QuizPoint = new int[]{2, 15, 22};
	
	int []TrapPoint = new int[]{13,18,23};
	
	// Variables for the Magic Card
	int[] magicCardPoints = new int[] { 4, 9, 19 };
	public GameObject magicCardPanel;
	public Text magicCardNumberText;

	Path player1Path;
	Path player2Path;
	
    // Use this for initialization
    void Start () {

        winnerText = GameObject.Find("WinnerText");
        player1Turn = GameObject.Find("Player1MoveTurnText");
        player2Turn = GameObject.Find("Player2MoveTurnText");

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        player1.GetComponent<Path>().moveAllowed = false;
        player2.GetComponent<Path>().moveAllowed = false;

        winnerText.gameObject.SetActive(false);
        player1Turn.gameObject.SetActive(true);
        player2Turn.gameObject.SetActive(false);
		
		player1Path = player1.GetComponent<Path>();
		player2Path = player2.GetComponent<Path>();
    }

    // Update is called once per frame
    void Update()
    {
		if (this == null) {
			Debug.LogError("No gameObject");
			return;
		}
		
		if(player1Path == null){
			player1Path = player1.GetComponent<Path>();
		}

		if(player2Path == null){
			player2Path = player2.GetComponent<Path>();
		}

		if (player1Path.pointsIndex > player1StartingPoint)
		
        if ((player1.GetComponent<Path>().pointsIndex > 
            player1StartingPoint + diceNumber) && (player1Path.moveAllowed))
        {
            player1.GetComponent<Path>().moveAllowed = false;
            player1StartingPoint = player1.GetComponent<Path>().pointsIndex - 1;

			// If player doesn't land on any special squares, next player gets the turn.
			// Otherwise, do the necessary tasks, and then switch player later.
			if (!CheckQuizActivation(player1StartingPoint)
				&& !CheckTrap(player1StartingPoint)
				&& !CheckMagicCardActivation(player1StartingPoint))
            {
					NextPlayersTurn();
			}
		}

		if ((player2.GetComponent<Path>().pointsIndex >
            player2StartingPoint + diceNumber) && (player2Path.moveAllowed))
        {
            player2.GetComponent<Path>().moveAllowed = false;
            player2StartingPoint = player2.GetComponent<Path>().pointsIndex - 1;

			if (!CheckQuizActivation(player2StartingPoint)
				&& !CheckTrap(player2StartingPoint)
				&& !CheckMagicCardActivation(player2StartingPoint))
			{
				NextPlayersTurn();
			}
		}

        if (player1.GetComponent<Path>().pointsIndex == 
            player1.GetComponent<Path>().points.Length)
        {
            winnerText.gameObject.SetActive(true);
            player1Turn.gameObject.SetActive(false);
            player2Turn.gameObject.SetActive(false);
            winnerText.GetComponent<Text>().text = "Player 1 Wins";
            gameOver = true;
        }

        if (player2.GetComponent<Path>().pointsIndex ==
            player2.GetComponent<Path>().points.Length)
        {
            winnerText.gameObject.SetActive(true);
            player1Turn.gameObject.SetActive(false);
            player2Turn.gameObject.SetActive(false);
            winnerText.GetComponent<Text>().text = "Player 2 Wins";
            gameOver = true;
        }
	}

	bool CheckMagicCardActivation(int pointsIndex)
    {
		for (int i = 0; i < magicCardPoints.Length; i++)
        {
			int id = magicCardPoints[i];

			// If player lands on a Magic Card square, ...
			if (id == pointsIndex)
            {
				// ...display Magic Card with random number from 1 to 3 ...
				int magicCardNumber = Random.Range(1, 4);
				magicCardNumberText.text = magicCardNumber.ToString();
				magicCardPanel.SetActive(true);

				// ...and the other player moves back.
				if (player1Turn.activeSelf)
                {
					Debug.Log("Player 1 landed on a magic card.");
					player2StartingPoint -= magicCardNumber;

					// If player's starting point is out of the board, stay in square 1.
					if (player2StartingPoint < 0)
                    {
						player2StartingPoint = 0;
                    }

					player2Path.pointsIndex = player2StartingPoint;
					player2Path.MoveInstant();

					SwitchPlayer();

					if (!CheckQuizActivation(player2StartingPoint)
						&& !CheckTrap(player2StartingPoint)
						&& !CheckMagicCardActivation(player2StartingPoint))
                    {
						NextPlayersTurn();
                    }
				}
				else if (player2Turn.activeSelf)
                {
					Debug.Log("Player 2 landed on a magic card.");
					player1StartingPoint -= magicCardNumber;

					if (player1StartingPoint < 0)
					{
						player1StartingPoint = 0;
					}

					player1Path.pointsIndex = player1StartingPoint;
					player1Path.MoveInstant();

					SwitchPlayer();

					if (!CheckQuizActivation(player1StartingPoint)
						&& !CheckTrap(player1StartingPoint)
						&& !CheckMagicCardActivation(player1StartingPoint))
					{
						NextPlayersTurn();
					}
				}
				return true;
			}
        }
		return false;
    }
	
	bool CheckTrap(int pointsIndex)
	{
		for(int i= 0; i < TrapPoint.Length; i++)
		{
			int id = TrapPoint[i];
			if (id == pointsIndex)
			{
				if (player1Turn.activeSelf)
				{
					player1StartingPoint -= 2;
					player1Path.pointsIndex = player1StartingPoint;
					player1Path.MoveInstant();

					if (!CheckQuizActivation(player1StartingPoint)
						&& !CheckTrap(player1StartingPoint)
						&& !CheckMagicCardActivation(player1StartingPoint))
					{
						NextPlayersTurn();
					}
				}
				else if (player2Turn.activeSelf)
				{
					player2StartingPoint -= 2;
					player2Path.pointsIndex = player2StartingPoint;
					player2Path.MoveInstant();

					if (!CheckQuizActivation(player2StartingPoint)
						&& !CheckTrap(player2StartingPoint)
						&& !CheckMagicCardActivation(player2StartingPoint))
					{
						NextPlayersTurn();
					}
				}
				return true;
			}
		}
		return false;
	}

	bool CheckQuizActivation(int pointsIndex)
	{
		for (int i = 0; i < QuizPoint.Length; i++)
		{
			int id = QuizPoint[i];
			if (id == pointsIndex) 
			{
				QuizPanel.SetActive(true);
				Quiz quiz = GameQuizes[i];
				QuestionText.text = quiz.Question;
				ButtonText1.text = quiz.Answer1;
				ButtonText2.text = quiz.Answer2;
				currentValidAnswerId = quiz.ValidAnswerId;
				dice.SetActive(false);
				return true;
			}
		}
		return false;
	}
	
	int currentValidAnswerId = -1;
	
	public void CheckValidAnswer(int id)
    {
		dice.SetActive(true);
		QuizPanel.SetActive(false);

		// If player answers correct
		if (currentValidAnswerId == id)
        {
			if (player1Turn.activeSelf)
			{
				player1StartingPoint += 2;
				player1Path.pointsIndex = player1StartingPoint;
				player1Path.MoveInstant();
				CheckPlayer1Win();
				
				if (!CheckMagicCardActivation(player1StartingPoint))
                {
					SwitchPlayer();
                }
			}
			else if (player2Turn.activeSelf)
			{
				player2StartingPoint += 2;
				player2Path.pointsIndex = player2StartingPoint;
				player2Path.MoveInstant();
				CheckPlayer2Win();

				if (!CheckMagicCardActivation(player2StartingPoint))
				{
					SwitchPlayer();
				}
			}
		}
		else // If player answers wrong
        {
			if (player1Turn.activeSelf)
			{
				player1StartingPoint -= 2;
				player1Path.pointsIndex = player1StartingPoint;
				player1Path.MoveInstant();

				if (!CheckTrap(player1StartingPoint))
				{
					SwitchPlayer();
				}
			}
			else if (player2Turn.activeSelf)
			{
				player2StartingPoint -= 2;
				player2Path.pointsIndex = player2StartingPoint;
				player2Path.MoveInstant();

				if (!CheckTrap(player2StartingPoint))
                {
					SwitchPlayer();
				}
			}
			NextPlayersTurn();
		}
	}
	
	void CheckPlayer1Win()
	{
		 if (player1.GetComponent<Path>().pointsIndex == 
            player1.GetComponent<Path>().points.Length - 1)
        {
            winnerText.gameObject.SetActive(true);
            player1Turn.gameObject.SetActive(false);
            player2Turn.gameObject.SetActive(false);
            winnerText.GetComponent<Text>().text = "Player 1 Wins";
            gameOver = true;
        }
	}
	
	void CheckPlayer2Win()
	{
		if (player2.GetComponent<Path>().pointsIndex ==
            player2.GetComponent<Path>().points.Length - 1)
        {
            winnerText.gameObject.SetActive(true);
            player1Turn.gameObject.SetActive(false);
            player2Turn.gameObject.SetActive(false);
            winnerText.GetComponent<Text>().text = "Player 2 Wins";
            gameOver = true;
        }
	}
	
    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove) { 
            case 1:
                player1.GetComponent<Path>().moveAllowed = true;
                break;

            case 2:
                player2.GetComponent<Path>().moveAllowed = true;
                break;
        }
    }

	void SwitchPlayer()
    {
		if (player1Turn.activeSelf)
		{
			player1Turn.gameObject.SetActive(false);
			player2Turn.gameObject.SetActive(true);
		}
		else if (player2Turn.activeSelf)
		{
			player2Turn.gameObject.SetActive(false);
			player1Turn.gameObject.SetActive(true);
		}
	} //  Switch to the next player no matter who is supposed to roll the dice.

	void NextPlayersTurn()
    {
		if (Dice.playerMove == 1)
		{
			player2Turn.gameObject.SetActive(false);
			player1Turn.gameObject.SetActive(true);
		}
		else if (Dice.playerMove == -1)
		{
			player1Turn.gameObject.SetActive(false);
			player2Turn.gameObject.SetActive(true);
		}
	} // Switch to the next player who is supposed to roll the dice.

	
}
