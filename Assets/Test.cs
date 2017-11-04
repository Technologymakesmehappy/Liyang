using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person {
    public string name;
    public int age;
    //public Person(string name, int age) {
    //    this.name = name;
    //    this.age = age;
    //}
}

public class Test : MonoBehaviour {

    string[] arrName = { "zhangsan", "lisi", "wangwu","zhaoliu" };
    int[] arrAge = { 11, 21, 31, 41 };
    Person[] arrPerson;
	void Start () {
        arrPerson = new Person[4];
        for (int i = 0; i < arrPerson.Length; i++)
        {
            //arrPerson[i] = new Person(
            //    name:arrName[i],
            //    age:arrAge[i]
            //    );
            arrPerson[i] = new Person();
            arrPerson[i].name = arrName[i];
            arrPerson[i].age = arrAge[i];
        }
        for (int i = 0; i < arrPerson.Length; i++)
        {
            print(i + ": ,name:" + arrPerson[i].name + " ,age:" + arrPerson[i].age);
        }
	}
	
}
