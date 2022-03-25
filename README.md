# ESC/POS Receipt Printer Emulator
🖨️ **This app emulates a networked receipt printer to test your ESC/POS commands against.**

### About
- Windows application (WPF + .NET 6)
- Binds to a TCP/IP interface and listens for ESC/POS commands
- Logs commands and visually represents the resulting receipt(s)

👷 **This is an unfinished experiment.** Use at your own risk and keep your expectations low. :)

### Supported commands

⚠️ Support is currently limited to only a subset of ESC/POS. Even the commands listed here may only be partially implemented.

- Raw Text
- LF: Line feed
- CR: Carriage return
- ESC Commands:
  - Initialize printer (`ESC @`)
  - Select font (`ESC M`)
  - Select justification (`ESC a`)
  - Toggle emphasis (`ESC E`)
  - Toggle italic (`ESC 4` / `ESC 5`) *[possibly deprecated?]*
  - Toggle underline (`ESC -`)
- GS Commands:
  - Select character size
  - Select cut mode and cut paper

### Emulated printer

This program emulates a printer with the following specifications:

 - 80mm paper width
 - 72mm printing width
 - 180x180dpi
 - ASCII Font A/B: 12x24 pixels
 - Automatic line feed