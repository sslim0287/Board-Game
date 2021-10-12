using UnityEngine;

public class Path : MonoBehaviour {

	public Transform[] points;

	[SerializeField]
	private float movement = 1f;

	[HideInInspector]
	public int pointsIndex = 0;

	public bool moveAllowed = false;

	// Use this for initialization
	private void Start () {
		transform.position = points[pointsIndex].transform.position;
	}
	
	// Update is called once per frame
	private void Update () {
		if (moveAllowed)
			Move();
	}

	public void Move()
	{
		if (pointsIndex <= points.Length - 1)
		{
			transform.position = Vector2.MoveTowards(transform.position,
			points[pointsIndex].transform.position,
			movement * Time.deltaTime);

			if (transform.position == points[pointsIndex].transform.position)
			{
				pointsIndex += 1;
			}
		}
	}
	
	public void MoveInstant()
	{
		transform.position = points[pointsIndex].transform.position;
	}
}
