using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour
{
    private static Preloader _instance;
    public static Preloader Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<Preloader>();

            return _instance;
        }
    }

    public void LoadManagerScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ManagerScene", LoadSceneMode.Single);
    }
}
