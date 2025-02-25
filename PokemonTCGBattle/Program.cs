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

        public class Attack
        {
            public string Description { get; set; }
            public int Damage { get; set; }
            public string Name { get; set; }

            public Attack(string desciption, int damage, string name)
            {
                Description = desciption;
                Damage = damage;
                Name = name;
            }

        }

        public class Pokemon
        {
            public string Name { get; private set; }
            public int HP { get;  private set; }
            public Attack[] Attacks { get; private set; }
            public PokemonType PType { get; private set; }
            public PokemonType? Weakness { get; private set; }
            public PokemonType? Resistance { get; private set; }

            public Pokemon(string name, int hp, Attack[] attacks, PokemonType pokemonType, PokemonType? weakness, PokemonType? resistance)
            {
                Name = name;
                HP = hp;
                Attacks = attacks;
                PType = pokemonType;
                Weakness = weakness;
                Resistance = resistance;
            }


            public override string ToString()
            {
                return $"Name: {Name}\n" +
                    $"HP: {HP}\n" +
                    $"Attack[0]: {Attacks[0]}\n" +
                    $"Attack[1]: Name - {Attacks[0].Name}, Description - {Attacks[0].Description}, Damage - {Attacks[0].Damage}\n" +
                    $"PType: Name - {Attacks[1].Name}, Description - {Attacks[1].Description}, Damage - {Attacks[1].Damage}\n" +
                    $"Weakness?: {Weakness}\n" +
                    $"Resistance?: {Resistance}";
            }

            public static PokemonType? CalculatePokemonWeakness(PokemonType pType)
            {
                switch (pType)
                {
                    case PokemonType.Dark:
                        return PokemonType.Grass;
                    case PokemonType.Dragon:
                        return null;
                    case PokemonType.Electric:
                        return PokemonType.Punch;
                    case PokemonType.Fire:
                        return PokemonType.Water;
                    case PokemonType.Grass:
                        return PokemonType.Fire;
                    case PokemonType.Metal:
                        return PokemonType.Fire;
                    case PokemonType.Normal:
                        return PokemonType.Punch;
                    case PokemonType.Psychic:
                        return PokemonType.Dark;
                    case PokemonType.Punch:
                        return PokemonType.Psychic;
                    case PokemonType.Water:
                        return PokemonType.Electric;
                }

                return null;
            }
            public static PokemonType? CalculatePokemonResistance(PokemonType pType)
            {
                switch (pType)
                {
                    case PokemonType.Dark:
                        return null;
                    case PokemonType.Dragon:
                        return null;
                    case PokemonType.Electric:
                        return null;
                    case PokemonType.Fire:
                        return null;
                    case PokemonType.Grass:
                        return null;
                    case PokemonType.Metal:
                        return PokemonType.Grass;
                    case PokemonType.Normal:
                        return null;
                    case PokemonType.Psychic:
                        return PokemonType.Punch;
                    case PokemonType.Punch:
                        return PokemonType.Normal;
                    case PokemonType.Water:
                        return null;
                }

                return null;
            }

            public static PokemonType? ReturnPokemonType(string pokemonTypeString)
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

        public class Card {
            public string Name { get; private set; }
            public int HP { get; private set; }
            public Attack[] Attacks {get; private set;}
            public PokemonType Type { get; private set; }
            public string Weakness { get; private set; }
            public string Resistance { get; private set; }

            public Card(string cardName, int PokemonHp, object[] pokemonAttacks, PokemonType Type)
            {

            }

            public static List<Card> GenerateCards(Pokemon[] pokemonArray)
            {
                List<Card> cardList = new List<Card>();
               foreach (Pokemon p in pokemonArray)
                {
                  Card card = new Card(p.Name, p.HP, p.Attacks, p.PType);
                  cardList.Add(card);
                }
               return cardList;
            }
        }

        public class Dealer
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

        public class JsonConverter
        {
            
            public string JsonString { get; private set; }

            public JsonConverter(string path, string fileName)
            {
                JsonString = File.ReadAllText(path + fileName);
            }

            public Pokemon[] JsonToPokemonArray()
            {
                JsonNode jsonPokemonArrayString = JsonNode.Parse(JsonString);
                Pokemon[] pokemonArray = new Pokemon[jsonPokemonArrayString.AsArray().Count()];
                int count = 0;
                foreach(JsonNode jNode in jsonPokemonArrayString.AsArray())
                {
                    string Name = jNode["name"].GetValue<string>();
                    int HP = Int32.Parse(jNode["hp"].GetValue<string>());
                    JsonNode attackNode= JsonNode.Parse(jNode["attacks"].ToJsonString());
                    Attack[] Attacks = new Attack[attackNode.AsArray().Count()];
                    int counter2 = 0;
                    foreach (JsonNode attack in attackNode.AsArray())
                    {
                   
                       Attacks[counter2] = new Attack(attack["Description"].GetValue<string>(), Int32.Parse(attack["Damage"].GetValue<string>()), attack["Name"].GetValue<string>());
                       counter2++;
                    }
                    PokemonType PType = (PokemonType) Pokemon.ReturnPokemonType(jNode["type"].GetValue<string>());
                    PokemonType? Weakness = Pokemon.CalculatePokemonWeakness(PType);
                    PokemonType? Resistance = Pokemon.CalculatePokemonResistance(PType);

                    pokemonArray[count] = new Pokemon(Name, HP, Attacks, PType, Weakness, Resistance);

                    count++;
                }
              
                return pokemonArray;
            }
        }

        static void Main(string[] args)
        {

            Card[] userCards;
            Card[] computerCards;
            List<Card> cardList;
            const string fileName = @"\PokemonCardData.json";
            const string path = @"C:\Users\rory_\source\repos\PokemonTCGBattle\";
            JsonConverter jsonConverter = new JsonConverter(path, fileName);

            Pokemon[] pokemonArray = jsonConverter.JsonToPokemonArray();

            //Debugging Code
            /*foreach(Pokemon p in pokemonArray)
            {
               Console.WriteLine(p.ToString());
               Console.WriteLine();
            }*/
            //Console.Read();

            cardList = Card.GenerateCards(pokemonArray);
            Menu.PrintMenu();
            Dealer dealer = new Dealer(cardList);

            //Working on deal method
            userCards = dealer.Deal();
            computerCards = dealer.Deal();
            Console.Read();
        }
    }
}
