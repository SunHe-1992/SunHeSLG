using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;

public class SubtitleBehaviour : PlayableBehaviour
{
    public string subtitleText;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TextMeshProUGUI text = playerData as TextMeshProUGUI;

        text.text = subtitleText;
        text.color = new Color(1, 1, 1, info.weight);
    }
}
