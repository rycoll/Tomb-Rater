﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TOMB BUILDING IMPLEMENTATION
 * In the building scene, have an isometric grid of tiles (size x)
 * GameController has an x-by-x 2d array of BuildTiles, which have game info about what is on a tile
 * The building scene has an x-by-x 2d array of MapTiles, which handle all in the in-scene stuff
 * These map tiles are attached to the tiles in the isometric grid
 * The building scene uses the BuildTiles to set up the MapTiles correctly.
 * Other scenes can still access important game info, via GameController's array of BuildTiles
 * MORE INFO:
 * Each BuildTile specifies the TombRoom it is part of - multiple BuildTiles can refer to the same TombRoom
 * Each BuildTile has a RoomSection, which indicates how to set up that particular tile (i.e. walls, decorations, etc)
 * PROBLEMS:
 * This could make it hard to change (on the dev side) later (the manual placement of isometric tiles is arduous as heck)
 * It would be very important to ensure the arrays could never fall out of sync
 */

public class ManageBuilding {

	private BuildTile[,] map;
	private int sizeX = MapTile.gridSize + 1, sizeY = MapTile.gridSize + 1;
	private List<TombRoom> roomsToBuild;
	private List<BuildMaterial> availableMaterials;
	private int workerExpectationThisYear = 2;

	public ManageBuilding () {
		map = new BuildTile[sizeX, sizeY];
		for (int x = 0; x < sizeX; x++) {
			for (int y = 0; y < sizeY; y++) {
				map [x, y] = new BuildTile ();
			}
		}

		roomsToBuild = new List<TombRoom> ();
		makeRoomAvailableToBuild (new Room_Hallway ());
		makeRoomAvailableToBuild (new Room_BurialChamber ());
		makeRoomAvailableToBuild (new Room_TreasureVault ());
		makeRoomAvailableToBuild (new Room_ServantBurialChamber ());

		availableMaterials = new List<BuildMaterial> ();
		addAvailableMaterial (new Mat_Clay ());
		addAvailableMaterial (new Mat_Wood ());
	}

	public void makeRoomAvailableToBuild (TombRoom room) {
		if (!roomsToBuild.Contains (room)) {
			roomsToBuild.Add (room);
		}
	}
	public void makeRoomUnavailableToBuild (TombRoom room) {
		foreach (TombRoom availableRoom in roomsToBuild) {
			if (availableRoom.getName ().Equals (room.getName ())) {
				roomsToBuild.Remove (availableRoom);
				makeRoomUnavailableToBuild (room);
				return;
			}
		}
	}
	public TombRoom[] getAvailableRooms () {
		return roomsToBuild.ToArray ();
	}
	public List<TombRoom> getAvailableRoomList () {
		return roomsToBuild;
	}

	public void addAvailableMaterial (BuildMaterial mat) {
		if (!isMaterialAvailable (mat)) {
			availableMaterials.Add (mat);
		}
	}
	public void removeAvailableMaterial (BuildMaterial mat) {
		foreach (BuildMaterial availableMat in availableMaterials) {
			if (mat.isSameAs (availableMat)) {
				availableMaterials.Remove (availableMat);
				removeAvailableMaterial (mat);
				return;
			}
		}
	}
	public BuildMaterial[] getAvailableMaterials () {
		return availableMaterials.ToArray ();
	}
	public List<BuildMaterial> getAvailableMaterialList () {
		return availableMaterials;
	}
	public void changeMaterialCost (BuildMaterial mat, int newCost) {
		foreach (BuildMaterial availableMat in availableMaterials) {
			if (mat.isSameAs (availableMat)) {
				availableMat.setCostPerTile (newCost);
				return;
			}
		}
	}
	public bool isMaterialAvailable (BuildMaterial mat) {
		foreach (BuildMaterial availableMat in availableMaterials) {
			if (mat.isSameAs (availableMat)) {
				return true;
			}
		}
		return false;
	}

	public int getWorkerExpectation () {
		return workerExpectationThisYear;
	}
	public void setWorkerExpectation (int n) {
		workerExpectationThisYear = n;
	}

	public BuildTile getTileAtCoord (int x, int y) {
		if (!(x < 0 || y < 0 || x >= sizeX || y >= sizeY)) {
			return map [x, y];
		} else {
			Debug.Log ("Bad coords! x: " + x + ", y: " + y);
			return null;
		}
	}

	public int[] getSizes () {
		return new int[]{ sizeX, sizeY };
	}

