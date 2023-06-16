using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed = 1f;
        [SerializeField] Transform target = null;
        [SerializeField] float targetHeightDivider = 1.5f;


        void Update()
        {
            if(target == null) { return; }
            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if(targetCapsule != null) { return target.position + Vector3.up; }
            return target.position + Vector3.up * targetCapsule.height / targetHeightDivider;
        }
    }
}
