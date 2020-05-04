using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettingsHolder : MonoBehaviour
{
    private static LevelSettingsHolder _instance;
    public static LevelSettingsHolder Instance 
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<LevelSettingsHolder>();

            return _instance;
        }
    }

    #pragma warning disable 0649
    
    [SerializeField]
    private LevelSettingsSO _levelSettingsSO;
    public LevelSettingsSO LevelSettingsSO 
    {
        get
        {
            return _levelSettingsSO;
        }
    }

    #pragma warning disable 0649

}
