using UnityEngine;

public class Factory : MonoBehaviour
{
    public GameObject CreateObject(GameObject prefab, Transform parent)
    {
        GameObject obj = Instantiate(prefab, parent);
        return obj;
    }
}
