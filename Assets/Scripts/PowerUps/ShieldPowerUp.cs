using Controllers;

namespace PowerUps
{
    public class ShieldPowerUp : PowerUp
    {
        public override void Activate()
        {
            StartCoroutine(Countdown());

            GameManager.instance.SetRiverTrigger(false);
            Player.Player.Instance.ShieldMode = true;
        }

        public override void Deactivate()
        {
            GameManager.instance.SetRiverTrigger(true);
            Player.Player.Instance.ShieldMode = false;
        }
    }
}