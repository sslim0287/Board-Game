using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour {

	private SpriteRenderer render;
	private Sprite[] dice;
	
	private bool coroutineAllowed = true;
	private int playerMove = 1;

	// Use this for initialization
	private void Start () {
		dice = Resources.LoadAll<Sprite>("Dice/");
		render = GetComponent<SpriteRenderer>();
		render.sprite = dice[5];
	}

	private void OnMouseDown()
	{
		if (!GameControl.gameOver && coroutineAllowed)
			StartCoroutine("RollingDice");
	}

	private IEnumerator RollingDice()
	{
		coroutineAllowed = false;
		int randomDiceNumber = 0;
		for (int i = 0; i <= 20; i++)
		{
			randomDiceNumber = Random.Range(0, 6);
			render.sprite = dice[randomDiceNumber];
			yield return new WaitForSeconds(0.05f);
		}

		GameControl.diceNumber = randomDiceNumber + 1;
		if (playerMove == 1)
		{
			GameControl.MovePlayer(1);
		} else if (playerMove == -1)
		{
			GameControl.MovePlayer(2);
		}
		playerMove *= -1;
		coroutineAllowed = true;
	}
}
