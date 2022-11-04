using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCapture : MonoBehaviour
{
    WebCamTexture texture;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        texture = new WebCamTexture(WebCamTexture.devices[0].name);
        spriteRenderer.material.SetTexture("WebcamTexture", texture);
        texture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
