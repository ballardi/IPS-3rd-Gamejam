using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

public class TitleScreenScript : MonoBehaviour
{
    public GameObject objToShow;

    [SerializeField]
    private GameObject _blur;

    public ToggleButtonScript SoundToggleButton;
    public ToggleButtonScript MusicToggleButton;
    public ToggleButtonScript CameraShakeToggleButton;

    public ToggleButtonScript PausePanelSoundToggleButton;
    public ToggleButtonScript PausePanelMusicToggleButton;
    public ToggleButtonScript PausePanelCameraShakeToggleButton;

    public TextMeshProUGUI HighscoreText;

    public UnityEvent OnSoundOn;
    public UnityEvent OnSoundOff;
    public UnityEvent OnMusicOn;
    public UnityEvent OnMusicOff;
    public UnityEvent OnCameraShakeEnable;
    public UnityEvent OnCameraShakeDisable;

    public GameObject CreditsPanel;

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
        Assert.IsNotNull(HighscoreText);
        Assert.IsNotNull(CreditsPanel);
    }

    public void Show(bool show)
    {
        UpdateOptionsToggleText();
        HighscoreText.text = ""+GameStateManager.instance.HighScore;
        objToShow.SetActive(show);
        _blur.SetActive(show);
        CreditsPanel.SetActive(false);
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
        SoundToggleButton.SetState(IsSoundOn);
        MusicToggleButton.SetState(IsMusicOn);
        CameraShakeToggleButton.SetState(IsCameraShakeOn);
        PausePanelSoundToggleButton.SetState(IsSoundOn);
        PausePanelMusicToggleButton.SetState(IsMusicOn);
        PausePanelCameraShakeToggleButton.SetState(IsCameraShakeOn);
    }

}
