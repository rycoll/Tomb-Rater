﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageYears  {

	private const int CALENDAR_SIZE = 100;

	private Year[] calendar;
	private Year[] bufferCalendar;
	private int yearIndex;

	private string yearReport;

	public ManageYears (int currentYear) {
		calendar = new Year[CALENDAR_SIZE];
		yearIndex = 0;
		for (int n = 0; n < CALENDAR_SIZE; n++) {
			calendar [n] = new Year (n + currentYear);
		}
	}

	public Year getCurrentYear () {
		return calendar [yearIndex];
	}
	public int getYearIndex () {
		return yearIndex;
	}
	public void setYearIndex (int n) {
		yearIndex = n;
	}

	public Year getYear (int yearNum) {
		if (yearNum < CALENDAR_SIZE) {
			return calendar [yearNum];
		} else {
			//this is just handling for cases where the game goes for lots of turns
			//this will probably never happen
			if (bufferCalendar == null) {
				bufferCalendar = new Year[CALENDAR_SIZE];
			}
			return bufferCalendar [yearNum - CALENDAR_SIZE];
		}
	}

	public void addSpecialEventInXYears (SpecialEvent specialEvent, int numYears) {
		Year year = getYear (yearIndex + numYears);
		year.addSpecialEvent (specialEvent);
	}

	public void progressThroughCurrentYear (GameController gameController) {
		Year currentYear = calendar [yearIndex];
		yearReport = "";

		//Health - get a little closer to death
		PathToDeath health = gameController.getHealth();
		health.decrement ();
		if (health.checkThreshold ()) {
			SpecialEvent nextHealthEvent = health.triggerNextSpecialEvent ();
			if (nextHealthEvent != null) {
				addSpecialEventInXYears (nextHealthEvent, 0);
			}
		}

		//Revolution - decrement counter according to public disfavour
		PathToDeath revolution = gameController.getRevolution();
		ManageOpinion opinion = gameController.getOpinionManagement ();
		if (opinion.getNetFavour () < 0) {
			revolution.increment (opinion.getNetFavour () * 2);
			if (revolution.checkThreshold ()) {
				SpecialEvent nextRevolutionEvent = revolution.triggerNextSpecialEvent ();
				if (nextRevolutionEvent != null) {
					addSpecialEventInXYears (nextRevolutionEvent, 0);
				}
			}
		}

		//Builders - build
		ManageBuilding buildingManagement = gameController.getBuildingManagement();
		if (BuildingMenu.currentlyBuilding != null) {
			bool success = buildingManagement.addRoom (BuildingMenu.currentlyBuilding, BuildingMenu.selectedTiles.ToArray (), BuildingMenu.materialToUse);
			if (!success) {
				Debug.Log ("Something went wrong in the building process! Error!");
			} else {
				yearReport += BuildingMenu.currentlyBuilding.getName () + " was built!\n";
			}
			BuildingMenu.currentlyBuilding = null;
			BuildingMenu.selectedTiles.Clear ();
		}

		//Advisors
		ManageAdvisors advisorManagement = gameController.getAdvisorManagement();
		foreach (Advisor advisor in advisorManagement.getAdvisors()) {
			if (advisor != null && advisor.getMilestone() != null) {
				AdvisorMilestone milestone = advisor.getMilestone ();
				int overflow = milestone.pay (advisor.getPayment ());
				gameController.setMoney (gameController.getMoney () - advisor.getPayment ());
				if (milestone.getPayment () >= milestone.getThreshold ()) {
					yearReport += milestone.reward () + "\n";
					advisor.proceedToNextMilestone ();
					if (overflow > 0 && advisor.getMilestone() != null) {
						advisor.getMilestone ().pay (overflow);
					}
				}
			}
		}
		// yearly boons
		gameController.setMoney(gameController.getMoney() + advisorManagement.getGPT());
		yearReport += "The Kingdom brought in " + advisorManagement.getGPT () + "g this year.";

		//run special events, move on once they're all done
		//shift to "turn" scene, give script all the info it needs
		//current picking just 1 random event each year (33% chance of 0 events picked)
		ManageSpecialEvents specialEventManagement = gameController.getSpecialEventManagement();
		SpecialEvent randEvent = specialEventManagement.chooseSpecialEventRandomly ();
		int chance = Random.Range (0, 3);
		Debug.Log ("chance " + chance);
		if (randEvent != null && chance > 0) {
			currentYear.addSpecialEvent (randEvent);
		}
		TurnoverScene.specialEvents = currentYear.getEvents().ToArray();
		TurnoverScene.eventIndex = 0;
		TurnoverScene.yearlyReport = yearReport;
		gameController.loadScene ("turn");

	}

	public void progressToNextYear () {
		//move index to the next year
		int currentYearName = getCurrentYear().getYearName() + 1;
		yearIndex++;
		if (yearIndex >= CALENDAR_SIZE) {
			//this is just handling for cases where the game goes for lots of turns
			//this will probably never happen
			if (bufferCalendar == null) {
				bufferCalendar = new Year[CALENDAR_SIZE];
				for (int n = 0; n < CALENDAR_SIZE; n++) {
					bufferCalendar [n] = new Year (n + currentYearName);
				}
			}
			calendar = bufferCalendar;
			bufferCalendar = null;
			yearIndex = 0;
		}
	}
		
	public Year[] getCalendar () {
		return calendar;
	}
	public Year[] getBufferCalendar () {
		return bufferCalendar;
	}
	public void setCalendar (Year[] c) {
		calendar = c;
	}
	public void setBufferCalendar(Year[] c) {
		bufferCalendar = c;
	}
}
