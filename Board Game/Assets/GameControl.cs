using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    private static GameObject winnerText, player1Turn, player2Turn;

    private static GameObject player1, player2;

    public static int diceNumber = 0;
    public static int player1StartingPoint = 0;
    public static int player2StartingPoint = 0;

    public static bool gameOver = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (player1.GetComponent<Path>().pointsIndex > 
            player1StartingPoint + diceNumber)
        {
            player1.GetComponent<Path>().moveAllowed = false;
            player1Turn.gameObject.SetActive(false);
            player2Turn.gameObject.SetActive(true);
            player1StartingPoint = player1.GetComponent<Path>().pointsIndex - 1;
        }

        if (player2.GetComponent<Path>().pointsIndex >
            player2StartingPoint + diceNumber)
        {
            player2.GetComponent<Path>().moveAllowed = false;
            player2Turn.gameObject.SetActive(false);
            player1Turn.gameObject.SetActive(true);
            player2StartingPoint = player2.GetComponent<Path>().pointsIndex - 1;
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
