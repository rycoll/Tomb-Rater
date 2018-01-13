﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure {

	private string name;
	private string desc;
	private Sprite sprite = Resources.Load ("Treasure Sprites/default_treasure", typeof(Sprite)) as Sprite;
	private int baseValue = 5;
	private List<TreasureType> typeList;

	public static GameController gameController;

	public void setName (string newName) {
		name = newName;
	}
	public string getName () {
		return name;
	}

	public void setDesc (string newDesc) {
		desc = newDesc;
	}
	public string getDesc () {
		return desc;
	}

	public void setSprite (string sprName) {
		sprite = Resources.Load ("Treasure Sprites/" + sprName, typeof(Sprite)) as Sprite;
	} 
	public Sprite getSprite () {
		return this.sprite;
	}

	public void setBaseValue (int newValue) {
		baseValue = newValue;
	}
	public int getBaseValue () {
		return baseValue;
	}

	public void addType (TreasureType newType) {
		if (typeList == null) {
			typeList = new List<TreasureType> ();
		}
		if (!typeList.Contains (newType)) {
			typeList.Add (newType);
		}
	}
	public List<TreasureType> getTypeList () {
		return typeList;
	}

	public virtual void onRate () {
		//called when the tomb is finally rated
		//probably?
	}
}

public enum TreasureType {
	RICHES,
	BURIAL,
	TRAP,
	HISTORICAL,
	RELIGIOUS,
	MUSICAL,
	MAGICAL
}

/* This was just made for testing, not really good enough for the actual game. */
public class Tre_Pawn : Treasure {
	public Tre_Pawn () {
		setName ("Pawn");
		setSprite ("worker_token");
		setDesc ("A fine pawn piece, for testing the treasure system.");
	}
}

public class Tre_Mural : Treasure {
	public Tre_Mural () {
		setName ("Mural");
		setDesc ("A carved mural of your many deeds and accomplishments.");
	}
}

public class Tre_Candelabra : Treasure {
	public Tre_Candelabra () {
		setName ("Candelabra");
		setSprite ("candelabra");
		setDesc ("A wrought iron, gold plated candelabra. The candles, made from high quality carp fat, will burn long after the tomb is sealed.");
	}
}

public class Tre_Sarcophagus : Treasure {
	public Tre_Sarcophagus () {
		setName ("Sarcophagus");
		setSprite ("coffin");
		addType (TreasureType.BURIAL);
	}
}

public class Tre_Tapestry : Treasure {
	public Tre_Tapestry () {
		setName ("Tapestry");
		setDesc ("A beautiful tapestry. The many accomplishments of your Kingdom are woven into the fabric.");
	}
}

public class Tre_StatueOfSomeone : Treasure {
	private string personName;
	public void setPersonName (string str) {
		personName = str;
	}
	public Tre_StatueOfSomeone (string str) {
		setPersonName (str);
		setName ("Statue of " + personName);
		setDesc ("A statue of " + personName + ".");
		addType (TreasureType.HISTORICAL);
	}
}

public class Tre_DartTrap : Treasure {
	public Tre_DartTrap () {
		setName ("Dart Trap");
		setDesc ("Once the tomb is sealed, this hidden trap will shoot poison-tipped darts at any would-be grave robbers who pass it.");
		addType (TreasureType.TRAP);
	}
}

public class Tre_CurseSapling : Treasure {
	public Tre_CurseSapling () {
		setName ("Curse Sapling");
		setDesc ("The sapling of a curse tree. Its magic will punish intruders.");
		addType (TreasureType.MAGICAL);
		addType (TreasureType.TRAP);
	}
}

public class Tre_RubySarcophagus : Treasure {
	public Tre_RubySarcophagus () {
		setName ("Ruby Sarcophagus");
		setDesc ("An ornate sarcophagus, somehow crafted entirely from rubies. Even the hinges appear to be carefully cut gems.");
		addType (TreasureType.BURIAL);
		addType (TreasureType.RICHES);
	}
}

public class Tre_ChestOfGreed : Treasure {
	public Tre_ChestOfGreed () {
		setName ("Chest of Greed");
		setDesc ("An cursed oaken chest. Designed to punish greedy tomb robbers, it will devour anyone who tried to open it.");
		addType (TreasureType.MAGICAL);
		addType (TreasureType.TRAP);
	}
}

public class Tre_FountainOfSilver : Treasure {
	public Tre_FountainOfSilver () {
		setName ("Fountain of Silver");
		setDesc ("Liquid silver flows continuously up and down this amazing contraption. Magic is surely involved.");
		addType (TreasureType.MAGICAL);
		addType (TreasureType.RICHES);
	}
}
	
public class Tre_ClockworkDrums : Treasure {
	public Tre_ClockworkDrums () {
		setName ("Clockwork Drums");
		setDesc ("Self-playing drums, to fill your tomb with beats.");
		addType (TreasureType.MUSICAL);
	}
}

public class Tre_BlackLightCandle : Treasure {
	public Tre_BlackLightCandle () {
		setName ("Candle of Black Light");
		setDesc ("Encased in an orb of violet glass, the light from this candle highlights fluorescent objects.");
	}
}

public class Tre_CryptHorn : Treasure {
	public Tre_CryptHorn () {
		setName ("Crypt Horn");
		setDesc ("A traditional fixture for noble tombs. It converts wind from outside into mournful droning inside.");
		addType (TreasureType.MUSICAL);
	}
}

// jacuzzi

public class Tre_BlackenedAlarmBells : Treasure {
	public Tre_BlackenedAlarmBells () {
		setName ("Blackened Alarm Bells");
		setDesc ("These bells will ring if intruders enter your tomb, and your corpse will rise to fight them!");
		addType (TreasureType.TRAP);
		addType (TreasureType.MAGICAL);
		addType (TreasureType.MUSICAL);
	}
	// do stuff after dead
}

public class Tre_BloodJar : Treasure {
	public Tre_BloodJar () {
		setName ("Blood Jar");
		setDesc ("When you die, put all your blood in this. Then, you can haunt the living!");
		addType (TreasureType.BURIAL);
		addType (TreasureType.MAGICAL);
	}
	// method after death
}