	//returns true if successful, false otherwise
	//does 3 loops through array. Should these loops be separate? I *think* so.
	public bool addRoom (TombRoom room, MapTile[] tiles, BuildMaterial mat) {
		//error checking!
		if (room == null || tiles == null) {
			return false;
		}
		if (tiles.Length < room.getMinSize ()) {
			Debug.Log ("More space is required!");
			return false;
		}
		foreach (MapTile tile in tiles) {
			int posX = tile.getX ();
			int posY = tile.getY ();
			bool positionOutOfBounds = (posX < 0 || posY < 0 || posX >= sizeX || posY >= sizeY);
			bool sizeCannotFit = posX >= sizeX || posY >= sizeY;
			bool coordAlreadyOccupied = map [posX, posY].getRoom () != null;
			//should we check for contiguity here??
			if (positionOutOfBounds || sizeCannotFit || coordAlreadyOccupied) {
				Debug.Log ("Bad coords! Failed to add room!");
				return false;
			}
		}
		//error checking finished
		//set rooms
		foreach (MapTile tile in tiles) {
			int posX = tile.getX ();
			int posY = tile.getY ();
			map [posX, posY].setRoom (room);
			room.setMaterial (mat);
		}
		//set walls (done separately so that we know where each part of the room has ended up
		foreach (MapTile tile in tiles) {
			int posX = tile.getX ();
			int posY = tile.getY ();
			BuildTile[] adjacentTiles = getAdjacentTiles (posX, posY);
			WallsToShow walls = WallsToShow.NONE;
			if (adjacentTiles[0] == null || adjacentTiles[0].getRoom() == null /* || adjacentTiles [0].getRoom() != room*/) {
				walls = WallsToShow.LEFT;
				if (adjacentTiles[1] == null || adjacentTiles[1].getRoom() == null /* || adjacentTiles [1].getRoom () != room*/) {
					walls = WallsToShow.BOTH;
				}
			} else if (adjacentTiles[1] == null || adjacentTiles[1].getRoom() == null /* || adjacentTiles [1].getRoom () != room*/) {
				walls = WallsToShow.RIGHT;
			}
			RoomSection newSection = new RoomSection ();
			newSection.setWalls (walls);
			map [posX, posY].setSection(newSection);
		}
		return true;
	}

	/* x and y specify a tile by coordinates
	 * this method returns an array of adjacent tiles
	 * the array is ordered north-east-west-south
	 * array is length 4 - empty tiles are null
	 */
	public BuildTile[] getAdjacentTiles (int x, int y) {
		BuildTile[] array = { null, null, null, null };
		if (x < 0 || y < 0 || x >= sizeX || y >= sizeY) {
			return array;
		}
		if (y > 0) {
			array [0] = map [x, y - 1];
		}
		if (x < sizeX - 1) {
			array [1] = map [x + 1, y];
		}
		if (y < sizeY - 1) {
			array [2] = map [x, y + 1];
		}
		if (x > 0) {
			array [3] = map [x - 1, y];
		}
		return array;
	}

	public BuildTile[,] getTileMap () {
		return map;
	}
	public void setTileMap (BuildTile[,] array) {
		map = array;
	}
	public void setRoomsToBuild (List<TombRoom> list) {
		roomsToBuild = list;
	}
	public void setAvailableMaterials (List<BuildMaterial> list) {
		availableMaterials = list;
	}
}

[System.Serializable]
public class BuildTile {

	//the room that this is part of
	private TombRoom room = null;
	//information about what goes on this particular tile
	private RoomSection section = null;

	public BuildTile () {
	}
	public BuildTile (TombRoom newRoom, RoomSection newSection) {
		room = newRoom;
		section = newSection;
	}


	public TombRoom getRoom () {
		return room;
	}
	public void setRoom (TombRoom newRoom) {
		this.room = newRoom;
	}

	public RoomSection getSection () {
		return section;
	}
	public void setSection (RoomSection newSection) {
		this.section = newSection;
	}

}

public enum WallsToShow {
	LEFT,
	RIGHT,
	BOTH,
	NONE
}

[System.Serializable]
public class RoomSection {
	private WallsToShow walls;
	private string decorationSpriteName = null;

	public WallsToShow getWalls () {
		return walls;
	}
	public void setWalls (WallsToShow w) {
		this.walls = w;
	}

	public string getDecorationSprite () {
		return decorationSpriteName;
	}
	public void setDecorationSprite (string sprName) {
		this.decorationSpriteName = sprName;
	}
}