using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using GorselFinalOdev.Models;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;

namespace GorselFinalOdev;

public partial class ToDoList : ContentPage
{
    // ObservableCollection ile dinamik veri kaynağı
    public ObservableCollection<Notes> Notes { get; set; } = new ObservableCollection<Notes>();

    public ToDoList()
    {
        InitializeComponent();

        // Veri kaynağını CollectionView'a bağlama
        toDoList.ItemsSource = Notes;
        toDoList.SelectionChanged += OnNoteSelected; // Not seçimi için event
    }

    static FirebaseClient client = new FirebaseClient("https://mauiproje-dd483-default-rtdb.firebaseio.com/");

    private async void Edit_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var note = Notes.First(o => o.Id == button.CommandParameter.ToString());

        string result = await DisplayPromptAsync("Notu Düzenle", "Notunuzu giriniz: ", "Tamam", "İptal");

        if (!string.IsNullOrEmpty(result))
        {
            note.NoteContent = result;
            await client.Child($"todoList/{note.Id}").PutAsync(note); // Firebase'de güncelle
        }
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var note = Notes.First(o => o.Id == button.CommandParameter.ToString());

        bool confirm = await DisplayAlert("Silmeyi Onayla", $"{note.NoteContent} notu silinsin mi?", "Evet", "Hayır");

        if (confirm)
        {
            Notes.Remove(note);
            await client.Child($"todoList/{note.Id}").DeleteAsync(); // Firebase'den sil
        }
    }

    private async void NoteAdd_Clicked(object sender, EventArgs e)
    {
        string noteContent = await DisplayPromptAsync("Not Ekle", "Notunuzu giriniz: ", "Tamam", "İptal");

        if (!string.IsNullOrEmpty(noteContent))
        {
            var note = new Notes
            {
                Id = Guid.NewGuid().ToString(),
                NoteContent = noteContent,
                IsCompleted = false,
                DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") // Tarih ve saati ekliyoruz
            };
            await client.Child($"todoList/{note.Id}").PutAsync(note);

            Notes.Add(note);
        }
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        string noteContent = await DisplayPromptAsync("Görev Kaydet", "Görevi giriniz: ", "Tamam", "İptal");

        if (!string.IsNullOrEmpty(noteContent))
        {
            var note = new Notes
            {
                Id = Guid.NewGuid().ToString(),
                NoteContent = noteContent,
                IsCompleted = false,
                DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            Notes.Add(note);
            await client.Child($"todoList/{note.Id}").PutAsync(note); // Firebase'e kaydet
        }
    }

    private void OnNoteSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedNote = e.CurrentSelection.FirstOrDefault() as Notes;
        if (selectedNote != null)
        {
            noteDetails.Text = $"Görev: {selectedNote.NoteContent}\nTarih: {selectedNote.DateCreated}\nDurum: {(selectedNote.IsCompleted ? "Tamamlandı" : "Tamamlanmadı")}";
        }
    }
    private void ToDoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = e.CurrentSelection.FirstOrDefault();
        if (selectedItem != null)
        {
            // Görev detaylarını güncelleyin
            noteDetailsContent.Text = $"Seçilen Görev: {selectedItem}";
        }
    }

    private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.BindingContext is Notes selectedNote)
        {
            try
            {
                // Görev durumunu güncelle
                selectedNote.IsCompleted = e.Value;

                // Firebase URL'si
                var firebaseUrl = "https://mauiproje-dd483-default-rtdb.firebaseio.com/"; // Firebase URL'nizi buraya ekleyin
                var httpClient = new HttpClient();

                // JSON verisi oluşturuluyor
                var jsonData = JsonSerializer.Serialize(new { IsCompleted = selectedNote.IsCompleted });

                // Firebase'deki notu güncelle
                var response = await httpClient.PatchAsync(
                    $"{firebaseUrl}/notes/{selectedNote.Id}.json",
                    new StringContent(jsonData, Encoding.UTF8, "application/json")
                );

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Başarılı", "Görev durumu Firebase'de güncellendi!", "Tamam");
                }
                else
                {
                    await DisplayAlert("Hata", "Firebase'e güncelleme yapılırken bir hata oluştu.", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Bir hata oluştu: {ex.Message}", "Tamam");
            }
        }
    }

}
