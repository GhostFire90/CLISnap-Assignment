using System.Collections.Generic;
using System.Text;


class SnapCLI
{
    static int turn_number=0;
    public static int TurnNumber { 
        get
        {
            return turn_number;
        }
    }
    static void Main(string[] _)
    {
        Console.SetWindowSize((Card.LABEL_WIDTH+2)*12, 11);
        Console.WriteLine("Welcome to SnapCLI");

        List<Card> default_hand = new List<Card> {
            new Card(0, () => 1, "Cpt. America"),
            new Card(0, () => 1, "Cpt. America"),

            new Card(1, () => 1, "Iron Man"),
            new Card(1, () => 1, "Iron Man"),
            
            new Card(1, () => 2, "Dr. Strange"),
            new Card(1, () => 2, "Dr. Strange"),
            
            new Card(3, () => TurnNumber, "Deadpool"),

            new Card(3, () => 2, "Widowmaker", false),

            new Card(4, () => 4, "Black Panther"),
            new Card(5, () => 6, "Black Panther"),

            new Card(6, () => 8, "Hulk", true, () => Console.WriteLine("HULK SMASH!"))
        };

        Player[] players = {new Player(default_hand), new Player(default_hand)}; // used array incase more players are added

        while (turn_number <= 6)
        {
            for(int i = 0; i < players.Length; i++)
            {
                players[i].Move(i);
                Console.ReadLine();
            }
            turn_number++;
        }

    }

    // fisher yates shuffle https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
    public static List<Card> Shuffle(List<Card> cards)
    {
        var rnd = new Random(); // not super secure cause its system random but good enough for this

        for (int n = cards.Count; n > 1; n--)
        {
            int swap_index = rnd.Next(n);
            (cards[n - 1], cards[swap_index]) = (cards[swap_index], cards[n - 1]);
        }
        return cards;
    }
}



class Card
{

    public const int LABEL_WIDTH = 13;

    public delegate int power_function();
    public delegate void play_function();

    readonly power_function power;
    readonly play_function? play;
    int energy;
    string name;
    bool discard;

    public Card(int energy, power_function power, string name, bool discard = true, play_function? play = null)
    {
        this.energy = energy;
        this.power = power;
        this.name = name;
        this.discard = discard;
        this.play = play;
    }
    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"+{"".PadRight(LABEL_WIDTH+2, '-')}+");
        builder.AppendLine($"| {name,LABEL_WIDTH} |");
        builder.AppendLine($"| {' ', LABEL_WIDTH} |");
        builder.AppendLine($"| {$"+{power()}p",LABEL_WIDTH} |");
        builder.AppendLine($"| {' ',LABEL_WIDTH} |");
        builder.AppendLine($"| {$"-{energy}e",LABEL_WIDTH} |");

        var discard_str = discard ? "discard" : "";

        builder.AppendLine($"| {discard_str,LABEL_WIDTH} |");



        builder.Append($"+{"".PadRight(LABEL_WIDTH + 2, '-')}+");
        return builder.ToString();
    }
}

class Player
{
    int power_level;
    List<Card> hand;
    
    public Player(List<Card> hand)
    {
        this.power_level = 0;
        this.hand = SnapCLI.Shuffle(hand);
    }

    public void Move(int id)
    {
        Console.Clear();
        Console.WriteLine($"Player {id+1}'s turn, your Power Level is {power_level}");
        Console.WriteLine($"You have {SnapCLI.TurnNumber} energy");
    }

}