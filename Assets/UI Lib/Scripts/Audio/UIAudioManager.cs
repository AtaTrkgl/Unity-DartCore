using UnityEngine;
using UnityEngine.Audio;

namespace UILib
{
    public class UIAudioManager : MonoBehaviour
    {
        public static void PlayOneShotAudio(AudioClip clip, float volume, AudioMixerGroup mixerGroup)
        {
            if (clip)
            {
                var obj = new GameObject();
                obj.name = "Temp. AudioSource";
                obj.AddComponent<AudioSource>();

                var audioSource = obj.GetComponent<AudioSource>();

                audioSource.loop = false;
                audioSource.clip = clip;
                audioSource.volume = volume;
                if (mixerGroup)
                    audioSource.outputAudioMixerGroup = mixerGroup;

                audioSource.Play();
                Destroy(obj, clip.length);
            }
        }
    }
}