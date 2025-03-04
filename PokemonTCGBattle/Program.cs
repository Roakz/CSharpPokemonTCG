using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text.Json.Nodes;
using System.IO;


namespace PokemonTCGBattle
{
    class Program
    {
        public class AnimationHelper
        {
            public static void WriteSlowly(string message, int timeDelay)
            {
                foreach (char c in message)
                {
                    Console.Write(c);
                    Thread.Sleep(timeDelay);
                }
            }
        }

        class GameInterface
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

            public static void PrintMatchUp(Card userCard, Card computerCard)
            {
                Console.WriteLine("Your cards have been dealt. Let the battle begin!!!\n");
                Console.WriteLine("YOUR CURRENT CARD:\n");
                userCard.PrintCard();
                Console.WriteLine("\n");
                Console.WriteLine("!!        !!    !!!!!!!\n" +
                                  " !!      !!     !!\n" +
                                  "  !!    !!      !!!!!!!\n" +
                                  "   !!  !!            !!\n"+
                                  "     !!         !!!!!!!\n\n");
                Console.WriteLine("COMPUTER CURRENT CARD:\n");
                computerCard.PrintCard();
            }

            public static void PrintPokemonList(Card[] userCards)
            {
                Console.WriteLine("Your Pokemon List: ");
                foreach (Card card in userCards)
                {
                    Console.WriteLine($"{Array.IndexOf(userCards, card) + 1}. {card.CardsPokemon.Name}: HP - {card.CardsPokemon.HP}, Status: {card.CardsPokemon.Status}");
                }
            }

            public static void PrintCoinTossMenu()
            {
                Console.WriteLine("Please enter your selection:\n" +
                                  "1. Heads\n" +
                                  "2. Tails\n");
            }

            public static void PrintCointTossAnimation()
            {
                for (int i = 0; i < 2; i++)
                {
                    Console.Write("Flipping Coin");
                    AnimationHelper.WriteSlowly("!!!", 800);
                    Console.Clear();
                }
                Console.Clear();
            }

            public static void PrintCoinTossResult(Boolean userTurn) {
                if (userTurn == true)
                {
                    Console.WriteLine("You won the coin toss!! You will go first.\n");
                }
                else
                {
                    Console.WriteLine("You lost the coin toss. The computer will go first.\n");
                }
            }

            public static void printGameMenu(User user, Computer computer, GameController gameController)
            {
                Console.WriteLine("\n");
                Console.WriteLine("Game Menu:\n" +
                    "1. Print Your Cards\n" +
                    "2. View Pokemon List\n" +
                    "3. View Computer Pokemon List\n" +
                    "4. Switch Pokemon\n" +
                    "5. Choose Attack\n" +
                    "6. Apply Potion\n" +
                    "7. Retreat\n" +
                    "8. Exit Game\n" +
                    "Please enter the number of the option you would like to select: ");
            }

            public static void PrintUserCards(Card[] userCards)
            {
                for (int i = 0; i < 2; i++)
                {
                    Console.Write("Loading your cards ");
                    AnimationHelper.WriteSlowly("...", 800);
                    Console.Clear();
                }
                Console.WriteLine("Your cards are: ");
                foreach (Card card in userCards)
                {
                    card.PrintCard();
                }
            }
        }

        public class GameController
        {

            public Card CurrentUserCard { get; set; }
            public Card CurrentComputerCard { get; set; }
            public Boolean UserTurn { get; set; } = false;
            private User User;
            private Computer Computer;

            public GameController(User user, Computer computer)
            {
                User = user;
                Computer = computer;
            }

            public void InitializeGame(Pokemon[] pokemonArray)
            {
                Pokemon[] userPokemon;
                Pokemon[] computerPokemon;
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

                User.UserCards = userCards;
                Computer.ComputerCards = computerCards;
                CurrentUserCard = User.UserCards[0];
                CurrentComputerCard = Computer.ComputerCards[0];
            }

            public void InitializeCoinToss()
            {
                GameInterface.PrintCoinTossMenu();
                int userSelection = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                GameInterface.PrintCointTossAnimation();
                Boolean isHeads = FlipCoin();
                UserTurn = userSelection == 1 && isHeads == true ? true : false;
                GameInterface.PrintCoinTossResult(UserTurn);
            }

