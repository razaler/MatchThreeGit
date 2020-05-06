using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSystemWithEvents : MonoBehaviour
{
    public Image achievementBanner;
    public Text achievementText;

    TileEvent cookiesEvent, cakeEvent, vitaminEvent;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();

        cookiesEvent = new CookiesTileEvent(3);
        cakeEvent = new CakeTileEvent(10);
        vitaminEvent = new VitaminTileEvent(5);

        PointOfInterestWithEvent.OnPointOfInterestEntered += PointOfInterestWithEvents_OnPointOfInterestEntered;
    }

    private void PointOfInterestWithEvents_OnPointOfInterestEntered(PointOfInterestWithEvent poi)
    {
        string achievementKey = "Achievement " + poi.Poiname;

        string key;

        if (poi.Poiname.Equals("Cookies event"))
        {
            cookiesEvent.OnMatch();
            if (cookiesEvent.AchievementCompleted())
            {
                key = "Match first cookies";
                NotifyAchievement(key, poi.Poiname);
            }
        }

        if (poi.Poiname.Equals("Cake event"))
        {
            cakeEvent.OnMatch();
            if (cakeEvent.AchievementCompleted())
            {
                key = "Match 10 cake";
                NotifyAchievement(key, poi.Poiname);
            }
        }

        if (poi.Poiname.Equals("Vitamin event"))
        {

            vitaminEvent.OnMatch();
            if (vitaminEvent.AchievementCompleted())
            {
                key = "Match 5 vitamin";
                NotifyAchievement(key, poi.Poiname);
            }
        }
    }

    void NotifyAchievement(string key, string value)
    {
        if (PlayerPrefs.GetInt(value) == 1)
            return;

        PlayerPrefs.SetInt(value, 1);
        achievementText.text = key + " Unlocked !";

        StartCoroutine(ShowAchievementBanner());
    }

    void ActivateAchievementBanner(bool active)
    {
        achievementBanner.gameObject.SetActive(active);
    }

    IEnumerator ShowAchievementBanner()
    {
        ActivateAchievementBanner(true);
        yield return new WaitForSeconds(2f);
        ActivateAchievementBanner(false);
    }
}

