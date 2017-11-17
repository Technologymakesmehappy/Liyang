using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class RankData : MonoBehaviour 
{
    private Text Tno;
    private Text Tscore;
    private Text TuserName;


    public void SetColor(Color color)
    {
        if (!Tno)
            Tno = Givevalue(this.transform, "Number");
        if (!Tscore)
            Tscore = Givevalue(this.transform, "Score");
        if (!TuserName)
            TuserName = Givevalue(this.transform, "ID");

        Tno.color = color;
        Tscore.color = color;
        TuserName.color = color;
    }

    public string Number
    {
        get {
            if (!Tno)
                Tno = Givevalue(this.transform, "Number");
            return Tno.text;
        }
        set
        {
            string str = Number;
                Tno.text = value;
        }

    }
    public string Score {
        get {
            if (!Tscore)
                Tscore = Givevalue(this.transform, "Score");
            return Tscore.text;
        }
        set
        {
            string str = Score;
                Tscore.text = value;
        }

    }
    public string ID {
        get { if (!TuserName) TuserName = Givevalue(this.transform, "ID");
            return TuserName.text;
        }
        set
        {
            string str = ID;
                TuserName.text = value;
        }

    }



    Text Givevalue(Transform target, string name)
    {

        if (target.transform.name == name)
        {
            if (target.GetComponent<Text>())
                return target.GetComponent<Text>();
        }
        for (int i = 0; i < target.transform.childCount; ++i)
        {
            var result = Givevalue(target.GetChild(i), name);

            if (result != null)
                return result;
        }

        return null;
    }


}
