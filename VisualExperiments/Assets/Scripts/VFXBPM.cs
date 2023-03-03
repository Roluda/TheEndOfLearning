using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[AddComponentMenu("VFX/Property Binders/Audio BPM Binder")]
[VFXBinder("Audio/BPM")]
public class VFXBPM : VFXBinderBase
{

    public string bpmProperty { get { return (string)m_CountProperty; } set { m_CountProperty = value; } }
    [VFXPropertyBinding("System.Int32"), SerializeField]
    protected ExposedProperty m_CountProperty = "BPM";

    public BPMMeasure bpmMeasure = null;
    public float lerpSpeed = 5;

    LinkedList<int> measures = new LinkedList<int>();


    public override bool IsValid(VisualEffect component)
    {
        return bpmMeasure;
    }

    public override void UpdateBinding(VisualEffect component)
    {
        int newMeasure = bpmMeasure.bpm;
        measures.AddFirst(newMeasure);
        if(measures.Count > 300)
        {
            measures.RemoveLast();
        }

        component.SetInt(bpmProperty, (int)measures.Average());
    }
}