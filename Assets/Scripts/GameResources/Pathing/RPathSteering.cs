using GameResources.Ship;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameResources.Pathing
{
    public class RPathSteering : MonoBehaviour, IShipComponent
    {
        public float _rotationSpeed;
        public float _dotProductThreshold;
        private Vector3? _currentDestination;
        private bool _onCourse = false;

        public bool IsTurning
        {
            get
            {
                return !_onCourse;
            }
        }
        
        public void OnInit()
        {
            _onCourse = true;
            GetComponent<RPathManager>().OnNewDestinationSet += OnNewCurrentDestination;
            GetComponent<RPathManager>().OnNoDestinationSet += OnClearDestination;
        }

        public void OnReset()
        {
            GetComponent<RPathManager>().OnNewDestinationSet -= OnNewCurrentDestination;
            GetComponent<RPathManager>().OnNoDestinationSet -= OnClearDestination;
        }

        public void OnUpdate()
        {
            if (_onCourse) return;

            Vector3 targetDirection = (_currentDestination.GetValueOrDefault() - transform.position).normalized;
            Vector3 targetRotation = Quaternion.LookRotation((targetDirection), Vector3.up).eulerAngles;
            targetRotation = new Vector3(0, targetRotation.y, 0);
            var dotProduct = Vector3.Dot(transform.forward, targetDirection);
            if(dotProduct < 1 - _dotProductThreshold)
            {
                // point ahead of ship. correct course
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation),
                    (_rotationSpeed * Time.deltaTime));
            }
            else
            {
                // rotation is close enough to desired threshold. clamp it to the target
                transform.rotation = Quaternion.Euler(targetRotation);
                _onCourse = true;
            }
        }

        public void OnNewCurrentDestination(Vector3 newDestination)
        {
            _currentDestination = newDestination;
            _onCourse = false;
        }

        public void OnClearDestination()
        {
            _currentDestination = null;
            _onCourse = true;
        }
    }
}