using UnityEngine;

public class OctopusSpawner : MonoBehaviour
{
    float spawnDelay;
    [SerializeField]
    float minSpawnDelay = 5;
    [SerializeField]
    float maxSpawnDelay = 10;

    [SerializeField]
    GameObject octopus;

    [SerializeField]
    Transform[] spawnPoints;

    public static float nextTimeToSpawn;
    int numberOfTimesSpawned = 0; //just an iterator
    bool delayer = true;

    public static bool twoOctosHavePassed; // to check when to change level

    private void Start()
    {
        spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        nextTimeToSpawn = Time.time + spawnDelay;
        twoOctosHavePassed = false;
        Octopus.objectsDestroyed = 0;
    }

    private void Update()
    {
        //Debug.Log(spawnDelay);
        //Debug.Log(nextTimeToSpawn);
        //Debug.Log(Octopus.objectsDestroyed);
        
        //when 2 octopus have been destroyed, you can set bool to true (For Levels script)
        if (Octopus.objectsDestroyed == 2)
        {
            twoOctosHavePassed = true;
            numberOfTimesSpawned += 1;
        }

        //This is to make sure that the next spawn of 2 octos has an interval between them.
        if (twoOctosHavePassed == false)
        {
            if(numberOfTimesSpawned > 2)
            {
                delayer = true;
                nextTimeToSpawn = Time.time + spawnDelay;
                numberOfTimesSpawned = 0;
            }
            Timer();
        }
    }

    void Timer()
    {
        if (nextTimeToSpawn <= Time.time) //Time.time is the number of seconds elapsed since the start of the game
        {
            if (numberOfTimesSpawned == 0) //for the first spawn
            {
                SpawnOcto();
                numberOfTimesSpawned += 1;
                spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            }
            //this following if-statement is just to set the nexttimetospawn so that in the next loop the second octopus will spawn at the given random time
            //otherwise the second octopus will just spawn immediately
            if (Octopus.objectsDestroyed == 1 && delayer == true)
            {
                nextTimeToSpawn = Time.time + spawnDelay;
                delayer = false; //just a boolean so that we don't go through this if-statement again
                return;
            }

            if (numberOfTimesSpawned == 1 && Octopus.objectsDestroyed == 1) //for all future spawns
            {
                SpawnOcto();
                numberOfTimesSpawned += 1;
                spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            }
        }
    }

    void SpawnOcto()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        Instantiate(octopus, spawnPoint.position, spawnPoint.rotation);
    }
}
