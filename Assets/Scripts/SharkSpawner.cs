using UnityEngine;

public class SharkSpawner : MonoBehaviour
{
    //SHARK SPAWNER AND TIMER

    float spawnDelay;

    [SerializeField]
    GameObject shark;

    [SerializeField]
    Transform[] spawnPoints;

    float nextTimeToSpawn = 0f;

    //float countDownTimer = 3f;

    private void Update()
    {
        //other way to do spawn
        //if (countDownTimer <= 0f)
        //{
        //    SpawnShark();
        //    countDownTimer = 3f;
        //}
        //else
        //{
        //    countDownTimer -= Time.deltaTime; //every frame decrease by 1
        //}
        
        spawnDelayDecrease();

        if (nextTimeToSpawn <= Time.time) //Time.time is the number of seconds elapsed since the start of the game
        {
            SpawnShark();
            nextTimeToSpawn = Time.time + spawnDelay;
        }
    }

    //the more levels we go through, the faster the sharks become
    void spawnDelayDecrease()
    {
        //spawnDelay = Mathf.Pow(1.4f, -(7-1) + 3.7f); // exponential
        spawnDelay = 4f / ((float)Levels.levels); // division
    }

    void SpawnShark()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        Instantiate(shark, spawnPoint.position, spawnPoint.rotation);
    }
}
