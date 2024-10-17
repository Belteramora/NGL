using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PBlockSummary : MonoBehaviour
{
    public TMP_Text blockLetterLabel;
    public TMP_Text gainLabel;

    public void Setup(GameParameter<string> blockLetter, GameParameter<BigDouble> gain)
    {
        blockLetter.SubscribeToModified(x => blockLetterLabel.text = x);
        gain.SubscribeToModified(x => gainLabel.text = "x" + x.ToString("G3"));
    }
}
