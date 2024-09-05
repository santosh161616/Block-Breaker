using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCredits : MonoBehaviour
{
    [SerializeField] TMP_Text gameCredit;
    // Start is called before the first frame update
    void Start()
    {
        GameCredit();
    }

    private void GameCredit()
    {
        string credit = "Congratulations! \r\nYou’ve completed all the levels!\r\n\r\nThank you for playing <color=red><b>BRICK BOUNCER</b></color>! Your dedication means a lot, and I hope you enjoyed the experience!\r\n\r\nCredits:\r\nGame Development, Design, Art, Sound & Music: \r\n\n\"<color=red>Santosh Kumar</color>\"\r\n\r\nStay tuned for more games and updates. Don’t forget to share your thoughts and feedback! ";
        gameCredit.text = credit;
    }
}
