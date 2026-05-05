using UnityEngine;

[CreateAssetMenu(fileName = "SfxLibrary", menuName = "Dastarkhan/SFX Library")]
public class SfxLibrarySO : ScriptableObject
{
    [Header("Player & Items")]
    public AudioClip Pickup;
    public AudioClip Drop;

    [Header("Stations")]
    public AudioClip StationPlace;
    public AudioClip StationTake;
    public AudioClip StationReady;
    public AudioClip StationBurned;

    [Header("Orders")]
    public AudioClip OrderSpawned;
    public AudioClip OrderDelivered;
    public AudioClip OrderExpired;

    [Header("Session")]
    public AudioClip SessionStart;
    public AudioClip SessionEnd;
    public AudioClip ButtonClick;

    [Header("Volumes")]
    [Range(0f, 1f)] public float MasterSfxVolume = 0.8f;
}
