//by tomimas
//https://github.com/tomimas/SEScripts   

void Main(string argument) {
  string CARGO_PANEL = "LCD (Cargo)";  
  IMyTextPanel cPanel = GridTerminalSystem.GetBlockWithName(CARGO_PANEL) as IMyTextPanel;

  cargoSpace(cPanel);
}

void cargoSpace(IMyTextPanel panel) {
  List<IMyTerminalBlock> containers = new List<IMyTerminalBlock>();
  List<IMyTerminalBlock> cache = new List<IMyTerminalBlock>();
  GridTerminalSystem.GetBlocksOfType<IMyCockpit>(cache);
  containers.AddRange(cache);
  GridTerminalSystem.GetBlocksOfType<IMyShipDrill>(cache);
  containers.AddRange(cache);
  GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(cache);
  containers.AddRange(cache);

  float currentVolume = 0f;
  float maxVolume = 0f;
  for (int i = 0; i < containers.Count; i++) {
    IMyInventory inventory = containers[i].GetInventory(0);
    currentVolume += (float) inventory.CurrentVolume;
    maxVolume += (float) inventory.MaxVolume;
  }
  rewrite(panel, "Cargo:\n" + (currentVolume * 1000) + " / " + (maxVolume * 1000) + "\n(" + Math.Round(currentVolume/maxVolume*100f, 0)  + "%)");
}

void rewrite(IMyTextPanel panel, String text) {
  panel.WritePublicText(text, false);
  panel.ShowPublicTextOnScreen();
}

void append(IMyTextPanel panel, String text) {
  panel.WritePublicText("\n " + text, true);
  panel.ShowPublicTextOnScreen();
} 
