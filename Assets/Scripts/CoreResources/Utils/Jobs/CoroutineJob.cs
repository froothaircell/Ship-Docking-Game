using System;
using System.Collections;
using CoreResources.Pool;
using GameResources;

namespace CoreResources.Utils.Jobs
{
    public class CoroutineJob : PoolableJob
    {
        public static CoroutineJob Get(IEnumerator coroutineEnumerator)
        {
            CoroutineJob job = AppHandler.JobPool.Get<CoroutineJob>();
            job.OnSpawnInit(coroutineEnumerator);
            return job;
        }
        
        public IEnumerator CoroutineEnumerator { get; private set; }

        protected void OnSpawnInit(IEnumerator coroutineEnumerator)
        {
            base.OnSpawnInit();
            
            CoroutineEnumerator = coroutineEnumerator;
        }
        
        protected override void OnDespawn()
        {
            base.OnDespawn();

            CoroutineEnumerator = null;
        }


        protected override void Execute()
        {
            
        }

        protected override void CancelExecute(Exception exception)
        {
            
        }
    }
}