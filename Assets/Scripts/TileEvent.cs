using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileEvent 
{
    //apa yang akan terjadi jika tile On Match
    public abstract void OnMatch();

    //cek persyaratan event terpenuhi
    public abstract bool AchievementCompleted();
}
