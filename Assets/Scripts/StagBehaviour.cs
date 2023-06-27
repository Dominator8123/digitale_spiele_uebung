using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class StagBehaviour : MonoBehaviour
{
    public float speed = 1.0f;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 velocity;
    private readonly Vector3 stagsMeetPos = new Vector3(0, 0, 48);
    private bool flockBuilt = false;
    private bool foxWasNear = false;
    private static readonly float FLEE_SPEED = 10.0f;
    private static readonly float FOX_IS_NEAR_DISTANCE = 15.0f;
    private static readonly string FOX_TAG = "Fox";
    private static readonly string STAG_TAG = "Stag";

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody>();
        InvokeRepeating("updateState", 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!flockBuilt)
        {
            
                if (Vector3.Distance(transform.position, stagsMeetPos) > 7.0f)
                {
                    transform.LookAt(stagsMeetPos);
                }
                else
                {
                    flockBuilt = true;
                }
            
        }
        else
        {
            if (!foxWasNear)
            {
                animator.SetBool("Eat_b", true);
            }
            else
            {
                animator.SetBool("Eat_b", false);
            }
        }
        Monitor.Enter(animator);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion"))
        {
            //rb.AddForce(Vector3.forward * Time.deltaTime * speed);
            velocity = transform.forward * speed;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Flee"))
        {
            foxWasNear = true;
            velocity = transform.forward * FLEE_SPEED;
        } else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Eat"))
        {
            velocity = Vector3.zero;
        }
        Monitor.Exit(animator);
    }

    private void updateState()
    {
        Dictionary<GameObject, float> distances = new Dictionary<GameObject, float>();


        foreach (GameObject fox in GameObject.FindGameObjectsWithTag(FOX_TAG))
        {
            distances.Add(fox, Vector3.Distance(transform.position, fox.transform.position));
        }
        KeyValuePair<GameObject, float> minDistancePair = getMinimumDistanceAndGameObject(distances);
        {
            Monitor.Enter(animator);
            {
                animator.SetBool("FoxIsNear", minDistancePair.Value <= FOX_IS_NEAR_DISTANCE);

            }
            Monitor.Exit(animator);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = velocity;
    }

    private static KeyValuePair<GameObject, float> getMinimumDistanceAndGameObject(Dictionary<GameObject, float> distances)
    {
        KeyValuePair<GameObject, float> minPair = distances.ElementAt(0);
        foreach (KeyValuePair<GameObject, float> entry in distances)
        {
            if (entry.Value < minPair.Value)
            {
                minPair = entry;
            }
        }
        return minPair;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(FOX_TAG))
        {
            Monitor.Enter(animator);
            {
                animator.SetBool("FoxIsNear", true);
            }
            Monitor.Exit(animator);
            Vector3 fleeDir = -(collision.gameObject.transform.position - transform.position);
            transform.rotation = Quaternion.LookRotation(fleeDir);
        }
        else if (collision.gameObject.CompareTag("StoneWall"))
        {
            int choice = Random.Range(0, 3);
            if (choice == 0)
            {
                transform.Rotate(0, 180, 0);
            }
            else if (choice == 1)
            {

            }
            else
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
