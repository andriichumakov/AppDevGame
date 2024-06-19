using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace AppDevGame
{
    public class AudioManager
    {
        private static AudioManager _instance;
        private Dictionary<string, SoundEffect> _soundEffects;
        private Song _backgroundMusic;
        private Song _levelMusic;

        private AudioManager()
        {
            _soundEffects = new Dictionary<string, SoundEffect>();
        }

        public static AudioManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AudioManager();
            }
            return _instance;
        }

        public void LoadContent(ContentManager content)
        {
            _backgroundMusic = content.Load<Song>("background_music");
            _levelMusic = content.Load<Song>("level_music");

            _soundEffects["button_click"] = content.Load<SoundEffect>("button_click");
            _soundEffects["player_attack"] = content.Load<SoundEffect>("player_attack");
            // Add other sound effects here as needed

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_backgroundMusic);
        }

        public void PlaySoundEffect(string soundName)
        {
            if (_soundEffects.ContainsKey(soundName))
            {
                _soundEffects[soundName].Play();
            }
        }

        public void PlayAttackSound()
        {
            PlaySoundEffect("player_attack");
        }

        public void PlayButtonClickSound()
        {
            PlaySoundEffect("button_click");
        }

        public void PlayMainMenuMusic()
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(_backgroundMusic);
            MediaPlayer.IsRepeating = true; // Loop the song
        }

        public void PlayLevelMusic()
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(_levelMusic);
            MediaPlayer.IsRepeating = true; // Loop the song
        }

        public void SetMasterVolume(float volume)
        {
            MediaPlayer.Volume = volume;
            SoundEffect.MasterVolume = volume;
        }
    }
}