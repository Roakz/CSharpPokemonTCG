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
        public class AnimationService
        {
            public static void WriteSlowly(string message, int timeDelay)
            {
                foreach (char c in message)
                {
                    Console.Write(c);
                    Thread.Sleep(timeDelay);
                }
            }
            public static void Animate(string words, string animationString)
            {
                for (int i = 0; i < 2; i++)
                {
                    Console.Write(words);
                    WriteSlowly(animationString, 800);
                    Console.Clear();
                }
                Console.Clear();
            }
        }
       
        static class InterfacePrinter
        {
            public static void PrintWelcome()
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

            public static void PrintAttacks(Attack[] attacks)
            {
                foreach (Attack attack in attacks)
                {
                    Console.WriteLine($"{Array.IndexOf(attacks, attack) + 1}. {attack.Name}: {attack.Description}, Damage: {attack.Damage}");
                }
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

            public static void printGameMenu(User user, Computer computer, GameFlowController gameController)
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
                    AnimationService.WriteSlowly("...", 800);
                    Console.Clear();
                }
                Console.WriteLine("Your cards are: ");
                foreach (Card card in userCards)
                {
                    card.PrintCard();
                }
            }
        }

        public enum CoinSide
        {
            Heads,
            Tails
        }

        public class CoinTossManager
        {
            private readonly Random _random = new Random();

            public CoinSide FlipCoin()
            {
                return _random.Next(2) == 0 ? CoinSide.Heads : CoinSide.Tails;
            }
        }

        public class BattleUtils
        {
            public static Boolean ApplyProbability()
            {
                int probabilty = 75;
                Random rnd = new Random();
                return rnd.Next(100) < probabilty;
            }
        }
        //
        /*BattleController:
          - Handles the specific rules and logic of the battle(attack selection, damage calculation, applying effects).
          - Keeps the battle rules encapsulated so they can be modified without affecting the game flow logic.*/
        public class BattleController {

            public Boolean UserTurn { get; set; }
            private readonly User _user;
            private readonly Computer _computer;

            public BattleController(User user, Computer computer)
            {
                _user = user;
                UserTurn = false;
                _computer = computer;
            }

            public void SwitchPokemonCard(int index, User user)
            {
                user.CurrentCard = user.PlayerCards[index];
            }

            private void UserTurnSwitch()
            {
                UserTurn = !UserTurn;
            }

            public void TakeComputerTurn()
            {
                Random rnd = new Random();
                int attackIndex = rnd.Next(0, _computer.CurrentCard.CardsPokemon.Attacks.Length);
                Attack(attackIndex);
                
            }

            public void Attack(int attackIndex)
            {
                Boolean attackSucceeds = BattleUtils.ApplyProbability();
                if (attackSucceeds)
                {
                    if (UserTurn)
                    {
                        int damage = _user.CurrentCard.CardsPokemon.Attacks[attackIndex].Damage;
                        _computer.CurrentCard.CardsPokemon.HP -= damage;
                    } else
                    {
                        int damage = _computer.CurrentCard.CardsPokemon.Attacks[attackIndex].Damage;
                        _user.CurrentCard.CardsPokemon.HP -= damage;
                    }

                    UserTurnSwitch();                    
                }
                else
                {
                    Console.WriteLine("The attack failed.");
                    UserTurnSwitch();
                }
            }
        }


        /*GameFlowController:
          - Manages overall game state and user interactions(menus, turn order, coin toss).
          - Orchestrates the progression of the game(starting the game, switching between battle and other phases, handling exits).
          - Acts as a mediator between the UI and the core battle logic.*/
        public class GameFlowController
        {
            
            private readonly User _user;
            private readonly Computer _computer;
            private readonly BattleController _battleController;

            public GameFlowController(User user, Computer computer, BattleController battleController)
            {
                _user = user;
                _computer = computer;
                _battleController = battleController;
            }

            public void PerformCoinToss()
            {
                CoinTossManager coinTossManager = new CoinTossManager();
                InterfacePrinter.PrintCoinTossMenu();
                int userSelection = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                AnimationService.Animate("Fliping Coin", "!!!");
                CoinSide isHeads = coinTossManager.FlipCoin();
                _battleController.UserTurn = userSelection == 1 && isHeads == CoinSide.Heads;
                InterfacePrinter.PrintCoinTossResult(_battleController.UserTurn);
            }

            public void UserMenuSwitch(BattleController battleCotroller)
            {

                string userChoice = Console.ReadLine();
                Console.Clear();
                switch (userChoice)
                {
                    case "1":
                        InterfacePrinter.PrintUserCards(_user.PlayerCards);
                        InterfacePrinter.printGameMenu(_user, _computer, this);
                        break;
                    case "2":
                        InterfacePrinter.PrintPokemonList(_user.PlayerCards);
                        InterfacePrinter.printGameMenu(_user, _computer, this);
                        break;
                    case "3":
                        InterfacePrinter.PrintPokemonList(_computer.PlayerCards);
                        InterfacePrinter.printGameMenu(_user, _computer, this);
                        break;
                    case "4":
                        InterfacePrinter.PrintPokemonList(_user.PlayerCards);
                        Console.WriteLine("\nPlease enter the number corresponding to the pokemon you want to send out:");
                        int index = Convert.ToInt32(Console.ReadLine());
                        battleCotroller.SwitchPokemonCard(index, _user);
                        Console.Clear();
                        InterfacePrinter.PrintMatchUp(_user.CurrentCard, _computer.CurrentCard);
                        InterfacePrinter.printGameMenu(_user, _computer, this);
                        break;
                    case "5":
                        InterfacePrinter.PrintAttacks(_user.CurrentCard.CardsPokemon.Attacks);
                        Console.WriteLine("\nPlease enter the number corresponding to the attack you want to use:");
                        int attackIndex = Convert.ToInt32(Console.ReadLine()) - 1;
                        Console.Clear();
                        battleCotroller.Attack(attackIndex);
                        Play();
                        break;
                    case "6":
                        break;
                    case "7":
                        break;
                    case "8":
                        ExitGame();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter a number between 1 and 7.");
                        InterfacePrinter.printGameMenu(_user, _computer, this);
                        break;
                }
            }

            public void Play()
            {
                InterfacePrinter.PrintMatchUp(_user.CurrentCard, _computer.CurrentCard);
                if (_battleController.UserTurn == true)
                {
                    InterfacePrinter.printGameMenu(_user, _computer, this);
                    UserMenuSwitch(_battleController);
                }
                else
                {
                    AnimationService.Animate("Computer Calculating Move", "...");
                    _battleController.TakeComputerTurn();
                    Play();
                }
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

        public class PokemonTypeHelper
        {
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

        public class Pokemon
        {
            public string Name { get; private set; }
            public int HP { get;  set; }
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

                AnimationService.WriteSlowly(cardDelineator, 20);
                Console.WriteLine($"{CardsPokemon.Name} - HP:{CardsPokemon.HP}");
                AnimationService.WriteSlowly(miniDeliniator, 20);
                foreach (Attack attack in CardsPokemon.Attacks){
                    AnimationService.WriteSlowly(attack.Name + "\n\n", 20);
                    Console.WriteLine(attack.Description + "\n");
                    Console.WriteLine($"DMG:{attack.Damage}");
                    AnimationService.WriteSlowly(miniDeliniator, 20);
                }
                string resistance = CardsPokemon.Resistance == null ? "N/A" : $"{CardsPokemon.Resistance}";
                string weakness = CardsPokemon.Resistance == null ? "N/A" : $"{CardsPokemon.Weakness}";
                Console.WriteLine($"Type: {CardsPokemon.PType}\n" +
                 $"Weakness: {weakness}\n" +
                 $"Resistance: {resistance}");
                AnimationService.WriteSlowly(cardDelineator, 20);
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
                    PokemonType PType = (PokemonType) PokemonTypeHelper.ReturnPokemonType(jNode["type"].GetValue<string>());
                    PokemonType? Weakness = PokemonTypeHelper.CalculatePokemonWeakness(PType);
                    PokemonType? Resistance = PokemonTypeHelper.CalculatePokemonResistance(PType);

                    pokemonArray[count] = new Pokemon(Name, HP, Attacks, PType, Weakness, Resistance);

                    count++;
                }
                return pokemonArray;
            }
        }

        interface IPLayer
        {
            Card[] PlayerCards { get; set; }
            Card CurrentCard { get; set; }
        }

        public class User:IPLayer
        {
            public Card[] PlayerCards { get; set; }
            public Card CurrentCard { get; set; }
            public User()
            {
            }
        }

        public class Computer: IPLayer
        {
            public Card[] PlayerCards { get; set; }
            public Card CurrentCard { get; set; }
            public Computer()
            {
            }
        }

        public class GameInitializer
        {
            public static void InitializeGame(Pokemon[] pokemonArray, User user, Computer computer, GameFlowController gameController, Dealer dealer, Random rnd)
            {
                Pokemon[] userPokemon = dealer.Deal(pokemonArray, rnd);
                Pokemon[] computerPokemon = dealer.Deal(pokemonArray, rnd);
                CreateCards(userPokemon, computerPokemon, user, computer, gameController);
            }

            private static void CreateCards(Pokemon[] userPokemon, Pokemon[] computerPokemon, User user, Computer computer, GameFlowController gameController)
            {
                Card[] userCards = userPokemon.Select(p => new Card(p)).ToArray();
                Card[] computerCards = computerPokemon.Select(p => new Card(p)).ToArray();

                user.PlayerCards = userCards;
                computer.PlayerCards = computerCards;
                user.CurrentCard = user.PlayerCards[0];
                computer.CurrentCard = computer.PlayerCards[0];
            }
        }

        static void Main(string[] args)
        {
            const string fileName = @"\PokemonCardData.json";
            const string path = @"C:\Users\rory_\source\repos\PokemonTCGBattle\";

            JsonParser jsonParser = new JsonParser(path, fileName);
            string jsonString = jsonParser.FileToJsonString();
            JsonToPokemonConverter jsonConverter = new JsonToPokemonConverter(jsonString);
            Pokemon[] pokemonArray = jsonConverter.JsonStringToPokemonArray();

            User user = new User();
            Computer computer = new Computer();
            BattleController battleController = new BattleController(user, computer);
            GameFlowController gameController = new GameFlowController(user, computer, battleController);
            Random rnd = new Random();
            Dealer dealer = new Dealer();
            GameInitializer.InitializeGame(pokemonArray, user, computer, gameController, dealer, rnd);
            InterfacePrinter.PrintWelcome();
            gameController.PerformCoinToss();
            gameController.Play();
            Console.Read();
        }
    }
}