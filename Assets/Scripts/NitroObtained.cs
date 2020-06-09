using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroObtained : MonoBehaviour
{
    //FUNCTION TO DISPLAY IF YOU HAVE OBTAINED NITRO

    [SerializeField]
    GameObject nitro;

    public static int nitros;

    // Start is called before the first frame update
    void Start()
    {
        nitros = 0;
        nitro.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (nitros)
        {
            case 1:
                nitro.gameObject.SetActive(true);
                break;
            case 0:
                nitro.gameObject.SetActive(false);
                break;
        }
    }
}
