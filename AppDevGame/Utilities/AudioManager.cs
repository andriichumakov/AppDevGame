using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace AppDevGame
{
    public class AudioManager
    {
        private static AudioManager _instance;
        private ContentManager _content;
        private Dictionary<string, SoundEffect> _soundEffects;
        private Dictionary<string, Song> _songs;

        private AudioManager(ContentManager content)
        {
            _content = content;
            _soundEffects = new Dictionary<string, SoundEffect>();
            _songs = new Dictionary<string, Song>();
            LoadContent();
        }

        public static AudioManager GetInstance(ContentManager content)
        {
            if (_instance == null)
            {
                _instance = new AudioManager(content);
            }
            return _instance;
        }

        private void LoadContent()
        {
            // Load all sound effects
            _soundEffects["button_click"] = _content.Load<SoundEffect>("Audio/button_click");
            _soundEffects["player_attack"] = _content.Load<SoundEffect>("Audio/player_attack");
            _soundEffects["player_damage"] = _content.Load<SoundEffect>("Audio/player_damage");
            _soundEffects["player_die"] = _content.Load<SoundEffect>("Audio/player_die");
            _soundEffects["enemy_attack"] = _content.Load<SoundEffect>("Audio/enemy_attack");
            _soundEffects["enemy_damage"] = _content.Load<SoundEffect>("Audio/enemy_damage");
            _soundEffects["enemy_die"] = _content.Load<SoundEffect>("Audio/enemy_die");
            _soundEffects["coin_collect"] = _content.Load<SoundEffect>("Audio/coin_collect");
            _soundEffects["heart_collect"] = _content.Load<SoundEffect>("Audio/heart_collect");
            _soundEffects["lantern_lit"] = _content.Load<SoundEffect>("Audio/lantern_lit");
            _soundEffects["portal_activate"] = _content.Load<SoundEffect>("Audio/portal_activate");
            _soundEffects["plantbeast_attack"] = _content.Load<SoundEffect>("Audio/plantbeast_attack");
            _soundEffects["plantbeast_damage"] = _content.Load<SoundEffect>("Audio/plantbeast_damage");
            _soundEffects["plantbeast_die"] = _content.Load<SoundEffect>("Audio/plantbeast_die");

            // Load songs
            _songs["background_music"] = _content.Load<Song>("Audio/background_music");
            _songs["level_music"] = _content.Load<Song>("Audio/level_music");
        }

        public void PlaySoundEffect(string soundName)
        {
            if (_soundEffects.ContainsKey(soundName))
            {
                _soundEffects[soundName].Play();
            }
        }

        public void PlaySong(string songName)
        {
            if (_songs.ContainsKey(songName))
            {
                MediaPlayer.Play(_songs[songName]);
                MediaPlayer.IsRepeating = true;
            }
        }

        public void StopSong()
        {
            MediaPlayer.Stop();
        }

        public void SetMasterVolume(float volume)
        {
            MediaPlayer.Volume = volume;
            SoundEffect.MasterVolume = volume;
        }
    }
}