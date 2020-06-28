using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Entity
{
    [SerializeField]
    public string type;
    [SerializeField]
    public float frequency;

    private float time = -1f;
    private float nextSpawn;

    protected override void Update()
    {
        time += Time.deltaTime;
        if(time >= nextSpawn)
        {
            GameObject spike = Instantiate(Resources.Load(type) as GameObject);

            GameObject plat = GameObject.Find("Platform");
            Vector3 platPos = plat.transform.position;
            Vector3 platScl = plat.transform.localScale;

            spike.transform.localPosition = new Vector3(Random.value*platScl.x*1.1f + platPos.x - platScl.x/2,0,0) + transform.position;

            time = 0;
            nextSpawn = (Random.value + 0.5f) * frequency;
        }

    }
}
