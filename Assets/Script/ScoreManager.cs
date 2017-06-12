using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score;
    public int bestScore;

    public Text[] scores;
    public Text[] bestScores;

	void Awake()
    {
        PlayerPrefs.GetInt("Best Score", bestScore);

        instance = this;
    }
	
	void Update ()
    {
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("Best Score", bestScore);
        }

        foreach (var a in scores)
            a.text = score.ToString();

        foreach (var b in bestScores)
            b.text = bestScore.ToString();
	}
}
