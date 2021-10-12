using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {
	public GameObject Dice; 
	
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
	
	private static GameObject winnerText, player1Turn, player2Turn, 
		activeTurn, lastPlayerWhoRolledTheDice;

	private static GameObject player1, player2;

	public static int diceNumber = 0;
	public static int player1StartingPoint = 0;
	public static int player2StartingPoint = 0;

	public static bool gameOver = false;
	
	// Variables for the Task Cards
	public GameObject quizPanel;
	public Text QuestionText, ButtonText1, ButtonText2;
	int[] QuizPoint = new int[] { 2, 15, 22 };

	// Variables for the Magic Cards
	public GameObject magicCardPanel;
	public Text magicCardNumberText;
	int[] magicCardPoints = new int[] { 4, 9, 19 };
	
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
		
		// Player 1 mmoves first, followed by Player 2.
		if ((player1.GetComponent<Path>().pointsIndex > 
			player1StartingPoint + diceNumber) && (player1Path.moveAllowed))
		{
			player1.GetComponent<Path>().moveAllowed = false;
			player1StartingPoint = player1.GetComponent<Path>().pointsIndex - 1;

			lastPlayerWhoRolledTheDice = player1Turn;

			// If player doesn't land on any special squares, switch to next player.
			if (!CheckQuizActivation(player1StartingPoint) && !CheckMagicCardActivation(player1StartingPoint))
			{
				SwitchToNextPlayer();
			}
		}
		
		if ((player2.GetComponent<Path>().pointsIndex >
			player2StartingPoint + diceNumber) && (player2Path.moveAllowed))
		{
			player2.GetComponent<Path>().moveAllowed = false;
			player2StartingPoint = player2.GetComponent<Path>().pointsIndex - 1;

			lastPlayerWhoRolledTheDice = player2Turn;

			if (!CheckQuizActivation(player2StartingPoint) && !CheckMagicCardActivation(player2StartingPoint))
			{
				SwitchToNextPlayer();
			}
		}

		// If player reaches the end of board, the player wins.
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
	
	// Check if player has landed on a Task Card square.
	bool CheckQuizActivation(int pointsIndex)
	{
		for (int i = 0; i < QuizPoint.Length; i++)
		{
			int id = QuizPoint[i];

			// If player lands on Task Card square, display quiz.
			if (id == pointsIndex) 
			{
				quizPanel.SetActive(true);
				Quiz quiz = GameQuizes[i];
				QuestionText.text = quiz.Question;
				ButtonText1.text = quiz.Answer1;
				ButtonText2.text = quiz.Answer2;
				currentValidAnswerId = quiz.ValidAnswerId;
				Dice.SetActive(false);
				return true;
			}
		}
		return false;
	}

	// Check if player has landed on a Magic Card square.
	bool CheckMagicCardActivation(int pointsIndex)
	{
		// Get the active player's turn, assign to activeTurn.
		if (player1Turn.activeSelf)
		{
			activeTurn = player1Turn;
		}
		else if (player2Turn.activeSelf)
		{
			activeTurn = player2Turn;
		}

		for (int i = 0; i < magicCardPoints.Length; i++)
		{
			int id = magicCardPoints[i];

			// If player lands on Magic Card square, display Magic Card...
			if (id == pointsIndex)
			{
				//...with a random number ranging from 1 to 3,...
				int magicCardNumber = Random.Range(1, 4);
				magicCardNumberText.text = magicCardNumber.ToString();
				magicCardPanel.SetActive(true);

				// ...and the other player moves back.
				if (player1Turn.activeSelf)
				{
					player2StartingPoint -= magicCardNumber;

					// If player's starting point is out of the board, stay in square 1.
					if (player2StartingPoint < 0)
					{
						player2StartingPoint = 0;
						player2Path.pointsIndex = player2StartingPoint;
					}
					// If not, move player.
					else
					{
						player2Path.pointsIndex = player2StartingPoint;
						player2Path.MoveInstant();
						CheckQuizActivation(player2StartingPoint);
					}
				}
				if (player2Turn.activeSelf)
				{
					player1StartingPoint -= magicCardNumber;
					if (player1StartingPoint < 0)
					{
						player1StartingPoint = 0;
						player1Path.pointsIndex = player1StartingPoint;
					}
					else
					{
						player1Path.pointsIndex = player1StartingPoint;
						player1Path.MoveInstant();
						CheckQuizActivation(player1StartingPoint);
					}
				}

				// If it's still the player's turn after execution of the code above, switch to the other player.
				if ((player1Turn.activeSelf && activeTurn == player1Turn) 
					|| (player2Turn.activeSelf && activeTurn == player2Turn))
				{
					SwitchToNextPlayer();
				}
				return true;
			}
		}
		return false;
	}
	
	int currentValidAnswerId = -1;
	
	// Check if player's answer is correct.
	public void CheckValidAnswer(int id)
	{
		if (player1Turn.activeSelf)
		{
			activeTurn = player1Turn;
		}
		else if (player2Turn.activeSelf)
		{
			activeTurn = player2Turn;
		}

		Dice.SetActive(true);
		quizPanel.SetActive(false);

		// If answer is correct, player moves forward 2 spaces.
		if (currentValidAnswerId == id)
		{
			if (player1Turn.activeSelf)
			{
				player1StartingPoint += 2;
				player1Path.pointsIndex = player1StartingPoint;
				player1Path.MoveInstant();
				CheckPlayer1Win();
				CheckMagicCardActivation(player1StartingPoint);
				return;
			}

			if (player2Turn.activeSelf)
			{
				player2StartingPoint += 2;
				player2Path.pointsIndex = player2StartingPoint;
				player2Path.MoveInstant();
				CheckPlayer2Win();
				CheckMagicCardActivation(player2StartingPoint);
			}
		}
		// If answer is incorrect, player moves back 2 spaces.
		else
		{
			if (player1Turn.activeSelf)
			{
				player1StartingPoint -= 2;
				player1Path.pointsIndex = player1StartingPoint;
				player1Path.MoveInstant();
				CheckMagicCardActivation(player1StartingPoint);
				return;
			}

			if (player2Turn.activeSelf)
			{
				player2StartingPoint -= 2;
				player2Path.pointsIndex = player2StartingPoint;
				player2Path.MoveInstant();
				CheckMagicCardActivation(player2StartingPoint);
			}
		}

		if ((player1Turn.activeSelf && activeTurn == player1Turn) 
			|| (player2Turn.activeSelf && activeTurn == player2Turn))
		{
			SwitchToNextPlayer();

			// If Player 1 is the last player to roll the dice, switch to Player 2 and vice versa.
			if ((player1Turn.activeSelf && lastPlayerWhoRolledTheDice == player1Turn) 
				|| (player2Turn.activeSelf && lastPlayerWhoRolledTheDice == player2Turn))
			{
				SwitchToNextPlayer();
			}
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

	void SwitchToNextPlayer()
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
	}
}
