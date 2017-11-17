using UnityEngine;
using System.Collections;

public abstract  class RankSingle<T> where T:class,new() 
{
    private static T instance;


    public static T _instance {
        get
        {
            if (instance == null)
                instance = new T();

          
            return instance;
        }

    }

  protected RankSingle()
    {
        Init();
    }

    public  abstract void Init();


}
