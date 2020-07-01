using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : FallingEntity
{
    public enum Types { points, health };
    [SerializeField]
    byte type;
    [SerializeField]
    byte minCollectables;
    [SerializeField]
    byte maxCollectables;
    [SerializeField]
    float radius;

    protected override void Start()
    {
        base.Start();
        for (byte i = 0; i < Random.Range(minCollectables, maxCollectables); i++)
        {
            GameObject obj = null;
            switch (type)
            {
                case (byte)Types.points:
                    obj = Instantiate(Resources.Load("PointCollectableParticle") as GameObject);
                    break;
                case (byte)Types.health:
                    obj = Instantiate(Resources.Load("HealthCollectableParticle") as GameObject);
                    break;
            }
            if (obj)
            {
                obj.transform.SetParent(gameObject.transform);
                obj.transform.localPosition = Random.insideUnitSphere * radius;
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected void OnTriggerStay(Collider other)
    {
        if (GameManager.Player && other.gameObject == GameManager.Player.gameObject)
        {
            //enable collected behaviour in cube
            foreach (Transform cube in GetComponentInChildren<Transform>())
            {
                CollectableParticle ccComp = cube.GetComponent<CollectableParticle>();
                if (ccComp)
                {
                    ccComp.Collect();
                    //sometime compnent is randomly disabled so this is a quick fix
                    ccComp.enabled = true;
                }
            }

            //add to health
            if (type == (byte)Types.health)
                GameManager.HealthBar.AddHealth();

            Destroy(gameObject);
        }
    }

    public override bool SpawnConditionsMet()
    {
        //only spawn health if player is below maxm health
        return type != (byte)Types.health || GameManager.HealthBar.Health < GameManager.HealthBar.MaxHealth;
    }
}
