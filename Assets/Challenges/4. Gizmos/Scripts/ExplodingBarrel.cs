using System;
using UnityEngine;
using UnityEditor;

namespace Challenges._4._Gizmos.Scripts
{
    public class ExplodingBarrel : MonoBehaviour
    {
        [SerializeField]
        private ExplodingBarrelData explodingBarrelData;
        //Edit below
        GUIStyle style;


        private void OnDrawGizmos()
        {
            style = new GUIStyle();
            style.normal.textColor = GetColor();
            Gizmos.color = GetColor();
            Gizmos.DrawSphere(transform.position, explodingBarrelData.ExplosionRadius);
            Handles.Label(transform.position + Vector3.up * (explodingBarrelData.ExplosionRadius + 5), explodingBarrelData.Damage.ToString(), style);
        }



        Color GetColor()
        {
            switch (explodingBarrelData.DamageType)
            {
                case DamageType.Water:
                    return Color.blue;

                case DamageType.Air:
                    return Color.white;


                case DamageType.Earth:
                    return Color.green;

                case DamageType.Fire:

                    return Color.red;
                case DamageType.LongAgoTheFourNationsLivedTogetherInHarmony:

                default:
                    return Color.black;
            }


        }
    }
}
