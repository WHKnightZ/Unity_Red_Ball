using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int scoreValue;
    private int scoreCurrent;
    private float delayScore;
    public TextMeshProUGUI score;

    void Start()
    {
        score = GetComponent<TextMeshProUGUI>();
        scoreCurrent= scoreValue = 0;
        delayScore = 0f;
    }

    void Update()
    {
        if (delayScore >= 0f)
            delayScore -= Time.deltaTime;
        if (scoreCurrent < scoreValue&&delayScore<0f)
        {
            delayScore += 0.04f;
            scoreCurrent++;
            score.text = scoreCurrent.ToString("000");
        }
    }

    public void UpdateScore(int n)
    {
        scoreValue += n;
    }
}
