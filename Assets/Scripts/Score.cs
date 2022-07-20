using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    public static int PlayerScore = 0;

    private void Start()
    {
        PlayerScore = 0;
    }

    public static void AddScore(int score)
    {
        PlayerScore += score;
    }

    private void Update()
    {
        scoreText.text = PlayerScore.ToString();
    }
}
