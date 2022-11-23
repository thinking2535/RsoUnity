using System;
using UnityEngine;

namespace rso.unity
{
    public class CAudioSound : CAudio
    {
        class _AudioClip
        {
            static TimeSpan _delay = TimeSpan.FromMilliseconds(100);

            AudioClip _clip;
            DateTime _lastPlayedTime = new DateTime(0);
            public _AudioClip(AudioClip clip)
            {
                _clip = clip;
                _lastPlayedTime = new DateTime(0);
            }
            public AudioClip playAndGetClip()
            {
                var now = DateTime.Now;

                if (now - _lastPlayedTime < _delay)
                    return null;

                _lastPlayedTime = now;
                return _clip;
            }
        }

        _AudioClip[] _Clips = null;
        public CAudioSound(AudioSource Source_, string[] ClipPaths_) :
            base(Source_)
        {
            _Clips = new _AudioClip[ClipPaths_.Length];

            for (Int32 i = 0; i < ClipPaths_.Length; ++i)
                _Clips[i] = new _AudioClip(Resources.Load<AudioClip>(ClipPaths_[i]));
        }
        public void SetVolume(float Volume_)
        {
            _Source.volume = Volume_;
        }
        public void PlayOneShot(Int32 ClipIndex_)
        {
            var clip = _Clips[ClipIndex_].playAndGetClip();

            if (clip == null)
                return;

            _Source.PlayOneShot(clip);
        }
    }
}