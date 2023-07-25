using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class TitleScreenScript : MonoBehaviour
{
    public GameObject objToShow;

    [SerializeField]
    private GameObject _blur;

    public UnityEngine.UI.Button SoundToggleButton;
    public UnityEngine.UI.Button MusicToggleButton;
    public UnityEngine.UI.Button CameraShakeToggleButton;

    public UnityEngine.UI.Button PausePanelSoundToggleButton;
    public UnityEngine.UI.Button PausePanelMusicToggleButton;
    public UnityEngine.UI.Button PausePanelCameraShakeToggleButton;

    public UnityEvent OnSoundOn;
    public UnityEvent OnSoundOff;
    public UnityEvent OnMusicOn;
    public UnityEvent OnMusicOff;
    public UnityEvent OnCameraShakeEnable;
    public UnityEvent OnCameraShakeDisable;

    private bool IsSoundOn;
    private bool IsMusicOn;
    private bool IsCameraShakeOn;

    public static TitleScreenScript instance;

    private void Awake()
    {
        Assert.IsNull(instance);
        instance = this; // singleton logic

        Assert.IsNotNull(objToShow);
        IsSoundOn = true;
        IsMusicOn = true;
        IsCameraShakeOn = true;

        Assert.IsNotNull(SoundToggleButton);
        Assert.IsNotNull(MusicToggleButton);
        Assert.IsNotNull(CameraShakeToggleButton);
        Assert.IsNotNull(PausePanelSoundToggleButton);
        Assert.IsNotNull(PausePanelMusicToggleButton);
        Assert.IsNotNull(PausePanelCameraShakeToggleButton);
    }

    public void Show(bool show)
    {
        UpdateOptionsToggleText();
        objToShow.SetActive(show);
        _blur.SetActive(show);
    }

    public void OnPlayButtonClick()
    {
        Show(false);
        GameStateManager.instance.OnPlay();
    }

    public void OnSoundToggleClick() {
        IsSoundOn = !IsSoundOn;
        if(IsSoundOn)
            OnSoundOn.Invoke();
        else 
            OnSoundOff.Invoke();
        UpdateOptionsToggleText();
    }

    public void OnMusicToggleClick() {
        IsMusicOn = !IsMusicOn;
        if (IsMusicOn)
            OnMusicOn.Invoke();
        else
            OnMusicOff.Invoke();
        UpdateOptionsToggleText();
    }

    public void OnCameraShakeToggleClick() {
        IsCameraShakeOn = !IsCameraShakeOn;
        if (IsCameraShakeOn)
            OnCameraShakeEnable.Invoke();
        else
            OnCameraShakeDisable.Invoke();
        UpdateOptionsToggleText();
    }

    public void UpdateOptionsToggleText() {
        SoundToggleButton.GetComponentInChildren<TextMeshProUGUI>().text = IsSoundOn ? "Turn SFX Off" : "Turn SFX On";
        MusicToggleButton.GetComponentInChildren<TextMeshProUGUI>().text = IsMusicOn ? "Turn Turn Music Off" : "Turn Music On";
        CameraShakeToggleButton.GetComponentInChildren<TextMeshProUGUI>().text = IsCameraShakeOn ? "Turn Camera Shake Off" : "Turn Camera Shake On";
        PausePanelSoundToggleButton.GetComponentInChildren<TextMeshProUGUI>().text = IsSoundOn ? "Turn SFX Off" : "Turn SFX On";
        PausePanelMusicToggleButton.GetComponentInChildren<TextMeshProUGUI>().text = IsMusicOn ? "Turn Music Off" : "Turn Music On";
        PausePanelCameraShakeToggleButton.GetComponentInChildren<TextMeshProUGUI>().text = IsCameraShakeOn ? "Turn Camera Shake Off" : "Turn Camera Shake On";
    }

}
