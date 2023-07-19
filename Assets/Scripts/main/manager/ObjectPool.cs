using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectPool : LoggableMonoBehaviour
{
    public GameObject prefabToPool;
    public int poolSize;

    private List<GameObject> InUseObjects = new List<GameObject>();
    private List<GameObject> AvailableObjects = new List<GameObject>();

    private void Awake() {
        Assert.IsNotNull(prefabToPool);
        Assert.IsTrue(poolSize > 0);

        // populate pool
        for(int i = 0; i < poolSize; i++) {
            GameObject obj = GameObject.Instantiate(prefabToPool, transform);
            obj.SetActive(false);
            AvailableObjects.Add(obj);
        }

        HideAll();
    }

    public void OnShowTitleScreen() {
        HideAll();
    }

    public void OnStartNewGame() {
        HideAll();
    }

    public void HideAll() {
        foreach (GameObject obj in InUseObjects.ToList()) {
            HideOne(obj);
        }
    }

    private void HideOne(GameObject obj) {
        obj.SetActive(false);
        AvailableObjects.Add(obj);
        InUseObjects.Remove(obj);
        obj.GetComponent<IPoolable>().DeInitializeOnPooling();
    }

    public GameObject RetrieveAvailableObject() {
        if (AvailableObjects.Count == 0)
            throw new System.Exception("Object pool had no objects free to retrieve");

        GameObject obj = AvailableObjects[0];
        InUseObjects.Add(obj);
        AvailableObjects.Remove(obj);

        obj.GetComponent<IPoolable>().InitializeOnUse();
        return obj;
    }

    public void ChangeObjBackToAvailable(GameObject gameObject) {
        Assert.IsTrue(InUseObjects.Contains(gameObject) || AvailableObjects.Contains(gameObject));
        HideOne(gameObject);
    }
}
