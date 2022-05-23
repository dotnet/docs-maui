namespace PlatformIntegration;

public partial class CommsPage : ContentPage
{
	public CommsPage()
	{
		InitializeComponent();
	}

    //<contact_select>
    private async void SelectContactButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var contact = await Contacts.Default.PickContactAsync();

            if (contact == null)
                return;

            string id = contact.Id;
            string namePrefix = contact.NamePrefix;
            string givenName = contact.GivenName;
            string middleName = contact.MiddleName;
            string familyName = contact.FamilyName;
            string nameSuffix = contact.NameSuffix;
            string displayName = contact.DisplayName;
            List<ContactPhone> phones = contact.Phones; // List of phone numbers
            List<ContactEmail> emails = contact.Emails; // List of email addresses
        }
        catch (Exception ex)
        {
            // Most likely permission denied
        }
    }
    //</contact_select>

    //<contact_all>
    public async IAsyncEnumerable<string> GetContactNames()
    {
        var contacts = await Contacts.GetAllAsync();

        // No contacts
        if (contacts == null)
            yield break;

        foreach (var contact in contacts)
            yield return contact.DisplayName;
    }
    //</contact_all>

    private async void SendEmailButton_Clicked(object sender, EventArgs e)
    {
        //<email_compose>
        if (Email.Default.IsComposeSupported)
        {

            string subject = "Hello friends!";
            string body = "It was great to see you last weekend.";
            string[] recipients = new[] { "john@contoso.com", "jane@contoso.com" };

            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                BodyFormat = EmailBodyFormat.PlainText,
                To = new List<string>(recipients)
            };

            await Email.Default.ComposeAsync(message);
        }
        //</email_compose>
    }

    private async void SendEmailPictureButton_Clicked(object sender, EventArgs e)
    {
        //<email_picture>
        if (Email.Default.IsComposeSupported)
        {

            string subject = "Hello friends!";
            string body = "It was great to see you last weekend. I've attached a photo of our adventures together.";
            string[] recipients = new[] { "john@contoso.com", "jane@contoso.com" };

            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                BodyFormat = EmailBodyFormat.PlainText,
                To = new List<string>(recipients)
            };

            string picturePath = Path.Combine(FileSystem.CacheDirectory, "memories.jpg");

            message.Attachments.Add(new EmailAttachment(picturePath));

            await Email.Default.ComposeAsync(message);
        }
        //</email_picture>
    }

    private async void SendSmsButton_Clicked(object sender, EventArgs e)
    {
        //<sms_send>
        if (Sms.Default.IsComposeSupported)
        {
            string[] recipients = new[] { "000-000-0000" };
            string text = "Hello, I'm interested in buying your vase.";

            var message = new SmsMessage(text, recipients);

            await Sms.Default.ComposeAsync(message);
        }
        //</sms_send>
    }

    private void DialPhoneButton_Clicked(object sender, EventArgs e)
    {
        //<phone_dial>
        if (PhoneDialer.Default.IsSupported)
            PhoneDialer.Default.Open("000-000-0000");
        //</phone_dial>
    }
}