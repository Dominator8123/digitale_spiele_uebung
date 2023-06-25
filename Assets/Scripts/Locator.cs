using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Locator : MonoBehaviour
{
    private List<GameObject> foxes;
    private List<GameObject> stags;

    private const float HUNT_DISTANCE = 2.0f;

    public float huntSpeed = 40.0f;

    // Start is called before the first frame update
    void Start()
    {
        foxes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Fox"));
        stags = new List<GameObject>(GameObject.FindGameObjectsWithTag("Stag"));
    }

    // Update is called once per frame
    void Update()
    {
        checkIfShallHunt();
    }

    public void addAnimals()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Fox"))
        {
            foxes.Add(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Stag"))
        {
            stags.Add(obj);
        }
        Debug.Log(foxes.Count);
        Debug.Log(stags.Count);
    }

    private void checkIfShallHunt()
    {
        foreach (GameObject fox in foxes)
        {
            foreach (GameObject stag in stags)
            {
                if (fox != null && stag != null)
                {
                    if (Vector3.Distance(fox.transform.position, stag.transform.position) < HUNT_DISTANCE)
                    {
                        fox.GetComponent<Animator>().SetBool("IsHunting", true);
                        stag.GetComponent<Animator>().SetBool("isHunted", true);
                    }
                    else if (Vector3.Distance(fox.transform.position, stag.transform.position) > 2*HUNT_DISTANCE)
                    {
                        fox.GetComponent<Animator>().SetBool("IsHunting", false);
                        fox.GetComponent<Animator>().SetBool("IsSeeking", false);
                    } else
                    {
                        fox.GetComponent<Animator>().SetBool("IsHunting", false);
                        fox.GetComponent<Animator>().SetBool("IsSeeking", true);
                    }
                }
            }
        }
    }

    private void seek(GameObject obj)
    {
        int index = Random.Range(0, stags.Count);
        obj.GetComponent<Rigidbody>().AddForce((stags[index].transform.position - obj.transform.position).normalized * huntSpeed);
        obj.transform.LookAt(stags[index].transform.position);
    }

    public void seekAll()
    {
        foreach (GameObject fox in foxes)
        {
            seek(fox);
        }
    }


    private void hunt(GameObject obj)
    {
        int index = Random.Range(0, stags.Count);
        obj.GetComponent<Rigidbody>().AddForce((stags[index].transform.position - obj.transform.position).normalized * huntSpeed);
        obj.transform.LookAt(stags[index].transform.position);
    }

    public void huntAll()
    {
        foreach (GameObject fox in foxes)
        {
            hunt(fox);
        }
    }
}
