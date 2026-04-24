using System;
using System.Collections.Generic;
using System.Linq;

namespace Scenario4_MusicPlaylistManager
{
    /// <summary>
    /// Interface for playlist operations
    /// </summary>
    public interface IPlaylistManager
    {
        void AddSong(string song);
        void RemoveSong(string song);
        void DisplayPlaylist();
    }

    /// <summary>
    /// MusicPlaylistManager class implements IPlaylistManager
    /// Uses advanced collections for dynamic playlist management
    /// </summary>
    public class MusicPlaylistManager : IPlaylistManager
    {
        // Constants
        private const string APP_NAME = "Music Playlist Manager";
        private const int MAX_SONGS = 5000;
        private const int MIN_RATING = 1;
        private const int MAX_RATING = 5;

        // Advanced Collections
        private LinkedList<string> playlistSongs;              // Easy insertion/removal
        private SortedList<int, string> songsByRating;         // Songs sorted by rating
        private SortedDictionary<string, string> artistSongMap;// Artist → Song (sorted by artist)
        private Dictionary<string, Song> songDatabase;         // Store all song details
        private Stack<string> playHistory;                     // Track play history

        public MusicPlaylistManager()
        {
            playlistSongs = new LinkedList<string>();
            songsByRating = new SortedList<int, string>();
            artistSongMap = new SortedDictionary<string, string>();
            songDatabase = new Dictionary<string, Song>();
            playHistory = new Stack<string>();
        }

        // Static method to display app info
        public static void DisplayAppInfo()
        {
            Console.WriteLine($"=== {APP_NAME} ===");
            Console.WriteLine($"Max Songs in Database: {MAX_SONGS}");
            Console.WriteLine($"Rating Range: {MIN_RATING}-{MAX_RATING}");
            Console.WriteLine();
        }

        /// <summary>
        /// Add song to database
        /// </summary>
        public void AddSongToDatabase(Song song)
        {
            if (!songDatabase.ContainsKey(song.Title))
            {
                songDatabase[song.Title] = song;
                Console.WriteLine($"✓ Song added to database: {song.Title}");
            }
            else
            {
                Console.WriteLine($"✗ Song '{song.Title}' already exists in database");
            }
        }

        /// <summary>
        /// Add song to playlist (LinkedList for easy insertion/removal)
        /// </summary>
        public void AddSong(string songTitle)
        {
            if (songDatabase.ContainsKey(songTitle))
            {
                if (playlistSongs.Count < MAX_SONGS)
                {
                    playlistSongs.AddLast(songTitle);
                    Song song = songDatabase[songTitle];

                    // Add to sorted list by rating (unique key challenge - using rating with counter)
                    int ratingKey = song.Rating * 1000 + songsByRating.Count;
                    songsByRating[ratingKey] = songTitle;

                    // Add to artist-song map
                    if (!artistSongMap.ContainsKey(song.Artist))
                    {
                        artistSongMap[song.Artist] = songTitle;
                    }

                    Console.WriteLine($"✓ Added to playlist: {songTitle}");
                }
                else
                {
                    Console.WriteLine($"✗ Playlist is full (max {MAX_SONGS} songs)");
                }
            }
            else
            {
                Console.WriteLine($"✗ Song '{songTitle}' not found in database");
            }
        }

        /// <summary>
        /// Remove song from playlist (LinkedList advantage)
        /// </summary>
        public void RemoveSong(string songTitle)
        {
            var node = playlistSongs.Find(songTitle);
            if (node != null)
            {
                playlistSongs.Remove(node);
                Console.WriteLine($"✓ Removed from playlist: {songTitle}");
            }
            else
            {
                Console.WriteLine($"✗ Song '{songTitle}' not found in playlist");
            }
        }

        /// <summary>
        /// Play a song (add to play history)
        /// </summary>
        public void PlaySong(string songTitle)
        {
            if (songDatabase.ContainsKey(songTitle))
            {
                playHistory.Push(songTitle);
                Console.WriteLine($"▶ Now playing: {songTitle}");
            }
            else
            {
                Console.WriteLine($"✗ Song '{songTitle}' not found");
            }
        }

        /// <summary>
        /// Display current playlist (LinkedList order)
        /// </summary>
        public void DisplayPlaylist()
        {
            Console.WriteLine("\n--- Current Playlist ---");
            if (playlistSongs.Count == 0)
            {
                Console.WriteLine("Playlist is empty");
                return;
            }

            int count = 1;
            foreach (var songTitle in playlistSongs)
            {
                Song song = songDatabase[songTitle];
                Console.WriteLine($"{count}. {song}");
                count++;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display songs sorted by rating
        /// </summary>
        public void DisplaySortedByRating()
        {
            Console.WriteLine("\n--- Songs Sorted by Rating (SortedList) ---");
            if (songsByRating.Count == 0)
            {
                Console.WriteLine("No songs available");
                return;
            }

            var uniqueSongs = new HashSet<string>();
            foreach (var song in songsByRating.Values)
            {
                if (uniqueSongs.Add(song))
                {
                    Song songObj = songDatabase[song];
                    Console.WriteLine($"⭐ {songObj}");
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display songs by artist (sorted)
        /// </summary>
        public void DisplayByArtist()
        {
            Console.WriteLine("\n--- Songs by Artist (SortedDictionary - Sorted by Artist) ---");
            if (artistSongMap.Count == 0)
            {
                Console.WriteLine("No artists available");
                return;
            }

            foreach (var artistEntry in artistSongMap)
            {
                string artist = artistEntry.Key;
                string songTitle = artistEntry.Value;
                Song song = songDatabase[songTitle];
                Console.WriteLine($"Artist: {artist}");
                Console.WriteLine($"  └─ {song}\n");
            }
        }

        /// <summary>
        /// Display all songs in database
        /// </summary>
        public void DisplayAllSongs()
        {
            Console.WriteLine("\n--- All Songs in Database ---");
            if (songDatabase.Count == 0)
            {
                Console.WriteLine("No songs in database");
                return;
            }

            int count = 1;
            foreach (var song in songDatabase.Values)
            {
                Console.WriteLine($"{count}. {song}");
                count++;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display play history
        /// </summary>
        public void DisplayPlayHistory()
        {
            Console.WriteLine("\n--- Play History (LIFO) ---");
            if (playHistory.Count == 0)
            {
                Console.WriteLine("No songs played yet");
                return;
            }

            var tempStack = new Stack<string>(playHistory);
            int count = 1;
            while (tempStack.Count > 0)
            {
                Console.WriteLine($"{count}. {tempStack.Pop()}");
                count++;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Get statistics
        /// </summary>
        public void DisplayStatistics()
        {
            Console.WriteLine("\n=== Playlist Statistics ===");
            Console.WriteLine($"Total Songs in Database: {songDatabase.Count}");
            Console.WriteLine($"Songs in Playlist: {playlistSongs.Count}");
            Console.WriteLine($"Unique Artists: {artistSongMap.Count}");
            Console.WriteLine($"Play History Length: {playHistory.Count}");
            Console.WriteLine();
        }

        public int GetPlaylistCount() => playlistSongs.Count;
        public int GetDatabaseCount() => songDatabase.Count;
    }
}
