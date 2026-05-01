namespace Cstewart.Portfolio.DataModel;

public class ShinyPokemon
{
    public required int DexNo { get; set; }
    public required string Name { get; set; }
    public required string DefaultImageUrl { get; set; }
    public required ICollection<ShinyForm> Forms { get; set; }
}