using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCredits : MonoBehaviour
{
    [SerializeField] TMP_Text gameCredit;
    [SerializeField] private Button playButton;

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
        playButton.onClick.AddListener(LoadGame);
        ShowRandomQuote();
    }

    private void GameCredit()
    {
        string credit = "Congratulations! \r\nYou’ve completed all the levels!\r\n\r\nThank you for playing <color=red><b>BRICK BOUNCER</b></color>! Your dedication means a lot, and I hope you enjoyed the experience!\r\n\r\nCredits:\r\nGame Development, Design, Art, Sound & Music: \r\n\n\"<color=red>Santosh Kumar</color>\"\r\n\r\nStay tuned for more games and updates. Don’t forget to share your thoughts and feedback! ";
        gameCredit.text = credit;
    }

    void ShowRandomQuote()
    {
        // Get a random index
        int randomIndex = Random.Range(0, quotes.Length);

        // Set the random quote in the text component
        gameCredit.text = quotes[randomIndex];
    }

    public void LoadGame()
    {
        string sceneToLoad = StaticUrlScript.Dashboard;
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
