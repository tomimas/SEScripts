//by tomimas
//https://github.com/tomimas/SEScripts   
    
void Main(string argument) {
  string STATUS_PANEL = "LCD (Cargo)";  
  IMyTextPanel panel = GridTerminalSystem.GetBlockWithName(STATUS_PANEL) as IMyTextPanel;
  panel.SetValueColor("FontColor", new Color(255, 255, 255, 255));
  rewrite(panel, "");

  carcgoSpace(panel);
}

void cargoSpace(iIMyTextPanel panel) {
  List<IMyTerminalBlock> containers = new List<IMyTerminalBlock>();
  GridTerminalSystem.GetBlocksOfType<IMyCockpit>(containers);
  GridTerminalSystem.GetBlocksOfType<IMyShipDrill>(containers);
  GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(containers);

  float currentVolume = 0f;
  float maxVolume = 0f;
  for (int i = 0; i < containers.Count; ++i) {
    IMyInventoryOwner inventoryOwner = containers[i] as IMyInventoryOwner;
    IMyInventory inventory = inventoryOwner.GetInventory(0);
    currentVolume += (float) inventory.CurrentVolume;
    maxVolume += (float)inventory.MaxVolume;
  }
  rewrite(panel, "Cargo: " + currentVolume + " / " + maxVolume + " (" + Math.Round(currentVolume/maxVolume*100f, 0)  +"%)");
}

void rewrite(IMyTextPanel panel, String text) {
  panel.WritePublicText(text, false);
  panel.ShowPublicTextOnScreen();
}

void append(IMyTextPanel panel, String text) {
  panel.WritePublicText("\n " + text, true);
  panel.ShowPublicTextOnScreen();
} 
