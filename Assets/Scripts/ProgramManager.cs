using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using SFB;
using TMPro;
using System.Text.RegularExpressions;
using System;

public class ProgramManager : MonoBehaviour
{
    //Areas to print text
    public TMP_InputField codeArea;
    public TMP_InputField highlightedCodeArea;
    public TMP_InputField outputArea;
    public TMP_InputField generatedCodeArea;
    public TMP_Text fileNameLabel;

    //File managing variables
    private string currentOpenPath;

    //Regex Syntax variables

    public List<string> keywords;
    public List<string> textRegex;
    void Start()
    {
        currentOpenPath = string.Empty;
        //keywords = new List<string> { "\\bfor\\b", "\\bif\\b", "\\bwhile\\b", "\\belse\\b" };
        textRegex = new List<string> {
            "\"(\\\\.|[^\"])*\"",
            "'(\\\\.|[^'])*'"
        };
        keywords = new List<string> {
            "\\band\\b",
            "\\bauto\\b",
            "\\bbool\\b",
            "\\bbreak\\b",
            "\\bcase\\b",
            "\\bcatch\\b",
            "\\bchar\\b",
            "\\bclass\\b",
            "\\bconst\\b",
            "\\bcontinue\\b",
            "\\bdefault\\b",
            "\\bdelete\\b",
            "\\bdo\\b",
            "\\bdouble\\b",
            "\\belse\\b",
            "\\benum\\b",
            "\\bexport\\b",
            "\\bfloat\\b",
            "\\bfor\\b",
            "\\bif\\b",
            "\\binline\\b",
            "\\bint\\b",
            "\\bimport\\b",
            "\\blong\\b",
            "\\bnamespace\\b",
            "\\bnew\\b",
            "\\bnot\\b",
            "\\bnull\\b",
            "\\bor\\b",
            "\\bprivate\\b",
            "\\bprotected\\b",
            "\\bpublic\\b",
            "\\brequires\\b",
            "\\breturn\\b",
            "\\bshort\\b",
            "\\bsigned\\b",
            "\\bsizeof\\b",
            "\\bstatic\\b",
            "\\bstruct\\b",
            "\\bswitch\\b",
            "\\bthis\\b",
            "\\bthrow\\b",
            "\\btypedef\\b",
            "\\btry\\b",
            "\\bunsigned\\b",
            "\\busing\\b",
            "\\bvoid\\b",
            "\\bwhile\\b",
            "\\bxor\\b",
            "\\boverride\\b",
            "\\bfinal\\b"
    };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenFile()
    {
        string [] path = StandaloneFileBrowser.OpenFilePanel("Select File...","","txt",false);
        if(path.Length > 0)
        {
            UnityEngine.Debug.Log("Path Selected: " + path[0]);
            currentOpenPath = path[0];
            fileNameLabel.text = path[0];
            string textRead = System.IO.File.ReadAllText(path[0]);
            codeArea.text = textRead;
        }
    }

    public void SaveFile()
    {
        if (currentOpenPath != string.Empty)
        {
            System.IO.File.WriteAllText(currentOpenPath, codeArea.text);
            UnityEngine.Debug.Log("Saved File");
        }
        else
        {
            SaveFileAs();
        }
    }

    public void SaveFileAs()
    {
        string path = StandaloneFileBrowser.SaveFilePanel("Save File...", "", "sampleFile", "txt");
        if (!string.IsNullOrEmpty(path))
        {
            System.IO.File.WriteAllText(path, codeArea.text);
            currentOpenPath = path;
            fileNameLabel.text = path;
        }
    }

    public void NewFile()
    {
        currentOpenPath = string.Empty;
        fileNameLabel.text = "None";
        codeArea.text = string.Empty;
    }

    public void CloseFile()
    {
        currentOpenPath = string.Empty;
        codeArea.text = string.Empty;
        fileNameLabel.text = "None";
    }

    public void CompileFile()
    {
        //Send the current open path to the lexical analyzer and extract tokens from output.
        generatedCodeArea.text = "";
        if (currentOpenPath == string.Empty) return;
        PerformLexicalAnalysis();
    }

    void PerformLexicalAnalysis()
    {
        try
        {
            //Define a process to run the lexical analyzer .exe with 
            //the currently open file
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Application.dataPath + "/CompilerUtileries/FlexLex/final.exe",
                    Arguments = currentOpenPath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            
            process.Start();
            //Start process and read each standard output JSON, transform
            //to object and print out on Lexicon tab.
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                Token readToken = JsonUtility.FromJson<Token>(line);
                generatedCodeArea.text += "Type: " + readToken.Type + " Lexeme: " + readToken.Lexeme + " Row: " + readToken.Row + " Col: " + readToken.Col + "\n";
            }
            //Read possible compiling errors and displaying them.
            var error = process.StandardError.ReadToEnd();
            if (error.Length > 0) outputArea.text = "<color=red>"+ error + "</color>";
            else outputArea.text = "<color=green> Compiled without errors </color>";
            process.WaitForExit();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.Message);
        }
    }

    public void ApplySyntaxColoring()
    {
        /*
        string inputText = codeArea.text;
        //UnityEngine.Debug.Log(word);
        //inputText = System.Text.RegularExpressions.Regex.Replace(inputText, "if", "<color=red>if</color>");
        //keywords = new List<string>{ "\\bfor\\b", "\\bif\\b", "\\bwhile\\b", "\\belse\\b" };
        foreach (string word in keywords)
        {
            UnityEngine.Debug.Log(word);
            inputText = Regex.Replace(inputText, word, "<color=blue>$&</color>");
        }
        foreach (string word in textRegex)
        {
            UnityEngine.Debug.Log(word);
            inputText = Regex.Replace(inputText, word, "<color=red>$&</color>");
        }

        generatedCodeArea.text = inputText;
        */
    }

}
