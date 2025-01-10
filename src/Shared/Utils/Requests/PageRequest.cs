namespace Shared.Utils.Requests;

public class PageRequest
{
    public int Index { get; set; }
    public int Size { get; set; }
    public bool IsAll { get; set; } = false;
}