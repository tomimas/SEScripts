//by tomimas
//https://github.com/tomimas/SEScripts   
    
void Main(string argument) {
  string STATUS_PANEL = "LCD (Automation)";  
  IMyTextPanel panel = GridTerminalSystem.GetBlockWithName(STATUS_PANEL) as IMyTextPanel;
  panel.SetValueColor("FontColor", new Color(255, 255, 255, 255));
  rewrite(panel, "");

  checkAirlockDoorInnerState();
  checkOxygenTankState(panel);
  checkAirVentState();
  checkReactorState(panel);
}

void checkAirVentState() {
  List<IMyTerminalBlock> airVents = new List<IMyTerminalBlock>();
  GridTerminalSystem.GetBlocksOfType<IMyAirVent>(airVents);
  
  for (int i = 0; i < airVents.Count; i++) {
      IMyAirVent airVent = airVents[i] as IMyAirVent;
      if (!airVent.IsDepressurizing && (airVent.GetOxygenLevel() > 0.95f) && airVent.IsWorking) {
        airVent.ApplyAction("OnOff_Off");
      }
      
      if (airVent.CanPressurize && !airVent.IsWorking && !airVent.IsDepressurizing && (airVent.GetOxygenLevel() < 0.7f)) {
        airVent.ApplyAction("OnOff_On");
      }
      
      if (airVent.IsDepressurizing && !airVent.IsWorking) {
        airVent.ApplyAction("OnOff_On");
      }
    }
}

void checkAirlockDoorInnerState() {
  List<IMyTerminalBlock> doors = new List<IMyTerminalBlock>();
  GridTerminalSystem.GetBlocksOfType<IMyDoor>(doors);
  
  for (int i = 0; i < doors.Count; i++) {
      IMyDoor door = doors[i] as IMyDoor;
      if (door.IsWorking && (door.OpenRatio == 1f) && door.CustomName.StartsWith("Airlock Door Inner (")) {
        door.ApplyAction("Open_Off");
      }
    }
}

void checkOxygenTankState(IMyTextPanel panel) {
  List<IMyTerminalBlock> tanks = new List<IMyTerminalBlock>();
  GridTerminalSystem.GetBlocksOfType<IMyOxygenTank>(tanks);
  IMyOxygenGenerator generator = GridTerminalSystem.GetBlockWithName("Oxygen Generator") as IMyOxygenGenerator;
  generator.ApplyAction("OnOff_Off");

  for (int i = 0; i < tanks.Count; i++) {
    IMyOxygenTank tank = tanks[i] as IMyOxygenTank;
    if (tank.IsWorking && (tank.GetOxygenLevel() < 0.5f)) {
      generator.ApplyAction("OnOff_On");
    }
  }
  
  if (generator.GetUseConveyorSystem()) {
    generator.SetUseConveyorSystem(false);
  }
  
  if (generator.HasInventory()) {
    float totalIce = 0f; 
    List<IMyInventoryItem> items = new List<IMyInventoryItem>();
    IMyInventory inventory = generator.GetInventory(0); 
    items.AddRange(inventory.GetItems());

    for (int j = 0; j< items.Count; j++) { 
      if (items[j].Content.SubtypeName == "Ice") { 
        totalIce += (float) items[j].Amount;
      }
    }
    if (totalIce == 0f) {
      panel.SetValueColor("FontColor", new Color(255, 0, 0, 255));
      append(panel, "NO ICE on Oxygen Generator!");
    } else {
      append(panel, "Ice: " + totalIce.ToString("n2") +" kg");
    }
  }
}

void checkReactorState(IMyTextPanel panel) {
  List<IMyTerminalBlock> reactors = new List<IMyTerminalBlock>();
  GridTerminalSystem.GetBlocksOfType<IMyReactor>(reactors);
  
  float totalUranium = getCount(reactors, "Uranium");
  
  if (totalUranium < (0.1f * reactors.Count)) {
    panel.SetValueColor("FontColor", new Color(255, 0, 0, 255));
    append(panel, "NO Uranium on Reactors!");
  } else if (totalUranium < (3f * reactors.Count)) {
    panel.SetValueColor("FontColor", new Color(255, 255, 0, 255));
    append(panel, "LOW on Uranium in Reactors!");
  } else {
    append(panel, "Uranium: " + totalUranium.ToString("n2") +" kg");
  }
}

float getCount(List<IMyTerminalBlock> blocks, string type) { 
  float total = 0f; 
  List<IMyInventoryItem> items = new List<IMyInventoryItem>(); 
  for (int i = 0; i < blocks.Count; i++){ 
    IMyInventory inventory = blocks[i].GetInventory(0); 
    items.AddRange(inventory.GetItems()); 
  } 
 
  for (int i = 0; i< items.Count; i++) { 
    if (items[i].Content.SubtypeName == type) { 
      total += (float) items[i].Amount; 
    } 
  } 
  return total; 
}

void rewrite(IMyTextPanel panel, String text) { 
  panel.WritePublicText(text, false); 
  panel.ShowPublicTextOnScreen(); 
}

void append(IMyTextPanel panel, String text) {
  panel.WritePublicText("\n " + text, true);
  panel.ShowPublicTextOnScreen();
} 
