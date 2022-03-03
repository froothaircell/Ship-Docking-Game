using System;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreResources.Promise
{
    public class PromiseTest : MonoBehaviour
    {
        private int num = 1;
        private RPromise<int> testPromise;
        private Coroutine routine;

        private void Start()
        {
            testPromise = RPromise.Get<int>();
            testPromise.Then(OnResolve, OnReject);
            DoSomethingAsync();
        }

        private async void DoSomethingAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(3.0f));
            RPromise.SafeResolve<int>(ref testPromise, 4);
        }

        private void OnResolve(int num)
        {
            Debug.Log($"The resolved value is {num}");
        }

        private void OnReject(Exception e)
        {
            throw e;
        }
        
    }
}