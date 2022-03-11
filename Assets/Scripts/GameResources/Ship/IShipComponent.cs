namespace GameResources.Ship
{
    public interface IShipComponent
    {
        void OnInit();
        void OnReset();
        void OnUpdate();
    }
}