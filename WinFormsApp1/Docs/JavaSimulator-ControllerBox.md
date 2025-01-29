# Operation of the JavaSimulator "Controller Box"

## Preface

The JavaSimulator Controller Box is used to control multiple different panels.
It seems to use an Arduino (compatible) microcontroler.

Tested devices:
- Switching panel
- ECAM

## Description

The Controller Box acts as a USB-to-Serial bridge, with a CH340 serial interface chip.

The following serial configuration seems to work:
- Baud rate: 19200
- New line: '\r\n'
- 1 stop bit
- No parity, no handshake, 8 data bits
- DTR & RTS enabled

## Backlight

The backlight seems to be controlled with only one command that affects all attached
devices. 

The serial command for backlight is 

```
PDLV,brightness
```
With brightness being an integer between (including) 0 and 9. 
Setting the brightness to 0 turns of the backlight.
Setting the brightness to 9 results in maximum brightness