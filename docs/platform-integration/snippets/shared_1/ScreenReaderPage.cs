namespace PlatformIntegration;

public class ScreenReaderPage : ContentPage
{
	public ScreenReaderPage()
	{
		Content = new VerticalStackLayout
		{
			Children = {
				new Button { Text = "Read",
							 Command = new Command(Announce) }
			}
		};
	}

	public void Announce() =>
		//<announce>
        SemanticScreenReader.Default.Announce("This is the announcement text.");
		//</announce>
}