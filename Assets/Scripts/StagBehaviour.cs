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
    private bool fleeDirDetermined = false;
    private Vector3 fleeDir;
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
       
        Monitor.Enter(animator);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion"))
        {
            velocity = transform.forward * speed;
            fleeDirDetermined = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Flee"))
        {
            transform.LookAt(transform.position + fleeDir);
            velocity = transform.forward * FLEE_SPEED;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Eat"))
        {
            velocity = Vector3.zero;
            fleeDirDetermined = false;

        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("BuildFlock"))
        {
            velocity = transform.forward * speed;
            fleeDirDetermined = false;

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
        if (Vector3.Distance(transform.position, stagsMeetPos) > 10.0f)
        {
            this.transform.LookAt(stagsMeetPos);
        }
        else
        {
            animator.SetBool("FlockBuilt", true);
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
            if (!fleeDirDetermined)
            {
                fleeDir = -(collision.transform.position - transform.position);
                fleeDirDetermined = true;
            }
            //  transform.rotation = Quaternion.LookRotation(fleeDir);
            //transform.rotation.SetLookRotation(fleeDir);

        }
        else if (collision.gameObject.CompareTag("StoneWall"))
        {
            transform.Rotate(0, Random.Range(-90, 90), 0);
        } else if (collision.gameObject.CompareTag("Tree"))
        {
            transform.Rotate(0, 45, 0);
        }
    }

    private void onCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(FOX_TAG))
        {
            Vector3 fleeDir = -(collision.transform.position - transform.position);
           // transform.rotation = Quaternion.LookRotation(fleeDir);
            //transform.rotation.SetLookRotation(fleeDir);
        }
    }
}
