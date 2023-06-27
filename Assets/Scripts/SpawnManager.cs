using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject fox1Prefab;
    public GameObject fox2Prefab;
    public GameObject stagPrefab;


    private const float SPAWN_RANGE = 48.0f;

    // Start is called before the first frame update
    void Start()
    {
        SpawnNPCs();        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnNPCs()
    {
        Instantiate(fox1Prefab, generateRandomPosition(), fox1Prefab.transform.rotation);
        fox1Prefab.transform.LookAt(new Vector3(0, 0, 0));
        Instantiate(fox2Prefab, generateRandomPosition(), fox2Prefab.transform.rotation);
        fox2Prefab.transform.LookAt(new Vector3(0, 0, 0));

        for (int i = 0; i < 8; i++)
        {
            Instantiate(stagPrefab, generateRandomPosition(), stagPrefab.transform.rotation);
            stagPrefab.transform.LookAt(new Vector3(48, 0, 0));
        }
    }

    Vector3 generateRandomPosition()
    {
        return new Vector3(Random.Range(-SPAWN_RANGE, SPAWN_RANGE), 0, Random.Range(-SPAWN_RANGE, SPAWN_RANGE));
    }
}
