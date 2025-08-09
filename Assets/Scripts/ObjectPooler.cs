using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [Header("Pool Settings")]
    public GameObject prefab;
    public int poolSize = 20;
    public bool expandable = true;

    private List<GameObject> pool;

    void Awake()
    {
        pool = new List<GameObject>();

        if (prefab == null)
        {
            Debug.LogError("ObjectPooler: Prefab atanmadı!");
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        if (expandable && prefab != null)
        {
            GameObject newObj = Instantiate(prefab);
            newObj.SetActive(false);
            pool.Add(newObj);
            return newObj;
        }

        return null;
    }

    public void ReturnToPool(GameObject obj)
    {
        if (obj != null)
            obj.SetActive(false);
    }

    public int GetActiveCount()
    {
        int count = 0;
        foreach (GameObject obj in pool)
        {
            if (obj != null && obj.activeInHierarchy)
                count++;
        }
        return count;
    }

    public int GetInactiveCount()
    {
        int count = 0;
        foreach (GameObject obj in pool)
        {
            if (obj != null && !obj.activeInHierarchy)
                count++;
        }
        return count;
    }
}
