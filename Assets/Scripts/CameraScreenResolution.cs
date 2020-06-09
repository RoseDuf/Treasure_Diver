using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScreenResolution : MonoBehaviour
{
    public SpriteRenderer backGround;

    private void Start()
    {
        //1 width
        //float orthoSize = backGround.bounds.size.x * Screen.height / Screen.width * 0.5f;
        //Camera.main.orthographicSize = orthoSize;

        //2 height
        //float orthoSize = backGround.bounds.size.y * 0.5f;
        //Camera.main.orthographicSize = orthoSize;


        //3 adjust the screen to always display the main area of the game
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = backGround.bounds.size.x / backGround.bounds.size.y;

        if (screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = backGround.bounds.size.y / 2;

        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = backGround.bounds.size.y / 2 * differenceInSize;
        }
    }


}
