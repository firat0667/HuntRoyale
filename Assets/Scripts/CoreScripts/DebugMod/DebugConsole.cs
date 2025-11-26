using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firat0667.CaseLib.Patterns;

public class DebugConsole : FoundationSingleton<DebugConsole>, IFoundationSingleton
{
    [SerializeField] private GameObject consolePanel;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI outputText;

    private Dictionary<string, Action<string[]>> _commandDictionary = new();

    public bool Initialized { get; set; }

    private void Awake()
    {
        if (!Initialized)
        {
            consolePanel.SetActive(false);
            RegisterCommands();
            Initialized = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleConsole();
        }
    }

    /// <summary>
    /// Toggles the visibility of the console.
    /// </summary>
    private void ToggleConsole()
    {
        consolePanel.SetActive(!consolePanel.activeSelf);
        if (consolePanel.activeSelf)
        {
            inputField.ActivateInputField();
        }
    }

    /// <summary>
    /// Registers available commands.
    /// </summary>
    private void RegisterCommands()
    {
        _commandDictionary.Add("godmode", GodMode);
        _commandDictionary.Add("noclip", NoClip);
        _commandDictionary.Add("setspeed", SetSpeed);
        _commandDictionary.Add("clear", ClearConsole);
    }

    /// <summary>
    /// Processes the input command.
    /// </summary>
    public void ProcessCommand()
    {
        string input = inputField.text.Trim();
        inputField.text = "";

        if (string.IsNullOrEmpty(input)) return;

        string[] splitInput = input.Split(' ');
        string command = splitInput[0].ToLower();
        string[] args = splitInput.Length > 1 ? splitInput[1..] : Array.Empty<string>();

        if (_commandDictionary.TryGetValue(command, out var action))
        {
            action.Invoke(args);
        }
        else
        {
            PrintToConsole($"Unknown command: {command}");
        }
    }

    /// <summary>
    /// Prints text to the debug console.
    /// </summary>
    private void PrintToConsole(string message)
    {
        outputText.text += message + "\n";
    }

    /// <summary>
    /// Enables God Mode.
    /// </summary>
    private void GodMode(string[] args)
    {
        PrintToConsole("God Mode Activated!");
    }

    /// <summary>
    /// Enables NoClip Mode.
    /// </summary>
    private void NoClip(string[] args)
    {
        PrintToConsole("NoClip Mode Enabled!");
    }

    /// <summary>
    /// Sets player speed.
    /// </summary>
    private void SetSpeed(string[] args)
    {
        if (args.Length == 0)
        {
            PrintToConsole("Usage: setspeed [value]");
            return;
        }

        if (float.TryParse(args[0], out float speed))
        {
            PrintToConsole($"Player speed set to {speed}");
        }
        else
        {
            PrintToConsole("Invalid speed value.");
        }
    }

    /// <summary>
    /// Clears the console output.
    /// </summary>
    private void ClearConsole(string[] args)
    {
        outputText.text = "";
    }



}

/*
 * 
 * 
 * 
 * 
 * 
PUSH TAB BUTTON FOR OPEN

DebugConsole.Instance.ProcessCommand();  UI button 


_commandDictionary.Add("killall", KillAllEnemies);  // registercommads



private void KillAllEnemies(string[] args)
{
    Debug.Log("All enemies destroyed!");
}



Command	    Description
godmode	    Enables god mode (invincibility).
noclip	    Allows passing through walls.
setspeed    10	Sets the player's speed to 10.
clear	    Clears the console output.

 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
*/
