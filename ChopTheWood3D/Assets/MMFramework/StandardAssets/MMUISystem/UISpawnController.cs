using System.Collections.Generic;
using UnityEngine;

public class UISpawnController : MonoBehaviour
{
    public List<GameObject> PlaceholderList;
    public int SpawnAmount;

    private void Awake()
    {
        PlaceholderList.ForEach(ph => ph.SetActive(false));
    }

    public void ChangePlaceholder(List<GameObject> newPlaceholderList)
    {
        PlaceholderList = new List<GameObject>(newPlaceholderList);

        PlaceholderList.ForEach(ph => ph.SetActive(false));
    }

    public List<T> LoadSpawnables<T>(int amount = -1, bool activateAll = false)
    {
        int spawnAmount = SpawnAmount;
        if (amount > 0)
            spawnAmount = amount;

        List<T> resultList = new List<T>();

        foreach(var placeholder in PlaceholderList)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                GameObject container = Instantiate(placeholder, placeholder.transform.position, Quaternion.identity, placeholder.transform.parent);
                container.transform.localPosition = placeholder.transform.localPosition;
                container.transform.localEulerAngles = placeholder.transform.localEulerAngles;

                resultList.Add(container.GetComponent<T>());

                container.name = container.name.Replace("Placeholder", "");

                container.SetActive(activateAll);
            }
        }

        return resultList;
    }
}
