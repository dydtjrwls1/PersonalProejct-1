using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.IO;
using System;

public class RankLines : MonoBehaviour
{
    string[] names = new string[5]; // 현재 랭커의 이름 text
    int[] scores = new int[5]; // 현재 랭커의 점수 text

    Transform[] rankLines;

    ScoreText gameScoreText;

    string path;
    string fullPath;

    private void Awake()
    {
        Transform panel = transform.GetChild(0);
        rankLines = new Transform[panel.childCount];
        for (int i = 0; i < rankLines.Length; i++)
            rankLines[i] = panel.GetChild(i); // rankLines = [RankLine, RankLine , ... ] 

        

        path = $"{Application.dataPath}/SaveData/";
        fullPath = $"{path}data.txt"; 
    }

    private void Start()
    {
        gameScoreText = GameManager.Instance.ScoreText;
        LoadData();
    }

    void UpdateData()
    {
        for(int i = 0; i < scores.Length; i++)
        {
            if (scores[i] < gameScoreText.Score)
            {
                scores[i] = gameScoreText.Score;
            }
        }
    }

    void SetRankLineText()
    {
        int index = 0;
        foreach (Transform rankLine in rankLines)
        {
            TextMeshProUGUI[] texts = rankLine.GetComponentsInChildren<TextMeshProUGUI>();
            texts[1].text = names[index];
            texts[2].text = scores[index].ToString();
            index++;
        }
    }
   

    void LoadData()
    {
        CheckSaveDirectory();
        if(File.Exists(fullPath))
        {
            SaveData data = new SaveData();
            string jsonData = File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<SaveData>(jsonData);

            names = data.names;
            scores = data.scores;
        } else
        {
            SetDefaultValue();
        }
    }

    void SaveData()
    {
        CheckSaveDirectory();
        SaveData data = new SaveData();
        data.scores = scores;
        data.names = names;

        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(fullPath, jsonData);
    }

    void CheckSaveDirectory()
    {
        if (!Directory.Exists(path)) 
            Directory.CreateDirectory(path);
    }

    void SetDefaultValue()
    {
        char alphabet = 'A';
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = $"{alphabet}{alphabet}{alphabet}";
            scores[i] = 0;
            alphabet = (char)(alphabet + 1);
        }
    }
}
