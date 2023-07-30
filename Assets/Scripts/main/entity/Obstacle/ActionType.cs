using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Action/Action Type")]
public class ActionType : ScriptableObject
{
    public ActionEnum dir;
    public Sprite success_image;
    public Sprite default_image;
}
