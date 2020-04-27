using System.Collections.Generic;
using UnityEngine;

public class FloatingTextArgs
{
    public Vector3 SpawnPos { get; private set; }
    public string Text { get; private set; }
    public bool IsRichText { get; private set; }

    public FloatingTextArgs(Vector3 spawnPos, string text, bool isRichText)
    {
        SpawnPos = spawnPos;
        Text = text;
        IsRichText = isRichText;
    }

    public void UpdateSpawnPos(Vector3 newPos)
    {
        SpawnPos = newPos;
    }
}

public class FloatingTextController : MonoBehaviour
{
    public UISpawnController SpawnController;

    public List<FloatingTextScript> DeactiveFloatingTextList { get; private set; }
    public List<FloatingTextScript> ActiveFloatingTextList { get; private set; }

    private void Awake()
    {
        ActiveFloatingTextList = new List<FloatingTextScript>();
        DeactiveFloatingTextList = SpawnController.LoadSpawnables<FloatingTextScript>();

        DeactiveFloatingTextList.ForEach(p => p.DeactivateText());
    }

    public void ActivateFloatingText(FloatingTextArgs args)
    {
        if (DeactiveFloatingTextList.Count == 0)
            return;
     
        FloatingTextScript newFloatingText = DeactiveFloatingTextList[0];

        newFloatingText.ActivateText(args, OnFloatingFinished);

        AddToActiveList(newFloatingText);
    }

    private void OnFloatingFinished(FloatingTextScript activeFloatingText)
    {
        activeFloatingText.DeactivateText();

        AddToDeactiveList(activeFloatingText);
    }

    private void AddToActiveList(FloatingTextScript newFloatingText)
    {
        DeactiveFloatingTextList.Remove(newFloatingText);
        ActiveFloatingTextList.Add(newFloatingText);
    }

    private void AddToDeactiveList(FloatingTextScript newFloatingText)
    {
        ActiveFloatingTextList.Remove(newFloatingText);
        DeactiveFloatingTextList.Add(newFloatingText);
    }
}
