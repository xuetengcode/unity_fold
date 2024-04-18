
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;

public class OnGroundCollision : MonoBehaviour
{
    [SerializeField] ObjectControl_pillar objectControl;
private void OnCollisionEnter (Collision other){
    if(other.gameObject.tag.Equals("Ground")){
        objectControl.CollideWithGround();
    }
   }
}