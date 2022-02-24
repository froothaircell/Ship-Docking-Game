using CoreResources.Utils.Singletons;
using GameResources.GameManager.States;
using UnityEngine;

namespace CoreResources.Utils.ResourceLoader
{
    public class AssetLoader : GenericSingleton<AssetLoader>
    {
        public bool HasAsset(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogWarning("AssetLoader | HasAsset called on null or empty string");
                return false;
            }

            return ResourceDB.Instance.HasAsset(name);
        }

        public T LoadAsset<T>(string name) where T : Object
        {
            ResourceItem resourceItem = ResourceDB.Instance.GetResourceItem(name);

            if (resourceItem == null)
            {
                Debug.LogWarning($"LoadAsset | Asset ({name}) not found in local DB");
                return null;
            }
            
            return resourceItem.Load<T>();
        }
    }
}