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
