using System;
using System.Collections.Generic;
using System.Linq;

public class InputControllerManager
{
    static InputControllerManager _instance;

    public static InputControllerManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new InputControllerManager();

            return _instance;
        }
    }

    List<InputControllerBase> _activeControllers;

    public List<WorldInputControllerBase> ActiveWorldInputControllers
    {
        get
        {
            return _activeControllers.FindAll(val => val is WorldInputControllerBase).Cast<WorldInputControllerBase>().ToList();
        }
    }

    InputControllerManager()
    {
        _activeControllers = new List<InputControllerBase>()
        {
            WorldInputController.Instance,
            UIInputController.Instance,
            WorldHighlighterInputController.Instance,
            UIHighlighterInputController.Instance,
        };
    }

    public void RegisterInputController(InputControllerBase ic)
    {
        if (_activeControllers.Contains(ic))
            return;

        _activeControllers.Add(ic);
    }

    public void UnregisterInputController(InputControllerBase ic)
    {
        _activeControllers.Remove(ic);
    }

    public void RestoreInputControllerTransmissions()
    {
        foreach (InputControllerBase ic in _activeControllers)
            ic.RestoreInputTransmission();
    }

    public void EnableInputControllerTransmissions()
    {
        foreach (InputControllerBase ic in _activeControllers)
            ic.EnableInputTransmission();
    }

    public void DisableInputControllerTransmissions()
    {
        foreach (InputControllerBase ic in _activeControllers)
            ic.DisableInputTransmission();
    }

    public void DisableInputControllerTransmissionsExcept<T>()
        where T: InputControllerBase
    {
        foreach (InputControllerBase ic in _activeControllers)
        {
            if (ic is T)
                continue;

            ic.DisableInputTransmission();
        }
    }

    public void DisableInputControllerTransmissionsExcept(params Type[] typeArr)
    {
        foreach (InputControllerBase ic in _activeControllers)
        {
            if (typeArr.Contains(ic.GetType()))
                continue;

            ic.DisableInputTransmission();
        }
    }

    public void RestoreInputControllerTransmissionsExcept(params Type[] typeArr)
    {
        foreach (InputControllerBase ic in _activeControllers)
        {
            if (typeArr.Contains(ic.GetType()))
                continue;

            ic.RestoreInputTransmission();
        }
    }

    T GetActiveInputController<T>()
        where T : InputControllerBase
    {
        return (T)_activeControllers.FirstOrDefault(val => val is T);
    }


}
