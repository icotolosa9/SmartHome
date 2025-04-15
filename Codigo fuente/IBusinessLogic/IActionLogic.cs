namespace IBusinessLogic
{
    public interface IActionLogic
    {
        void DetectPersonCamera(Guid hardwareId, string email);
        void OpenOrCloseWindow(Guid hardwareId, bool action);
        void TurnSmartLampOnOrOff(Guid hardwareId, bool isOn);
        void DetectMotionCamera(Guid hardwareId);
        void DetectMotionSensor(Guid hardwareId);
    }
}
