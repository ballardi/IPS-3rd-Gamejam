using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable {

    // called by ObjectPool when an object is taken out of the pool to be used
    public void InitializeOnUse();

    // called by ObjectPool when an object is being put back into the pool
    public void DeInitializeOnPooling();

}
