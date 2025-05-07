namespace CardGame
{
    public enum EventID
    {

        ///Level
        OnCardRevealed,
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

        //Loading
        RequestQuickLoadingScreen,
        OnLoadingComplete,
        OnNewLevelRequested,
        OnLevelResumeRequested,
        OnPlayLevelRequested,
        OnLevelQuitRequested,
        OnGameStartRequested,
        OnLevelComplete,
        OnPlayerDataLoaded,
        OnGameInit,
    }
}