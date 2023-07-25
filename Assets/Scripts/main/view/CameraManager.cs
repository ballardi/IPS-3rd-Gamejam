using UnityEngine;
using Cinemachine;
using UnityEngine.Assertions;

public class CameraManager : MonoBehaviour
{

    public CinemachineVirtualCamera MainVirtualCamera;
    public CinemachineVirtualCamera ShakeyVirtualCamera;
    public CinemachineVirtualCamera AccessibilityVirtualCamera;

    public float ShakeyDuration;

    private void Awake()
    {
        Assert.IsNotNull(MainVirtualCamera);
        Assert.IsNotNull(ShakeyVirtualCamera);
        Assert.IsNotNull(AccessibilityVirtualCamera);
        Assert.IsTrue(ShakeyDuration >= 0);

    }

    void Start()
    {
        PlayerScript.instance.OnObstacleCollision.AddListener(ObstacleCollisionCallback);
        PlayerScript.instance.OnObstacleResolution.AddListener(ObstacleResolutionCallback);
        PlayerScript.instance.OnPowerupResolution.AddListener(PowerupResolutionCallback);
        TitleScreenScript.instance.OnCameraShakeEnable.AddListener(DisableAccessibilityCamera);
        TitleScreenScript.instance.OnCameraShakeDisable.AddListener(OverrideAllAndEnableAccessibilityCamera);

        MainVirtualCamera.Follow = PlayerScript.instance.transform;
        ShakeyVirtualCamera.Follow = PlayerScript.instance.transform;
        AccessibilityVirtualCamera.Follow = PlayerScript.instance.transform;
    }

    public void ObstacleCollisionCallback()
    {
        ShakeyVirtualCamera.Priority = MainVirtualCamera.Priority + 1;
        Invoke(nameof(SwitchBackToMainCamera), ShakeyDuration);
    }

    public void ObstacleResolutionCallback()
    {
        ShakeyVirtualCamera.Priority = MainVirtualCamera.Priority + 1;
        Invoke(nameof(SwitchBackToMainCamera), ShakeyDuration);
    }

    public void PowerupResolutionCallback()
    {
        ShakeyVirtualCamera.Priority = MainVirtualCamera.Priority + 1;
        Invoke(nameof(SwitchBackToMainCamera), ShakeyDuration);
    }

    public void SwitchBackToMainCamera()
    {
        ShakeyVirtualCamera.Priority = MainVirtualCamera.Priority - 1;

    }

    public void OverrideAllAndEnableAccessibilityCamera() => AccessibilityVirtualCamera.Priority = 999;
    public void DisableAccessibilityCamera() => AccessibilityVirtualCamera.Priority = 1;
}
