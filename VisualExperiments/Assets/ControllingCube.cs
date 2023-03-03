using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllingCube : MonoBehaviour {

    private void Start()
    {
            EasyController.esconsend.Send_data(1, 0);
            EasyController.esconsend.Send_data(2, 0);
            EasyController.esconsend.Send_data(3, 0);
    }
    void Update () {

        this.transform.Rotate(EasyController.escon.get_state(1, 0.0f, 5.0f), EasyController.escon.get_state(2), EasyController.escon.get_state(3));

        if(EasyController.escon.get_newstate(101)==1)
        {
            this.transform.Translate(new Vector3(0.0f,0.0f,-2.0f));
        }
        if (EasyController.escon.get_newstate(101)==0)
        {
            this.transform.Translate(new Vector3(0.0f, 0.0f, 2.0f));
        }

    }
}
