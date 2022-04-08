using System;
using UnityEngine;

namespace Challenges._4._Gizmos.Scripts
{
    public class BezierCurve : MonoBehaviour
    {
        [SerializeField]
        private Transform point1;
        [SerializeField]
        private Transform handle1;
        [SerializeField]
        private Transform point2;
        [SerializeField]
        private Transform handle2;
        //Edit below


        Vector3 p1, h1, h2, p2;

        void OnDrawGizmos()
        {
            p1 = point1.position;
            h1 = handle1.position;
            h2 = handle2.position;
            p2 = point2.position;


            Gizmos.color = Color.red;

            Vector3 prevPos = p1;

            float resolution = 0.02f;


            int loops = Mathf.FloorToInt(1f / resolution);

            for (int i = 1; i <= loops; i++)
            {

                float t = i * resolution;
                Vector3 nextPos = Interpolate(t);
                Gizmos.DrawLine(prevPos, nextPos);


                prevPos = nextPos;
            }

            Gizmos.color = Color.green;

            Gizmos.DrawLine(p1, h1);
            Gizmos.DrawLine(h2, p2);
        }


        Vector3 Interpolate(float t)
        {



            float oneMinusT = 1f - t;


            Vector3 a = oneMinusT * p1 + t * h1;
            Vector3 b = oneMinusT * h1 + t * h2;
            Vector3 c = oneMinusT * h2 + t * p2;

            Vector3 d = oneMinusT * a + t * b;
            Vector3 e = oneMinusT * b + t * c;

            return oneMinusT * d + t * e;
        }

    }
}
