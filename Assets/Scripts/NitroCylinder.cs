using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroCylinder : MonoBehaviour
{
    //NITRO PREFAB

    // Update is called once per frame
    //only destroy nitro on command (only when the code says it can be destroyed) (See NitroSpawner)
    void Update()
    {
        if (NitroSpawner.destroyNitro == true)
        {
            Destroy(gameObject);
            NitroSpawner.destroyNitro = false;
        }
    }
}
