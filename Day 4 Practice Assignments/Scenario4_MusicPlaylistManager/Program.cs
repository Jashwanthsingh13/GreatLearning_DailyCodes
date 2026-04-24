using System;

namespace Scenario4_MusicPlaylistManager
{
    class Program
    {
        static void Main(string[] args)
        {
            // Display app info
            MusicPlaylistManager.DisplayAppInfo();

            // Create playlist manager
            MusicPlaylistManager playlistManager = new MusicPlaylistManager();

            // Add songs to database
            playlistManager.AddSongToDatabase(new Song("Blinding Lights", "The Weeknd", 3.2, 5, "Synthwave"));
            playlistManager.AddSongToDatabase(new Song("Shape of You", "Ed Sheeran", 3.53, 4, "Pop"));
            playlistManager.AddSongToDatabase(new Song("Bohemian Rhapsody", "Queen", 5.55, 5, "Rock"));
            playlistManager.AddSongToDatabase(new Song("Imagine", "John Lennon", 3.3, 5, "Rock"));
            playlistManager.AddSongToDatabase(new Song("Someone Like You", "Adele", 4.45, 4, "Pop"));
            playlistManager.AddSongToDatabase(new Song("Stairway to Heaven", "Led Zeppelin", 8.02, 5, "Rock"));
            playlistManager.AddSongToDatabase(new Song("Hotline Bling", "Drake", 4.28, 3, "Hip-Hop"));
            playlistManager.AddSongToDatabase(new Song("Say Something", "Justin Timberlake", 4.09, 4, "Pop"));

            // Display all songs in database
            playlistManager.DisplayAllSongs();

            // Add songs to playlist
            Console.WriteLine("--- Adding Songs to Playlist ---");
            playlistManager.AddSong("Blinding Lights");
            playlistManager.AddSong("Bohemian Rhapsody");
            playlistManager.AddSong("Imagine");
            playlistManager.AddSong("Shape of You");
            playlistManager.AddSong("Someone Like You");
            playlistManager.AddSong("Stairway to Heaven");

            // Display current playlist
            playlistManager.DisplayPlaylist();

            // Play some songs
            Console.WriteLine("--- Playing Songs ---");
            playlistManager.PlaySong("Blinding Lights");
            playlistManager.PlaySong("Bohemian Rhapsody");
            playlistManager.PlaySong("Imagine");
            playlistManager.PlaySong("Shape of You");

            // Display songs sorted by rating
            playlistManager.DisplaySortedByRating();

            // Display songs by artist (sorted alphabetically)
            playlistManager.DisplayByArtist();

            // Display play history
            playlistManager.DisplayPlayHistory();

            // Remove a song from playlist
            Console.WriteLine("\n--- Removing Song from Playlist ---");
            playlistManager.RemoveSong("Hotline Bling");
            playlistManager.RemoveSong("Someone Like You");

            // Display updated playlist
            playlistManager.DisplayPlaylist();

            // Display statistics
            playlistManager.DisplayStatistics();
        }
    }
}
