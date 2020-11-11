using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSystem : Observer
{
    public Image achievementBanner;
    public Text achievementText;

    //event
    TileEvent cookiesEvent, cakeEvent, candyEvent;

    void Start()
    {
        PlayerPrefs.DeleteAll();

        //buat event
        cookiesEvent = new CookiesTileEvent(3);
        cakeEvent = new CakeEventTile(10);
        candyEvent = new CandyEventTile(5);

        foreach(var poi in FindObjectsOfType<PointOfInterest>())
        {
            poi.RegisterObserver(this);
        }
    }

    public override void OnNotify(string value)
    {
        string key;

        if(value.Equals("Cookies Event"))
        {
            cookiesEvent.OnMatch();

            if (cookiesEvent.AchievementCompleted())
            {
                key = "Match first cookies";
                NotifyAchievement(key, value);
            }
        }

        if (value.Equals("Cake Event"))
        {
            cakeEvent.OnMatch();

            if (cookiesEvent.AchievementCompleted())
            {
                key = "Match 10 cakes";
                NotifyAchievement(key, value);
            }
        }

        if (value.Equals("Candy Event"))
        {
            candyEvent.OnMatch();

            if (cookiesEvent.AchievementCompleted())
            {
                key = "Match 5 candies";
                NotifyAchievement(key, value);
            }
        }
    }

    void NotifyAchievement(string key, string value)
    {
        //cek jika achievement sudah diperoleh
       if(PlayerPrefs.GetInt(value) == 1)
        {
            return;
        }

        PlayerPrefs.SetInt(value, 1);
        achievementText.text = key + " Unlocked!";

        //pop up notifikasi
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

public class CookiesTileEvent : TileEvent
{
    private int matchCount;
    private int requiredAmount;

    public CookiesTileEvent(int amount)
    {
        requiredAmount = amount;
    }

    public override void OnMatch()
    {
        matchCount++;
        Debug.Log(matchCount);
    }

    public override bool AchievementCompleted()
    {
        if(matchCount == requiredAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class CakeEventTile : TileEvent
{
    private int matchCount;
    private int requiredAmount;

    public CakeEventTile(int amount)
    {
        requiredAmount = amount;
    }

    public override void OnMatch()
    {
        matchCount++;
    }

    public override bool AchievementCompleted()
    {
        if (matchCount == requiredAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class CandyEventTile : TileEvent
{
    private int matchCount;
    private int requiredAmount;

    public CandyEventTile(int amount)
    {
        requiredAmount = amount;
    }

    public override void OnMatch()
    {
        matchCount++;
    }

    public override bool AchievementCompleted()
    {
        if (matchCount == requiredAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}