using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using prototype3;

public class MoveLeft : MonoBehaviour
{
    private float speed = 30;
    private prototype3.PlayerController playerControllerScript;
    private const float leftBound = -15;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<prototype3.PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerControllerScript.gameOver)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
