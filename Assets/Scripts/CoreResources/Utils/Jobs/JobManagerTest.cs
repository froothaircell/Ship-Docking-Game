using System.Collections;
using GameResources;
using UnityEngine;

namespace CoreResources.Utils.Jobs
{
    public class JobManagerTest : MonoBehaviour
    {
        public UpdateJob C1;
        public UpdateJob C2;
        public UpdateJob C3;
        
        private void Start()
        {
            C1 = AppHandler.JobHandler.ExecuteCoroutine(coroutine1());
            C2 = AppHandler.JobHandler.ExecuteCoroutine(coroutine2());
            C3 = AppHandler.JobHandler.ExecuteCoroutine(coroutine3());
        }

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                JobManager.SafeStopUpdate(ref C3);
            }
        }

        public IEnumerator coroutine1()
        {
            while(true)
            {
                Debug.Log($"{nameof(coroutine1)} is running");
                yield return new WaitForSeconds(1);
            }
        }

        public IEnumerator coroutine2()
        {
            while (true)
            {
                Debug.Log($"{nameof(coroutine2)} is running");
                yield return new WaitForSeconds(2);
            } 
        }

        public IEnumerator coroutine3()
        {
            while (true)
            {
                Debug.Log($"{nameof(coroutine3)} is running");
                yield return new WaitForSeconds(1);
            }
        }
    }
}