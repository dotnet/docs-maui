using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XamlExample;

class ClockViewModel: INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    
    private DateTime _dateTime;
    private Timer _timer;

    public DateTime DateTime
    {
        get => _dateTime;
        set
        {
            if (_dateTime != value)
            {
                _dateTime = value;
                OnPropertyChanged(); // reports this property
            }
        }
    }

    public ClockViewModel()
    {
        this.DateTime = DateTime.Now;

        // Update the DateTime property every second.
        _timer = new Timer(new TimerCallback((s) => this.DateTime = DateTime.Now),
                           null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }

    ~ClockViewModel() =>
        _timer.Dispose();

    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
