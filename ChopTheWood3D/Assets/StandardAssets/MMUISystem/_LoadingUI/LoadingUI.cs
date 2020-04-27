using TMPro;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    public TextMeshProUGUI ErrorMessage, LoadingMessage;

    public bool IsActive { get; private set; }
    public bool IsShowingError { get; private set; }

    public void InitAndActivate()
    {
        IsActive = true;
        IsShowingError = false;

        gameObject.SetActive(true);

        ErrorMessage.text = "";
        ErrorMessage.gameObject.SetActive(false);

        LoadingMessage.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        IsActive = false;
        IsShowingError = false;

        gameObject.SetActive(false);
    }

    public void ShowError(string errorMessage)
    {
        IsShowingError = true;

        ErrorMessage.text = errorMessage;

        ErrorMessage.gameObject.SetActive(true);

        LoadingMessage.gameObject.SetActive(false);
    }
}
