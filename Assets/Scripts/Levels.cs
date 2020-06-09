using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{
    //LEVEL DISPLAY

    //levels
    public static int levels;
    public static bool nextLevel;

    [SerializeField]
    Text levelText;

    private void Start()
    {
        levelText = GetComponent<Text>();

        //initialise
        nextLevel = false;
        levels = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //only increase level when 2 octopus have passed and have been destroyed
        if (OctopusSpawner.twoOctosHavePassed == true) 
        {
            levels += 1;
            nextLevel = true;
            OctopusSpawner.twoOctosHavePassed = false;
            Octopus.objectsDestroyed = 0;
        }

        levelText.text = "Level: " + levels.ToString();
    }
}
