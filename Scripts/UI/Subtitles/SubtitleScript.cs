using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class Line
{
    [SerializeField] public PersistentColor color;
    [SerializeField] public string line;
    [SerializeField] public AudioClip audioClip;

}
[CreateAssetMenu(fileName = "new Subtitle Script", menuName = "Subtitles/Script", order = 0)]
public class SubtitleScript : ScriptableObject
{
    [TextArea(15, 20)]
    private int currentIndex;
    private int lineAmount;
    [SerializeField] Line[] lines;
    public void Initialize()
    {
        currentIndex = 0;
        lineAmount = lines.Length;
    }
    public bool Read(out string line, out Color color, out AudioClip clip)
    {
        if (currentIndex > lineAmount - 1) { line = ""; color = Color.white; clip = null; return false; }
        line = lines[currentIndex].line;
        color = lines[currentIndex].color.color;
        color.a = 255;
        clip = lines[currentIndex].audioClip;
        currentIndex++;
        return true;
    }
}