            public void UserMenuSwitch()
            {

                string userChoice = Console.ReadLine();
                Console.Clear();
                switch (userChoice)
                {
                    case "1":
                        GameInterface.PrintUserCards(User.UserCards);
                        GameInterface.printGameMenu(User, Computer, this);
                        break;
                    case "2":
                        GameInterface.PrintPokemonList(User.UserCards);
                        GameInterface.printGameMenu(User, Computer, this);
                        break;
                    case "3":
                        GameInterface.PrintPokemonList(Computer.ComputerCards);
                        GameInterface.printGameMenu(User, Computer, this);
                        break;
                    case "4":
                        GameInterface.PrintPokemonList(User.UserCards);
                        Console.WriteLine("\nPlease enter the number corresponding to the pokemon you want to send out:");
                        int index = Convert.ToInt32(Console.ReadLine());
                        SwitchPokemon(index, User);
                        Console.Clear();
                        GameInterface.PrintMatchUp(CurrentUserCard, CurrentComputerCard);
                        GameInterface.printGameMenu(User, Computer, this);
                        break;
                    case "5":
                        ChooseAttack(User);
                        break;
                    case "6":
                        ApplyPotion(User);
                        break;
                    case "7":
                        Retreat();
                        break;
                    case "8":
                        ExitGame();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter a number between 1 and 7.");
                        GameInterface.printGameMenu(User, Computer, this);
                        break;
                }
            }

            public void Play()
            {
                if(UserTurn == true)
                {
                    GameInterface.printGameMenu(User, Computer, this);
                    UserMenuSwitch();
                }
                else
                {
                    ComputerTakesTurn();
                }
            }

            public void ComputerTakesTurn()
            {
                Console.WriteLine("The computer is taking its turn.");
            }
            public void SwitchPokemon(int index, User user)
            {
                CurrentUserCard = user.UserCards[index - 1];
            }

            public Boolean FlipCoin()
            {
                Random rand = new Random();
                int HeadorTails =  rand.Next(1,3);
                return HeadorTails == 1;
            }
            public void ChooseAttack(User user)
            {
                
            }
            public void ApplyPotion(User user)
            {
                
            }
            public void Retreat()
            {
                
            }
            public void ExitGame()
            {
                Environment.Exit(0);
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
            public string Status { get; set; }

            public Pokemon(string name, int hp, Attack[] attacks, PokemonType pokemonType, PokemonType? weakness, PokemonType? resistance)
            {
                Name = name;
                HP = hp;
                Attacks = attacks;
                PType = pokemonType;
                Weakness = weakness;
                Resistance = resistance;
                Status = "Standing";
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

                AnimationHelper.WriteSlowly(cardDelineator, 20);
                Console.WriteLine($"{CardsPokemon.Name} - HP:{CardsPokemon.HP}");
                AnimationHelper.WriteSlowly(miniDeliniator, 20);
                foreach (Attack attack in CardsPokemon.Attacks){
                    AnimationHelper.WriteSlowly(attack.Name + "\n\n", 20);
                    Console.WriteLine(attack.Description + "\n");
                    Console.WriteLine($"DMG:{attack.Damage}");
                    AnimationHelper.WriteSlowly(miniDeliniator, 20);
                }
                string resistance = CardsPokemon.Resistance == null ? "N/A" : $"{CardsPokemon.Resistance}";
                string weakness = CardsPokemon.Resistance == null ? "N/A" : $"{CardsPokemon.Weakness}";
                Console.WriteLine($"Type: {CardsPokemon.PType}\n" +
                 $"Weakness: {weakness}\n" +
                 $"Resistance: {resistance}");
                AnimationHelper.WriteSlowly(cardDelineator, 20);
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

        public class JsonParser
        {
            private string path;
            private string fileName;
            public JsonParser(string path, string fileName)
            {
                this.path = path;
                this.fileName = fileName;
            }

            public string FileToJsonString() {

                return File.ReadAllText(path + fileName);
            }
        }

        public class JsonToPokemonConverter
        {
            private string jsonString;
            public JsonToPokemonConverter(string jsonString)
            {
               this.jsonString = jsonString;
            }

            public Pokemon[] JsonStringToPokemonArray()
            {
                JsonNode jsonPokemonArrayString = JsonNode.Parse(jsonString);
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
            public Card[] UserCards { get; set; }
            public User()
            {
            }
        }

        public class Computer
        {
            public Card[] ComputerCards { get; set; }
            public Computer()
            {
            }
        }

        static void Main(string[] args)
        {
            const string fileName = @"\PokemonCardData.json";
            const string path = @"C:\Users\rory_\source\repos\PokemonTCGBattle\";

            GameInterface.PrintMenu();

            JsonParser jsonParser = new JsonParser(path, fileName);
            string jsonString = jsonParser.FileToJsonString();
            JsonToPokemonConverter jsonConverter = new JsonToPokemonConverter(jsonString);
            Pokemon[] pokemonArray = jsonConverter.JsonStringToPokemonArray();

            User user = new User();
            Computer computer = new Computer();
            GameController gameController = new GameController(user, computer);

            gameController.InitializeGame(pokemonArray);
            gameController.InitializeCoinToss();
            GameInterface.PrintMatchUp(gameController.CurrentUserCard, gameController.CurrentComputerCard);
            gameController.Play();
            Console.Read();
        }
    }
}
