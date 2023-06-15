using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class CameraRotation : MonoBehaviour
{
    [SerializeField]
    InputActionReference inputAction;
    [SerializeField]
    VisualEffect laserEffect;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float radiusLean = 10f;
    [SerializeField]
    private float leanSpeed = 5f;
    [SerializeField]
    private float transformOffset;

    float rotationZ;
    float lean;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var input = inputAction.action.ReadValue<float>();
        var radius = laserEffect.GetFloat("TubeRadiusX");
        lean = Mathf.Lerp(lean, radius * radiusLean, Time.deltaTime * leanSpeed);
        rotationZ += input * Time.deltaTime * rotationSpeed;
        transform.position = new Vector3(radius * transformOffset, 0, 0);


        var leanRotation = Quaternion.Euler(0, lean, 0);
        leanRotation *= Quaternion.Euler(0, 0, rotationZ);
        transform.rotation = leanRotation;
    }
}
