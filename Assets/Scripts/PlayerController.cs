using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.D)) {
            transform.Translate(Vector3.right* 10);
            playerAnimator.SetBool("WalkRight", true);
            playerAnimator.SetTrigger("WalkRight");
        }
        playerAnimator.SetBool("WalkRight", false);
    }
}
