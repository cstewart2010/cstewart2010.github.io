using Cstewart.Portfolio.DataModel;
using Cstewart.Portfolio.DTOs;

namespace Cstewart.Portfolio.Services;

public interface IDamageService
{
    public ICollection<DamageRoll> GetDamageRolls(
        StattedPokemon offensivePokemon,
        StattedPokemon defensivePokemon,
        MoveData move,
        Conditionals conditionals,
        StringPair statusConditions,
        string weather,
        string terrain);
}