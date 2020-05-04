using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static partial class Utilities
{
    public static bool IsTouchPlatform()
    {
        return Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android;
    }
}
