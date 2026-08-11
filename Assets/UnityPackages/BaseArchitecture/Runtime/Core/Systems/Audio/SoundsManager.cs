using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Persisted volume/mute state, keyed by an arbitrary channel name.
    /// </summary>
    public class SoundsSaveData : ISaveData
    {
        public const string SaveKey = "SoundsSettings";
        public Dictionary<string, float> ChannelVolumes = new();
        public bool Muted;
    }

    /// <summary>
    /// Generic audio playback system, organized into caller-named "channels" (volume buses).
    /// Looping clips on a channel crossfade between each other automatically.
    /// </summary>
    public interface ISoundsManager : IInitializable
    {
        void PlayOneShot(AudioClip clip, string channel, float volume = 1f);
        void PlayLoop(AudioClip clip, string channel, float volume = 1f, float crossfadeDuration = 0.5f);
        void StopLoop(string channel, float fadeDuration = 0.5f);

        /// <summary>
        /// True if this clip is the active track on some looping channel. One-shots aren't tracked.
        /// </summary>
        bool IsPlaying(AudioClip clip);

        bool IsChannelPlaying(string channel);
        float GetChannelVolume(string channel);
        void SetChannelVolume(string channel, float volume);
        bool IsMuted { get; }
        void SetMuted(bool muted);
    }

    public class SoundsManager : ISoundsManager
    {
        /// <summary>Two sources per channel, so an incoming loop can fade in while the previous one
        /// fades out.</summary>
        private class LoopChannelDTO
        {
            public AudioSource SourceA;
            public AudioSource SourceB;
            public AudioSource Active;
            public Tween FadeTween;
        }

        [Inject] private readonly IPersistenceManager _persistenceManager;

        private SoundsSaveData _data;
        private GameObject _root;
        private readonly Dictionary<string, AudioSource> _oneShotSources = new();
        private readonly Dictionary<string, LoopChannelDTO> _loopChannels = new();

        public bool IsMuted => _data.Muted;

        public void Initialize()
        {
            _data = _persistenceManager.Load<SoundsSaveData>(SoundsSaveData.SaveKey);
            _root = new GameObject(nameof(SoundsManager));
            Object.DontDestroyOnLoad(_root);
        }

        public void PlayOneShot(AudioClip clip, string channel, float volume = 1f)
        {
            if (clip == null)
            {
                return;
            }

            GetOrCreateOneShotSource(channel).PlayOneShot(clip, GetOutputVolume(channel, volume));
        }

        public void PlayLoop(AudioClip clip, string channel, float volume = 1f, float crossfadeDuration = 0.5f)
        {
            if (clip == null)
            {
                return;
            }

            var loopChannel = GetOrCreateLoopChannel(channel);

            if (loopChannel.Active.clip == clip && loopChannel.Active.isPlaying)
            {
                return;
            }

            if (loopChannel.FadeTween != null)
            {
                loopChannel.FadeTween.Kill();
            }

            var outgoing = loopChannel.Active;
            var incoming = outgoing == loopChannel.SourceA ? loopChannel.SourceB : loopChannel.SourceA;

            incoming.clip = clip;
            incoming.loop = true;
            incoming.volume = 0f;
            incoming.Play();

            float targetVolume = GetOutputVolume(channel, volume);
            var sequence = DOTween.Sequence();
            sequence.Join(FadeVolume(incoming, targetVolume, crossfadeDuration));

            if (outgoing.isPlaying)
            {
                sequence.Join(FadeVolume(outgoing, 0f, crossfadeDuration).OnComplete(outgoing.Stop));
            }

            loopChannel.FadeTween = sequence;
            loopChannel.Active = incoming;
        }

        public void StopLoop(string channel, float fadeDuration = 0.5f)
        {
            if (!_loopChannels.TryGetValue(channel, out var loopChannel) || !loopChannel.Active.isPlaying)
            {
                return;
            }

            if (loopChannel.FadeTween != null)
            {
                loopChannel.FadeTween.Kill();
            }

            var active = loopChannel.Active;
            loopChannel.FadeTween = FadeVolume(active, 0f, fadeDuration).OnComplete(active.Stop);
        }

        private static Tween FadeVolume(AudioSource source, float targetVolume, float duration)
        {
            return DOTween.To(() => source.volume, x => source.volume = x, targetVolume, duration);
        }

        public bool IsPlaying(AudioClip clip)
        {
            foreach (var loopChannel in _loopChannels.Values)
            {
                if (loopChannel.Active.clip == clip && loopChannel.Active.isPlaying)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsChannelPlaying(string channel)
        {
            return _loopChannels.TryGetValue(channel, out var loopChannel) && loopChannel.Active.isPlaying;
        }

        public float GetChannelVolume(string channel)
        {
            if (_data.ChannelVolumes.TryGetValue(channel, out float volume))
            {
                return volume;
            }

            return 1f;
        }

        public void SetChannelVolume(string channel, float volume)
        {
            _data.ChannelVolumes[channel] = volume;
            _persistenceManager.Save(SoundsSaveData.SaveKey, _data);

            if (_loopChannels.TryGetValue(channel, out var loopChannel))
            {
                loopChannel.Active.volume = GetOutputVolume(channel, volume);
            }
        }

        public void SetMuted(bool muted)
        {
            _data.Muted = muted;
            _persistenceManager.Save(SoundsSaveData.SaveKey, _data);

            foreach (var pair in _loopChannels)
            {
                pair.Value.Active.volume = GetOutputVolume(pair.Key, GetChannelVolume(pair.Key));
            }
        }

        /// <summary>Combines the requested volume with the channel volume and the mute state.</summary>
        private float GetOutputVolume(string channel, float baseVolume)
        {
            if (IsMuted)
            {
                return 0f;
            }

            return baseVolume * GetChannelVolume(channel);
        }

        private AudioSource GetOrCreateOneShotSource(string channel)
        {
            if (_oneShotSources.TryGetValue(channel, out var source))
            {
                return source;
            }

            source = CreateSource($"{channel}_OneShot");
            _oneShotSources[channel] = source;
            return source;
        }

        private LoopChannelDTO GetOrCreateLoopChannel(string channel)
        {
            if (_loopChannels.TryGetValue(channel, out var loopChannel))
            {
                return loopChannel;
            }

            var sourceA = CreateSource($"{channel}_LoopA");
            var sourceB = CreateSource($"{channel}_LoopB");

            loopChannel = new LoopChannelDTO { SourceA = sourceA, SourceB = sourceB, Active = sourceA };
            _loopChannels[channel] = loopChannel;
            return loopChannel;
        }

        private AudioSource CreateSource(string name)
        {
            var go = new GameObject(name);
            go.transform.SetParent(_root.transform);
            return go.AddComponent<AudioSource>();
        }
    }
}
