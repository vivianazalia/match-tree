using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    //singleton untuk class ObjectPooler
    public static ObjectPooler instance;

    private void Awake()
    {
        instance = this;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        AddIntoPool();
    }

    //menambahkan prefab object ke dalam class Pool
    void AddIntoPool()
    {
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            int i = 0;

            while(i < pool.size)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                i++;
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    //mengambil object dari class Pool
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Object with tag : " + tag + "doesn't exist");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);                 //agar jumlah queue (pool.size) setiap prefab tetap 30
        //Debug.Log("tag : " + tag + " Queue : " + poolDictionary[tag].Count);

        return objectToSpawn;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
