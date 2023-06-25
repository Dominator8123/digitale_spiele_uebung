using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 40.0f;
    private Animator animator;
    private Locator locator;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
        locator = GameObject.Find("Locator").GetComponent<Locator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion"))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hunt"))
        {
            Debug.Log("Hunting");
            locator.huntAll();
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Seek"))
        {
            Debug.Log("Seeking");
            locator.seekAll();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("StoneWall"))
        {
            transform.Rotate(0, Random.Range(1, 90), 0);
            Debug.Log("Collided with Stone Wall");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StoneWall"))
        {
            transform.Rotate(0, Random.Range(90, 270), 0);
            Debug.Log("Collided with Stone Wall");
        }
    }
}
