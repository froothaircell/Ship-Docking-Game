using System;

namespace CoreResources.Promise
{
    public partial class Promise
    {
        public static Promise Get()
        {
            Promise promise = AppHandler.AppPool.Get<Promise>();
            return promise;
        }

        public static Promise<T> Get<T>()
        {
            Promise<T> promise = AppHandler.AppPool.Get<Promise<T>>();
            return promise;
        }

        public static Promise Resolved()
        {
            Promise promise = AppHandler.AppPool.Get<Promise>();
            promise.Resolve();
            return promise;
        }

        public static Promise Rejected(Exception exception)
        {
            Promise promise = AppHandler.AppPool.Get<Promise>();
            promise.Reject(exception);
            return promise;
        }

        public static IPromise<T> Resolved<T>(T resolveValue)
        {
            Promise<T> promise = AppHandler.AppPool.Get<Promise<T>>();
            promise.Resolve(resolveValue);
            return promise;
        }

        public static IPromise<T> Rejected<T>(Exception exception)
        {
            Promise<T> promise = AppHandler.AppPool.Get<Promise<T>>();
            promise.Reject(exception);
            return promise;
        }

        public static void SafeResolve(ref Promise promise)
        {
            InternalSafePromiseBehaviour(ref promise)?.Resolve();
        }

        public static void SafeReject(ref Promise promise, Exception exception)
        {
            InternalSafePromiseBehaviour(ref promise)?.Reject(exception);
        }
        
        public static void SafeResolve<T>(ref Promise<T> promise, T resolveValue)
        {
            InternalSafePromiseBehaviour(ref promise)?.Resolve(resolveValue);
        }
        
        public static void SafeReject<T>(ref Promise<T> promise, Exception exception)
        {
            InternalSafePromiseBehaviour(ref promise)?.Reject(exception);
        }

        private static Promise InternalSafePromiseBehaviour(ref Promise promise)
        {
            if (promise == null) return null;

            if (promise.State != PromiseState.Pending)
            {
                promise = null;
                return null;
            }

            Promise tmpPromise = promise;
            promise = null;
            return tmpPromise;
        }

        private static Promise<T> InternalSafePromiseBehaviour<T>(ref Promise<T> promise)
        {
            if (promise == null) return null;

            if (promise.State != PromiseState.Pending)
            {
                promise = null;
                return null;
            }

            Promise<T> tmpPromise = promise;
            promise = null;
            return tmpPromise;
        }
    }
}