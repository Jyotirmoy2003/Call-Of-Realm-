using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGatherable 
{
   public int ID { get; set; }
   public void CollectItem();
}
