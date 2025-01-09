using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorselFinalOdev.Models
{
    internal static class BL
    {
        public static ObservableCollection<Notes> Notes = new ObservableCollection<Notes>();

        public static bool LoadNotes()
        {
            Notes = DL.GetAllNotes();
            return true;
        }

        public static bool EditNote(Notes note)
        {
            return DL.EditNote(note);
        }

        public static bool DeleteNote(Notes note)
        {
            return DL.DeleteNote(note);
        }

        public static bool AddNote(Notes note)
        {
            return DL.AddNote(note);
        }
    }
}