using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameResources.Ship
{
    public class TestMovementController1 : MonoBehaviour
    {
        [Range(0f, 15f)]
        public float translationSpeed = 5f;
        [Range(0, 0.5f)]
        public float rotationSpeed = 0.2f;
        public List<Transform> _pathPoints = new List<Transform>();

        private void Update()
        {
            if (_pathPoints.Count <= 0) return;
            
            GetNextPoint();
            
            transform.position += transform.forward * translationSpeed * Time.deltaTime;

            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                Quaternion.LookRotation(_pathPoints[0].position - transform.position, Vector3.up), 
                rotationSpeed);
        }

        private void GetNextPoint()
        {
            if (Vector3.Distance(transform.position, _pathPoints[0].position) < 0.2f)
            {
                _pathPoints.RemoveAt(0);
            }
        }
    }
}