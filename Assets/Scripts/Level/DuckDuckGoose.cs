using UnityEngine;
public class DuckDuckGoose : MonoBehaviour
{
    public enum Location { LeftBox, RightBox, DuckSpawnBox, PlayerSpawn };

    public static Location myLoc;
    public GameManager gm;
 
    public void FindGameManager(){
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
}