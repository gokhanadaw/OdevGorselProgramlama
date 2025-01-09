using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.ObjectModel;

namespace GorselFinalOdev.Models
{
    internal static class DL
    {
        // Firebase Realtime Database URL
        static FirebaseClient client = new FirebaseClient("https://mauiproje-dd483-default-rtdb.firebaseio.com/");

        public static bool AddNote(Notes note)
        {
            try
            {
                client.Child($"mauiproje-dd483/{note.Id}").PutAsync(note).Wait();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding note: {ex.Message}");
                return false;
            }
        }

        public static bool EditNote(Notes note)
        {
            try
            {
                client.Child($"mauiproje-dd483/{note.Id}").PutAsync(note).Wait();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while editing note: {ex.Message}");
                return false;
            }
        }

        public static bool DeleteNote(Notes note)
        {
            try
            {
                client.Child($"mauiproje-dd483/{note.Id}").DeleteAsync().Wait();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deleting note: {ex.Message}");
                return false;
            }
        }

        public static ObservableCollection<Notes> GetAllNotes()
        {
            ObservableCollection<Notes> notes = new ObservableCollection<Notes>();

            try
            {
                var _notes = client.Child("mauiproje-dd483").OnceAsync<Notes>().Result;
                foreach (var n in _notes)
                {
                    notes.Add(n.Object);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving notes: {ex.Message}");
            }

            return notes;
        }
    }
}
