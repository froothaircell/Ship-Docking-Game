using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace GameResources.Ship
{
    public class TestMovementController : MonoBehaviour
    {
        public Transform target;
        public float stoppingDistance = 5f;
        public float speed = 5f;
        public float rotationSpeed = 100f;

        [Range(0.01f, 1f)]
        public float _lookAhead;
        private Vector3 _previousPosition;
        private Vector3 _currentVelocity = Vector3.zero;
        
        public List<Transform> pathPoints = new List<Transform>();

        [ContextMenu("DoPath")]
        private void DoPath()
        {
            var pathPointPositions = new List<Vector3>();
            pathPointPositions.Add(transform.position);
            pathPointPositions.AddRange(pathPoints.Select(item => item.position));
            var pathTween = transform.DOPath(pathPointPositions.ToArray(), 5f, PathType.CatmullRom);
            pathTween.plugOptions.orientType = OrientType.ToPath;
            pathTween.plugOptions.lookAhead = _lookAhead;
        }

        private void Start()
        {
            _previousPosition = transform.position;
        }

        private void Update()
        {
            /*
            CalculateVelocity();
            if(_currentVelocity != Vector3.zero)
                transform.LookAt(-1 * _currentVelocity);
            if (Vector3.Distance(target.position, transform.position) < stoppingDistance) return;

            transform.Translate(transform.forward * Time.deltaTime * speed);
            
            Vector3 targetDirection = (target.position - transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation , rotationSpeed * Time.deltaTime);
            */

        }

        private void CalculateVelocity()
        {
            _currentVelocity = (_previousPosition - transform.position) / Time.deltaTime;
            _previousPosition = transform.position;
        }
    }
}