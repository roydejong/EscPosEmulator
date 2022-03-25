# ESC/POS Receipt Printer Emulator

🖨️ **This app emulates a receipt printer to test your ESC/POS commands against.**

- Windows application (WPF + .NET 6)
- Binds to a TCP/IP interface and listens for ESC/POS commands
- Logs commands and visually represents the resulting receipt(s)

👷 This is a work in progress. It's also pretty experimental. Use at your own risk and keep your expectations low. :)

## Supported commands

⚠️ Support is currently limited to only a subset of ESC/POS:

- Raw Text
- LF: Print and line feed
- ESC Commands:
  - Initialize printer 
  - Select font
  - Select justification
- GS Commands:
  - Select character size
  - Select cut mode and cut paper