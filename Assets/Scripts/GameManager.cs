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

    public static readonly uint[] POINTS_TO_ADVANCE_LEVEL = { 10, 30, 80, 150, 400, 1000 };

    //main level info
    public static readonly Tuple<string, float>[][] LEVEL_SPAWN_INFO =
    { new Tuple<string, float>[]{ new Tuple<string, float>("BasicSpike", 1f), new Tuple<string, float>("Collectable", 6f) },
      new Tuple<string, float>[]{ new Tuple<string, float>("BasicSpike", 0.7f), new Tuple<string, float>("Collectable", 6f)},
      new Tuple<string, float>[]{ new Tuple<string, float>("BasicSpike", 0.7f), new Tuple<string, float>("HomingSpike", 2f), new Tuple<string, float>("Collectable", 6f)  },
      new Tuple<string, float>[]{ new Tuple<string, float>("BasicSpike", 0.6f), new Tuple<string, float>("HomingSpike", 1f), new Tuple<string, float>("Collectable", 6f)  },
      new Tuple<string, float>[]{ new Tuple<string, float>("BasicSpike", 0.4f), new Tuple<string, float>("HomingSpike", 1f), new Tuple<string, float>("Collectable", 6f)  }};

    private static GameObject _plat;
    private static Player _player;
    private static TMPro.TextMeshPro _pointsText;
    private static HealthBar _healthBar;

    private static GameObject spawnerSpawnLoc;
    private static List<GameObject> spawners = new List<GameObject>();

    [RuntimeInitializeOnLoadMethod]
    private static void Intitialise()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        //initialise unchanging fields
        _plat = GameObject.Find("Platform");
        _pointsText = GameObject.Find("Points").GetComponent<TMPro.TextMeshPro>();
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
        _healthBar = UnityEngine.Object.Instantiate(Resources.Load("StartingEntities/HealthBar") as GameObject).GetComponent<HealthBar>();

        //reset spawners
        AddPoints(0);
    }

    public static void AddPoints(byte amount)
    {
        points += amount;

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

            oldLevel = level;
        }
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

    public static HealthBar HealthBar
    {
        get { return _healthBar; }
    }

    public static uint Points
    {
        get { return points; }
    }
}
