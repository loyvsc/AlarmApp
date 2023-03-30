using AlarmApp.Models;
using System.Collections.Generic;

namespace AlarmApp
{
    /// <summary>
    /// Default values to be used by the app for various reasons
    /// </summary>
    public static class Defaults
    {
        public static readonly List<AlarmTone> Tones = new List<AlarmTone>()
        {
            new AlarmTone("Выбор своего рингтона...", null),
            new AlarmTone("Buzz", "buzz.mp3"),
            new AlarmTone("Synth", "synth.mp3"),
            new AlarmTone("Xylophone", "xylophone.mp3"),
            new AlarmTone("Shooting Stars", "shooting_stars.mp3"),
            new AlarmTone("Sixteen Bit", "sixteen_bit.mp3"),
            new AlarmTone("Sci-fi", "sci_fi.mp3")
        };
    }

    public enum AlarmListType
    {
        All, Today
    }
}