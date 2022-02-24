using System.Threading.Tasks;
using GameResources;
using UnityEngine;

namespace CoreResources.Utils.ResourceLoader
{
    public class AssetLoaderTest : MonoBehaviour
    {
        public async void AddBoxAfterDelay()
        {
            Debug.Log("Started async task");
            await Task.Delay(3000);
            GameObject sample = AppHandler.AssetHandler.LoadAsset<GameObject>("Three Capsules");
            sample.transform.position = new Vector3(0, 0, 0);
            Instantiate(sample);
            Debug.Log("Ended async task");
        }

        private void Start()
        {
            AddBoxAfterDelay();
        }
    }
}