using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuManager : MonoBehaviour
{
    #region Events
    public Action<UIMenu> OnUIMenuInitCompleted;
    private void FireOnUIMenuInitCompleted(UIMenu uiMenu)
    {
        if (OnUIMenuInitCompleted != null)
            OnUIMenuInitCompleted(uiMenu);
    }
    #endregion

    static UIMenuManager _instance;
    public static UIMenuManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<UIMenuManager>();

            return _instance;
        }
    }
    public bool ToggleDebug;

    public List<UIMenu> UIMenuList { get; private set; }
    public List<UIMenu> ActiveUIMenuColl { get; private set; }

    private List<UIMenu> _closeMenuColl = new List<UIMenu>();
    private KeyValuePair<UIMenu, ParameterEncapsulation> _openMenu;
    private bool _isDeactivationFinished;

    private void Awake()
    {
        ActiveUIMenuColl = new List<UIMenu>();

        _isDeactivationFinished = true;

        UIMenu.OnPostDeactivation += UIMenuClosed;
        UIMenu.OnUIMenuInitCompleted += OnNewUIInitCompleted;
        UIMenu.OnUIMenuDestroyed += OnUIMenuDestroyed;

        UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDestroy()
    {
        UIMenu.OnPostDeactivation -= UIMenuClosed;
        UIMenu.OnUIMenuInitCompleted -= OnNewUIInitCompleted;
        UIMenu.OnUIMenuDestroyed -= OnUIMenuDestroyed;

        UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnSceneUnloaded;

        _instance = null;
    }

    public void OnBackPressed()
    {
        if (ActiveUIMenuColl.Count > 0)
            ActiveUIMenuColl[ActiveUIMenuColl.Count - 1].OnBackPressed(null);
    }

    private void OnSceneUnloaded(UnityEngine.SceneManagement.Scene unloadedScene)
    {
        if (ToggleDebug)
            Debug.Log("Scene Unloaded, Removing All Registered Windows!");

        ActiveUIMenuColl.Clear();

        _isDeactivationFinished = true;
    }

    public void OpenUIMenu(UIMenu menuRequestedToActivate, ParameterEncapsulation param = null)
    {
        if (ActiveUIMenuColl.Contains(menuRequestedToActivate))
            return;

        if (ActiveUIMenuColl.Count > 0)
        {
            if (menuRequestedToActivate.DisableMenusUnderneath)
            {
                foreach (var underneathMenu in ActiveUIMenuColl)
                {
                    if (underneathMenu != menuRequestedToActivate)
                        _closeMenuColl.Add(underneathMenu);

                    if (underneathMenu.DisableMenusUnderneath)
                        break;
                }
            }

            var previousCanvas = ActiveUIMenuColl[ActiveUIMenuColl.Count - 1].Canvas;

            menuRequestedToActivate.transform.SetAsLastSibling();
        }

        _openMenu = new KeyValuePair<UIMenu, ParameterEncapsulation>(menuRequestedToActivate, param);

        if (!_isDeactivationFinished)
            return;

        StartCloseMenu(null, null);
    }

    private void UIMenuClosed(UIMenu closedMenu)
    {
        StartCloseMenu(closedMenu, null);
    }

    private void StartCloseMenu(UIMenu closedMenu, Action callback)
    {
        if (_closeMenuColl.Count == 0)
        {
            if (callback != null)
                callback();

            if (_openMenu.Key == null)
            {
                foreach (var menu in ActiveUIMenuColl)
                {
                    if (!menu.IsPreActivationFinished)
                        menu.Activate(null);

                    if (menu.DisableMenusUnderneath)
                        break;
                }
            }
            else
            {
                if (!_openMenu.Key.IsPreActivationFinished)
                {
                    ActiveUIMenuColl.Add(_openMenu.Key);

                    _openMenu.Key.Activate(_openMenu.Value);
                }

                _openMenu = new KeyValuePair<UIMenu, ParameterEncapsulation>(null, null);
            }

            _isDeactivationFinished = true;

            return;
        }

        _isDeactivationFinished = false;

        var instance = _closeMenuColl[0];
        _closeMenuColl.RemoveAt(0);
        if (!instance.IsPreDeactivationFinished)
            instance.Deactivate();
    }

    public void CloseUIMenu(UIMenu menuRequestedToDeactivate)
    {
        if (ToggleDebug)
            Debug.Log("Trying to Close Window: " + menuRequestedToDeactivate.GetType());

        if (ActiveUIMenuColl.Count == 0)
        {
            Debug.LogWarningFormat(menuRequestedToDeactivate, "{0} cannot be closed because menu list is empty", menuRequestedToDeactivate.GetType());
            return;
        }

        //if (ActiveUIMenuColl[ActiveUIMenuColl.Count - 1] != menuRequestedToDeactivate)
        //{
        //    Debug.LogWarningFormat(menuRequestedToDeactivate, "{0} cannot be closed because it is not on top of list, it is {1}", menuRequestedToDeactivate.GetType(), ActiveUIMenuColl[ActiveUIMenuColl.Count - 1]);
        //    return;
        //}

        CloseMenu(menuRequestedToDeactivate);
    }

    public void CloseMenu(UIMenu instance, Action callback = null)
    {
        ActiveUIMenuColl.Remove(instance);

        if (ToggleDebug)
            Debug.Log("Close Top Menu called, closing window: " + instance.GetType());

        if (_closeMenuColl.Contains(instance))
        {
            Debug.LogWarningFormat("Close Menu list already contains window: " + instance.GetType());
            return;
        }

        _closeMenuColl.Add(instance);

        if (_closeMenuColl.Count >= 2)
            return;

        StartCloseMenu(null, callback);
    }

    public void CloseTopMenu(Action callback = null)
    {
        var instance = ActiveUIMenuColl[ActiveUIMenuColl.Count - 1];

        ActiveUIMenuColl.RemoveAt(ActiveUIMenuColl.Count - 1);

        if (ToggleDebug)
            Debug.Log("Close Top Menu called, closing window: " + instance.GetType());

        if (_closeMenuColl.Contains(instance))
        {
            Debug.LogWarningFormat("Close Menu list already contains window: " + instance.GetType());
            return;
        }

        _closeMenuColl.Add(instance);

        if (_closeMenuColl.Count >= 2)
            return;

        StartCloseMenu(null, callback);
    }

    public void CloseAllUIMenus(Action callback)
    {
        if (ActiveUIMenuColl.Count == 0)
        {
            if (callback != null)
                callback();
        }

        for (int i = ActiveUIMenuColl.Count - 1; i >= 0; i--)
            CloseTopMenu(callback);
    }

    public void CloseAllUIMenusExcept(Type menuType, Action callback)
    {
        if (ActiveUIMenuColl.Count == 0)
        {
            if (callback != null)
                callback();
        }

        for (int i = ActiveUIMenuColl.Count - 1; i >= 0; i--)
        {
            if (ActiveUIMenuColl[ActiveUIMenuColl.Count - 1].GetType().Equals(menuType))
                break;

            CloseTopMenu(callback);
        }
    }

    public T GetOpenMenu<T>()
        where T : UIMenu
    {
        return (T)ActiveUIMenuColl.FirstOrDefault(val => val is T);
    }

    public T GetUIMenu<T>()
        where T : UIMenu
    {
        T target = (T)UIMenuList.FirstOrDefault(val => val is T);

        if (target == null)
            target = FindObjectOfType<T>();

        return target;
    }

    public bool IsUIActive<T>()
        where T : UIMenu
    {
        return GetOpenMenu<T>() != null;
    }

    public bool IsAnyUIActive()
    {
        return ActiveUIMenuColl != null && ActiveUIMenuColl.Count > 0;
    }

    public bool IsAnyUIActive(params Type[] menuTypeCollection)
    {
        foreach (Type type in menuTypeCollection)
        {
            UIMenu targetUI = ActiveUIMenuColl.FirstOrDefault(val => val.GetType().Equals(type));

            if (targetUI != null)
                return true;
        }

        return false;
    }

    public bool IsAnyUIActive(out List<UIMenu> activeUIMenuCollection, params Type[] menuTypeCollection)
    {
        activeUIMenuCollection = new List<UIMenu>();

        foreach (Type type in menuTypeCollection)
        {
            UIMenu targetUI = ActiveUIMenuColl.FirstOrDefault(val => val.GetType().Equals(type));

            if (targetUI != null)
                activeUIMenuCollection.Add(targetUI);
        }

        return activeUIMenuCollection.Count > 0;
    }

    private void OnNewUIInitCompleted(UIMenu uiMenu)
    {
        if (UIMenuList == null)
            UIMenuList = new List<UIMenu>();

        UIMenuList.Add(uiMenu);

        FireOnUIMenuInitCompleted(uiMenu);
    }

    private void OnUIMenuDestroyed(UIMenu uiMenu)
    {
        if (UIMenuList == null)
            return;

        UIMenuList.Remove(uiMenu);
    }
}
