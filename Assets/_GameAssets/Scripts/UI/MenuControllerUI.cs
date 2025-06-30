using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControllerUI : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(Consts.SceneNames.GAME_SCENE);
        });
        
        _exitButton.onClick.AddListener(() =>
        {
            Debug.Log("Exit");
            Application.Quit();
        });
    }
}
