using TMPro;
using UnityEngine;

public class RandomText : MonoBehaviour
{
    // Text component to display the quote
    public TMP_Text quoteText;

    // Array to hold all the quotes
    private string[] quotes = new string[]
    {
        "Every <color=green><b>champion</b></color> was once a <color=green><b>contender</b></color> who refused to give up!",
        "That was <color=green><b>close</b></color>! One more game could be your <color=green><b>win</b></color>!",
        "<color=green><b>Practice</b></color> makes <color=green><b>perfect</b></color>. Ready to try again?",
        "<color=green><b>Great</b></color> effort! Let’s go for a <color=green><b>victory</b></color> next time!",
        "One step closer to <color=green><b>mastering</b></color> the game!",
        "You’ve got the <color=green><b>skills</b></color>—let’s give it another shot!",
        "<color=green><b>Success</b></color> is just around the corner. Keep <color=green><b>playing</b></color>!",
        "You’re <color=green><b>improving</b></color> with every game! Keep going!",
        "It’s not about <color=green><b>winning</b></color>, it’s about getting <color=green><b>better</b></color> each time!",
        "The game’s not <color=green><b>over</b></color> until you say it is. Ready for a <color=green><b>rematch</b></color>?",
        "You’ve got this! Every round brings new <color=green><b>opportunities</b></color>!",
        "<color=green><b>Losing</b></color> is part of the journey to <color=green><b>victory</b></color>. Let’s go again!",
        "<color=green><b>Great</b></color> things come to those who <color=green><b>play</b></color> one more time!",
        "<color=green><b>Failure</b></color> is just practice for <color=green><b>success</b></color>. Give it another go!",
        "Each game is a <color=green><b>lesson</b></color>. Time to show what you’ve <color=green><b>learned</b></color>!"
    };

    // Start is called before the first frame update
    void Start()
    {
        ShowRandomQuote();
    }

    // Function to select and display a random quote
    void ShowRandomQuote()
    {
        // Get a random index
        int randomIndex = Random.Range(0, quotes.Length);

        // Set the random quote in the text component
        quoteText.text = quotes[randomIndex];
    }
}
