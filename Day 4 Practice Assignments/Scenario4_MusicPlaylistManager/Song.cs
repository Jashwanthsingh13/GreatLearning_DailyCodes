using System;

namespace Scenario4_MusicPlaylistManager
{
    /// <summary>
    /// Song class represents a song in the music system
    /// </summary>
    public class Song
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public double Duration { get; set; } // in minutes
        public int Rating { get; set; } // 1-5 scale
        public string Genre { get; set; }

        public Song(string title, string artist, double duration, int rating, string genre)
        {
            Title = title;
            Artist = artist;
            Duration = duration;
            Rating = rating;
            Genre = genre;
        }

        public override string ToString()
        {
            return $"\"{Title}\" by {Artist} ({Duration} min) - Rating: {Rating}/5 - Genre: {Genre}";
        }
    }
}
