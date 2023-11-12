using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReloadButton : MonoBehaviour
{
    private Button reloadButton;

    void Start()
    {
        reloadButton = GetComponent<Button>();
        reloadButton.onClick.AddListener(ReloadScene);
    }

    private void ReloadScene()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
