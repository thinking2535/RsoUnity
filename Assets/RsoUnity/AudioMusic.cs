using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace rso.unity
{
    // 한번 Stop 했으면 Volume이 0이 될때 까지 멈출 수 없음. 따라서 Stop 직후 Play 했다 하더라도 Volume이 0이 된 후에 다시 재생됨
    // 씬 전환이 급하게 되더라도 BGM은 안정적으로 재생되도록 하기 위함
    // 따라서 _IsPlaying, _IsStopping 은 별도로 존재하여 서로 true 인 상태라 할지라도 모순된 상태가 아니고
    // _IsStopping == true 라면 이 변수가 우선 작용하여 완전히 멈추어 _IsStopping == false 가 되면 이후 _IsPlaying == true 라면 재생 시작됨
    public class CAudioMusic : CAudio
    {
        const Int32 _musicGroupIndexNull = -1;
        const float _changeMusicFadeOutTime = 1.5f;

        System.Random _random = new System.Random(DateTime.Now.Second);
        AudioClip[][] _clips = null;
        float _volumeScale = 1.0f;
        Int32 _curMusicGroupIndex = _musicGroupIndexNull;
        Int32 _curClipIndex = 0;
        bool _isPlaying = false;
        bool _isStopping = false;
        bool _isRandom = false;
        CLinear _fadeOutTimer = new CLinear();
        public CAudioMusic(AudioSource source, string[][] clipPath, bool isRandom) :
            base(source)
        {
            Debug.Assert(clipPath.Length > 0, "Invalid ClipPaths_.Length");
            _clips = new AudioClip[clipPath.Length][];

            for (Int32 musicGroupIndex = 0; musicGroupIndex < clipPath.Length; ++musicGroupIndex)
            {
                Debug.Assert(clipPath[musicGroupIndex].Length > 0, "Invalid clipPath[musicGroupIndex].Length");
                _clips[musicGroupIndex] = new AudioClip[clipPath[musicGroupIndex].Length];

                for (Int32 ClipIndex = 0; ClipIndex < clipPath[musicGroupIndex].Length; ++ClipIndex)
                    _clips[musicGroupIndex][ClipIndex] = Resources.Load<AudioClip>(clipPath[musicGroupIndex][ClipIndex]);
            }

            _isRandom = isRandom;
        }
        public void setVolume(float Volume_)
        {
            _volumeScale = Volume_;

            if (!_isStopping)
                _Source.volume = Volume_;
        }
        public void stop()
        {
            if (_isPlaying)
                _isPlaying = false;

            if (_isStopping)
                return;

            _curMusicGroupIndex = _musicGroupIndexNull;
            _fadeOut();
        }
        void _fadeOut()
        {
            _isStopping = true;
            _fadeOutTimer.SetDuration(_changeMusicFadeOutTime, 1.0f, 0.0f);
        }
        public void play(Int32 musicGroupIndex)
        {
            if (musicGroupIndex == _curMusicGroupIndex || musicGroupIndex < 0 || musicGroupIndex >= _clips.Length)
                return;

            _curMusicGroupIndex = musicGroupIndex;

            if (!_isPlaying)
                _isPlaying = true;
            else if (!_isStopping)
                _fadeOut();
        }
        public void update()
        {
            if (_isStopping)
            {
                if (_Source.isPlaying)
                {
                    float Scale = 0.0f;
                    if (_fadeOutTimer.Get(ref Scale))
                    {
                        _Source.volume = (Scale * _volumeScale);
                    }
                    else
                    {
                        _isStopping = false;
                        _Source.volume = _volumeScale;
                        _Source.Stop();
                    }
                }
                else
                {
                    _isStopping = false;
                    _Source.volume = _volumeScale;
                }
            }
            else if (_isPlaying)
            {
                if (_Source.isPlaying)
                {
                    if (_Source.clip.length - _Source.time < _changeMusicFadeOutTime)
                        _fadeOut();
                }
                else
                {
                    _setNextClipIndex();
                    _Source.clip = _clips[_curMusicGroupIndex][_curClipIndex];
                    _Source.Play();
                }
            }
        }
        void _setNextClipIndex()
        {
            if (_isRandom)
                _curClipIndex += _random.Next(1, _clips[_curMusicGroupIndex].Length); // Random 이라 할지라도 동일한 음악이 다시 재생되지 않도록
            else
                ++_curClipIndex;

            if (_curClipIndex >= _clips[_curMusicGroupIndex].Length)
                _curClipIndex %= _clips[_curMusicGroupIndex].Length;

            _Source.clip = _clips[_curMusicGroupIndex][_curClipIndex];
        }
    }
}