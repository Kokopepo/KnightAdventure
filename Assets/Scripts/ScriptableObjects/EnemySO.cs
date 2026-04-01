using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()] //эта строчка позволяет нам создавать объекты этого класса в редакторе юнити, то есть мы можем создать объект EnemySO и заполнить его данными, а потом использовать эти данные в нашем коде 
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public int enemyHealth;
    public int enemyDamageAmount;
}
