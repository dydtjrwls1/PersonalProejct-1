using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.IO;
using System;

public class RankLines : MonoBehaviour
{
    TextMeshProUGUI[] names = new TextMeshProUGUI[5]; // 각 rankLine 의 이름 text
    TextMeshProUGUI[] scores = new TextMeshProUGUI[5]; // 각 rankLine 의 점수 text

    string path;
    string fullPath;

    [Serializable]
    protected class Data
    {
        string name;
        int score;
    }

    private void Awake()
    {
        Transform panel = transform.GetChild(0);
        Transform[] rankLines = new Transform[panel.childCount];
        for (int i = 0; i < rankLines.Length; i++)
        {
            rankLines[i] = panel.GetChild(i); // rankLines = [RankLine, RankLine , ... ] 
        }

        int index = 0;
        foreach (Transform rankLine in rankLines)
        {
            TextMeshProUGUI[] texts = rankLine.GetComponentsInChildren<TextMeshProUGUI>();
            names[index] = texts[1];
            scores[index] = texts[2];
            index++;
        }
    }

    private void Start()
    {
        string path = Application.dataPath + "/SaveData/";
        Debug.Log(Directory.Exists(path));
        SetDefaultValue();
    }

    void SetDefaultValue()
    {
        char alphabet = 'A';
        for (int i = 0; i < names.Length; i++)
        {
            names[i].text = $"{alphabet}{alphabet}{alphabet}";
            scores[i].text = "0000";
            alphabet = (char)(alphabet + 1);
        }
    }
}
