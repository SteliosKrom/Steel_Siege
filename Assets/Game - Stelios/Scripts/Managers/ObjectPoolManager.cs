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
        public string tag;
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
            poolDict.Add(item.tag, objects);
        }
    }

    public GameObject GetObject(string tag)
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.LogWarning("No pool found!");
            return null;
        }

        if (poolDict[tag].Count == 0)
        {
            GameObject newObj = AutoExpandPool(tag);
            return newObj;
        }

        GameObject obj = poolDict[tag].Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(string tag, GameObject obj)
    {
        obj.SetActive(false);
        poolDict[tag].Enqueue(obj);
    }

    public GameObject AutoExpandPool(string tag)
    {
        PoolItems selectedPool = pools.Find(p => p.tag == tag);
        GameObject newObj = factory.CreateObject(selectedPool.prefab, selectedPool.parent);
        return newObj;
    }
}
