using UnityEngine;
public class DuckDuckGoose : MonoBehaviour
{
    public enum Location{LeftBox, RightBox,DuckSpawnBox, PlayerSpawn};

    public Location myLoc;
    
    public virtual void UpdateLocation(Location now)
    {
        myLoc = now;
    }

    
}
