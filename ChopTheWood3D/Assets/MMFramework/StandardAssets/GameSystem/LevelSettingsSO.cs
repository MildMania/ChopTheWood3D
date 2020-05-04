using System.Linq;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LevelSettings
{
}

[CreateAssetMenu(fileName = "LevelSettingsSO", menuName = "Level/Create LevelSettingsSO", order = 1)]
public class LevelSettingsSO : ScriptableObject
{
    public LevelSettings[] LevelSettings;
}
