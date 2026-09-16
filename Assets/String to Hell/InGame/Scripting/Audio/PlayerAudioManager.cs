using UnityEngine;
using FMODUnity;
using FMOD.Studio;
namespace StringToHell.InGame
{
    public class PlayerAudioManager : MonoBehaviour, IAudioPlayer
    {
        [Header("Basic Actions")]
        public EventReference jumpEvent;
        public EventReference landEvent;
        public EventReference deathEvent;

        [Header("String / Bungie")]
        public EventReference stringPlaceEvent;
        public EventReference stringStretchEvent;
        public EventReference bungieEvent;

        EventInstance bungieInstance;

        //void Start()
        //{
        //    bungieInstance = RuntimeManager.CreateInstance(bungieEvent);
        //    bungieInstance.start();
        //}

        public void PlayJump()
        {
            RuntimeManager.PlayOneShot(jumpEvent, transform.position);
        }

        public void PlayLand()
        {
            RuntimeManager.PlayOneShot(landEvent, transform.position);
        }

        public void PlayDeath()
        {
            RuntimeManager.PlayOneShot(deathEvent, transform.position);
        }

        public void PlayStringPlace()
        {
            RuntimeManager.PlayOneShot(stringPlaceEvent, transform.position);
        }

        public void PlayStringStretch(float stretchAmount)
        {
            var inst = RuntimeManager.CreateInstance(stringStretchEvent);
            inst.setParameterByName("StretchAmount", stretchAmount);
            inst.start();
            inst.release();
        }

        public void UpdateBungie(float tension, float wind)
        {
            bungieInstance.setParameterByName("Tension", tension);
            bungieInstance.setParameterByName("Wind", wind);
        }
    }
}