using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "ObjectSettings/PlayerMovementSettings", order = 0)]
    public class PlayerMovementSettings : ScriptableObject
    {
        public float journeyTime;
        public float jumpHeight;
    }
}