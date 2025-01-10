namespace Shared.Utils.Requests;

public class DateFilterDto
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }

    private DateTime _startDateSettedTime;
    private DateTime _endDateSettedTime;

    // Zaman dilimi olmadan saat bilgisi koruyarak tarih alınıyor
    public DateTime StartDateSettedTime
    {
        get => StartDate.LocalDateTime.Date;  // Zaman dilimini kaldırır
        set => _startDateSettedTime = value;
    }

    public DateTime EndDateSettedTime
    {
        get => EndDate.LocalDateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59);  // Zaman dilimi olmadan son saat
        set => _endDateSettedTime = value;
    }
}