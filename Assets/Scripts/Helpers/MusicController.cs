using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuanPayan.Helpers
{
    public class MusicController : MonobehaviourEvents
    {
        [Space(15)]
        [Header("Music")]

        [Stem.PlaylistID]
	    public Stem.ID playlistId = Stem.ID.None;
    
	    [Stem.MusicPlayerID]
	    public Stem.ID musicPlayerId = Stem.ID.None;

        [Space(15)]
        [Header("Ambience Sound")]
        [Stem.MusicPlayerID]
	    public Stem.ID ambiencePlayerId = Stem.ID.None;

        [SerializeField] private string ambienceSound1;

        [Stem.MusicPlayerID]
	    public Stem.ID ambiencePlayerId2 = Stem.ID.None;

         [SerializeField] private string ambienceSound2;
        // Start is called before the first frame update
        
        public override void Behaviour()
        {
            if(musicPlayerId !=Stem.ID.None && playlistId !=Stem.ID.None) Stem.MusicManager.SetPlaylist(musicPlayerId, playlistId);

            if(ambiencePlayerId !=Stem.ID.None && !string.IsNullOrEmpty(ambienceSound1)) 
            {
                Stem.MusicManager.Seek(ambiencePlayerId, ambienceSound1);
                Stem.MusicManager.Play(ambiencePlayerId);
            }
            if(ambiencePlayerId2 !=Stem.ID.None && !string.IsNullOrEmpty(ambienceSound2))
            {
                Stem.MusicManager.Seek(ambiencePlayerId2,ambienceSound2);
                Stem.MusicManager.Play(ambiencePlayerId2);
            } 

           if(string.IsNullOrEmpty(ambienceSound1)) Stem.MusicManager.Stop(ambiencePlayerId);
           
           if(string.IsNullOrEmpty(ambienceSound1)) Stem.MusicManager.Stop(ambiencePlayerId2);

        }//Closes Behaviour method

       
    }
}
