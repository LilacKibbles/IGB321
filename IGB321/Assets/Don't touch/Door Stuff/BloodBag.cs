using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBag : MonoBehaviour
{
    private Vector3 gaugePos;

    // Start is called before the first frame update
    void Start()
    {
        gaugePos = GameManager.instance.blood.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider otherObject)
    {
        if (otherObject.transform.tag == "Player")
        {
            if (otherObject.GetComponent<PlayerAvatar>().maxBlood <= 9)
            {
                otherObject.GetComponent<PlayerAvatar>().maxBlood += 4;
                if (otherObject.GetComponent<PlayerAvatar>().maxBlood >= 10) { otherObject.GetComponent<PlayerAvatar>().maxBlood = 10; }

                GameManager.instance.blood.maxValue = otherObject.GetComponent<PlayerAvatar>().maxBlood;

                if (otherObject.GetComponent<PlayerAvatar>().maxBlood == 10)
                {
                    GameManager.instance.blood.transform.localScale = new Vector3(1f, 1f, 1f);
                    GameManager.instance.blood.transform.position = new Vector3(gaugePos.x, gaugePos.y, 0f);
                }
                else
                {
                    GameManager.instance.blood.transform.localScale = new Vector3(GameManager.instance.blood.transform.localScale.x + 0.4f, 1f, 1f);
                    GameManager.instance.blood.transform.position = new Vector3(GameManager.instance.blood.transform.position.x + 28f, GameManager.instance.blood.transform.position.y, 0f);
                }

                Destroy(this.gameObject);
            }
        }
    }
}
