using UnityEngine;
using UnityEngine.Assertions;

public class TitleScreenScript : MonoBehaviour
{
    public GameObject objToShow;

    [SerializeField]
    private GameObject _blur;

    public static TitleScreenScript instance;

    private void Awake()
    {
        Assert.IsNull(instance);
        instance = this; // singleton logic

        Assert.IsNotNull(objToShow);
    }

    public void Show(bool show)
    {
        objToShow.SetActive(show);
        _blur.SetActive(show);
    }

    public void OnPlayButtonClick()
    {
        Show(false);
        GameStateManager.instance.OnPlay();
    }
}
