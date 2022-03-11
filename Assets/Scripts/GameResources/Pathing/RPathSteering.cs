using GameResources.Ship;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameResources.Pathing
{
    public class RPathSteering : MonoBehaviour, IShipComponent
    {
        public float _rotationSpeed;
        public float _dotProductThreshold;
        private Vector3 _currentDestination;
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
        }

        public void OnReset()
        {
            GetComponent<RPathManager>().OnNewDestinationSet -= OnNewCurrentDestination;
        }

        public void OnUpdate()
        {
            if (_onCourse) return;

            Vector3 targetDirection = (_currentDestination - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation((targetDirection), Vector3.up);
            var dotProduct = Vector3.Dot(transform.forward, targetDirection);
            if(dotProduct < 1 - _dotProductThreshold)
            {
                // point ahead of ship. correct course
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                    (_rotationSpeed * Time.deltaTime));
            }
            else
            {
                // rotation is close enough to desired threshold. clamp it to the target
                transform.rotation = targetRotation;
                _onCourse = true;
            }
        }

        public void OnNewCurrentDestination(Vector3 newDestination)
        {
            _currentDestination = newDestination;
            _onCourse = false;
        }
    }
}