namespace GorselFinalOdev;

public partial class NewsDetail : ContentPage
{
    public NewsDetail(string url)
    {
        InitializeComponent();

        webView.Source = url;
    }

    private async void ShareNews_Clicked(object sender, EventArgs e)
    {
        await ShareUri(Share.Default);
    }

    public async Task ShareUri(IShare share)
    {
        await share.RequestAsync(new ShareTextRequest
        {

        });
    }
}
