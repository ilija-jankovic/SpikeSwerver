using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class GameManager
{
    private static byte level = 1, oldLevel = 0;
    private static uint points;

    private static readonly uint[] POINTS_TO_ADVANCE_LEVEL = { 10, 20, 50, 100, 150, 200 };

    private static readonly Tuple<string, float>[][] LEVEL_SPAWN_INFO =
    { new Tuple<string, float>[]{ new Tuple<string, float>("BasicSpike", 1f) },
      new Tuple<string, float>[]{ new Tuple<string, float>("BasicSpike", 0.4f), new Tuple<string, float>("HomingSpike", 2f) }};

    [RuntimeInitializeOnLoadMethod]
    private static void Intitialise()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Reset();
    }

    public static void Reset()
    {
        points = oldLevel = 0;
        level = 1;

        foreach (Entity entity in UnityEngine.Object.FindObjectsOfType<Entity>())
            UnityEngine.Object.Destroy(entity.gameObject);

        //load intitial entities into scene
        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/StartingEntities");
        FileInfo[] info = dir.GetFiles("*.prefab");
        foreach (FileInfo f in info)
            UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/Resources/StartingEntities/"+f.Name, typeof(UnityEngine.Object)));

        //reset spawners
        AddPoints(0);
    }

    public static void AddPoints(byte amount)
    {
        points += amount;

        GameObject.Find("Points").GetComponent<TMPro.TextMeshPro>().text = points.ToString();

        if (!(level > POINTS_TO_ADVANCE_LEVEL.Length) && points >= POINTS_TO_ADVANCE_LEVEL[level - 1])
            level++;

        if (level != oldLevel && !(level > LEVEL_SPAWN_INFO.Length))
        {
            foreach (Spawner spawner in UnityEngine.Object.FindObjectsOfType<Spawner>())
                UnityEngine.Object.Destroy(spawner.gameObject);

            Tuple<string, float>[] spawnInfo = LEVEL_SPAWN_INFO[level - 1];
            foreach(Tuple<string, float> item in spawnInfo)
            {
                GameObject spawner = new GameObject(item.Item1 + "Spawner");
                spawner.transform.SetParent(GameObject.Find("Spawners").transform);
                spawner.transform.localPosition = Vector3.zero;

                Spawner spawnerComp = spawner.AddComponent<Spawner>();
                spawnerComp.type = item.Item1;
                spawnerComp.frequency = item.Item2;
            }

            oldLevel = level;
        }
    }

    public static byte Level
    {
        get { return level; }
    }

    public static float MinHeight
    {
        get
        {
            GameObject platform = GameObject.Find("Platform");
            return platform.transform.position.y - 0.5f;
        }
    }
}
