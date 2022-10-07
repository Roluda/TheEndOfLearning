using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TunnelEffect : MonoBehaviour
{
    [SerializeField]
    VisualEffect effect;

    public float speed;
    public Vector3 direction;
    public Vector3 center;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
