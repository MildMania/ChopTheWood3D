using UnityEngine;

public class Input_WI_OnFingerUpNotOnStartPos : Input_WI_OnFingerUp
{
    public Input_WI_OnFingerUpNotOnStartPos(int fingerIndex, Vector2 pos) 
        : base(fingerIndex, pos)
    {
    }
}
