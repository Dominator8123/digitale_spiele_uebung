using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace protoype4
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody playerRb;
        public float speed = 5.0f;
        private GameObject focalPoint;
        public bool hasPowerup;
        private float powerupStrength = 15.0f;
        public GameObject powerupIndicator;

        // Start is called before the first frame update
        void Start()
        {
            playerRb = GetComponent<Rigidbody>();
            focalPoint = GameObject.Find("Focal Point");

        }

        // Update is called once per frame
        void Update()
        {
            float forwardInput = Input.GetAxis("Vertical");
            playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
            powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Powerup"))
            {
                hasPowerup = true;
                Destroy(other.gameObject);
                StartCoroutine(PowerupCountdownRoutine());
                powerupIndicator.gameObject.SetActive(true);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
            {
                Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 awayFromPlayer = (collision.transform.position - playerRb.transform.position);
                Debug.Log("Player collided with " + collision.gameObject.name + " with powerup set to " + hasPowerup);
                enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
        }

        IEnumerator PowerupCountdownRoutine()
        {
            yield return new WaitForSeconds(7);
            hasPowerup = false;
            powerupIndicator.gameObject.SetActive(false);
        }
    }
}
