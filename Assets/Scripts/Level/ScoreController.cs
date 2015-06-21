using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour
{
	private int score = 0;
	private int currentScore = -1;

	private GUIText displayedScore;

	private void Start()
	{
		displayedScore = this.GetComponent<GUIText>();
		if (displayedScore == null) throw new UnityException("Failed to initialized score GUIText");
	}

	private void Update ()
	{
		if (currentScore != score)
		{
			displayedScore.text = score.ToString();
			currentScore = score;
		}
	}

	public void AddScore(int addedScore)
	{
		score += addedScore;
	}
}
