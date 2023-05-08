using UnityEngine;

namespace MapEntities
{
    public class ObjectBounce : MonoBehaviour
    {
        public float bounceSpeed = 8;
        public float bounceAmplitude = 0.05f;
        public float rotationSpeed = 90;

        private float startHeight;
        private float timeOffset;

        private void Start()
        {
            startHeight = transform.localPosition.y;
            timeOffset = Random.value * Mathf.PI * 2;
        }

        public void Update()
        {
            var finalheight = startHeight + Mathf.Sin(Time.time * bounceSpeed + timeOffset) * bounceAmplitude;
            var position = transform.localPosition;
            position.y = finalheight;
            transform.localPosition = position;

            var rotation = transform.localRotation.eulerAngles;
            rotation.y += rotationSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        
        }
    }
}
