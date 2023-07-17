using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxParent : MonoBehaviour
{
    public float pseudoVelocity = 1.0f; //the velocity used for the background, the game should set this in gameplay (since player "moves" right, this will be subtracted from position)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pseudoVelocity = GameStateManager.instance.GetDistanceToTravelThisFrame();
    }
}
