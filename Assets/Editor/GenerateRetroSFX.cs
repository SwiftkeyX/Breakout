using System.IO;
using UnityEditor;
using UnityEngine;

public class GenerateRetroSFX
{
    private const int RATE = 44100;

    public static void Execute()
    {
        string dir = Application.dataPath + "/Audio/SFX";
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        // Short punchy hit when ball strikes a reinforced brick
        WriteSquare(dir + "/SfxHitBrick.wav",   800f, 0.12f, decay: 0.10f);
        // Noisy crunch when a brick is destroyed
        WriteCrunch(dir + "/SfxBrickBreak.wav", 0.28f);
        // Softer thud when ball hits the paddle
        WriteSquare(dir + "/SfxHitPaddle.wav",  360f, 0.18f, decay: 0.15f);
        // Crisp ping when ball hits a wall
        WriteSquare(dir + "/SfxHitWall.wav",    600f, 0.12f, decay: 0.09f);
        // Falling tone when ball is lost
        WriteFall(dir + "/SfxBallLost.wav",     380f, 80f, 0.9f);
        // 4-note ascending arpeggio for level clear
        WriteArpeggio(dir + "/SfxLevelClear.wav", new[]{523.25f,659.25f,783.99f,1046.5f}, 0.16f, 1.4f);
        // Quick rising shimmer for power-up pickup
        WriteArpeggio(dir + "/SfxPowerUp.wav",    new[]{523.25f,659.25f,783.99f,987.77f}, 0.09f, 0.6f);

        AssetDatabase.Refresh();
        Debug.Log("GenerateRetroSFX: 7 clips written to Assets/Audio/SFX/");
    }

    // Square wave with attack+decay envelope
    static void WriteSquare(string path, float freq, float dur, float decay)
    {
        int n = (int)(RATE * dur);
        var d = new float[n];
        float attack = 0.005f;
        for (int i = 0; i < n; i++)
        {
            float t = (float)i / RATE;
            float env = t < attack
                ? t / attack
                : Mathf.Exp(-(t - attack) / (decay * 0.3f));
            d[i] = (Mathf.Sin(2f * Mathf.PI * freq * t) >= 0f ? 0.7f : -0.7f) * env;
        }
        WriteWav(path, d);
    }

    // Noise burst + pitch-drop for brick destroy
    static void WriteCrunch(string path, float dur)
    {
        int n = (int)(RATE * dur);
        var d = new float[n];
        var rng = new System.Random(7);
        for (int i = 0; i < n; i++)
        {
            float t  = (float)i / RATE;
            float env = Mathf.Exp(-t * 10f);
            float noise = (float)(rng.NextDouble() * 2.0 - 1.0);
            float freq  = 320f * (1f - t / dur);
            float tone  = Mathf.Sin(2f * Mathf.PI * freq * t) >= 0f ? 1f : -1f;
            d[i] = (noise * 0.55f + tone * 0.45f) * env * 0.8f;
        }
        WriteWav(path, d);
    }

    // Pitch falls from startFreq to endFreq over dur seconds
    static void WriteFall(string path, float startFreq, float endFreq, float dur)
    {
        int n = (int)(RATE * dur);
        var d = new float[n];
        float phase = 0f;
        for (int i = 0; i < n; i++)
        {
            float t   = (float)i / RATE;
            float env = Mathf.Clamp01(1f - t / dur * 0.75f);
            float hz  = Mathf.Lerp(startFreq, endFreq, t / dur);
            phase += 2f * Mathf.PI * hz / RATE;
            d[i] = (Mathf.Sin(phase) >= 0f ? 0.7f : -0.7f) * env;
        }
        WriteWav(path, d);
    }

    // Ascending arpeggio — each note plays for noteLen seconds
    static void WriteArpeggio(string path, float[] notes, float noteLen, float totalDur)
    {
        int n = (int)(RATE * totalDur);
        var d = new float[n];
        for (int i = 0; i < n; i++)
        {
            float t      = (float)i / RATE;
            int   idx    = Mathf.Min((int)(t / noteLen), notes.Length - 1);
            float noteT  = t - idx * noteLen;
            float decay  = idx == notes.Length - 1 ? 1.5f : 8f;
            float env    = Mathf.Exp(-noteT * decay);
            d[i] = (Mathf.Sin(2f * Mathf.PI * notes[idx] * t) >= 0f ? 0.7f : -0.7f) * env;
        }
        WriteWav(path, d);
    }

    static void WriteWav(string path, float[] samples)
    {
        int dataBytes = samples.Length * 2;
        using var fs = new FileStream(path, FileMode.Create);
        using var bw = new BinaryWriter(fs);
        bw.Write(new[]{'R','I','F','F'});
        bw.Write(36 + dataBytes);
        bw.Write(new[]{'W','A','V','E'});
        bw.Write(new[]{'f','m','t',' '});
        bw.Write(16); bw.Write((short)1); bw.Write((short)1);
        bw.Write(RATE); bw.Write(RATE * 2); bw.Write((short)2); bw.Write((short)16);
        bw.Write(new[]{'d','a','t','a'});
        bw.Write(dataBytes);
        foreach (float s in samples)
            bw.Write((short)(Mathf.Clamp(s, -1f, 1f) * 32767f));
    }
}
