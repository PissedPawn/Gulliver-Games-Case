﻿using UnityEngine;

namespace Challenges._4._Gizmos.Scripts
{
    [CreateAssetMenu(fileName = "ClusterBombData", menuName = "GizmoChallenge/ClusterBombData")]
    public class ClusterBombData : ScriptableObject
    {
        [Header("Self")]
        [SerializeField]
        private int selfDamage;
        [SerializeField]
        private float selfExplosionRadius;
        [Header("Spawned Bombs")]
        [SerializeField]
        private int childCount;
        [SerializeField]
        private float childInitialJumpHeight;
        [SerializeField]
        private float childTravelDistance;
        [SerializeField]
        private int childBounceCount; // should be int thats why Im changing it
        [SerializeField]
        private float childBounceFalloff; // should be float


        [SerializeField]
        private int childDamage;
        [SerializeField]
        private float childExplosionRadius;

        public float ChildExplosionRadius => childExplosionRadius;
        public float ChildBounceFalloff => childBounceFalloff;
     
        public int ChildDamage => childDamage;
    

        public int ChildBounceCount => childBounceCount;

        public float ChildTravelDistance => childTravelDistance;

        public float ChildInitialJumpHeight => childInitialJumpHeight;

        public int ChildCount => childCount;

        public float SelfExplosionRadius => selfExplosionRadius;

        public int SelfDamage => selfDamage;
    }
}
