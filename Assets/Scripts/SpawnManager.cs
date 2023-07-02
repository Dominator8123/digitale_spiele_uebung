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
        Instantiate(fox2Prefab, generateRandomPosition(), fox2Prefab.transform.rotation);


        for (int i = 0; i < 8; i++)
        {
            Instantiate(stagPrefab, generateRandomPosition(), stagPrefab.transform.rotation);
        }
    }

    Vector3 generateRandomPosition()
    {
        return new Vector3(Random.Range(-SPAWN_RANGE, SPAWN_RANGE), 0, Random.Range(-SPAWN_RANGE, SPAWN_RANGE));
    }
}
