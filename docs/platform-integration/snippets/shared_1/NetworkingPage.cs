namespace PlatformIntegration;

public class NetworkingPage : ContentPage
{
    public NetworkingPage()
	{
        this.BindingContext = this;

        Content = new VerticalStackLayout
        {
        };
    }


    public void Test()
    {
        //<network_test>
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;

        if (accessType == NetworkAccess.Internet)
        {
            // Connection to internet is available
        }
        //</network_test>

        //<network_profiles>
        IEnumerable<ConnectionProfile> profiles = Connectivity.Current.ConnectionProfiles;

        if (profiles.Contains(ConnectionProfile.WiFi))
        {
            // Active Wi-Fi connection.
        }
        //</network_profiles>
    }

    //<network_implementation>
    public class ConnectivityTest
    {
        public ConnectivityTest() =>
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

        ~ConnectivityTest() =>
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.ConstrainedInternet)
                Console.WriteLine("Internet access is available but is limited.");

            else if (e.NetworkAccess != NetworkAccess.Internet)
                Console.WriteLine("Internet access has been lost.");

            // Log each active connection
            Console.Write("Connections active: ");

            foreach (var item in e.ConnectionProfiles)
            {
                switch (item)
                {
                    case ConnectionProfile.Bluetooth:
                        Console.Write("Bluetooth");
                        break;
                    case ConnectionProfile.Cellular:
                        Console.Write("Cell");
                        break;
                    case ConnectionProfile.Ethernet:
                        Console.Write("Ethernet");
                        break;
                    case ConnectionProfile.WiFi:
                        Console.Write("WiFi");
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine();
        }
    }
    //</network_implementation>
}