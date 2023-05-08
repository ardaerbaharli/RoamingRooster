using Player;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "ObjectSettings/CameraSettings", order = 0)]
    public class CameraSettings : ScriptableObject
    {
       public Vector3 offset;
       public float passiveSpeed;
       public float followSpeed;
       public float cameraThreshold;
       public bool passiveMove;
    }
}