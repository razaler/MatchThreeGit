using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }
}
