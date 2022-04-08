using System;
using System.Collections.Generic;
using UnityEngine;

namespace Challenges._4._Gizmos.Scripts
{
    public class Node : MonoBehaviour
    {
        [SerializeField]
        private List<Node> childrenNodes;
        //Edit below


        private void OnDrawGizmos()
        {
            foreach (var child in childrenNodes)
            {
                Vector3 dir = child.transform.position - transform.position;
                ConnectNodes(this.transform.position, dir, Color.red, Color.yellow, 1, 10);
            }
        }

        public static void ConnectNodes(Vector3 pos, Vector3 direction, Color mainColor, Color markerColor, float markerLength, float markerAngle)
        {
            Gizmos.color = mainColor;
            Gizmos.DrawRay(pos, direction);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + markerAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - markerAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.color = markerColor;
            Gizmos.DrawRay(pos + direction, right * markerLength);
            Gizmos.DrawRay(pos + direction, left * markerLength);
            Gizmos.DrawLine(pos + direction + right * markerLength, pos + direction + left * markerLength);
        }
    }
}
