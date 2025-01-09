using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GorselFinalOdev.Models
{
    public class Notes : INotifyPropertyChanged
    {

        public Notes() { }
        private string noteContent, id;
        private bool isCompleted;
        internal string DateCreated { get; set; }
        
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public string Id
        {
            get
            {
                if (id == null)
                    id = Guid.NewGuid().ToString();
                return id;
            }
            set { id = value; }
        }

        public string NoteContent
        {
            get { return noteContent; }
            set
            {
                noteContent = value;
                OnPropertyChanged();
            }

        }

        public bool IsCompleted
        {
            get { return isCompleted; }
            set
            {
                isCompleted = value;
                OnPropertyChanged();
            }
        }


    }
}