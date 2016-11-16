//by tomimas
//https://github.com/tomimas/SEScripts   

void Main(string argument) {
  string POWER_PANEL = "LCD (Power)";  
  IMyTextPanel pPanel = GridTerminalSystem.GetBlockWithName(POWER_PANEL) as IMyTextPanel;
  pPanel.SetValueColor("FontColor", new Color(255, 255, 255, 255));
  rewrite(pPanel, "");

  reactorInfo(pPanel);
  batteryInfo(pPanel);
}

void reactorInfo(IMyTextPanel panel) {
  List<IMyTerminalBlock> reactors = new List<IMyTerminalBlock>();
  GridTerminalSystem.GetBlocksOfType<IMyReactor>(reactors);
  
  float totalUranium = getItemCount(reactors, "Uranium");
  
  if (totalUranium < (0.1f * reactors.Count)) {
    panel.SetValueColor("FontColor", new Color(255, 0, 0, 255));
    append(panel, "NO Uranium on Reactors!");
  } else if (totalUranium < (3f * reactors.Count)) {
    panel.SetValueColor("FontColor", new Color(255, 255, 0, 255));
    append(panel, "LOW on Uranium in Reactors!");
  } else {
    append(panel, "Uranium: " + totalUranium.ToString("n2") +" kg");
  }
  
  string status = "";
  for (int i = 0; i < reactors.Count; i++) {
    status += (reactors[i].IsWorking ? " +" : " -");
  }
  append(panel, "Reactors:" + status);
}

float getItemCount(List<IMyTerminalBlock> blocks, string type) { 
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

void batteryInfo(IMyTextPanel panel) {
  List<IMyTerminalBlock> batteries = new List<IMyTerminalBlock>();
  GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(batteries);
  string status = "";
  for (int i = 0; i < batteries.Count; i++) {
    status += "#";
    status += (i+1);
    status += ": ";
    status += batteries[i].CurrentStoredPower.ToString("n2");
    status += batteries[i].IsCharging ? " (Charging)" : "";
    status += "\n";
  }
  append(panel, "Batteries:\n" + status);
}

void rewrite(IMyTextPanel panel, String text) {
  panel.WritePublicText(text, false);
  panel.ShowPublicTextOnScreen();
}

void append(IMyTextPanel panel, String text) {
  panel.WritePublicText("\n " + text, true);
  panel.ShowPublicTextOnScreen();
}
