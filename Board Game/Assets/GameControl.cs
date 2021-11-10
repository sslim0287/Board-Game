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
	
    private static GameObject winnerText, player1Turn, player2Turn;

    private static GameObject player1, player2;

    public static int diceNumber = 0;
    public static int player1StartingPoint = 0;
    public static int player2StartingPoint = 0;

    public static bool gameOver = false;
	
	public GameObject QuizPanel;
	public Text QuestionText, ButtonText1, ButtonText2;
	int []QuizPoint = new int[]{2, 15, 22};
	
	int []TrapPoint = new int[]{13,18,23};

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
		
        if ((player1.GetComponent<Path>().pointsIndex > 
            player1StartingPoint + diceNumber) && (player1Path.moveAllowed))
        {
            player1.GetComponent<Path>().moveAllowed = false;
            player1Turn.gameObject.SetActive(false);
            player2Turn.gameObject.SetActive(true);
            player1StartingPoint = player1.GetComponent<Path>().pointsIndex - 1;
			CheckQuizActivation(player1StartingPoint);
			CheckTrap(player1StartingPoint);
        }
		

        if ((player2.GetComponent<Path>().pointsIndex >
            player2StartingPoint + diceNumber) && (player2Path.moveAllowed))
        {
            player2.GetComponent<Path>().moveAllowed = false;
            player2Turn.gameObject.SetActive(false);
            player1Turn.gameObject.SetActive(true);
            player2StartingPoint = player2.GetComponent<Path>().pointsIndex - 1;
			CheckQuizActivation(player2StartingPoint);
			CheckTrap(player2StartingPoint);
        }

        if (player1.GetComponent<Path>().pointsIndex == 
            player1.GetComponent<Path>().points.Length)
        {
            winnerText.gameObject.SetActive(true);
            player1Turn.gameObject.SetActive(false);
            player2Turn.gameObject.SetActive(false);
            winnerText.GetComponent<Text>().text = "Player 1 Win";
            gameOver = true;
        }

        if (player2.GetComponent<Path>().pointsIndex ==
            player2.GetComponent<Path>().points.Length)
        {
            winnerText.gameObject.SetActive(true);
            player1Turn.gameObject.SetActive(false);
            player2Turn.gameObject.SetActive(false);
            winnerText.GetComponent<Text>().text = "Player 2 Win";
            gameOver = true;
        }
	}
	
	bool CheckTrap(int pointsIndex)
	{
		for(int i= 0; i < TrapPoint.Length; i++)
		{
			int id = TrapPoint[i];
			if (id == pointsIndex)
			{
				if (player2Turn.activeSelf) 
				{
					player1StartingPoint -= 2;
					player1Path.pointsIndex = player1StartingPoint;
					player1Path.MoveInstant();
					return true;
				}
				
				if (player1Turn.activeSelf)
				{
					player2StartingPoint -= 2;
					player2Path.pointsIndex = player2StartingPoint;
					player2Path.MoveInstant();
					return true;

				}
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
				Dice.SetActive(false);
				return true;
			}
		}
		return false;
	}
	
	int currentValidAnswerId = -1;
	
	public void CheckValidAnswer(int id)
	{
		Dice.SetActive(true);
		QuizPanel.SetActive(false);
		if (currentValidAnswerId == id)
		{
			if (player2Turn.activeSelf)
			{
				player1StartingPoint += 2;
				player1Path.pointsIndex = player1StartingPoint;
				player1Path.MoveInstant();
				CheckPlayer1Win();
				return;
			}
			
			if (player1Turn.activeSelf)
			{
				player2StartingPoint += 2;
				player2Path.pointsIndex = player2StartingPoint;
				player2Path.MoveInstant();
				CheckPlayer2Win();
			}
		}
		else if(currentValidAnswerId != id && player1StartingPoint == 15)
		{
				player1StartingPoint -= 4;
				player1Path.pointsIndex = player1StartingPoint;
				player1Path.MoveInstant();
				return;
		}
		else if(currentValidAnswerId != id && player2StartingPoint == 15)
		{
				player2StartingPoint -= 4;
				player2Path.pointsIndex = player2StartingPoint;
				player2Path.MoveInstant();
				return;
		}
		else
		{
			if (player2Turn.activeSelf) 
			{
				player1StartingPoint -= 2;
				player1Path.pointsIndex = player1StartingPoint;
				player1Path.MoveInstant();
				return;
			}
			
			if (player1Turn.activeSelf)
			{
				player2StartingPoint -= 2;
				player2Path.pointsIndex = player2StartingPoint;
				player2Path.MoveInstant();
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
            winnerText.GetComponent<Text>().text = "Player 1 Win";
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
            winnerText.GetComponent<Text>().text = "Player 2 Win";
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
}
