using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class GameManager
{
    private static byte level = 1, oldLevel = 0;
    private static uint points;

    public static readonly uint[] POINTS_TO_ADVANCE_LEVEL = { 10, 30, 80, 150, 400, 1000, 2500 };

    public static readonly Color[] LEVEL_COLOURS = { Color.gray, Color.cyan/2, Color.green/2, Color.yellow/2, Color.red/2, Color.black, Color.white };

    //main level info
    public static readonly Tuple<string, float>[][] LEVEL_SPAWN_INFO =
    { new Tuple<string, float>[]{ new Tuple<string, float>("BasicSpike", 1f), new Tuple<string, float>("PointCollectable", 6f), new Tuple<string, float>("HealthCollectable", 6f)  },
      new Tuple<string, float>[]{ new Tuple<string, float>("BasicSpike", 0.7f), new Tuple<string, float>("PointCollectable", 6f), new Tuple<string, float>("HealthCollectable", 6f)},
      new Tuple<string, float>[]{ new Tuple<string, float>("BasicSpike", 0.7f), new Tuple<string, float>("HomingSpike", 2f), new Tuple<string, float>("PointCollectable", 6f), new Tuple<string, float>("HealthCollectable", 5f)  },
      new Tuple<string, float>[]{ new Tuple<string, float>("BasicSpike", 0.6f), new Tuple<string, float>("HomingSpike", 1f), new Tuple<string, float>("PointCollectable", 5f), new Tuple<string, float>("HealthCollectable", 5f)  },
      new Tuple<string, float>[]{ new Tuple<string, float>("BasicSpike", 0.4f), new Tuple<string, float>("HomingSpike", 1f), new Tuple<string, float>("PointCollectable", 4f), new Tuple<string, float>("HealthCollectable", 4f)  },
      new Tuple<string, float>[]{ new Tuple<string, float>("BasicSpike", 0.35f), new Tuple<string, float>("HomingSpike", 0.5f), new Tuple<string, float>("PointCollectable", 3f), new Tuple<string, float>("HealthCollectable", 3f) },
      new Tuple<string, float>[]{ new Tuple<string, float>("BasicSpike", 0.2f), new Tuple<string, float>("HomingSpike", 0.25f), new Tuple<string, float>("PointCollectable", 1.5f), new Tuple<string, float>("HealthCollectable", 2f) } };

    private static GameObject _plat;
    private static Player _player;
    private static TMPro.TextMeshPro _pointsText;

    private static HighScorePanel highScorePanel;
    private static GameObject background;
    private static TMPro.TextMeshPro helpText;
    private static GoogleMobileAdsScript googleAds;

    private static GameObject spawnerSpawnLoc;
    private static List<GameObject> spawners = new List<GameObject>();

    private static uint gamesPlayed = 0;

    [RuntimeInitializeOnLoadMethod]
    private static void Intitialise()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        //initialise unchanging fields
        _plat = GameObject.Find("Platform");
        _pointsText = GameObject.Find("Points").GetComponent<TMPro.TextMeshPro>();

        highScorePanel = GameObject.FindObjectOfType<HighScorePanel>();
        background = GameObject.Find("Background");
        helpText = GameObject.Find("HelpText").GetComponent<TMPro.TextMeshPro>();
        googleAds = GameObject.Find("GoogleAds").GetComponent<GoogleMobileAdsScript>();

        spawnerSpawnLoc = GameObject.Find("Spawners");

        Reset();
    }

    public static void Reset()
    {
        points = oldLevel = 0;
        level = 1;

        foreach (Entity entity in UnityEngine.Object.FindObjectsOfType<Entity>())
            UnityEngine.Object.Destroy(entity.gameObject);

        //load intitial entities into scene
        /*DirectoryInfo dir = new DirectoryInfo("Assets/Resources/StartingEntities");
        FileInfo[] info = dir.GetFiles("*.prefab");
        foreach (FileInfo f in info)
            UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/Resources/StartingEntities/"+f.Name, typeof(UnityEngine.Object)));*/

        //initialise volitile fields
        _player = UnityEngine.Object.Instantiate(Resources.Load("StartingEntities/Player") as GameObject).GetComponent<Player>();
        HealthBar = UnityEngine.Object.Instantiate(Resources.Load("StartingEntities/HealthBar") as GameObject).GetComponent<HealthBar>();

        //reset highscore panel
        highScorePanel.Reset();

        //reset spawners
        AddPoints(0);
    }

    public static void AddPoints(byte amount)
    {
        points += amount;

        //flash background
        Flash flash = PointsText.GetComponentInParent<Flash>();
        if (flash)
            flash.Go();

        if (!(level > POINTS_TO_ADVANCE_LEVEL.Length) && points >= POINTS_TO_ADVANCE_LEVEL[level - 1])
            level++;

        if (level != oldLevel && !(level > LEVEL_SPAWN_INFO.Length))
        {
            //get rid of old spawners
            foreach (GameObject spawner in spawners)
                UnityEngine.Object.Destroy(spawner);
            spawners.Clear();

            Tuple<string, float>[] spawnInfo = LEVEL_SPAWN_INFO[level - 1];
            foreach(Tuple<string, float> item in spawnInfo)
            {
                GameObject spawner = new GameObject(item.Item1 + "Spawner");
                spawner.transform.SetParent(spawnerSpawnLoc.transform);
                spawner.transform.localPosition = Vector3.zero;

                Spawner spawnerComp = spawner.AddComponent<Spawner>();
                spawnerComp.type = item.Item1;
                spawnerComp.frequency = item.Item2;

                spawners.Add(spawner);
            }

            //show level animation
            PointsText.GetComponent<TextAnimation>().Write("Level " + level);

            //change background colour
            if (!(level > LEVEL_COLOURS.Length))
            {
                Fade fade = background.GetComponent<Fade>();
                if (fade)
                    fade.Go(LEVEL_COLOURS[level - 1]);
            }

            oldLevel = level;
        }
    }

    public static void EndGame()
    {
        MenuManager.SwitchToMenu("EndScreen");

        //update highscore
        if (points > HighScore)
            HighScore = points;

        highScorePanel.Show();

        gamesPlayed++;
        //show interstitials every interval with an offset of a number of games played
        if ((GoogleMobileAdsScript.GAMES_BETWEEN_INTERSTITALS + gamesPlayed - GoogleMobileAdsScript.GAMES_BEFORE_FIRST_INTERSTITIAL) % GoogleMobileAdsScript.GAMES_BETWEEN_INTERSTITALS == 0)
            googleAds.RequestInterstitial();
    }

    public static void DisableHelpText()
    {
        helpText.enabled = false;
    }

    public static byte Level
    {
        get { return level; }
    }

    public static float MinHeight
    {
        get { return Platform.transform.position.y - 0.5f; }
    }

    public static GameObject Platform
    {
        get { return _plat; }
    }

    public static Player Player
    {
        get { return _player; }
    }

    public static TMPro.TextMeshPro PointsText
    {
        get { return _pointsText; }
    }

    public static HealthBar HealthBar { get; private set; }

    public static uint Points
    {
        get { return points; }
    }

    public static uint HighScore
    {
        get
        {
            if (!PlayerPrefs.HasKey("highScore"))
                PlayerPrefs.SetInt("highScore", 0);
            return (uint)PlayerPrefs.GetInt("highScore");
        }
        set
        {
            PlayerPrefs.SetInt("highScore", (int)value);
        }
    }
}
