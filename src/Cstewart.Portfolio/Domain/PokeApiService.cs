using Cstewart.Portfolio.Constants;
using Cstewart.Portfolio.DataModel;
using Cstewart.Portfolio.DTOs;
using Cstewart.Portfolio.Extensions;
using Cstewart.Portfolio.Services;
using PokeApiNet;

namespace Cstewart.Portfolio.Domain;

public class PokeApiService : IPokeService
{
    private readonly PokeApiClient _client;
    private ShinyPokemon[] _currentShinyPokemon = [];

    public PokeApiService()
    {
        _client = new PokeApiClient();
    }

    public async Task<List<RawPokemon>> GetPokemonAsync(string name)
    {
        var pokemon = await _client.GetResourceAsync<Pokemon>(name);
        var species = await _client.GetResourceAsync(pokemon.Species);
        List<Pokemon> collection = [];
        foreach (var item in species.Varieties)
        {
            collection.Add(await _client.GetResourceAsync(item.Pokemon));
        }

        return [.. collection.Select(x => new RawPokemon
        {
            SpeciesName = x.Name,
            Stats = new BaseStats
            {
                HP = GetBaseStat(x, Stats.HP),
                Attack = GetBaseStat(x, Stats.Attack),
                Defense = GetBaseStat(x, Stats.Defense),
                SpecialAttack = GetBaseStat(x, Stats.SpecialAttack),
                SpecialDefense = GetBaseStat(x, Stats.SpecialDefense),
                Speed = GetBaseStat(x, Stats.Speed),
            },
            Types = [.. x.Types.Select(y => y.Type.Name)],
            Weight = x.Weight,
            Abilities = [.. x.Abilities.Select(y => new AbilityData
            {
                Name = y.Ability.Name,
                IsHidden = y.IsHidden,
            })],
        })];
    }

    public async Task<ShinyPokemon[]> GetPokemonAsync(ICollection<PokemonHomeData> data)
    {
        if (_currentShinyPokemon.Length == 0)
        {
            _currentShinyPokemon = await Task.WhenAll(data.Select(GetCurrentShinies));
        }
        
        return _currentShinyPokemon;
    }

    public async Task<MoveData?> GetMoveAsync(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        var move = await _client.GetResourceAsync<Move>(name);
        return new MoveData
        {
            Name = move.Name,
            BasePower = move.Power,
            DamageClass = move.DamageClass.Name,
            Target = move.Target.Name,
            Type = move.Type.Name,
            FlavorText = move.FlavorTextEntries.FirstOrDefault(x => x.Language.Name == "en" && x.VersionGroup.Name == "scarlet-violet")?.FlavorText ?? "Not present in Scarlet/Violet",
            Accuracy = move.Accuracy
        };
    }

    public async Task<NatureData> GetNatureAsync(int index)
    {
        var natures = await _client.GetNamedResourcePageAsync<Nature>(30, 0);
        var nature = await _client.GetResourceAsync(natures.Results[index]);

        return new NatureData
        {
            Name = nature.Name,
            IncreasedStat = nature.IncreasedStat?.Name,
            DecreasedStat = nature?.DecreasedStat?.Name
        };
    }

    public async Task<ICollection<string>> GetPokedexAsync()
    {
        var list = await _client.GetNamedResourcePageAsync<PokemonSpecies>(2000, 0);
        return [.. list.Results.Select(x => x.Name.ToCapitalized())];
    }

    public async Task<ICollection<string>> GetMovesAsync()
    {
        var list = await _client.GetNamedResourcePageAsync<Move>(1000, 0);
        return [.. list.Results.Select(x => x.Name.ToCapitalized())];
    }

    public async Task<ICollection<string>> GetItemsAsync()
    {
        var list = await _client.GetNamedResourcePageAsync<Item>(3000, 0);
        return [.. list.Results.Select(x => x.Name.ToCapitalized())];
    }

    public async Task<List<string>> GetNaturesAsync()
    {
        var natures = await _client.GetNamedResourcePageAsync<Nature>(30, 0);
        return await Task.Run<List<string>>(() =>
        {
            return [.. natures.Results.Select(x => x.Name.ToCapitalized())];
        });
    }

    private static int GetBaseStat(Pokemon pokemon, string statName)
    {
        return pokemon.Stats.First(x => x.Stat.Name == statName).BaseStat;
    }

    private async Task<ShinyPokemon> GetCurrentShinies(PokemonHomeData data)
    {
        var pokemon = await _client.GetResourceAsync<Pokemon>(data.DexNo);
        var forms = new List<ShinyForm>();
        if (data.IsShinyDefault)
        {
            if (data.Genders.HasFlag(Genders.Male))
            {
                forms.Add(new ShinyForm
                {
                    FormName = "",
                    FormUrl = pokemon.Sprites.Other.Showdown.FrontShiny
                });
            }
            if (data.Genders.HasFlag(Genders.Female))
            {
                forms.Add(new ShinyForm
                {
                    FormName = "Female",
                    FormUrl = pokemon.Sprites.Other.Showdown.FrontShinyFemale
                });
            }
        }

        foreach (var item in data.Forms)
        {
            var form = await _client.GetResourceAsync<PokemonForm>($"{pokemon.Species.Name}-{item}");
            forms.Add(new ShinyForm
            {
                FormName = form.FormName.ToCapitalized(),
                FormUrl = form.Sprites.FrontShiny
            });
        }

        return new ShinyPokemon
        {
            Name = pokemon.Species.Name.ToCapitalized(),
            DexNo = data.DexNo,
            DefaultImageUrl = pokemon.Sprites.FrontDefault,
            Forms = forms,
        };
    }
}