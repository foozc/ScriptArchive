using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.Scripts.Controller;

public class DontDestroyOnLoadGame : MonoBehaviour
{
    public UIRoot uiRoot;
    public Camera camera;
    public GameObject FingerGestures;
    void Start()
    {
        DontDestroyOnLoad(uiRoot.gameObject);
        DontDestroyOnLoad(camera.gameObject);
        DontDestroyOnLoad(FingerGestures);
        SceneManager.LoadScene("MainUI");
        UIManager.getInstance().mainMenuUI.gameObject.SetActive(true);
        UIManager.getInstance().setActiveUI(UIType.MainMenu);
    }
}
