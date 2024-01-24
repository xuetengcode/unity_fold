using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine;

public class ProfillerStats : MonoBehaviour
{
    ProfilerRecorder triangleRecorder;
    ProfilerRecorder drawCallsRecorder;
    ProfilerRecorder verticesRecorder;
    
    ProfilerRecorder mainThreadTimeRecorder;
    ProfilerRecorder renderThreadTimeRecorder;
    
    public TMPro.TextMeshProUGUI statOverlay;

    private int framesCount;
    private float framesTime, lastFPS;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = -1;
        if (statOverlay == null)
            statOverlay = GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var sb = new StringBuilder(500);

        framesCount++;
        framesTime += Time.unscaledDeltaTime;
        if (framesTime > 0.5f)
        {
            float fps = framesCount / framesTime;
            lastFPS = fps;
            framesCount = 0;
            framesTime = 0;
        }
        sb.AppendLine($"FPS: {lastFPS}");
        sb.AppendLine($"Main Thread: {GetRecorderFrameAverage(mainThreadTimeRecorder) * (1e-6f):F1} ms");
        //sb.AppendLine($"Render Thread: {renderThreadTimeRecorder.LastValue * (1e-6f):F1} ms");
        
        //sb.AppendLine($"Verts: {verticesRecorder.LastValue / 1000}k");
        //sb.AppendLine($"Tris: {triangleRecorder.LastValue / 1000}k");
        //sb.AppendLine($"DrawCalls: {drawCallsRecorder.LastValue}");

        
        statOverlay.text = sb.ToString();
    }

    void OnEnable()
    {
        triangleRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
        drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
        verticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
        
        mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15);
    }

    void OnDisable()
    {
        triangleRecorder.Dispose();
        drawCallsRecorder.Dispose();
        verticesRecorder.Dispose();
    }

    static double GetRecorderFrameAverage(ProfilerRecorder recorder)
    {
        var samplesCount = recorder.Capacity;
        if (samplesCount == 0)
            return 0;

        double r = 0;
        unsafe
        {
            var samples = stackalloc ProfilerRecorderSample[samplesCount];
            recorder.CopyTo(samples, samplesCount);
            for (var i = 0; i < samplesCount; ++i)
                r += samples[i].Value;
            r /= samplesCount;
        }

        return r;
    }
}
