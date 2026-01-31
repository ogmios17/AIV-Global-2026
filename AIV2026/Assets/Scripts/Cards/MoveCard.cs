using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveCard", menuName = "Scriptable Objects/MoveCard")]
public class MoveCard : ScriptableObject
{
    public String cardName;
    public MoveCard wins;
    public MoveCard loses;
    public List<MoveCard> draws;
    public List<MoveCard> clashes;
}
