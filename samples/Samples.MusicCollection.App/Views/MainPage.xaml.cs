namespace Samples.MusicCollection.App;

/// <summary>
/// 
/// </summary>
public sealed partial class MainPage : ContentPage
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="MainPage"/>
    /// </summary>
    public MainPage()
    {
        InitializeComponent();
    }

    #endregion


    int count = 0;
    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}