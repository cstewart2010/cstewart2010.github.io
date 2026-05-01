namespace JackpotLibrary;

public class JackpotRecord
{
    public DateTime RollDate => DateTime.Parse(Date);
    public int Roll1 { get; set; }
    public int Roll2 { get; set; }
    public int Roll3 { get; set; }
    public int Roll4 { get; set; }
    public int Roll5 { get; set; }
    public int Powerball { get; set; }
    public int PowerPlay { get; set; }
    public string Value { get; set; }
    public bool IsJackpotWon => JackpotWon == "TRUE";
    public string JackpotWon { get; set; }
    public string Date { get; set; }
}