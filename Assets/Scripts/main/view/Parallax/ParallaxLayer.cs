using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor = 1.0f; //modifer to scale parallax, 0 is a static object on screen and 1 moves with player velocity
    public int startSeed = 0; //used to determine if the sprite is placed in the left, right, or center spot on load
    private float textureUnitSize;
    // Start is called before the first frame update
    void Start()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        Vector3 parentScale = transform.parent.localScale;
        textureUnitSize = texture.width*parentScale.x / sprite.pixelsPerUnit;
        switch (startSeed % 3){
            case 0:
            transform.position += new Vector3(textureUnitSize,0.0f);
                break;
            case 1:
            transform.position -= new Vector3(textureUnitSize,0.0f);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 parentScale = transform.parent.localScale;
        Vector3 deltaMovement = new Vector3(transform.parent.gameObject.GetComponent<ParallaxParent>().pseudoVelocity*-1.0f,0.0f);
        if (parallaxFactor != 0 || deltaMovement != Vector3.zero){
            Vector3 desiredPosition = transform.position + (deltaMovement * parallaxFactor);
            if (desiredPosition.x < transform.parent.position.x - textureUnitSize){
                desiredPosition.x += textureUnitSize * 2;
            }
            else if (desiredPosition.x > transform.parent.position.x + textureUnitSize){
                desiredPosition.x -= textureUnitSize * 2;
            }
            
            transform.position = desiredPosition;
        }
    }
}
