namespace StringToHell.InGame
{
    public interface IAudioPlayer
    {
        void PlayDeath();
        void PlayJump();
        void PlayLand();
        void PlayStringPlace();
        void PlayStringStretch(float stretchAmount);
        void UpdateBungie(float tension, float wind);
    }
}