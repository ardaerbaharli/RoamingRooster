using System.Collections;
using UnityEngine;

namespace Train
{
    public class RailLightController : MonoBehaviour
    {
        [SerializeField] private RailLight leftLight;
        [SerializeField] private RailLight rightLight;
    
        public bool isEnabled;
        private bool toggle;

        public void StartBlinking()
        {
            isEnabled = true;
            StartCoroutine(StartLightCoroutine());
        }

        public void Stop() 
        {
            isEnabled = false;
            leftLight.Toggle(false);
            rightLight.Toggle(false);
        }

        private IEnumerator StartLightCoroutine()
        {
            while (isEnabled)
            {
                leftLight.Toggle(toggle);
                rightLight.Toggle(!toggle);
                toggle = !toggle;
                yield return new WaitForSeconds(0.5f);
            }
        }
    
   
    }
}