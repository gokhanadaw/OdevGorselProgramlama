using System.Text.Json;

namespace GorselFinalOdev;

public partial class Kurlar : ContentPage
{
    private static Kurlar instance;

    public Kurlar()
    {
        InitializeComponent();
    }

    public static Kurlar Page
    {
        get
        {
            if (instance == null)
                instance = new Kurlar();
            return instance;
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await Load();
    }

    async Task Load()
    {
        try
        {
            string jsondata = await GetAltinDovizGuncelKurlar();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using JsonDocument doc = JsonDocument.Parse(jsondata);
            JsonElement root = doc.RootElement;

            var currencies = new List<Currency>();

            foreach (JsonProperty property in root.EnumerateObject())
            {
                if (property.Name != "Update_Date")
                {
                    var currency = new Currency
                    {
                        Name = property.Name,
                        Alis = property.Value.GetProperty("Alýþ").GetString(),
                        Satis = property.Value.GetProperty("Satýþ").GetString(),
                        Degisim = property.Value.GetProperty("Deðiþim").GetString()
                    };
                    currencies.Add(currency);
                }
            }

            dovizliste.ItemsSource = currencies;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    async Task<string> GetAltinDovizGuncelKurlar()
    {
        string url = "https://finans.truncgil.com/today.json";
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        string jsondata = await response.Content.ReadAsStringAsync();
        return jsondata;
    }

    async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        await Load();
    }
}

public class Currency
{
    public string Name { get; set; }
    public string Alis { get; set; }
    public string Satis { get; set; }
    public string Degisim { get; set; }
}
