using CSCore;
using CSCore.Codecs.WAV;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.Streams;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using WinformsVisualization.Visualization;

public class BPMMeasure : MonoBehaviour
{
    // Use this for initialization

    public int numBars = 30;

    public int minFreq = 5;
    public int maxFreq = 4500;
    public int barSpacing = 0;
    public bool logScale = true;
    public bool isAverage = false;

    public float highScaleAverage = 2.0f;
    public float highScaleNotAverage = 3.0f;

    public float measureTime = 5;

    [SerializeField]
    FftSize fftSize = FftSize.Fft4096;



    LineSpectrum lineSpectrum;

    WasapiCapture capture;

    float[] fftBuffer;

    BasicSpectrumProvider spectrumProvider;

    IWaveSource finalSource;

    public int bpm => (int)bpmCorrelationHistory.OrderByDescending(value => value.Item1).Take(1).Average(bpm => bpm.Item2);
    public float variance => Variance(bpmCorrelationHistory.Max(value => value.Item1));

    private float Variance(float energy)
    {
        float average = bpmCorrelationHistory.Average(value => value.Item1);
        float variance = bpmCorrelationHistory.Sum(entry => (entry.Item1 - average) * (entry.Item1 - average)) / bpmCorrelationHistory.Length;
        float relativeVariance = variance / average;
        return relativeVariance;
    }

    public int minExpectedBPM;
    public int maxExpectedBPM;

    public ComputeShader computeShader;
    public TMP_Text text;
    int energyHistoryLength => (int)(measureTime / Time.fixedDeltaTime);

    int correlationHistoyLength => (int)(1 / Time.fixedDeltaTime);
    int m_correlationHistoyIndex;
    int correlationHistoyIndex
    {
        get => m_correlationHistoyIndex;
        set
        {
            m_correlationHistoyIndex = value % correlationHistoyLength;
        }
    }

    LinkedList<float> energyHistory = new LinkedList<float>();

    (float, int)[] bpmCorrelationHistory = new (float,int)[] { (0, 0) };

    void Start()
    {
        // This uses the wasapi api to get any sound data played by the computer
        capture = new WasapiLoopbackCapture();
        capture.Initialize();

        // Get our capture as a source
        IWaveSource source = new SoundInSource(capture);


        // From https://github.com/filoe/cscore/blob/master/Samples/WinformsVisualization/Form1.cs

        // Actual fft data
        fftBuffer = new float[(int)fftSize];

        // These are the actual classes that give you spectrum data
        // The specific vars of lineSpectrum are changed below in the editor so most of these aren't that important here
        spectrumProvider = new BasicSpectrumProvider(capture.WaveFormat.Channels,
                    capture.WaveFormat.SampleRate, fftSize);

        lineSpectrum = new LineSpectrum(fftSize)
        {
            SpectrumProvider = spectrumProvider,
            UseAverage = true,
            BarCount = numBars,
            BarSpacing = 2,
            IsXLogScale = false,
            ScalingStrategy = ScalingStrategy.Linear
        };

        // Tells us when data is available to send to our spectrum
        var notificationSource = new SingleBlockNotificationStream(source.ToSampleSource());

        notificationSource.SingleBlockRead += NotificationSource_SingleBlockRead;

        // We use this to request data so it actualy flows through (figuring this out took forever...)
        finalSource = notificationSource.ToWaveSource();

        capture.DataAvailable += Capture_DataAvailable;
        capture.Start();

        bpmCorrelationHistory = new (float, int)[correlationHistoyLength];

        for(int i=0; i< energyHistoryLength; i++)
        {
            energyHistory.AddFirst(0);
        }

    }

    private void Capture_DataAvailable(object sender, DataAvailableEventArgs e)
    {
        finalSource.Read(e.Data, e.Offset, e.ByteCount);
    }

    private void NotificationSource_SingleBlockRead(object sender, SingleBlockReadEventArgs e)
    {
        spectrumProvider.Add(e.Left, e.Right);
    }


    void FixedUpdate()
    {
        if (spectrumProvider.IsNewDataAvailable)
        {
            lineSpectrum.MinimumFrequency = minFreq;
            lineSpectrum.MaximumFrequency = maxFreq;
            lineSpectrum.IsXLogScale = logScale;
            lineSpectrum.BarSpacing = barSpacing;
            lineSpectrum.SpectrumProvider.GetFftData(fftBuffer, this);
            float[] data = lineSpectrum.GetSpectrumPoints(100, fftBuffer);
            float energy = data.Sum(data => data * data);
            energyHistory.AddFirst(energy);
            energyHistory.RemoveLast();
        }

        CalculateBPM();
        text.text = $"{bpm}";
    }

    void CalculateBPM()
    {
        float[] bufferArray = new float[maxExpectedBPM - minExpectedBPM];
        ComputeBuffer energyCorrelationBuffer = new ComputeBuffer(bufferArray.Length, sizeof(float));
        energyCorrelationBuffer.SetData(bufferArray);

        ComputeBuffer energyHistoryBuffer = new ComputeBuffer(energyHistoryLength, sizeof(float));
        energyHistoryBuffer.SetData(energyHistory.ToArray());

        computeShader.SetBuffer(0, "energyCorrelation", energyCorrelationBuffer);
        computeShader.SetBuffer(0, "energyHistory", energyHistoryBuffer);
        computeShader.SetInt("minBPM", minExpectedBPM);
        computeShader.SetInt("maxBPM", maxExpectedBPM);
        computeShader.SetInt("samples", energyHistoryLength);
        computeShader.SetFloat("historyTime", measureTime);

        int width = (int)Mathf.Sqrt(maxExpectedBPM - minExpectedBPM);
        computeShader.Dispatch(0, 10, 10, 1);

        energyCorrelationBuffer.GetData(bufferArray);
        float maxCorrelation = Mathf.Max(bufferArray);
        int bpm = minExpectedBPM + Array.IndexOf(bufferArray, maxCorrelation);

        bpmCorrelationHistory[correlationHistoyIndex] = (maxCorrelation, bpm);
        correlationHistoyIndex++;
        energyCorrelationBuffer.Dispose();
        energyHistoryBuffer.Dispose();
    }


    void OnApplicationQuit()
    {
        if (enabled)
        {
            capture.Stop();
            capture.Dispose();
        }
    }
}
