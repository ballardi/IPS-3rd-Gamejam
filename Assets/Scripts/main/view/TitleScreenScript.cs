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

    public UnityEngine.UI.Button PausePanelSoundToggleButton;
    public UnityEngine.UI.Button PausePanelMusicToggleButton;


    public UnityEvent OnSoundOn;
    public UnityEvent OnSoundOff;
    public UnityEvent OnMusicOn;
    public UnityEvent OnMusicOff;

    private bool IsSoundOn;
    private bool IsMusicOn;

    public static TitleScreenScript instance;

    private void Awake()
    {
        Assert.IsNull(instance);
        instance = this; // singleton logic

        Assert.IsNotNull(objToShow);
        IsSoundOn = true;
        IsMusicOn = true;

        Assert.IsNotNull(SoundToggleButton);
        Assert.IsNotNull(MusicToggleButton);
        Assert.IsNotNull(PausePanelSoundToggleButton);
        Assert.IsNotNull(PausePanelMusicToggleButton);
    }

    public void Show(bool show)
    {
        UpdateSoundAndMusicToggleText();
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
        UpdateSoundAndMusicToggleText();
    }

    public void OnMusicToggleClick() {
        IsMusicOn = !IsMusicOn;
        if (IsMusicOn)
            OnMusicOn.Invoke();
        else
            OnMusicOff.Invoke();
        UpdateSoundAndMusicToggleText();
    }

    public void UpdateSoundAndMusicToggleText() {
        SoundToggleButton.GetComponentInChildren<TextMeshProUGUI>().text = IsSoundOn ? "SFX Off" : "SFX On";
        MusicToggleButton.GetComponentInChildren<TextMeshProUGUI>().text = IsMusicOn ? "Music Off" : "Music On";
        PausePanelSoundToggleButton.GetComponentInChildren<TextMeshProUGUI>().text = IsSoundOn ? "SFX Off" : "SFX On";
        PausePanelMusicToggleButton.GetComponentInChildren<TextMeshProUGUI>().text = IsMusicOn ? "Music Off" : "Music On";
    }

}
