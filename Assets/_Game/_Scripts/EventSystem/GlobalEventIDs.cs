namespace CardGame
{
    public enum EventID
    {

        ///Level
        OnCardFlipped,
        OnCardMatchSuccess,
        OnCardMatchFailed,



        //Audio
        RequestToPlaySFXWithClip,
        RequestToPlayBGMWithClip,
        RequestToPlaySFXWithId,
        RequestToPlayBGMWithId,
        OnToggleSFX,
        OnToggleMusic,
        RequestToStopSFX,
    }
}