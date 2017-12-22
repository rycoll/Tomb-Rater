﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombRoom {

	private string name;
	private string description;
	private RoomFeature[] features;
	private int minSize = 1;

	public string getName() {
		return name;
	}
	public void setName (string str) {
		this.name = str;
	}

	public string getDescription () {
		return description;
	}
	public void setDescription (string str) {
		this.description = str;
	}

	public RoomFeature[] getFeatures() {
		return features;
	}
	public void setParts (RoomFeature[] newFeatures) {
		this.features = (RoomFeature[]) newFeatures.Clone();
	}

	public int getMinSize () {
		return minSize;
	}
	public void setMinSize (int newSize) {
		this.minSize = newSize;
	}

}

public class RoomFeature {

	private string name;
	private int cost;

	public string getName() {
		return name;
	}
	public void setName (string str) {
		this.name = str;
	}

	public int getCost () {
		return cost;
	}
	public void setCost (int n) {
		this.cost = n;
	}

}

//*ROOMS************************************************************
public class Room_Hallway : TombRoom {
	public Room_Hallway () {
		setName ("Hallway");
		setDescription ("A hallway, for connecting rooms.");
		setMinSize (1);
	}
}

public class Room_BurialChamber : TombRoom {
	public Room_BurialChamber () {
		setName ("Burial Chamber");
		setDescription ("A chamber designed to house the sarchophagus of our Glorious Leader.");
		setMinSize (6);
	}
}