using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SubtitleManager : MonoBehaviour
{

    //How long each letter takes to display;
    [Tooltip("How long each letter takes to display")][SerializeField] float displayTime = 0.1f;
    //Time in between subtitles 
    [Tooltip("Time in between subtitles")][SerializeField] float readBuffer = 1;
    TextMeshProUGUI textElement;
    private AudioSource source;

    static SubtitleManager instance;
    public static SubtitleManager Instance
    {
        get => instance;
    }
    void Awake()
    {
        instance = this;
        textElement = this.GetComponent<TextMeshProUGUI>();
        Color32 vertexColor = textElement.color;
        vertexColor.a = 0;
        textElement.color = vertexColor;
        source = GetComponent<AudioSource>();
    }
    public void Display(SubtitleScript script)
    {
        script.Initialize();
        StartCoroutine(AnimateScript(script));
    }

    IEnumerator AnimateScript(SubtitleScript script)
    {
        string currentLine;
        Color color;
        AudioClip clip;
        while (script.Read(out currentLine, out color, out clip))
        {
            source.PlayOneShot(clip);

            color.a = 0;
            textElement.color = color;
            textElement.text = currentLine;
            textElement.ForceMeshUpdate();
            var textInfo = textElement.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                 var charInfo = textInfo.characterInfo[i];

                 if (!charInfo.isVisible) { continue; }


                int meshIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;
                Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;
                float animationTimer = 0;

                Color32 currentColor = vertexColors[vertexIndex + 0];
                currentColor.a = (byte)0;
                while (animationTimer <= displayTime)
                {
                    currentColor.a = 255;
                    vertexColors[vertexIndex + 0] = currentColor;
                    vertexColors[vertexIndex + 1] = currentColor;
                    vertexColors[vertexIndex + 2] = currentColor;
                    vertexColors[vertexIndex + 3] = currentColor;
                    animationTimer += Time.deltaTime;
                    textElement.mesh.colors32 = vertexColors;
                    textElement.UpdateVertexData();
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForSeconds(clip.length + readBuffer);
        }
        textElement.text = "";
    }
}
