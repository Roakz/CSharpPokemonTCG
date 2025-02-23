using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.IO;

namespace PokemonTCGBattle
{
    class Program
    {
        class Menu
        {
            public static void PrintMenu()
            {
                Console.WriteLine("Welcome to Pokemon TCG Console Game!!!\n" +
                    "Press any key to begin to play!");
                Console.Read();
                Console.Clear();
                Console.WriteLine("You will be dealt 5 5andom pokemon cards. The computer will also be dealt 5 random cards.\n" +
                    "Then the battle will begin!!!\n" +
                    "Press any key to recieve your cards.");
                Console.Read();
            }
        }

        public enum PokemonType
        {
            Electric,
            Water,
            Fire,
            Punch,
            Psychic,
            Grass,
            Dragon,
            Metal,
            Normal,
            Dark
        }

        class Pokemon
        {
            public string Name { get; set; }
            public string HP { get; set; }
            public object[] Attacks { get; set; }
            public PokemonType PType { get; set; }

            public static void AssignJsonLoadedTypes(Pokemon[] pokemonArray, JsonConverter jsonConverter)
            {
                foreach (Pokemon p in pokemonArray)
                {
                    string pokemonTypeString = jsonConverter.ExtractTypeString();
                    PokemonType? pokemonType = p.ReturnPokemonType(pokemonTypeString);
                    p.PType = (PokemonType)pokemonType;
                }
            }

            public PokemonType? ReturnPokemonType(string pokemonTypeString)
            {
                switch (Enum.Parse(typeof(PokemonType), pokemonTypeString))
                {
                    case PokemonType.Dark:
                        return PokemonType.Dark;
                    case PokemonType.Dragon:
                        return PokemonType.Dragon;
                    case PokemonType.Electric:
                        return PokemonType.Electric;
                    case PokemonType.Fire:
                        return PokemonType.Fire;
                    case PokemonType.Grass:
                        return PokemonType.Grass;
                    case PokemonType.Metal:
                        return PokemonType.Metal;
                    case PokemonType.Normal:
                        return PokemonType.Normal;
                    case PokemonType.Psychic:
                        return PokemonType.Psychic;
                    case PokemonType.Punch:
                        return PokemonType.Punch;
                    case PokemonType.Water:
                        return PokemonType.Water;
                }
                return null;
            }
        }

        class Card {
            public string Name { get; private set; }
            public int HP { get; private set; }
            public object[] Attacks {get; private set;}
            public PokemonType Type { get; private set; }
            public string Weakness { get; private set; }
            public string Resistance { get; private set; }

            public Card(string cardName, int PokemonHp, object[] pokemonAttacks, PokemonType Type)
            {}

            public static List<Card> GenerateCards(Pokemon[] pokemonArray)
            {
                List<Card> cardList = new List<Card>();
                foreach (Pokemon p in pokemonArray)
                {
                    Card card = new Card(p.Name, int.Parse(p.HP), p.Attacks, p.PType);
                    cardList.Add(card);
                    //Console.WriteLine($"{card.Name}, {card.Type}, {card.HP}, {card.Attacks}");
                }
                return cardList;
            }
        }

        class Dealer
        {
            private List<Card> cardList;

            public Dealer(List<Card> cards)
            {
                cardList = cards;
            }

            public Card[] Deal()
            {
                int randInt;
                Card[] dealerArray = new Card[5];
                //Randomize numbers between 0 and list length. Use that index to select a pokemon card and add it to the dealers array.
                // Test me!!
                Random rnd = new Random();
                for(int i = 0; i < 5; i++){
                    randInt = rnd.Next(0, cardList.Count - 1);
                    Console.WriteLine(randInt);
                }
                return dealerArray;
            }
        }

        class JsonConverter
        {
            private const string fileName = @"\PokemonCardData.json";
            public string JsonString { get; private set; }

            public JsonConverter()
            {
                JsonString = File.ReadAllText(Environment.CurrentDirectory + fileName);
            }

            public Pokemon[] JsonToPokemonArray()
            {
                Pokemon[] pokemonArray = JsonSerializer.Deserialize<Pokemon[]>(JsonString);
                return pokemonArray;
            }

            public string ExtractTypeString()
            {
                JsonNode pokemonString = JsonNode.Parse(JsonString);
                return pokemonString[0]["Type"].GetValue<string>();
            }
        }

        static void Main(string[] args)
        {

            Card[] userCards;
            Card[] computerCards;
            List<Card> cardList;
            JsonConverter jsonConverter = new JsonConverter();

            Pokemon[] pokemonArray = jsonConverter.JsonToPokemonArray();
            Pokemon.AssignJsonLoadedTypes(pokemonArray, jsonConverter);
            cardList = Card.GenerateCards(pokemonArray);

            //Validate pokemon contains all fields and properties after Json has been loaded, Add/Calculate remaining fields
            //Before generating cards.

            Menu.PrintMenu();

            Dealer dealer = new Dealer(cardList);

            //Working on deal method
            userCards = dealer.Deal();
            computerCards = dealer.Deal();
            Console.Read();
        }
    }
}
