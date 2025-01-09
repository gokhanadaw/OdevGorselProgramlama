using GorselFinalOdev.Models;
using System.Text.Json;

namespace GorselFinalOdev;

public partial class News : ContentPage
{
    public News()
    {
        InitializeComponent();
        lstKategori.ItemsSource = haberKategoriList;

        selectedCategory = haberKategoriList[0];
    }

    List<HaberKategori> haberKategoriList = new List<HaberKategori>()
    {
         new HaberKategori(){Baslik = "Manþet", Link = "https://www.trthaber.com/manset_articles.rss"},
         new HaberKategori(){Baslik = "Son Dakika", Link = "https://www.trthaber.com/sondakika_articles.rss"},
         new HaberKategori(){Baslik = "Spor", Link = "https://www.trthaber.com/spor_articles.rss"},
         new HaberKategori(){Baslik = "Ekonomi", Link = "https://www.trthaber.com/ekonomi_articles.rss"},
         new HaberKategori(){Baslik = "Bilim & Teknoloji", Link = "https://www.trthaber.com/bilim_teknoloji_articles.rss"},
    };

    HaberKategori selectedCategory = null;

    private void LoadNews(object sender, EventArgs e)
    {
        Load();
        refreshView.IsRefreshing = false;
    }

    async Task Load()
    {
        try
        {
            if (selectedCategory == null)
            {
                await DisplayAlert("Hata", "Bir kategori seçilmedi.", "Tamam");
                return;
            }

            string jsondata = await NewsService.GetNews(selectedCategory.Link);
            if (!string.IsNullOrEmpty(jsondata))
            {
                try
                {
                    var haberler = JsonSerializer.Deserialize<HaberRoot>(jsondata);
                    if (haberler != null && haberler.items != null)
                    {
                        lstHaberler.ItemsSource = haberler.items;
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Haber verisi iþlenemedi.", "Tamam");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Deserialization Error: {ex.Message}");
                    await DisplayAlert("Hata", $"Haberler yüklenirken hata oluþtu: {ex.Message}", "Tamam");
                }
            }
            else
            {
                await DisplayAlert("Hata", "Haber verisi boþ döndü.", "Tamam");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            await DisplayAlert("Hata", "Haberler yüklenirken bir hata oluþtu.", "Tamam");
        }
    }



    private async void lstKategori_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        selectedCategory = lstKategori.SelectedItem as HaberKategori;
        await Load();
    }

    private void lstHaberler_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var url = (lstHaberler.SelectedItem as Item).link;
        NewsDetail page = new NewsDetail(url);
        Navigation.PushAsync(page);
    }
}

public class HaberKategori
{
    public string Baslik { get; set; }
    public string Link { get; set; }
}