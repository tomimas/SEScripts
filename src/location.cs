// by tomimas
// https://github.com/tomimas/SEScripts
void Main(String argument) {
  string LOCATION_PANEL = "LCD (Location)";
  IMyTextPanel lPanel = GridTerminalSystem.GetBlockWithName(LOCATION_PANEL) as IMyTextPanel;

  positionInfo(lPanel, "Cockpit");
}

void positionInfo(IMyTextPanel panel, String source) {
  IMyCubeBlock block = GridTerminalSystem.GetBlockWithName(source) as IMyCubeBlock;

  double x = position(block, 0);
  double y = position(block, 1);
  double z = position(block, 2);

  rewrite(panel, "X: " + x.ToString() + "\nY: " + y.ToString() + "\nZ: " + z.ToString());
}

double position(IMyCubeBlock block, int axis) {
  return Math.Round(block.GetPosition().GetDim(axis), 2);
}

void rewrite(IMyTextPanel panel, String text) {
  panel.WritePublicText(text, false);
  panel.ShowPublicTextOnScreen();
}

void append(IMyTextPanel panel, String text) {
  panel.WritePublicText("\n " + text, true);
  panel.ShowPublicTextOnScreen();
}
