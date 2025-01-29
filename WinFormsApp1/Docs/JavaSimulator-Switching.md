# Operation of the JavaSimulator "Switching" panel

## Preface

## Description

The Switching panel consists of 4 rotary knobs. Each rotary knob can moved to one 
of 3 positions.

## Protocol

Immediately after connecting to the controller box, if the panel is connected,
it returns the status of all 4 buttons.

All status changes are sent using an ASCII string terminated with '\r\n'.

Each line consists of the knob ID and a value, separated by a comma: ID,VALUE

Example:

```
SS01,0
SS01,1
SS01,0
SS01,1
SS01,2
SS01,1
```

Value is always an integer.

# Knobs

From left to right

ID: SS01
Values: 
- 0: CAPT
- 1: ATT HDG NORM
- 2: F/O

ID: SS02
- 0: CPAT
- 1: AIR DATA NORM
- 2: F/O

ID: SS03
- 0: CAPT
- 1: EIS DMC NORM
- 2: F/O

ID: SS04
- 0: CAPT
- 1: ECAM/ND XFR NORM
- 2: F/O