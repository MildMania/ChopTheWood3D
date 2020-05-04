using UnityEngine;

public class ThrowerController : MonoBehaviour
{

    [SerializeField] private Thrower[] _throwers;
    public Thrower[] Throwers
    {
        get
        {
            return _throwers;
        }
    }

    [SerializeField] private bool _showSimulation;
    public bool ShowSimulation
    {
        get
        {
            return _showSimulation;
        }
    }

    [SerializeField] private float _simulateTime;
    public float SimulateTime
    {
        get
        {
            return _simulateTime;
        }
    }

    [SerializeField] private float _simulateDuration;
    public float SimulateDuration
    {
        get
        {
            return _simulateDuration;
        }
    }

    [SerializeField] [Range(10, 50)] private int _resolution;
    public int Resolution
    {
        get
        {
            return _resolution;
        }
    }


    public void ActivateThrowers()
    {
        foreach (Thrower t in _throwers)
            t.Throw();
    }
}
