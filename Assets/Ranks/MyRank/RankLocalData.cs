using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class RankLocalData : RankSingle<RankLocalData>
{

    string LoadFile(string _path,string _name)
    {
        string data = "";
     
        FileStream stream = new FileStream(_path + "//" + _name, FileMode.OpenOrCreate);

        StreamReader reader = new StreamReader(stream);
      
    data=    reader.ReadLine();
        reader.Close();
        reader.Dispose();

        return data;


    }
     void SaveData(string _path,string name,string data)
    {
      
        FileStream stream = new FileStream(_path + "//" + name, FileMode.OpenOrCreate);

        StreamWriter write = new StreamWriter(stream);
        write.Write(data);
        write.Flush();
        write.Close();
        write.Dispose();       


    }



    public string ReadData() {

        string dt = string.Empty;

        dt = LoadFile(Application.persistentDataPath, "LocalScore.txt");
   


        return dt;

    }
    public void SaveDt(string data)
    {

        SaveData(Application.persistentDataPath, "LocalScore.txt", data);




    }

    public override void Init()
    {
       
    }
}
