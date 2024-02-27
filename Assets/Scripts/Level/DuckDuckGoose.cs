using UnityEngine;
public class DuckDuckGoose : MonoBehaviour
{
    public enum Location { LeftBox, RightBox, DuckSpawnBox, PlayerSpawn };

    public Location myLoc;
    public GameManager gm;
    public virtual void UpdateLocation(Location now)
    {
        myLoc = now;
    }
    public void FindGameManager(){
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
}