using UnityEngine;

namespace MapEntities
{
    public class BigBird : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float lifeTime;

        private void FixedUpdate()
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
                Destroy(gameObject);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}