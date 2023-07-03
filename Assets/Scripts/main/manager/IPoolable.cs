using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable {

    public void InitializeOnUse();

    public void DeInitializeOnPooling();

}
