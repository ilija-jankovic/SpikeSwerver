using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer),typeof(Rigidbody))]
public class Player : Entity
{
    [SerializeField]
    private float speed = 0.03f;
    private byte lives = 3;

    private Material mat;

    private Color normalCol;
    private readonly Color damagedCol = Color.red;
    private float colTransition = 1f;

    private float timeHeld;
    private const float SPEED_LOSS_MULTIPLER = 5f;
    private const float SPEED_RECOVERY_MULTIPLIER = 3f;

    protected override void Start()
    {
        base.Start();
        mat = Instantiate(GetComponent<MeshRenderer>().sharedMaterial);
        GetComponent<MeshRenderer>().sharedMaterial = mat;
        normalCol = mat.color;
    }

    protected override void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 pos = Input.mousePosition;
            Move(pos.x >= Screen.width / 2 ? 'r' : 'l');
        }
        else if (Input.anyKey)
            Move(Input.GetKey(KeyCode.RightArrow) ? 'r' : 'l');
        else
            //recover full speed if not holding
            timeHeld = Mathf.Max(timeHeld - Time.deltaTime * SPEED_RECOVERY_MULTIPLIER, 0);

        mat.color = Color.Lerp(damagedCol, normalCol, colTransition);
        colTransition = Mathf.Clamp(colTransition + 0.005f, 0, 1);

        //check if player fell
        if (transform.position.y < GameManager.MinHeight)
        {
            FindObjectOfType<HealthBar>().RemoveHealthPoints(lives);
            Kill();
        }
    }

    void Move(char dir)
    {
        GetComponent<Rigidbody>().AddForce(dir == 'r' ? new Vector3(speed / (1 + timeHeld), 0, 0) : dir == 'l' ? new Vector3(-speed / (1 + timeHeld), 0, 0) : Vector3.zero);
        //sap speed if input held
        timeHeld = 1/Mathf.Pow(timeHeld,Time.deltaTime * SPEED_LOSS_MULTIPLER);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Spike>() != null)
        {
            lives--;
            FindObjectOfType<HealthBar>().RemoveHealthPoints(1);
            if (lives == 0)
                Kill();

            colTransition = 0;
        }
    }

    private void Kill()
    {
        MenuManager.SwitchToMenu("EndScreen");
        Destroy(gameObject);
    }
}
