using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFollow : MonoBehaviour
{
    public Transform Follow;

    private Camera MainCamera;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;    
    }

    // Update is called once per frame
    void Update()
    {
        var screenPos = MainCamera.WorldToScreenPoint(Follow.position);

        transform.position = screenPos + new Vector3(1,0,0) * 200 + new Vector3(0,1,0) * 250;
    }
}
