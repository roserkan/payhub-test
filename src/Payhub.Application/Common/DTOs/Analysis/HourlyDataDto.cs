namespace Payhub.Application.Common.DTOs.Analysis;

public sealed record HourlyDataDto
{
    // Saat bilgisi formatlanmış olarak (örn: "00:00", "01:00")
    public string HourFormatted { get; set; } = string.Empty;

    // O saatteki toplam işlem miktarı (yatırım veya çekim)
    public decimal TotalAmount { get; set; }

    public int Count { get; set; }
}