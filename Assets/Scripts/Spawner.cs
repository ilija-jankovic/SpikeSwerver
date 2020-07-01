using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Entity
{
    [SerializeField]
    public string type;
    [SerializeField]
    public float frequency;

    private float time = -3f;
    private float nextSpawn;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        time += Time.deltaTime;
        if(time >= nextSpawn)
        {
            time = 0;
            nextSpawn = (Random.value + 0.5f) * frequency;

            Entity obj = Instantiate(Resources.Load(type) as GameObject).GetComponent<Entity>();

            //don't spawn if specific conditions are not met
            if (!obj.SpawnConditionsMet())
            {
                Destroy(obj.gameObject);
                return;
            }

            GameObject plat = GameManager.Platform;
            Vector3 platPos = plat.transform.position;
            Vector3 platScl = plat.transform.localScale;

            //spawn obj within platofrm boundries
            obj.transform.localPosition = new Vector3(Random.value * platScl.x + platPos.x - platScl.x / 2, 0, 0) + transform.position;
        }

    }
}
