using System;

[Serializable]
public class SaveData
{
    public float[] VolumeValues;

    public SaveData() 
    {
        VolumeValues = new float[2] { 0.3f, 0.5f };
    }
}