using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class FoxBehaviour : MonoBehaviour
{
    public float speed = 3.0f;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 velocity;
    private GameObject nearestGameObject;
    private Vector3 foxMeetPos = new Vector3(0, 0, -48);
    private static readonly float WANDER_SPEED = 3.0f;
    private static readonly float BUILD_FLOCK_SPEED = 4.0f;
    private static readonly float HUNT_SPEED = 7.0f;
    private static readonly float SEEK_SPEED = 2.0f;
    private static readonly float TURN_ANGLE = 90.0f;
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
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Wander"))
        {
            //rb.AddForce(Vector3.forward * Time.deltaTime * speed);
            velocity = transform.forward * WANDER_SPEED;
            transform.LookAt(nearestGameObject.transform.position);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hunt"))
        {
            velocity = transform.forward * HUNT_SPEED;
            transform.LookAt(nearestGameObject.transform.position);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Seek"))
        {
            velocity = transform.forward * SEEK_SPEED;
            transform.LookAt(nearestGameObject.transform.position);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Eat"))
        {
            velocity = Vector3.zero;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("BuildFlock"))
        {
            
                if (Vector3.Distance(this.transform.position, foxMeetPos) > 5.0f)
                {
                    this.transform.LookAt(foxMeetPos);
                    velocity = transform.forward * BUILD_FLOCK_SPEED;
                    animator.SetBool("FlockBuilt", false);
                }
                else
                {
                    animator.SetBool("FlockBuilt", true);
                }
            
        }
    }

    void FixedUpdate()
    {
        rb.velocity = velocity;
    }

    private void updateState()
    {

        Dictionary<GameObject, float> distances = new Dictionary<GameObject, float>();

        foreach (GameObject stag in GameObject.FindGameObjectsWithTag(STAG_TAG))
        {
            distances.Add(stag, Vector3.Distance(transform.position, stag.transform.position));
        }
        KeyValuePair<GameObject, float> minDistancePair = getMinimumDistanceAndGameObject(distances);
        nearestGameObject = minDistancePair.Key;
        {
            Monitor.Enter(animator);
            animator.SetFloat("minimumDistance", minDistancePair.Value);
            Monitor.Exit(animator);
        }
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
        if (collision.gameObject.tag == STAG_TAG)
        {
            Vector3 forceVector = (collision.gameObject.transform.position - transform.position).normalized;
            //collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 10);
            //rb.AddForce(-forceVector * 5);
        } else if (collision.gameObject.CompareTag("StoneWall"))
        {
            transform.Rotate(0, Random.Range(-TURN_ANGLE, TURN_ANGLE), 0);
        } else if (collision.gameObject.CompareTag(FOX_TAG))
        {
            rb.AddForce(-transform.forward * 5);
            transform.Rotate(0, 180, 0);
        } else if (collision.gameObject.CompareTag("Tree"))
        {
            transform.Rotate(0, 45, 0);  
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag(STAG_TAG))
        {
            Vector3 forceVector = (collision.gameObject.transform.position - transform.position);
            //collision.gameObject.GetComponent<Rigidbody>().AddForce(forceVector * 5);
            rb.AddForce(-forceVector * 50);
        }
    }
}
