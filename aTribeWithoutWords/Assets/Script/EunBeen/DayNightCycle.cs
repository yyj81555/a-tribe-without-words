using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour {

    [SerializeField] Transform centerOfGameMap;
    [SerializeField] Transform sun, moon;
    [SerializeField] GameObject sunObj, moonObj;

    [SerializeField] int distanceFromOrigin = 50;
    [SerializeField] float daySpeed = .1f;

    [SerializeField] int orbitAngle = 35;
    [SerializeField] int sunrisePosition;
    [SerializeField] int dayProgress = 10;

    void Start () {
        SetupOrbital();
        SetOrbitalPath();
	}

    void SetupOrbital()
    {
        Vector3 distanceFromOriginVector = new Vector3(distanceFromOrigin, 0, 0);
        sun.position = distanceFromOriginVector;
        moon.position = -distanceFromOriginVector;

        sun.rotation = Quaternion.Euler(0, -90, 0);
        moon.rotation = Quaternion.Euler(0, 90, 0);

        this.transform.position = centerOfGameMap.position;
    }

    void SetOrbitalPath()
    {
        transform.rotation = Quaternion.Euler(orbitAngle, sunrisePosition, dayProgress);
    }
	
	void FixedUpdate () {
        transform.Rotate(0, 0, daySpeed);
        sunObj.transform.position = sun.position;
        moonObj.transform.position = moon.position;

       if((transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180) && !GameLevelManager.Instance.isSunRise)
       {
            GameLevelManager.Instance.isSunRise = true;
            GameLevelManager.Instance.inGameDays++;
       }
       else if ((transform.eulerAngles.z > 180 && transform.eulerAngles.z < 360) && GameLevelManager.Instance.isSunRise)
       {
            GameLevelManager.Instance.isSunRise = false;
       }
    }
}
