namespace PowerUps
{
    public class GhostPowerUp : PowerUp
    {
        public override void Activate()
        {
            StartCoroutine(Countdown());

            Player.Player.Instance.GhostMode = true;
        }

        public override void Deactivate()
        {
            Player.Player.Instance.GhostMode = false;
        }
    }
}