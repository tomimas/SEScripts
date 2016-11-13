// by tomimas
// https://github.com/tomimas/SEScripts
void Main(String argument) {
  string STATUS_PANEL = "LCD (Location)";
  string SOURCE = "Cockpit";
  IMyTextPanel panel = GridTerminalSystem.GetBlockWithName(STATUS_PANEL) as IMyTextPanel;
  panel.SetValueColor("FontColor", new Color(255, 255, 255, 255));
  rewrite(panel, "");

  IMyCubeBlock block = GridTerminalSystem.GetBlockWithName(SOURCE) as IMyCubeBlock;

  double x = position(block,0); 
  double y = position(block,1); 
  double z = position(block,2);

  rewrite(panel, "X: " + x.ToString() + "\nY: " + y.ToString() + "\nZ: " + z.ToString());
}

double position(IMyCubeBlock block, int axis) {
  return Math.Round(block.GetPosition().GetDim(axis), 4);
}

void rewrite(IMyTextPanel panel, String text) {
  panel.WritePublicText(text, false);
  panel.ShowPublicTextOnScreen();
}

void append(IMyTextPanel panel, String text) {
  panel.WritePublicText("\n " + text, true);
  panel.ShowPublicTextOnScreen();
}
