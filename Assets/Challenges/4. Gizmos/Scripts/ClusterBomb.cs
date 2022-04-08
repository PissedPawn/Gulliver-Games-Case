using System;
using System.Collections;
using UnityEngine;

namespace Challenges._4._Gizmos.Scripts
{
    public class ClusterBomb : MonoBehaviour
    {
        [SerializeField]
        private ClusterBombData clusterBombData;

        [SerializeField]
        private GizmoVisualData visualData;
        //Edit below




        

        private void OnDrawGizmos()
        {

            Gizmos.color = visualData.Color;


            Gizmos.DrawSphere(transform.position, clusterBombData.SelfExplosionRadius);

            CalcuateChildPosition();

        }





        void CalcuateChildPosition()
        {


            int childCount = clusterBombData.ChildCount;
            double h = clusterBombData.ChildInitialJumpHeight;
            double b = clusterBombData.ChildBounceCount;
            double d = clusterBombData.ChildTravelDistance;
            double f = clusterBombData.ChildBounceFalloff;

            int segments = visualData.SmootingValue;


            double increment = d / segments;

            float angleDelta = 360 / (float)childCount;
            float angle = 0;
            for (int i = 1; i <= childCount; i++)
            {
                Vector3 prevPoint = transform.position;
                Vector3 childLandPos = Vector3.zero;

                float xDir = Mathf.Sin((angle * Mathf.PI) / 180);
                float zDir = Mathf.Cos((angle * Mathf.PI) / 180);





                for (double x = 0; x <= d; x += increment)
                {

                    Vector3 nextPos = new Vector3((float)x * xDir, (float)Height(x, f, b, h, d), (float)x * zDir) + transform.position;

                    childLandPos = nextPos;

                    Vector3 dir = nextPos - prevPoint;
                    Gizmos.DrawRay(prevPoint, dir);
                    prevPoint = nextPos;



                }


                angle += angleDelta;
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(childLandPos, clusterBombData.ChildExplosionRadius);
                Gizmos.color = visualData.Color;

            }
        }




        double F(double x, double h, double f)
        {
            return h / Math.Pow((x + 1), f);
        }

        double B(double x, double d, double b)
        {
            return Math.Sin(b * x / d * Mathf.PI);
        }

        double J(double x, double d, double b)
        {
            return (Mathf.Floor((float)(x * b / d)) * d) / b + d / (2 * b);
        }

        double Height(double x, double falloff, double bounce, double height, double distance)
        {
            return Math.Abs(B(x, distance, bounce)) * F(J(x, distance, bounce), height, falloff);
        }
    }
}
