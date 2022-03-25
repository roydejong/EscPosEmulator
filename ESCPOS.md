# ESC/POS 
**Technical notes for implementing ESC/POS commands.**

## Basics

### What is it
- ESC/POS is a command [standard created by Epson](https://reference.epson-biz.com/modules/ref_escpos/index.php)
- The standard is dated but it's well supported by modern receipt printers

### General structure
- Commands are typically sent over a TCP/IP or Serial interface
- ESC/POS data and commands are simply sent as plain text (ASCII), with line breaks
- Each line is executed/interpreted as it comes in, in order


- To print text, you simply send a text line with a line break
- The `ESC` or `GS` characters (ASCII 27 and 29) denote the start of a command

- **Command reference**: https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=72#commands

### Weird things

#### Italics etc
- The official Epson docs don't have `ESC 4` or `ESC 5` listed at all (no reference to italics anywhere).
- The Pyramid ESC/POS docs list `ESC 4` as a command with an `n` parameter for off/on toggle.
- The node-escpos library I'm testing sends `ESC 4` to turn italics on, and `ESC 5` to turn italics off, but both with no params
- The Epson FX commands list also lists `ESC 4` as "Select italic mode" and `ESC 5` as "Cancel italic mode", but also lists "ESC E" and "ESC F" as emphasised mode on/off which conflicts with modern docs


- My best guess: Italics are probably not supported in modern ESC/POS. Pyramid's implementation may be vendor specific. Epson FX docs are *really old* (dot matrix era) and probably not a good reference. node-escpos may have made a mistake. In fact every esc/pos software library seems to reference old and deprecated commands.
- Conclusion: This is basically hardware specific there is no universal answer
