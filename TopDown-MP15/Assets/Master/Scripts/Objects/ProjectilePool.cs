using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool instance;

    private Dictionary<string, List<GameObject>> pooledObjects = new Dictionary<string, List<GameObject>>();
    private int amountToPool = 30;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void InitializePool(GameObject prefab, string type)
    {
        if (!pooledObjects.ContainsKey(type))
        {
            pooledObjects[type] = new List<GameObject>();
            for (int i = 0; i < amountToPool; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pooledObjects[type].Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string type)
    {
        if (!pooledObjects.ContainsKey(type)) return null;

        for (int i = 0; i < pooledObjects[type].Count; i++)
        {
            if (!pooledObjects[type][i].activeInHierarchy)
            {
                return pooledObjects[type][i];
            }
        }
        return null;
    }
}
