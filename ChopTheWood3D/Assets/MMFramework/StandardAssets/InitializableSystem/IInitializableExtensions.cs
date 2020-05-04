using System;

public static class IInitializableExtensions
{
    /// <summary>
    /// Callback is not called if Initilizable is inited already
    /// </summary>
    /// <param name="initializable"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static bool IsInited(this IInitializable initializable, Action callback)
    {
        if (initializable.Initializable.IsInited)
            return true;

        if (callback != null)
            WaitForInit(initializable, callback);

        return false;
    }

    /// <summary>
    /// Callback is called immediately if Initializable is inited already
    /// </summary>
    /// <param name="initializable"></param>
    /// <param name="callback"></param>
    public static void WaitForInit(this IInitializable initializable, Action callback)
    {
        if(initializable.Initializable.IsInited)
        {
            if(callback != null)
                callback();

            return;
        }

        initializable.Initializable.WaitForInit(callback);
    }
}
