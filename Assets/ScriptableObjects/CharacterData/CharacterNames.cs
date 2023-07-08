using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharacterNames", menuName = "ScriptableObjects/CharacterNames")]

public class CharacterNames : ScriptableObject
{
    public List<string> NameList;

    public void Start()
    {
        string names = "Peppermint,Donuts,Chip,Bug,Beetle,Skinny Minny,Dino,Sport,Freckles,Honey Locks,Friendo,Betty Boop,Beast,Cuddle Pig,Buster,Gizmo,Fifi,Anvil,Inchworm,Goose,Chuckles,Tickles,Buzz,Manatee,Bean,Dimples,Boo Boo,Donut,Nerd,Heisenberg,Red Hot,Superman,Button,Pecan,Headlights,Boo,Kitty,Cheeky,Bridge,Itchy,Cheesestick,Cottonball,Goonie,Skunk,Fury,Double Bubble,Hubby,Flower,Frogger,Dunce,Pickles,4-Wheel,Cowboy,Frodo,Cold Brew,Fun Size,Bootsie,Sherlock,Carrot,Happy,Half Pint,MomBod,Rumplestiltskin,Marge,First Mate,Twinkie,Hermione,Turkey,Doll,Belle,Beef,Conductor,Slick,Big Bird,Bellbottoms,Pookie,Ghoulie,DJ,Azkaban,Pinata,Gumdrop,Amour,Lefty,Rosebud,Queenie,Hot Sauce,Dorito,Cookie,Mimi,Cheerio,Highbeam,Twiggy,Amorcita,Frauline,Ash,Freak,Brutus,Muscles,Grease,Rocketfuel";

        string[] split = names.Split(',');
        NameList = new();
        foreach(string name in split)
        {
            Debug.Log(name);
            NameList.Add(name.Trim());
        }
    }
}
