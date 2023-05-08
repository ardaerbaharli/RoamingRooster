using UnityEngine;

namespace Train
{
    public class RailLight : MonoBehaviour
    {
        private Light lightControl;
        private Material mat;

        private void Start()
        {
            lightControl = GetComponent<Light>();
            mat = GetComponent<MeshRenderer>().material;
        }

        public void Toggle(bool value)
        {
            if (lightControl == null) return;

            lightControl.enabled = value;
            if (!value)
                mat.DisableKeyword("_EMISSION");
            else
                mat.EnableKeyword("_EMISSION");
        }
    }
}