using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [SerializeField] private Factory factory;

    [System.Serializable]
    public class PoolItems
    {
        public Transform parent;
        public string poolID;
        public GameObject prefab;
        public int size;
    }

    public List<PoolItems> pools = new List<PoolItems>();
    private Dictionary<string, Queue<GameObject>> poolDict;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (PoolItems item in pools)
        {
            Queue<GameObject> objects = new Queue<GameObject>();

            for (int i = 0; i < item.size; i++)
            {
                GameObject obj = factory.CreateObject(item.prefab, item.parent);
                obj.SetActive(false);
                objects.Enqueue(obj);
            }
            poolDict.Add(item.poolID, objects);
        }
    }

    public GameObject GetObject(string poolID)
    {
        if (!poolDict.ContainsKey(poolID))
        {
            Debug.LogWarning("No pool found!");
            return null;
        }

        GameObject obj = poolDict[poolID].Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(string poolID, GameObject obj)
    {
        obj.SetActive(false);
        poolDict[poolID].Enqueue(obj);
    }
}
