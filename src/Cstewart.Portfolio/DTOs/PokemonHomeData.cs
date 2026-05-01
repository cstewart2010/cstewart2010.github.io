using Cstewart.Portfolio.DataModel;

namespace Cstewart.Portfolio.DTOs;

public class PokemonHomeData
{
    public required int DexNo { get; set; }
    public required bool IsShiny { get; set; }
    public required bool IsShinyDefault { get; set; }
    public required ICollection<string> Forms { get; set; }
    public required Genders Genders { get; set; }
}