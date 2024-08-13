using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.IO;
using System;

public class RankLines : MonoBehaviour
{
    string[] names = new string[5]; // ���� ��Ŀ�� �̸� text
    int[] scores = new int[5]; // ���� ��Ŀ�� ���� text

    TMP_InputField inputField;

    Transform[] rankLines;

    int rankerLineIndex;

    ScoreText gameScoreText;

    // resoucres ���� ���
    string path;

    // saveData ���� ���
    string fullPath;

    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>(true);
        Transform panel = transform.GetChild(0);
        rankLines = new Transform[panel.childCount];
        for (int i = 0; i < rankLines.Length; i++)
            rankLines[i] = panel.GetChild(i); // rankLines = [RankLine, RankLine , ... ] 

        path = $"{Application.dataPath}/SaveData/";
        fullPath = $"{path}data.txt"; 
    }

    private void Start()
    {
        inputField.onEndEdit.AddListener(EndEdit);
        gameScoreText = GameManager.Instance.ScoreText;
        GameManager.Instance.Player.onDie += UpdateData;
        LoadData();
    }

    void EndEdit(string text)
    {
        names[rankerLineIndex] = text;
        inputField.gameObject.SetActive(false);
        SetRankLineText();
    }

    // GameOver �� ��� ������ �����Ѵ�.
    void UpdateData()
    {
        for(int i = 0; i < scores.Length; i++)
        {
            if (scores[i] < gameScoreText.Score)
            {
                // ���� �ٲٱ�
                for(int j = scores.Length - 1; j > i; j--)
                {
                    scores[j] = scores[j - 1];
                }

                rankerLineIndex = i;
                scores[i] = gameScoreText.Score; // ���� ����

                // �̸��� �Է¹ޱ� ���� inputField Ȱ��ȭ
                inputField.gameObject.SetActive(true);
                inputField.transform.position = rankLines[i].GetChild(1).transform.position;

                break;
            }
        }

        SetRankLineText();
    }

    // RankLine �� ���� ������ �̸����� ����
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
