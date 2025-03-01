using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.IO;
using Xunit;

namespace PokemonTCGBattle
{
    class Program
    {
        public static void WriteSlowly(string message, int timeDelay)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(timeDelay);
            }
        }
        class Menu
        {
            public static void PrintMenu()
            {
                Console.WriteLine("Welcome to Pokemon TCG Console Game!!!\n" +
                    "\nPress any key to begin to play!");
                Console.Read();
                Console.Clear();
                Console.WriteLine("You will be dealt 5 Pokemon cards at random to battle the computer.\n\n" +
                    "Then the battle will begin!!!\n\n" +
                    "Press any key to recieve your cards.");
                Console.Read();
                Console.Clear();
            }

            public static void PrintUserCards(Card[] userCards)
            {
                for (int i = 0; i < 2; i++)
                {
                    Console.Write("Loading your cards ");
                    WriteSlowly("...", 800);
                    Console.Clear();
                }
                Console.WriteLine("Your cards are: ");
                foreach (Card card in userCards)
                {
                    card.PrintCard();
                }
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
            public Pokemon CardsPokemon { get; private set; }

            public Card(Pokemon pokemon)
            {
                CardsPokemon = pokemon;
            }

            public void PrintCard()
            {
                string cardDelineator = "-------------------------------------------\n";
                string miniDeliniator = "-----\n";

                WriteSlowly(cardDelineator, 20);
                Console.WriteLine($"{CardsPokemon.Name} - HP:{CardsPokemon.HP}");
                WriteSlowly(miniDeliniator, 20);
                foreach (Attack attack in CardsPokemon.Attacks){
                    WriteSlowly(attack.Name + "\n\n", 20);
                    Console.WriteLine(attack.Description + "\n");
                    Console.WriteLine($"DMG:{attack.Damage}");
                    WriteSlowly(miniDeliniator, 20);
                }
                string resistance = CardsPokemon.Resistance == null ? "N/A" : $"{CardsPokemon.Resistance}";
                string weakness = CardsPokemon.Resistance == null ? "N/A" : $"{CardsPokemon.Weakness}";
                Console.WriteLine($"Type: {CardsPokemon.PType}\n" +
                 $"Weakness: {weakness}\n" +
                 $"Resistance: {resistance}");
                WriteSlowly(cardDelineator, 20);
            }
        }

        public class Dealer
        {
            public Pokemon[] Deal(Pokemon[] pokemonArray, Random rnd)
            {
                Pokemon[] dealerArray = new Pokemon[5];
                List<int> selectedIntList = new List<int>();

                while (selectedIntList.Count() < 5)
                {
                    int possibleIndex = rnd.Next(pokemonArray.Length);
                    if(selectedIntList.Contains(possibleIndex) == false)
                    {
                        selectedIntList.Add(possibleIndex);
                    }
                }

                foreach (int i in selectedIntList)
                {
                    dealerArray[selectedIntList.IndexOf(i)] = pokemonArray[i];
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

        public class User
        {
            public Card[] UserCards { get; private set; }
            public User(Card[] userCards)
            {
                UserCards = userCards;
            }
        }

        public class Computer
        {
            public Card[] ComputerCards { get; private set; }
            public Computer(Card[] computerCards)
            {
                ComputerCards = computerCards;
            }
        }

        static void Main(string[] args)
        {
            Pokemon[] userPokemon;
            Pokemon[] computerPokemon;
            
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
                        
            Menu.PrintMenu();
            Random rnd = new Random();
            Dealer dealer = new Dealer();
            userPokemon = dealer.Deal(pokemonArray, rnd);
            computerPokemon = dealer.Deal(pokemonArray, rnd);
            Card[] userCards = new Card[5];
            Card[] computerCards = new Card[5];
            for (int i = 0; i < 5; i++)
            {
                userCards[i] = new Card(userPokemon[i]);
                computerCards[i] = new Card(computerPokemon[i]);
            }
            User user = new User(userCards);
            Computer computer = new Computer(computerCards);
            Menu.PrintUserCards(userCards);
            


            //Debugging Code
            /*foreach(Pokemon p in userPokemon)
            {
                Console.WriteLine(p.Name);
            }
            Console.WriteLine("--------------------------------------------");
            foreach(Pokemon p in computerPokemon)
            {
                Console.WriteLine(p.Name);
            }*/
            Console.Read();
        }
    }
}
