using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DoubleSwitch : MonoBehaviour
    {
        public enum ActiveSwitch
        {
            First,
            Second
        }

        public Dictionary<string, ActiveSwitch> Switches = new Dictionary<string, ActiveSwitch>();
        public Dictionary<ActiveSwitch, string> SwitchNames = new Dictionary<ActiveSwitch, string>();

        [SerializeField] private Image switch1Image;
        [SerializeField] private Sprite switch1OnSprite;
        [SerializeField] private Sprite switch1OffSprite;

        [SerializeField] private Image switch2Image;
        [SerializeField] private Sprite switch2OnSprite;
        [SerializeField] private Sprite switch2OffSprite;

        public ActiveSwitch activeSwitch;

        public delegate void SwitchChanged(string s);

        public event SwitchChanged OnSwitchChanged;


        public void Switch()
        {
            switch (activeSwitch)
            {
                case ActiveSwitch.First:
                    activeSwitch = ActiveSwitch.Second;
                    switch1Image.sprite = switch1OffSprite;
                    switch2Image.sprite = switch2OnSprite;
                    break;
                case ActiveSwitch.Second:
                    activeSwitch = ActiveSwitch.First;
                    switch1Image.sprite = switch1OnSprite;
                    switch2Image.sprite = switch2OffSprite;
                    break;
            }
            OnSwitchChanged?.Invoke(SwitchNames[activeSwitch]);
        }

        public void ActivateSwitch(ActiveSwitch s)
        {
            activeSwitch = s;
            switch (s)
            {
                case ActiveSwitch.First:
                    switch1Image.sprite = switch1OnSprite;
                    switch2Image.sprite = switch2OffSprite;
                    break;
                case ActiveSwitch.Second:
                    switch1Image.sprite = switch1OffSprite;
                    switch2Image.sprite = switch2OnSprite;
                    break;
            }
            OnSwitchChanged?.Invoke(SwitchNames[activeSwitch]);
        }

        public void ActivateSwitch(string switchName)
        {
            if (Switches.ContainsKey(switchName))
                ActivateSwitch(Switches[switchName]);
        }

        public void SetSwitchNames(string leftSwitchName, string rightSwitchName)
        {
            if (!Switches.ContainsKey(leftSwitchName))
            {
                Switches.Add(leftSwitchName, ActiveSwitch.First);
                SwitchNames.Add(ActiveSwitch.First, leftSwitchName);
            }

            if (!Switches.ContainsKey(rightSwitchName))
            {
                Switches.Add(rightSwitchName, ActiveSwitch.Second);
                SwitchNames.Add(ActiveSwitch.Second, rightSwitchName);
            }
        }
    }
}