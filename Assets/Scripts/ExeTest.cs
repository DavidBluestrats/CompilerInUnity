using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
public class ExeTest : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField code;
    public TMP_InputField gen;
    void Start()
    {
        UnityEngine.Debug.Log("Project in: " + Application.dataPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveInputToFile()
    {
        System.IO.File.WriteAllText(Application.dataPath + "/TextFiles/tst.txt", code.text);
        UnityEngine.Debug.Log("Wrote to file: "+ code.text);
    }

    public void TryToExec()
    {
        Process proc = new Process();
        proc.StartInfo.FileName = Application.dataPath + "/executables/array.exe";
        proc.Start();
    }

    public void CopyTextFromCodeToGen()
    {
        gen.text = code.text;
    }
}
