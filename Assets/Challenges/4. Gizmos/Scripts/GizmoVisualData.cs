using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GizmoVisualData", menuName = "GizmoChallenge/VisualData")]
public class GizmoVisualData : ScriptableObject
{
    [SerializeField] Color color;
    [SerializeField] int smoothingValue;
    [SerializeField] float textSize;

    public int SmootingValue => smoothingValue;
    public float TextSize => textSize;

    public Color Color => color;


}
