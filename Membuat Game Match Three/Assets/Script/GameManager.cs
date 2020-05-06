using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Instance sebagai global access
    public static GameManager instance;
    public GameObject endpanel;
    int playerScore;
    public int time;
    public Text scoreText, timeText, finalScText, comboText;
    public int combo = 1;


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (time > 98)
        {
            time--;
        }
        int tm = time / 100;
        timeText.text = tm.ToString();
        comboText.text = combo.ToString();

        if (time == 98)
        {
            Time.timeScale = 0;
            endpanel.SetActive(true);
        }
    }

    //Update score dan ui
    public void GetScore(int point)
    {
        playerScore += point;
        scoreText.text = playerScore.ToString();
        finalScText.text = playerScore.ToString();
    }




}
