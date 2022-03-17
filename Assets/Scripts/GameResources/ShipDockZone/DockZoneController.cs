using GameResources.Ship;
using UnityEngine;

namespace GameResources.ShipDockZone
{
    public static class ShipAndMarkerColors
    {
        public static Color RedM => new Color(0.98f, 0.17f, 0.17f, 0.73f);
        public static Color GreenM => new Color(0.15f, 1f, 0f, 0.73f);
        public static Color BlueM => new Color(0.64f, 0f, 1f, 0.73f);
    }
    
    public class DockZoneController : MonoBehaviour
    {
        public GameObject marker;
        public ShipColors assignedShipColor;
        
        private void OnEnable()
        {
            Renderer renderer = marker.GetComponent<Renderer>();
            switch (assignedShipColor)
            {
                case ShipColors.Red:
                    renderer.material.SetColor("_BaseColor", ShipAndMarkerColors.RedM);
                    break;
                case ShipColors.Green:
                    renderer.material.SetColor("_BaseColor", ShipAndMarkerColors.GreenM);
                    break;
                case ShipColors.Blue:
                    renderer.material.SetColor("_BaseColor", ShipAndMarkerColors.BlueM);
                    break;
                default:
                    renderer.material.SetColor("_BaseColor", Color.white);
                    break;
            }
        }
    }
}