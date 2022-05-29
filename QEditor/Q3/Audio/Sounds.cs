using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrrKlang;
namespace Q.Audio
{
    public class VSound
    {
        public ISound Snd;

        /// <summary>
        /// Returns true if song/sound is still playing.
        /// </summary>
        public bool Playing
        {
            get
            {
                return Snd.Finished == false;
            }
        }

        /// <summary>
        /// Stops the current sound from playing.
        /// </summary>
        public void Stop()
        {
            Snd.Stop();
        }
    }

    /// <summary>
    /// VSoundSource is a loaded sound/song, that is not yet being played.
    /// </summary>
    public class VSoundSource
    {
        public ISoundSource Src;
        /// <summary>
        /// Path to the file this soundsource was loaded from.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Plays the sound.
        /// </summary>
        /// <returns>A Sound object that can be changed in real-time.</returns>
        public VSound Play()
        {
            return Audio.PlaySource(this, true);
        }

        /// <summary>
        /// Name of the sound.
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }

    /// <summary>
    /// This is the main class to load and play sounds and songs.
    /// </summary>
    public static class Audio
    {
        public static ISoundEngine engine;
        public static ISound SongSound;

        /// <summary>
        /// Loads a sound from disk, ready to play/use.
        /// </summary>
        /// <param name="path">The path on your HD to the sound.</param>
        /// <returns>The SoundSource, ready to be played.</returns>
        public static VSoundSource LoadSound(string path)
        {
            if (engine == null)
            {
                engine = new ISoundEngine();
            }

            var src = engine.AddSoundSourceFromFile(path);
            var vs = new VSoundSource();
            vs.Src = src;
            vs.Name = new FileInfo(path).Name;
            vs.Path = path;
            return vs;
        }

        /// <summary>
        /// Plays a given sound.
        /// </summary>
        /// <param name="src">The loaded sound source.</param>
        /// <param name="loop">If true, the sound will loop, otherwise just play once.</param>
        /// <returns></returns>
        public static VSound PlaySource(VSoundSource src, bool loop = false)
        {
            var snd = engine.Play2D(src.Src, loop, false, false);
            var vs = new VSound();
            vs.Snd = snd;
            return vs;
        }

        /// <summary>
        /// Plays a song from HD.
        /// </summary>
        /// <param name="song">The path to the song, on your HD.</param>
        public static void PlaySong(string song)
        {
            if (engine == null)
            {
                engine = new ISoundEngine();
            }

            // To play a sound, we only to call play2D(). The second parameter
            // tells the engine to play it looped.

            SongSound = engine.Play2D(song, false);
        }

        /// <summary>
        /// Stops the current song from playing.
        /// </summary>
        public static void StopSong()
        {
            if (SongSound != null)
            {
                if (!SongSound.Finished)
                {
                    SongSound.Stop();
                }
                SongSound = null;
            }
        }
    }
}
