using UnityEngine;
using CoreResources.Pool;

public class SampleListItem : Poolable
{
    protected override void OnSpawn()
    {
        Debug.Log($"This item was spawned");
    }

    protected override void OnDespawn()
    {
        Debug.Log($"This item was despawned");
    }
}

public class PoolTest : MonoBehaviour
{
    private TypePool samplePool = new TypePool("sample");
    
    
    // Start is called before the first frame update
    void Start()
    {
        samplePool.UpdatePoolName("SampleList");
        SampleListItem singleItem = samplePool.Get<SampleListItem>();
        samplePool.Return(singleItem);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
