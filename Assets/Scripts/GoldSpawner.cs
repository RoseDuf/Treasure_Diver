using UnityEngine;

public class GoldSpawner : MonoBehaviour
{
    //desired time between each spawn.
    [SerializeField]
    float spawnDelay;
    
    //3 types of gold
    [SerializeField]
    GameObject gold;
    [SerializeField]
    GameObject gold2;
    [SerializeField]
    GameObject gold3;

    //3 probabilities. Should add up to 100
    [SerializeField]
    int probability1;
    [SerializeField]
    int probability2;
    [SerializeField]
    int probability3;

    //their spawning coordinates
    [SerializeField]
    float coordY;
    [SerializeField]
    float minX;
    [SerializeField]
    float maxX;

    float nextTimeToSpawn = 0f;

    //Timer
    private void Update()
    {
        if (nextTimeToSpawn <= Time.time) //Time.time is the number of seconds elapsed since the start of the game
        {
            SpawnGold();
            nextTimeToSpawn = Time.time + spawnDelay;
        }
    }

    //spawning gold depends on probability
    void SpawnGold()
    {
        float coordX = Random.Range(minX, maxX);
        float prob = Random.Range(0, 100);

        //assuming the probability of gold > gold2 > gold3
        if (prob <= probability1)
        {
            Instantiate(gold, new Vector2(coordX, coordY), Quaternion.identity);
        }
        else if ((prob <= probability2+probability1) && (prob > probability1))
        {
            Instantiate(gold2, new Vector2(coordX, coordY), Quaternion.identity);
        }
        else if ((prob <= probability3+probability2+probability1) && (prob > probability2+probability1))
        {
            Instantiate(gold3, new Vector2(coordX, coordY), Quaternion.identity);
        }
    }
}
