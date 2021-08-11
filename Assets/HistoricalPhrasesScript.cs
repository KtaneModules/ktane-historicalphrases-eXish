using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using System.Text.RegularExpressions;
using System;

public class HistoricalPhrasesScript : MonoBehaviour {

    public KMAudio audio;
    public KMBombInfo bomb;
    public KMSelectable[] buttons;
    public TextMesh[] btnLabels;
    public TextMesh quoteDisp;

    private string[] quoteList = { "\"I hope, or I could\nnot live.\"\n- H. G. Wells", "\"Few people realise the\nimmensity of vacancy\nin which the dust of the\nmaterial universe swims.\"\n- H. G. Wells", "\"Don\'t panic.\"\n- Douglas Adams", "\"Anyone who is\ncapable of getting\nthemselves made President\nshould on no account be\nallowed to do the job.\"\n- Douglas Adams", "\"Let us prepare\nto grapple with the\nineffable itself, and see\nif we may not eff\nit after all.\"\n- Douglas Adams", "\"No employee makes the\nsame mistake twice. He\nis fired the first time.\"\n- Isaac Asimov", "\"The miserable have no\nother medicine // But\nonly hope.\"\n- William Shakespeare", "\"Many wearing rapiers\nare afraid of goose\nquills.\"\n- William Shakespeare", "\"O! It is excellent to\nhave a giant\'s strength,\nbut it is tyrannous to\nuse it like a giant.\"\n- William Shakespeare", "\"Cowards die many times\nbefore their deaths.\"\n- William Shakespeare", "\"And a thousand\nthousand slimy things //\nLived on; and so did I.\"\n- Samuel Taylor Coleridge", "\"\'I¸ll be judge, I¸ll be jury,\'\nsaid cunning old Fury;\n\'I¸ll try the whole cause,\nand condemn you to death.\'\"\n- Lewis Carroll", "\"Because I could not\nstop for Death // He\nkindly stopped for me.\"\n- Emily Dickinson", "\"If you hide your\nignorance, no one will\nhit you and you\'ll never\nlearn.\"\n- Ray Bradbury", "\"We did everything\nadults would do.\nWhat went wrong?\"\n- William Golding", "\"I call to mind flatness\nand dampness; and then\nall is madness.\"\n- Edgar Allan Poe", "\"I am a brain, Watson.\nThe rest of me is a mere\nappendix.\"\n- Sir Arthur Conan\nDoyle", "\"A man is not a bird,\nto come and go with the\nspringtime.\"\n- Arthur Miller", "\"A small man can be just\nas exhausted as a great\nman.\"\n- Arthur Miller", "\"Cause sometimes it\'s\nhard to let the future\nbegin!\"\n- Lorraine Hansberry", "\"Ignorance is the\nparent of fear.\"\n- Herman Melville", "\"If you want to keep a\nsecret, you must also\nhide it from yourself.\"\n- George Orwell", "\"Weak or strong,\nclever or simple, we\nare all brothers.\"\n- George Orwell", "\"A hunted man sometimes\nwearies of distrust and\nlongs for friendship.\"\n- J. R. R. Tolkien", "\"I was benevolent and good;\nmisery made me a fiend.\nMake me happy, and I\nshall again be virtuous.\"\n- Mary Shelley" };
    private string[] quoteListLog = { "“I hope, or I could not live.” ― H. G. Wells", "“Few people realise the immensity of vacancy in which the dust of the material universe swims.” ― H. G. Wells", "“Don’t panic.” ― Douglas Adams", "“Anyone who is capable of getting themselves made President should on no account be allowed to do the job.” ― Douglas Adams", "“Let us prepare to grapple with the ineffable itself, and see if we may not eff it after all.” ― Douglas Adams", "“No employee makes the same mistake twice. He is fired the first time.” ― Isaac Asimov", "“The miserable have no other medicine // But only hope.” ― William Shakespeare", "“Many wearing rapiers are afraid of goose quills.” ― William Shakespeare", "“O! It is excellent to have a giant’s strength, but it is tyrannous to use it like a giant.” ― William Shakespeare", "“Cowards die many times before their deaths.” ― William Shakespeare", "“And a thousand thousand slimy things // Lived on; and so did I.” ― Samuel Taylor Coleridge", "“‘I’ll be judge, I’ll be jury,’ said cunning old Fury; ‘I’ll try the whole cause, and condemn you to death.’” ― Lewis Carroll", "“Because I could not stop for Death // He kindly stopped for me.” ― Emily Dickinson", "“If you hide your ignorance, no one will hit you and you’ll never learn.” ― Ray Bradbury", "“We did everything adults would do. What went wrong?” ― William Golding", "“I call to mind flatness and dampness; and then all is madness.” ― Edgar Allan Poe", "“I am a brain, Watson. The rest of me is a mere appendix.” ― Sir Arthur Conan Doyle", "“A man is not a bird, to come and go with the springtime.” ― Arthur Miller", "“A small man can be just as exhausted as a great man.” ― Arthur Miller", "“Cause sometimes it’s hard to let the future begin!” ― Lorraine Hansberry", "“Ignorance is the parent of fear.” ― Herman Melville", "“If you want to keep a secret, you must also hide it from yourself.” ― George Orwell", "“Weak or strong, clever or simple, we are all brothers.” ― George Orwell", "“A hunted man sometimes wearies of distrust and longs for friendship.” ― J. R. R. Tolkien", "“I was benevolent and good; misery made me a fiend. Make me happy, and I shall again be virtuous.” ― Mary Shelley" };
    private string[] btnPosNames = { "first", "second", "third", "fourth", "fifth", "sixth" };
    private List<string> btnLabelList = new List<string> { "Thoughtful", "Insightful", "Perceptive", "Contrived", "Enlightened", "Pioneering" };
    private int usedQuote = -1;
    private int correctBtn = -1;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        moduleSolved = false;
        foreach(KMSelectable obj in buttons){
            KMSelectable pressed = obj;
            pressed.OnInteract += delegate () { PressButton(pressed); return false; };
        }
    }

    void Start () {
        btnLabelList = btnLabelList.Shuffle();
        for (int i = 0; i < 6; i++)
            btnLabels[i].text = btnLabelList[i];
        usedQuote = UnityEngine.Random.Range(0, quoteList.Length);
        quoteDisp.text = quoteList[usedQuote];
        if (usedQuote == 0 || usedQuote == 2 || usedQuote == 14 || usedQuote == 20 || usedQuote == 22)
            quoteDisp.gameObject.transform.localScale = new Vector3(0.0004f, 0.0004f, 1f);
        else if (usedQuote == 24)
            quoteDisp.gameObject.transform.localScale = new Vector3(0.00029f, 0.00029f, 1f);
        Debug.LogFormat("[Historical Phrases #{0}] Displayed Quote: {1}", moduleId, quoteListLog[usedQuote]);
        GetCorrectButton();
    }

    void PressButton(KMSelectable pressed)
    {
        if (moduleSolved != true)
        {
            pressed.AddInteractionPunch();
            audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, pressed.transform);
            if (Array.IndexOf(buttons, pressed) == correctBtn)
            {
                moduleSolved = true;
                GetComponent<KMBombModule>().HandlePass();
                Debug.LogFormat("[Historical Phrases #{0}] The {1} button was pressed, which is correct. Module solved!", moduleId, btnPosNames[Array.IndexOf(buttons, pressed)]);
            }
            else
            {
                GetComponent<KMBombModule>().HandleStrike();
                Debug.LogFormat("[Historical Phrases #{0}] The {1} button was pressed, which is incorrect. Strike!", moduleId, btnPosNames[Array.IndexOf(buttons, pressed)]);
            }
        }
    }

    void GetCorrectButton()
    {
        string quote = quoteListLog[usedQuote].Substring(0, quoteListLog[usedQuote].IndexOf('”'));
        string author = quoteListLog[usedQuote].Substring(quoteListLog[usedQuote].IndexOf('―')).ToUpper();
        int wordCount = quote.Split(' ').Length;
        int eCount = quote.Count(x => x.EqualsAny('E', 'e'));
        int rCount = quote.Count(x => x.EqualsAny('R', 'r'));
        int sCount = quote.Count(x => x.EqualsAny('S', 's'));
        int batCount = bomb.GetBatteryCount();
        bool vowelInSerial = bomb.GetSerialNumberLetters().Count(x => x.EqualsAny('A', 'a', 'E', 'e', 'I', 'i', 'O', 'o', 'U', 'u')) > 0;
        bool carIndPresent = bomb.IsIndicatorOn(Indicator.CAR);
        bool containsComma = quote.Contains(',');
        bool authorDouble = false;
        char prevChar = ' ';
        for (int i = 0; i < author.Length; i++)
        {
            if (prevChar.Equals(author[i]))
            {
                authorDouble = true;
                break;
            }
            prevChar = author[i];
        }
        if (eCount > 7)
        {
            if (batCount > 2)
            {
                if (wordCount > 8 && wordCount < 15)
                {
                    if (sCount < 4)
                    {
                        correctBtn = 0;
                    }
                    else
                    {
                        correctBtn = 3;
                    }
                }
                else if (rCount % 2 == 1)
                {
                    if (authorDouble)
                    {
                        correctBtn = 1;
                    }
                    else
                    {
                        correctBtn = 2;
                    }
                }
                else if (sCount < 4)
                {
                    correctBtn = 2;
                }
                else
                {
                    correctBtn = 1;
                }
            }
            else if (rCount % 2 == 1)
            {
                if (containsComma)
                {
                    correctBtn = 4;
                }
                else
                {
                    correctBtn = 1;
                }
            }
            else if (wordCount < 15)
            {
                correctBtn = 0;
            }
            else if (carIndPresent)
            {
                correctBtn = 1;
            }
            else
            {
                correctBtn = 5;
            }
        }
        else if (authorDouble)
        {
            if (containsComma)
            {
                if (carIndPresent)
                {
                    if (rCount % 2 == 1)
                    {
                        if (wordCount > 8 && wordCount < 15)
                        {
                            correctBtn = 4;
                        }
                        else
                        {
                            correctBtn = 2;
                        }
                    }
                    else if (wordCount > 8 && wordCount < 15)
                    {
                        if (sCount < 4)
                        {
                            correctBtn = 5;
                        }
                        else
                        {
                            correctBtn = 1;
                        }
                    }
                    else
                    {
                        correctBtn = 2;
                    }
                }
                else if (wordCount < 10)
                {
                    correctBtn = 0;
                }
                else if (sCount < 4)
                {
                    correctBtn = 4;
                }
                else
                {
                    correctBtn = 2;
                }
            }
            else if (vowelInSerial)
            {
                if (rCount % 2 == 1)
                {
                    if (wordCount > 8 && wordCount < 15)
                    {
                        correctBtn = 3;
                    }
                    else
                    {
                        correctBtn = 4;
                    }
                }
                else if (sCount < 4)
                {
                    correctBtn = 1;
                }
                else
                {
                    correctBtn = 3;
                }
            }
            else if (wordCount > 8 && wordCount < 15)
            {
                if (sCount < 4)
                {
                    correctBtn = 0;
                }
                else if (rCount % 2 == 1)
                {
                    correctBtn = 2;
                }
                else if (batCount > 2)
                {
                    correctBtn = 5;
                }
                else
                {
                    correctBtn = 3;
                }
            }
            else if (eCount > 5)
            {
                correctBtn = 0;
            }
            else
            {
                correctBtn = 3;
            }
        }
        else if (vowelInSerial)
        {
            if (wordCount > 8 && wordCount < 15)
            {
                if (rCount % 2 == 1)
                {
                    if (carIndPresent)
                    {
                        correctBtn = 0;
                    }
                    else
                    {
                        correctBtn = 4;
                    }
                }
                else if (containsComma)
                {
                    correctBtn = 3;
                }
                else
                {
                    correctBtn = 1;
                }
            }
            else
            {
                correctBtn = 5;
            }
        }
        else if (containsComma)
        {
            if (batCount > 2)
            {
                correctBtn = 0;
            }
            else
            {
                correctBtn = 5;
            }
        }
        else if (sCount < 4)
        {
            if (wordCount < 10)
            {
                correctBtn = 4;
            }
            else
            {
                correctBtn = 3;
            }
        }
        else if (eCount > 5)
        {
            correctBtn = 5;
        }
        else
        {
            correctBtn = 2;
        }
        Debug.LogFormat("[Historical Phrases #{0}] Correct Action: Press the {1} button", moduleId, btnPosNames[correctBtn]);
    }

    //twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} press <#> [Presses the specified button by position, with 1-6 being the positions from top to bottom]";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] parameters = command.Split(' ');
        if (Regex.IsMatch(parameters[0], @"^\s*press\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (parameters.Length > 2)
            {
                yield return "sendtochaterror Too many parameters!";
            }
            else if (parameters.Length == 2)
            {
                int temp = -1;
                if (!int.TryParse(parameters[1], out temp))
                {
                    yield return "sendtochaterror!f The specified position '" + parameters[1] + "' is invalid!";
                    yield break;
                }
                if (temp < 1 || temp > 6)
                {
                    yield return "sendtochaterror The specified position '" + parameters[1] + "' is out of range 1-6!";
                    yield break;
                }
                buttons[temp - 1].OnInteract();
            }
            else if (parameters.Length == 1)
            {
                yield return "sendtochaterror Please specify the position of the button you wish to press!";
            }
            yield break;
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        buttons[correctBtn].OnInteract();
        yield return new WaitForSeconds(.1f);
    }
}
