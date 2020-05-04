using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();

            return _instance;
        }
    }

    public SceneManager SceneManager { get; private set; }
    public PhaseFlowController PhaseFlowController { get; private set; }

    private void Awake()
    {
        Application.targetFrameRate = 60;

        SceneManager = new SceneManager(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void StartFlowController(PhaseFlowController controller)
    {
        PhaseFlowController = controller;

        PhaseFlowController.StartPhases();
    }

    private void OnDestroy()
    {
        SceneManager.Dispose();
    }
}
