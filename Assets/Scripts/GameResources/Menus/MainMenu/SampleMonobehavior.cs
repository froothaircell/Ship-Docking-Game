using UnityEngine;
using UnityEngine.UI;

namespace GameResources.Menus.MainMenu
{
    public class SampleMonobehavior : MonoBehaviour
    {
        public Button sampleButton;

        public void Start()
        {
            sampleButton.onClick.AddListener(() => { Debug.Log("Button callback works"); });
        }
        
        public void Awake()
        {
            sampleButton.onClick.AddListener(() => { Debug.Log("Button callback works"); });
        }
        
        public void OnEnable()
        {
            sampleButton.onClick.AddListener(() => { Debug.Log("Button callback works"); });
        }
    }
}