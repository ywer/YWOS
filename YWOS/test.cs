/*
// Isy's Docked Ships Info
// ==================
// Version: 1.5.1
// Date: 2019-04-09

// =======================================================================================
//                                                                            --- Configuration ---
// =======================================================================================

// --- LCD panels ---
// =======================================================================================

// Just add the keyword below to all your LCD names, where you want to show docked ships on.
// The configuration, what to show, is entirely done via the LCD's custom data.
string keyword = "!docked";

// LCD Master options: This set of options is applied to every new LCD and every LCD that has master options enabled.
// Basic Options
const bool showNumbers = true;
const bool showConnectorName = true;
const bool showBatteryCharge = true;
const bool showHydrogenLevel = true;
const bool showOxygenLevel = true;
const bool showCargoLevel = true;

// Visual settings
const bool compactHeading = false;
const bool compactShipStats = false;
const bool scrollText = true;
const bool showFreeConnectors = true;
const bool sortByConnector = true;
const bool emptyLineBetweenEntries = true;

// Monitored connectors:
const bool showGroupHeading = true;


// --- Script execution ---
// =======================================================================================

// Script cycle time in seconds (default: 2).
double scriptExecutionTime = 2;


// =======================================================================================
//                                                                      --- End of Configuration ---
//                                                        Don't change anything beyond this point!
// =======================================================================================


// Lists
List<IMyShipConnector> allConnectors = new List<IMyShipConnector>();
HashSet<IMyShipConnector> freeConnectors = new HashSet<IMyShipConnector>();
HashSet<IMyCubeGrid> scannedGrids = new HashSet<IMyCubeGrid>();

// Ship dictionary
SortedDictionary<string, List<string>> ships = new SortedDictionary<string, List<string>>();

// Ship stats (List contains 6 items: charge[0], chargeMax[1], hydro ratio[2], oxy ratio[3], cargo[4], cargoMax[5])
Dictionary<string, List<double>> shipStats = new Dictionary<string, List<double>>();

// LCD variables
string[] workingIndicator = { "/", "-", "\\", "|" };
int workingCounter = 0;
string[] defaultData = {
	"Master options (disable to set individual options for this LCD):",
	"================================================",
	"useMasterOptions=true",
	"",
	"Basic settings:",
	"============",
	"showNumbers=" + showNumbers.ToString(),
	"showConnectorName=" + showConnectorName.ToString(),
	"showBatteryCharge=" + showBatteryCharge.ToString(),
	"showHydrogenLevel=" + showHydrogenLevel.ToString(),
	"showOxygenLevel=" + showOxygenLevel.ToString(),
	"showCargoLevel=" + showCargoLevel.ToString(),
	"",
	"Visual settings:",
	"============",
	"compactHeading=" + compactHeading.ToString(),
	"compactShipStats=" + compactShipStats.ToString(),
	"scrollText=" + scrollText.ToString(),
	"showFreeConnectors=" + showFreeConnectors.ToString(),
	"sortByConnector=" + sortByConnector.ToString(),
	"emptyLineBetweenEntries=" + emptyLineBetweenEntries.ToString(),
	"",
	"Monitored connectors:",
	"=================",
	"Put a single connector name or a groupname here.",
	"Leave blank to monitor all!",
	"monitoredConnectors=",
	"showGroupHeading=" + showGroupHeading.ToString(),
};

// Variables
bool gameModeChecked = false;
bool creativeMode = false;
bool firstRun = true;

// Script timing variables
int ticksSinceLastRun = 0;
int ticksPerScriptStep = 0;
bool refreshLCDs = true;
bool init = true;
int execCounter = 1;

// Debugging
double maxInstructions, maxRuntime;
int avgCounter = 0;
List<int> instructions = new List<int>(new int[100]);
List<double> runtime = new List<double>(new double[100]);
Dictionary<string, int> instructionsPerMethodDict = new Dictionary<string, int>();
string[] methodName = {
	"",
	"Finding ships",
	"Getting ship stats",
};

// Pre-Run preparations
public Program()
{
	ticksPerScriptStep = (int)(scriptExecutionTime * 60 / 2);

	// Set UpdateFrequency for starting the programmable block over and over again
	Runtime.UpdateFrequency = UpdateFrequency.Update1;
}


// =======================================================================================
// Main program
// =======================================================================================

void Main()
{
	// Script timing
	if (ticksSinceLastRun < ticksPerScriptStep)
	{
		ticksSinceLastRun++;
		return;
	}
	else
	{
		// Get all blocks, the script should use
		if (init)
		{
			GetBlocks();

			init = false;
			return;
		}

		// Refresh the LCD between executions
		if (refreshLCDs)
		{
			WriteLCD();

			refreshLCDs = false;
			return;
		}

		ticksSinceLastRun = 0;
		init = true;
		refreshLCDs = true;
	}


	// Main script functions
	try
	{
		if (execCounter == 1)
		{
			// Find and store all docked ships
			FindShips();
		}

		if (execCounter == 2)
		{
			// Get the stats of all ships
			GetShipStats();
		}
	}
	catch (Exception e)
	{
		string info = e + " \n\n";
		info += "The error occured while executing the following script step:\n" + methodName[execCounter] + " (ID: " + execCounter + ")";
		throw new Exception(info);
	}

	// Write terminal message
	WriteTerminal();

	// Update the script execution counter
	if (execCounter >= 2)
	{
		// Disable first run after the first full cycle
		if (firstRun) firstRun = false;

		// Reset the counter
		execCounter = 1;
	}
	else
	{
		execCounter++;
	}

	// Update the working counter for the LCDs
	workingCounter = workingCounter >= 3 ? 0 : workingCounter + 1;
}


/// <summary>
/// Gets all blocks that should be used by the script
/// </summary>
void GetBlocks()
{
	// Get base grid of the PB (lowest grid in a chain of pistons or rotors)
	if (baseGrid == null)
	{
		GetBaseGrid(Me.CubeGrid);
	}

	// Get connected grids
	GetConnectedGrids(baseGrid, true);

	// Connectors
	GridTerminalSystem.GetBlocksOfType<IMyShipConnector>(allConnectors, c => connectedGrids.Contains(c.CubeGrid));
}


/// <summary>
/// FindShips stores all ships with their connector in the ships dictionary
/// </summary>
void FindShips()
{
	ships.Clear();
	scannedGrids.Clear();
	freeConnectors.Clear();

	// Only continue if connectors were found
	if (allConnectors.Count > 0)
	{
		foreach (var connector in allConnectors)
		{
			// Get all connected connectors and save them in the dictionary: shipname => connected connector name
			if (connector.Status == MyShipConnectorStatus.Connected)
			{
				ships[connector.OtherConnector.CubeGrid.CustomName] = new List<string> { connector.CustomName, connector.CustomName };
				scannedGrids.Add(connector.CubeGrid);
				ScanRecursive(connector.OtherConnector, connector.CustomName, connector.CustomName);
			}
			else
			{
				freeConnectors.Add(connector);
			}
		}
	}
}


/// <summary>
/// Scans grids recursively for other connected ships based on the connector
/// </summary>
/// <param name="connector">The connector on a grid to scan</param>
/// <param name="mainConnector">The main connector of the chain for the ships dicitonary</param>
void ScanRecursive(IMyShipConnector connector, string mainConnector, string connectorChain)
{
	// Add the current grid to the grids list
	scannedGrids.Add(connector.CubeGrid);

	// Get all connectors on the grid
	List<IMyShipConnector> shipConnectors = new List<IMyShipConnector>();
	GridTerminalSystem.GetBlocksOfType<IMyShipConnector>(shipConnectors, c => c.CubeGrid == connector.CubeGrid);

	// Search for connectors again
	foreach (var shipConnector in shipConnectors)
	{
		if (shipConnector.Status == MyShipConnectorStatus.Connected)
		{
			if (!scannedGrids.Contains(shipConnector.OtherConnector.CubeGrid))
			{
				string currentConnectorChain = connectorChain + " -> " + shipConnector.CustomName;
				ships[shipConnector.OtherConnector.CubeGrid.CustomName] = new List<string> { mainConnector, currentConnectorChain };
				ScanRecursive(shipConnector.OtherConnector, mainConnector, currentConnectorChain);
			}
		}
	}
}


/// <summary>
/// Gets the stats: battery charge, hydrogen level, oxygen level and cargo level of all ships
/// </summary>
void GetShipStats()
{
	shipStats.Clear();

	List<IMyBatteryBlock> batteries = new List<IMyBatteryBlock>();
	List<IMyGasTank> hydrogenTanks = new List<IMyGasTank>();
	List<IMyGasTank> oxygenTanks = new List<IMyGasTank>();
	List<IMyCargoContainer> containers = new List<IMyCargoContainer>();
	List<IMyShipConnector> connectors = new List<IMyShipConnector>();
	List<IMyCockpit> cockpits = new List<IMyCockpit>();
	List<IMyTerminalBlock> inventories = new List<IMyTerminalBlock>();

	foreach (var ship in ships)
	{
		string shipName = ship.Key;
		shipStats[shipName] = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0 };

		// Get the ship's blocks and store them in the lists
		GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(batteries, b => b.CubeGrid.CustomName == shipName);
		GridTerminalSystem.GetBlocksOfType<IMyGasTank>(hydrogenTanks, t => t.CubeGrid.CustomName == shipName && t.BlockDefinition.SubtypeId.Contains("Hydrogen"));
		GridTerminalSystem.GetBlocksOfType<IMyGasTank>(oxygenTanks, t => t.CubeGrid.CustomName == shipName && !t.BlockDefinition.SubtypeId.Contains("Hydrogen"));
		GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(containers, b => b.CubeGrid.CustomName == shipName);
		GridTerminalSystem.GetBlocksOfType<IMyShipConnector>(connectors, b => b.CubeGrid.CustomName == shipName);
		GridTerminalSystem.GetBlocksOfType<IMyCockpit>(cockpits, b => b.CubeGrid.CustomName == shipName);

		inventories.Clear();
		inventories.AddRange(containers);
		inventories.AddRange(connectors);
		inventories.AddRange(cockpits);

		if (!gameModeChecked)
		{
			if (inventories.Count != 0)
			{
				if (inventories[0].GetInventory().MaxVolume == VRage.MyFixedPoint.MaxValue || inventories[0].GetInventory().MaxVolume == 0) creativeMode = true;
				gameModeChecked = true;
			}
		}

		// Cycle through all lists and store the values in the ship stats dictionary
		foreach (var battery in batteries)
		{
			shipStats[shipName][0] += battery.CurrentStoredPower;
			shipStats[shipName][1] += battery.MaxStoredPower;
		}

		foreach (var tank in hydrogenTanks)
		{
			shipStats[shipName][2] += tank.Capacity * tank.FilledRatio;
			shipStats[shipName][3] += tank.Capacity;
		}

		foreach (var tank in oxygenTanks)
		{
			shipStats[shipName][4] += tank.Capacity * tank.FilledRatio;
			shipStats[shipName][5] += tank.Capacity;
		}

		foreach (var inventory in inventories)
		{
			shipStats[shipName][6] += (double)inventory.GetInventory().CurrentVolume;
			shipStats[shipName][7] += (double)inventory.GetInventory().MaxVolume;
		}

		// Creative mode adjustements
		if (creativeMode)
		{
			if (hydrogenTanks.Count > 0) shipStats[shipName][3] = int.MaxValue;
			if (oxygenTanks.Count > 0) shipStats[shipName][5] = int.MaxValue;
			if (inventories.Count > 0) shipStats[shipName][7] = int.MaxValue;
		}
	}
}


/// <summary>
/// Returns a list of connector names
/// </summary>
/// <param name="monitoredConnectors">Groupname of connectors or single connector name</param>
/// <returns>List of strings</returns>
List<string> GetConnectorList(string monitoredConnectors)
{
	List<IMyShipConnector> connectors = new List<IMyShipConnector>();
	List<String> connectorList = new List<String>();

	// If the item is a group, get the connectors and join the list with connectors list
	var group = GridTerminalSystem.GetBlockGroupWithName(monitoredConnectors);

	if (group != null)
	{
		var tempConnectors = new List<IMyShipConnector>();
		group.GetBlocksOfType<IMyShipConnector>(tempConnectors);
		connectors.AddRange(tempConnectors);

	}
	else
	{
		// Else try adding a single connectors
		GridTerminalSystem.GetBlocksOfType(connectors, c => c.CustomName.Contains(monitoredConnectors) && connectedGrids.Contains(c.CubeGrid));
	}

	// Create the connectorList with the names of the found connectors
	foreach (var connector in connectors)
	{
		connectorList.Add(connector.CustomName);
	}

	// Sort the list
	connectorList.Sort((a, b) => a.CompareTo(b));

	return connectorList;
}


/// <summary>
/// Create the information string for terminal and LCD output
/// </summary>
string CreateInformation(float fontSize = 0.65f, int charsPerline = 26,
	bool showNumbers = true, bool showConnectorName = true, bool showBatteryCharge = true, bool showHydrogenLevel = true, bool showOxygenLevel = true, bool showCargoLevel = true,
	bool compactHeading = false, bool compactShipStats = false, bool sortByConnector = false, bool emptyLineBetweenEntries = true,
	string monitoredConnectors = "", bool showGroupHeading = true, bool showFreeConnectors = true)
{
	bool infoShown = false;
	string info = "";

	int counter = 1;
	string counterText = "";
	string tab = "";

	List<String> connectorNames = new List<String>();
	string freeConnector = "- No docked ship -";
	string warning = null;

	if (monitoredConnectors != "")
	{
		connectorNames = GetConnectorList(monitoredConnectors);

		// If no connectors were found, prepare a warning
		if (connectorNames.Count == 0)
		{
			warning = "Connector not found:\n'" + monitoredConnectors + "'\nCheck your LCD's custom data!";
		}
	}

	// Terminal / LCD information string
	info = "Currently docked ships " + workingIndicator[workingCounter] + "\n";

	// Add underline if compactHeading is false
	if (!compactHeading)
	{
		info += "========================\n\n";
	}

	if (firstRun)
	{
		info += CreateBar(fontSize, charsPerline, methodName[execCounter] + ' '.Repeat(18 - methodName[execCounter].Length), execCounter, 2);
		return info;
	}

	// Add warning message for minor errors
	if (warning != null)
	{
		info += "Warning!\n";
		info += warning + "\n\n";
		return info;
	}

	if (connectorNames.Count > 1 && showGroupHeading)
	{
		info += "Group '" + monitoredConnectors + "':\n\n";
	}

	// Shipslist dictionary: Key: Shipname, Value: [0]Main Connectorname, [1]Connectorchain
	Dictionary<string, List<string>> shipsList = new Dictionary<string, List<string>>();

	// Build a special list, if only certain connectors should be monitored
	if (connectorNames.Count != 0)
	{
		foreach (var connector in connectorNames)
		{
			foreach (var ship in ships)
			{
				if (connector == ship.Value[0])
				{
					shipsList[ship.Key] = new List<string> { ship.Value[0], ship.Value[1] };
				}
			}
		}
	}
	else
	{
		// Use all connectors
		foreach (var ship in ships)
		{
			shipsList[ship.Key] = new List<string> { ship.Value[0], ship.Value[1] };
		}
	}

	// Append free connectors
	if (showFreeConnectors)
	{
		if (connectorNames.Count != 0)
		{
			int freeConnectorAmount = 0;

			foreach (var connector in freeConnectors)
			{
				if (connectorNames.Contains(connector.CustomName))
				{
					shipsList[freeConnector + ' '.Repeat(freeConnectorAmount)] = new List<string> { connector.CustomName, connector.CustomName };
					freeConnectorAmount++;
				}
			}
		}
		else
		{
			int freeConnectorAmount = 0;

			foreach (var connector in freeConnectors)
			{
				shipsList[freeConnector + ' '.Repeat(freeConnectorAmount)] = new List<string> { connector.CustomName, connector.CustomName };
				freeConnectorAmount++;
			}
		}
	}

	if (sortByConnector)
	{
		// Sort by connecteor
		var tempList = shipsList.OrderBy(s => s.Value[0]).ToList();
		shipsList.Clear();

		foreach (var item in tempList)
		{
			shipsList[item.Key] = new List<string> { item.Value[0], item.Value[1] };
		}

	}
	else
	{
		// Sort by shipname
		var tempList = shipsList.OrderBy(s => s.Key).ToList();
		shipsList.Clear();

		foreach (var item in tempList)
		{
			shipsList[item.Key] = new List<string> { item.Value[0], item.Value[1] };
		}
	}

	// Check whether ships amount = shipstats amount
	if (ships.Count != shipStats.Count)
	{
		info += "Gathering ship informations...";
		return info;
	}

	// Cycle through all ships
	foreach (var ship in shipsList)
	{
		string shipName = ship.Key;
		bool showStats = true;

		// Check if the current ship is in the stats list
		if (!shipStats.ContainsKey(shipName))
		{
			showStats = false;
		}

		// Numbers
		if (showNumbers)
		{
			counterText = counter + ". ";
			tab = ' '.Repeat(counterText.Length);

			info += counterText;
			counter++;
		}

		// Add shipname and connector name
		if (sortByConnector && showConnectorName)
		{
			info += ship.Value[1] + ":\n";
			info += tab + shipName + "\n";
		}
		else
		{
			info += shipName + "\n";
		}

		// Add Connector
		if (showConnectorName && !sortByConnector)
		{
			info += tab + "At: " + ship.Value[1] + "\n";
		}

		if (showStats)
		{
			string compactStats = tab;
			string comma = "";

			// Add Battery Charge
			if (showBatteryCharge && shipStats[shipName][1] > 0)
			{
				if (compactShipStats)
				{
					compactStats += "Bat " + shipStats[shipName][0].ToPercentString(shipStats[shipName][1]);
					comma = ", ";
				}
				else
				{
					info += CreateBar(fontSize, charsPerline, tab + "Batteries", shipStats[shipName][0], shipStats[shipName][1], shipStats[shipName][0].ToPowerString(true), shipStats[shipName][1].ToPowerString(true), singleLine: true);
				}
			}

			// Add Hydrogen Level
			if (showHydrogenLevel && shipStats[shipName][3] > 0)
			{
				if (compactShipStats)
				{
					compactStats += comma + "H2 " + shipStats[shipName][2].ToPercentString(shipStats[shipName][3]);
					comma = ", ";
				}
				else
				{
					info += CreateBar(fontSize, charsPerline, tab + "H2 Tanks ", shipStats[shipName][2], shipStats[shipName][3], shipStats[shipName][2].ToTankVolumeString(), shipStats[shipName][3].ToTankVolumeString(), singleLine: true);
				}
			}

			// Add Oxygen Level
			if (showOxygenLevel && shipStats[shipName][5] > 0)
			{
				if (compactShipStats)
				{
					compactStats += comma + "O2 " + shipStats[shipName][4].ToPercentString(shipStats[shipName][5]);
					comma = ", ";
				}
				else
				{
					info += CreateBar(fontSize, charsPerline, tab + "O2 Tanks ", shipStats[shipName][4], shipStats[shipName][5], shipStats[shipName][4].ToTankVolumeString(), shipStats[shipName][5].ToTankVolumeString(), singleLine: true);
				}
			}

			// Add Cargo Level
			if (showCargoLevel && shipStats[shipName][7] > 0)
			{
				if (compactShipStats)
				{
					compactStats += comma + "Car " + shipStats[shipName][6].ToPercentString(shipStats[shipName][7]);
				}
				else
				{
					info += CreateBar(fontSize, charsPerline, tab + "Cargo    ", shipStats[shipName][6], shipStats[shipName][7], shipStats[shipName][6].ToCargoVolumeString(), shipStats[shipName][7].ToCargoVolumeString(), singleLine: true);
				}
			}

			if (compactStats != tab)
			{
				info += compactStats + "\n";
			}
		}

		// Add empty line between entries
		if (emptyLineBetweenEntries)
		{
			info += "\n";
		}

		infoShown = true;
	}


	if (!infoShown)
	{
		info += "-- No docked ships --";
	}

	return info;
}


/// <summary>
/// Write the informationsString on all specified LCDs
/// </summary>
void WriteLCD()
{
	List<IMyTextPanel> lcds = new List<IMyTextPanel>();
	GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(lcds, tp => tp.CustomName.Contains(keyword) && connectedGrids.Contains(tp.CubeGrid));

	if (lcds.Count == 0) return;

	HashSet<string> uniqueNames = new HashSet<string>();

	foreach (var lcd in lcds)
	{
		uniqueNames.Add(System.Text.RegularExpressions.Regex.Match(lcd.CustomName, keyword + @":\D+").Value);
	}

	uniqueNames.RemoveWhere(n => n == "");

	foreach (var name in uniqueNames)
	{
		List<IMyTextPanel> lcdGroup = new List<IMyTextPanel>();
		GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(lcdGroup, l => l.CustomName.Contains(name));
		lcds = lcds.Except(lcdGroup).ToList();

		string pattern = keyword + @":\w+";
		lcdGroup.Sort((a, b) => System.Text.RegularExpressions.Regex.Match(a.CustomName, pattern).Value.CompareTo(System.Text.RegularExpressions.Regex.Match(b.CustomName, pattern).Value));
		string info = "";

		// Prrepare the LCD
		float fontSize = lcdGroup[0].FontSize;
		int charsPerline = 26;
		if (lcdGroup[0].BlockDefinition.SubtypeName.Contains("Wide")) charsPerline = 52;
		int headingHeight = 3;
		bool scrollTextLcd = true;

		if (ReadCDBool(lcdGroup[0], "useMasterOptions"))
		{
			// Use master options
			info = CreateInformation(fontSize, charsPerline,
				showNumbers, showConnectorName, showBatteryCharge, showHydrogenLevel, showOxygenLevel, showCargoLevel,
				compactHeading, compactShipStats, sortByConnector, emptyLineBetweenEntries,
				ReadCDString(lcdGroup[0], "monitoredConnectors"), showGroupHeading, showFreeConnectors);
		}
		else
		{
			// Use custom options
			info = CreateInformation(fontSize, charsPerline,
				ReadCDBool(lcdGroup[0], "showNumbers"), ReadCDBool(lcdGroup[0], "showConnectorName"), ReadCDBool(lcdGroup[0], "showBatteryCharge"), ReadCDBool(lcdGroup[0], "showHydrogenLevel"), ReadCDBool(lcdGroup[0], "showOxygenLevel"), ReadCDBool(lcdGroup[0], "showCargoLevel"),
				ReadCDBool(lcdGroup[0], "compactHeading"), ReadCDBool(lcdGroup[0], "compactShipStats"), ReadCDBool(lcdGroup[0], "sortByConnector"), ReadCDBool(lcdGroup[0], "emptyLineBetweenEntries"),
				ReadCDString(lcdGroup[0], "monitoredConnectors"), ReadCDBool(lcdGroup[0], "showGroupHeading"), ReadCDBool(lcdGroup[0], "showFreeConnectors"));
			scrollTextLcd = ReadCDBool(lcdGroup[0], "scrollText");
		}

		info = CreateScrollingText(fontSize, info, lcdGroup[0], headingHeight, scrollTextLcd, lcdGroup.Count);

		var lines = info.Split('\n');
		int totalLines = lines.Length;
		int writtenLines = 0;
		int linesPerLCD = (int)Math.Ceiling(17 / fontSize);

		foreach (var lcd in lcdGroup)
		{
			lcd.WritePublicTitle("Isy's Docked Ship Names (LCD group: " + name.Replace(keyword + ":", "") + ")");
			lcd.FontSize = fontSize;
			lcd.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.LEFT;
			lcd.Font = "Monospace";
			lcd.TextPadding = 0;
			lcd.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;

			int writtenCurrent = 0;
			string newText = "";
			while (writtenLines < totalLines && writtenCurrent < linesPerLCD)
			{
				newText += lines[writtenLines] + "\n";
				writtenLines++;
				writtenCurrent++;
			}

			lcd.WriteText(newText);
		}
	}

	foreach (var lcd in lcds)
	{
		string info = "";

		// Prrepare the LCD
		float fontSize = lcd.FontSize;
		int charsPerline = 26;
		if (lcd.BlockDefinition.SubtypeName.Contains("Wide")) charsPerline = 52;
		int headingHeight = 3;
		bool scrollTextLcd = true;

		if (ReadCDBool(lcd, "useMasterOptions"))
		{
			// Use master options
			info = CreateInformation(fontSize, charsPerline,
				showNumbers, showConnectorName, showBatteryCharge, showHydrogenLevel, showOxygenLevel, showCargoLevel,
				compactHeading, compactShipStats, sortByConnector, emptyLineBetweenEntries,
				ReadCDString(lcd, "monitoredConnectors"), showGroupHeading, showFreeConnectors);
		}
		else
		{
			// Use custom options
			info = CreateInformation(fontSize, charsPerline,
				ReadCDBool(lcd, "showNumbers"), ReadCDBool(lcd, "showConnectorName"), ReadCDBool(lcd, "showBatteryCharge"), ReadCDBool(lcd, "showHydrogenLevel"), ReadCDBool(lcd, "showOxygenLevel"), ReadCDBool(lcd, "showCargoLevel"),
				ReadCDBool(lcd, "compactHeading"), ReadCDBool(lcd, "compactShipStats"), ReadCDBool(lcd, "sortByConnector"), ReadCDBool(lcd, "emptyLineBetweenEntries"),
				ReadCDString(lcd, "monitoredConnectors"), ReadCDBool(lcd, "showGroupHeading"), ReadCDBool(lcd, "showFreeConnectors"));
			scrollTextLcd = ReadCDBool(lcd, "scrollText");
		}

		info = CreateScrollingText(fontSize, info, lcd, headingHeight, scrollTextLcd);

		// Print contents to its public text
		lcd.WritePublicTitle("Isy's Docked Ship Names");
		lcd.WriteText(info, false);
		lcd.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.LEFT;
		lcd.Font = "Monospace";
		lcd.TextPadding = 0;
		lcd.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
	}
}


void WriteTerminal()
{
	if (avgCounter == 99)
	{
		avgCounter = 0;
	}
	else
	{
		avgCounter++;
	}

	string performanceText = "Isy's Docked Ships Info " + workingIndicator[workingCounter] + "\n====================\n\n";
	performanceText += "The script is running.\n\n";
	performanceText += "Build LCD panels and add the keyword '" + keyword + "' to their name.\n\n";
	performanceText += "The configuration is done per LCD via their custom data field.\n\n";

	performanceText += "Task: " + methodName[execCounter] + "\n";
	performanceText += "Script step: " + execCounter + " / " + (methodName.Length - 1) + "\n\n";

	int curInstructions = Runtime.CurrentInstructionCount;
	if (curInstructions > maxInstructions) maxInstructions = curInstructions;
	instructions[avgCounter] = curInstructions;
	double avgInstructions = instructions.Sum() / instructions.Count;

	performanceText += "Instructions: " + curInstructions + " / " + Runtime.MaxInstructionCount + "\n";
	performanceText += "Max. Instructions: " + maxInstructions + " / " + Runtime.MaxInstructionCount + "\n";
	performanceText += "Avg. Instructions: " + Math.Floor(avgInstructions) + " / " + Runtime.MaxInstructionCount + "\n\n";

	double curRuntime = Runtime.LastRunTimeMs;
	if (curRuntime > maxRuntime) maxRuntime = curRuntime;
	runtime[avgCounter] = curRuntime;
	double avgRuntime = runtime.Sum() / runtime.Count;

	performanceText += "Last runtime " + Math.Round(curRuntime, 4) + " ms\n";
	performanceText += "Max. runtime " + Math.Round(maxRuntime, 4) + " ms\n";
	performanceText += "Avg. runtime " + Math.Round(avgRuntime, 4) + " ms\n\n";

	performanceText += "Instructions per Method:\n===================\n\n";
	instructionsPerMethodDict[methodName[execCounter]] = curInstructions;

	foreach (var item in instructionsPerMethodDict.OrderByDescending(i => i.Value))
	{
		performanceText += "- " + item.Key + ": " + item.Value + "\n";
	}
	performanceText += "\n";

	Echo(performanceText);
}


/// <summary>
/// Read a LCD's custom data
/// </summary>
/// <param name="lcd">LCD to read</param>
/// <param name="field">Field to read</param>
/// <returns>Fieldvalue as bool</returns>
bool ReadCDBool(IMyTextPanel lcd, string field)
{
	CheckCD(lcd);
	var customData = lcd.CustomData.Replace(" ", "").Split('\n');

	foreach (var line in customData)
	{
		if (line.Contains(field + "="))
		{
			try
			{
				return Convert.ToBoolean(line.Replace(field + "=", ""));
			}
			catch
			{
				return true;
			}
		}
	}

	return true;
}


/// <summary>
/// Read a LCD's custom data
/// </summary>
/// <param name="lcd">LCD to read</param>
/// <param name="field">Field to read</param>
/// <returns>Fieldvalue as string</returns>
string ReadCDString(IMyTextPanel lcd, string field)
{
	CheckCD(lcd);
	var customData = lcd.CustomData.Split('\n');

	foreach (var line in customData)
	{
		if (line.Contains(field + "="))
		{
			return line.Replace(field + "=", "");
		}
	}

	return "";
}


/// <summary>
/// Checks a LCD's custom data and restores the default custom data, if it is too short
/// </summary>
/// <param name="lcd">LCD to check</param>
void CheckCD(IMyTextPanel lcd)
{
	var customData = lcd.CustomData.Split('\n');

	// Create new default customData if a too short one is found and set the default font size
	if (customData.Length != defaultData.Length)
	{
		string newCustomData = "";

		foreach (var item in defaultData)
		{
			newCustomData += item + "\n";
		}

		lcd.CustomData = newCustomData.TrimEnd('\n');
		lcd.FontSize = 0.5f;
	}
}

string CreateBar(double fontSize, int charsPerLine, string heading, double value, double valueMax, string valueStr = null, string valueMaxStr = null, bool noBar = false, bool singleLine = false)
{
	string current = value.ToString();
	string max = valueMax.ToString();

	if (valueStr != null)
	{
		current = valueStr;
	}

	if (valueMaxStr != null)
	{
		max = valueMaxStr;
	}

	string percent = value.ToPercentString(valueMax);
	percent = ' '.Repeat(6 - percent.Length) + percent;
	string values = current + " / " + max;
	int lcdWidth = (int)(charsPerLine / fontSize);

	double level = 0;
	if (valueMax > 0) level = value / valueMax >= 1 ? 1 : value / valueMax;

	StringBuilder firstLine = new StringBuilder(heading + " ");
	StringBuilder secondLine = new StringBuilder();

	if (singleLine)
	{
		if (fontSize <= 0.5 || (fontSize <= 1 && charsPerLine == 52))
		{
			// Create the bar for wide LCDs
			firstLine.Append(' '.Repeat(9 - current.Length) + current);
			firstLine.Append(" / " + max + ' '.Repeat(9 - max.Length));

			int dotStart = firstLine.Length + 1;
			int dotsAmount = lcdWidth - firstLine.Length - percent.Length - 2;
			int fillLevel = (int)Math.Ceiling(dotsAmount * level);

			firstLine.Append("[" + 'I'.Repeat(fillLevel) + '.'.Repeat(dotsAmount - fillLevel) + "]");
			firstLine.Append(percent + "\n");
		}
		else
		{
			// Create the bar for regular and corner LCDs
			int dotsAmount = lcdWidth - firstLine.Length - percent.Length - 2;
			int fillLevel = (int)Math.Ceiling(dotsAmount * level);

			firstLine.Append("[" + 'I'.Repeat(fillLevel) + '.'.Repeat(dotsAmount - fillLevel) + "]");
			firstLine.Append(percent + "\n");
		}

		return firstLine.ToString();
	}
	else
	{
		if (fontSize <= 0.6 || (fontSize <= 1 && charsPerLine == 52))
		{
			firstLine.Append(' '.Repeat(lcdWidth / 2 - (firstLine.Length + current.Length)));
			firstLine.Append(current + " / " + max);
			firstLine.Append(' '.Repeat(lcdWidth - (firstLine.Length + percent.Length)));
			firstLine.Append(percent + "\n");

			if (!noBar)
			{
				int dotsAmount = lcdWidth - 2;
				int fillLevel = (int)Math.Ceiling(dotsAmount * level);
				secondLine = new StringBuilder("[" + 'I'.Repeat(fillLevel) + '.'.Repeat(dotsAmount - fillLevel) + "]\n");
			}
		}
		else
		{
			firstLine.Append(' '.Repeat(lcdWidth - (firstLine.Length + values.Length)));
			firstLine.Append(values + "\n");

			if (!noBar)
			{
				int dotsAmount = lcdWidth - percent.Length - 2;
				int fillLevel = (int)Math.Ceiling(dotsAmount * level);
				secondLine = new StringBuilder("[" + 'I'.Repeat(fillLevel) + '.'.Repeat(dotsAmount - fillLevel) + "]");
				secondLine.Append(percent + "\n");
			}
		}

		return firstLine.Append(secondLine).ToString();
	}
}

DateTime scrollTime = DateTime.Now;
Dictionary<long, List<int>> scroll = new Dictionary<long, List<int>>();

string CreateScrollingText(float fontSize, string text, IMyTextPanel lcd, int headingHeight = 3, bool scrollText = true, int lcdAmount = 1)
{
	// Get the LCD EntityId
	long id = lcd.EntityId;

	// Create default entry for the LCD in the dictionary
	if (!scroll.ContainsKey(id))
	{
		scroll[id] = new List<int> { 1, 3, headingHeight, 0 };
	}

	int scrollDirection = scroll[id][0];
	int scrollWait = scroll[id][1];
	int lineStart = scroll[id][2];
	int scrollSecond = scroll[id][3];

	// Figure out the amount of lines for scrolling content
	var linesTemp = text.TrimEnd('\n').Split('\n');
	List<string> lines = new List<string>();
	int lcdHeight = (int)Math.Ceiling(17 / fontSize * lcdAmount);
	int lcdWidth = (int)(26 / fontSize);
	string lcdText = "";

	// Adjust height for corner LCDs
	if (lcd.BlockDefinition.SubtypeName.Contains("Corner"))
	{
		if (lcd.CubeGrid.GridSize == 0.5)
		{
			lcdHeight = (int)Math.Floor(5 / fontSize);
		}
		else
		{
			lcdHeight = (int)Math.Floor(3 / fontSize);
		}
	}

	// Adjust width for wide LCDs
	if (lcd.BlockDefinition.SubtypeName.Contains("Wide"))
	{
		lcdWidth = (int)(52 / fontSize);
	}

	// Build the lines list out of lineTemp and add line breaks if text is too long for one line
	foreach (var line in linesTemp)
	{
		if (line.Length <= lcdWidth)
		{
			lines.Add(line);
		}
		else
		{
			try
			{
				string currentLine = "";
				var words = line.Split(' ');
				string number = System.Text.RegularExpressions.Regex.Match(line, @".+(\.|\:)\ ").Value;
				string tab = ' '.Repeat(number.Length);

				foreach (var word in words)
				{
					if ((currentLine + " " + word).Length > lcdWidth)
					{
						lines.Add(currentLine);
						currentLine = tab + word + " ";
					}
					else
					{
						currentLine += word + " ";
					}
				}

				lines.Add(currentLine);
			}
			catch
			{
				lines.Add(line);
			}
		}
	}

	if (scrollText)
	{
		if (lines.Count > lcdHeight)
		{
			if (DateTime.Now.Second != scrollSecond)
			{
				scrollSecond = DateTime.Now.Second;
				if (scrollWait > 0) scrollWait--;
				if (scrollWait <= 0) lineStart += scrollDirection;

				if (lineStart + lcdHeight - headingHeight >= lines.Count && scrollWait <= 0)
				{
					scrollDirection = -1;
					scrollWait = 3;
				}
				if (lineStart <= headingHeight && scrollWait <= 0)
				{
					scrollDirection = 1;
					scrollWait = 3;
				}
			}
		}
		else
		{
			lineStart = headingHeight;
			scrollDirection = 1;
			scrollWait = 3;
		}

		// Save the current scrolling in the dictionary
		scroll[id][0] = scrollDirection;
		scroll[id][1] = scrollWait;
		scroll[id][2] = lineStart;
		scroll[id][3] = scrollSecond;
	}
	else
	{
		lineStart = headingHeight;
	}

	// Always create header
	for (var line = 0; line < headingHeight; line++)
	{
		lcdText += lines[line] + "\n";
	}

	// Create content based on the starting line
	for (var line = lineStart; line < lines.Count; line++)
	{
		lcdText += lines[line] + "\n";
	}

	return lcdText;
}

IMyCubeGrid baseGrid = null;
HashSet<IMyCubeGrid> checkedGrids = new HashSet<IMyCubeGrid>();

void GetBaseGrid(IMyCubeGrid currentGrid)
{
	checkedGrids.Add(currentGrid);

	List<IMyMotorStator> scanRotors = new List<IMyMotorStator>();
	List<IMyPistonBase> scanPistons = new List<IMyPistonBase>();
	GridTerminalSystem.GetBlocksOfType<IMyMotorStator>(scanRotors, r => r.IsAttached && r.TopGrid == currentGrid && !checkedGrids.Contains(r.CubeGrid));
	GridTerminalSystem.GetBlocksOfType<IMyPistonBase>(scanPistons, p => p.IsAttached && p.TopGrid == currentGrid && !checkedGrids.Contains(p.CubeGrid));

	if (scanRotors.Count == 0 && scanPistons.Count == 0)
	{
		baseGrid = currentGrid;
		return;
	}
	else
	{
		foreach (var rotor in scanRotors)
		{
			GetBaseGrid(rotor.CubeGrid);
		}

		foreach (var piston in scanPistons)
		{
			GetBaseGrid(piston.CubeGrid);
		}
	}
}

HashSet<IMyCubeGrid> connectedGrids = new HashSet<IMyCubeGrid>();

void GetConnectedGrids(IMyCubeGrid currentGrid, bool clearGridList = false)
{
	if (clearGridList) connectedGrids.Clear();

	connectedGrids.Add(currentGrid);

	List<IMyMotorStator> scanRotors = new List<IMyMotorStator>();
	List<IMyPistonBase> scanPistons = new List<IMyPistonBase>();
	GridTerminalSystem.GetBlocksOfType<IMyMotorStator>(scanRotors, r => r.CubeGrid == currentGrid && r.IsAttached && !connectedGrids.Contains(r.TopGrid));
	GridTerminalSystem.GetBlocksOfType<IMyPistonBase>(scanPistons, p => p.CubeGrid == currentGrid && p.IsAttached && !connectedGrids.Contains(p.TopGrid));

	foreach (var rotor in scanRotors)
	{
		GetConnectedGrids(rotor.TopGrid);
	}

	foreach (var piston in scanPistons)
	{
		GetConnectedGrids(piston.TopGrid);
	}
}

}
public static partial class Extensions
{
	public static string Repeat(this char charToRepeat, int numberOfRepetitions)
	{
		if (numberOfRepetitions <= 0)
		{
			return "";
		}
		return new string(charToRepeat, numberOfRepetitions);
	}
}

public static partial class Extensions
{
	public static string ToCargoVolumeString(this double value)
	{
		string unit = "kL";

		if (value < 1)
		{
			value *= 1000;
			unit = "L";
		}
		else if (value >= 1000 && value < 1000000)
		{
			value /= 1000;
			unit = "ML";
		}
		else if (value >= 1000000 && value < 1000000000)
		{
			value /= 1000000;
			unit = "BL";
		}
		else if (value >= 1000000000)
		{
			value /= 1000000000;
			unit = "TL";
		}

		return Math.Round(value, 1) + " " + unit;
	}
}

public static partial class Extensions
{
	public static string ToPercentString(this double numerator, double denominator)
	{
		double percentage = Math.Round(numerator / denominator * 100, 1);
		if (denominator == 0)
		{
			return "0%";
		}
		else
		{
			return percentage + "%";
		}
	}
}

public static partial class Extensions
{
	public static string ToPowerString(this double value, bool wattHours = false)
	{
		string unit = "MW";

		if (value < 1)
		{
			value *= 1000;
			unit = "kW";
		}
		else if (value >= 1000 && value < 1000000)
		{
			value /= 1000;
			unit = "GW";
		}
		else if (value >= 1000000 && value < 1000000000)
		{
			value /= 1000000;
			unit = "TW";
		}
		else if (value >= 1000000000)
		{
			value /= 1000000000;
			unit = "PW";
		}

		if (wattHours) unit += "h";

		return Math.Round(value, 1) + " " + unit;
	}
}

public static partial class Extensions
{
	public static string ToTankVolumeString(this double value)
	{
		string unit = "L";

		if (value >= 1000 && value < 1000000)
		{
			value /= 1000;
			unit = "KL";
		}
		else if (value >= 1000000 && value < 1000000000)
		{
			value /= 1000000;
			unit = "ML";
		}
		else if (value >= 1000000000 && value < 1000000000000)
		{
			value /= 1000000000;
			unit = "BL";
		}
		else if (value >= 1000000000000)
		{
			value /= 1000000000000;
			unit = "TL";
		}

		return Math.Round(value, 1) + " " + unit;
	}
	*/