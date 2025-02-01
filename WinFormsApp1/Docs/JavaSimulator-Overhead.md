# JavaSimulator Overhead Panel

## Preface

## Description

The following document describes the operation of the desktop/mini overhead panel by
JavaSimulator. The connection details (serial port, baud rate etc.) is the same
as for the JavaSimulator Controller Box:

The Controller Box acts as a USB-to-Serial bridge, with a CH340 serial interface chip.

The following serial configuration seems to work:
- Baud rate: 19200
- New line: '\r\n'
- 1 stop bit
- No parity, no handshake, 8 data bits
- DTR & RTS enabled

## Protocol

Immediately after connecting to the controller box, if the panel is connected,
it returns the status of all 4 buttons.

All status changes are sent using an ASCII string terminated with '\r\n'.

Each line consists of the knob ID and a value, separated by a comma: ID,VALUE

Example:

```
SS01,0;
SS01,1;
SS01,0;
SS01,1;
SS01,2;
SS01,1;
```

Value is always an integer.

The format for Korry switch lights is `K` followed by an underscore `_`, followed by `L` 
for the lower light or `U` for the upper light, followed by the switch number.

Example:

```
K_L11,1; # Turns on the lower light on Korry switch 11
K_U11,1; # Turns on the upper light on Korry switch 11
```

## Knobs, Lights

### ADIRS

- `K_U10`: Light IR 1 "FAULT"
- `K_L10`: Light IR 1 "ALIGN"

- `K_U11`: Light IR 3 "FAULT"
- `K_L11`: Light IR 3 "ALIGN"

- `K_U12`: Light IR 2 "FAULT"
- `K_L12`: Light IR 2 "ALIGN"

- `BTT`: Light "ON BAT"

### APU

- `K_U1`: Start "AVAIL"
- `K_L1`: Start "ON"

- `K_U2`: Master "FAULT"
- `K_L2`: Master "ON"

### Anti Ice

- `K_U3`: Wing "FAULT"
- `K_L3`: Wing "ON"

- `K_U4`: ENG 1 "FAULT"
- `K_L4`: ENG 1 "ON"

- `K_U5`: ENG 2 "FAULT"
- `K_L5`: ENG 2 "ON"

### Oxygen

- `K_U6`: Crew Supply ".."
- `K_L6`: Crew Supply "OFF"

### Air Cond

- `K_U7`: PACK 1 "FAULT"
- `K_L7`: PACK 1 "OFF"

- `K_U8`: APU BLEED "FAULT"
- `K_L8`: APU BLEED "ON"

- `K_U9`: PACK 2 "FAULT"
- `K_L9`: PACK 2 "OFF"

### GPWS

- `K_U13`: LDG FLAP 3 ".."
- `K_L13`: LDG FLAP 3 "ON"

### ELEC

- `K_U14`: BAT 1 "FAULT"
- `K_L14`: BAT 1 "OFF"

- `K_U15`: BAT 2 "FAULT"
- `K_L15`: BAT 2 "OFF"

- `K_U16`: EXT PWR "AVAIL"
- `K_L16`: EXT PWR "ON"

### FUEL

- `K_U17`: L TK PUMPS 1 "FAULT"
- `K_L17`: L TK PUMPS 1 "OFF"

- `K_U18`: L TK PUMPS 2 "FAULT"
- `K_L18`: L TK PUMPS 2 "OFF"

- `K_U19`: CTR TK L XFR "FAULT"
- `K_L19`: CTR TK L XFR "OFF"

- `K_U20`: XFEED "OPEN"
- `K_L20`: XFEED "ON"
- 
- `K_U21`: CTR TK R XFR "FAULT"
- `K_L21`: CTR TK R XFR "OFF"

- `K_U22`: R TK PUMPS 1 "FAULT"
- `K_L22`: R TK PUMPS 1 "OFF"

- `K_U23`: R TK PUMPS 2 "FAULT"
- `K_L23`: R TK PUMPS 2 "OFF"

### Buttons/knobs/...

```
3pos switch

S_OH_INT_LT_DOME                InteriorLight_Dome_Switch           T12
S_OH_NAV_IR1_MODE               ADIRS_IR_1_Knob                     R01
S_OH_NAV_IR2_MODE               ADIRS_IR_2_Knob                     R02
S_OH_NAV_IR3_MODE               ADIRS_IR_3_Knob                     R03
S_MISC_WIPER_CAPT               Wiper_Captain_Knob                  R04


Button

S_OH_CALLS_ALL					Call_All_Button						B04
S_OH_FIRE_ENG1_TEST             Fire_Engine_1_Test_Button           B01
S_OH_FIRE_APU_TEST              Fire_APU_Test_Button                B02
S_OH_FIRE_ENG2_TEST             Fire_Engine_2_Test_Button           B03
S_OH_ELEC_APU_START             APU_Start_Button                    K01
S_OH_ELEC_APU_MASTER            APU_Master_Button                   K02
S_OH_PNEUMATIC_WING_ANTI_ICE    Icing_Wing_Button                   K03
S_OH_PNEUMATIC_ENG1_ANTI_ICE    Icing_Engine_1_Button               K04
S_OH_PNEUMATIC_ENG2_ANTI_ICE    Icing_Engine_2_Button               K05
S_OH_OXYGEN_CREW_OXYGEN         Oxygen_Crew_Button                  K06
S_OH_PNEUMATIC_PACK_1           Pneumatic_Pack_1_Button             K07
S_OH_PNEUMATIC_APU_BLEED        Pneumatic_APU_Bleed_Button          K08
S_OH_PNEUMATIC_PACK_2           Pneumatic_Pack_2_Button             K09
S_OH_NAV_IR1_SWITCH             ADIRS_IR_1_Button                   K10
S_OH_NAV_IR3_SWITCH             ADIRS_IR_3_Button                   K11
S_OH_NAV_IR2_SWITCH             ADIRS_IR_2_Button                   K12
S_OH_GPWS_LDG_FLAP3             GPWS_Landing_Flap_3_Button          K13
S_OH_ELEC_BAT1                  Electrical_Battery_1_Button         K14
S_OH_ELEC_BAT2                  Electrical_Battery_2_Button         K15
S_OH_ELEC_EXT_PWR               Electrical_External_Power_Button    K16
S_OH_FUEL_LEFT_1                Fuel_Wing_Tank_Pump_Left_1_Button   K17
S_OH_FUEL_LEFT_2                Fuel_Wing_Tank_Pump_Left_2_Button   K18
S_OH_FUEL_CENTER_1              Fuel_Center_Tank_Pump_1_Button      K19
S_OH_FUEL_XFEED                 Fuel_Crossfeed_Button               K20
S_OH_FUEL_CENTER_2              Fuel_Center_Tank_Pump_2_Button      K21
S_OH_FUEL_RIGHT_1               Fuel_Wing_Tank_Pump_Right_1_Button  K22
S_OH_FUEL_RIGHT_2               Fuel_Wing_Tank_Pump_Right_2_Button  K23

LCD

N_ELEC_VOLT_BAT_1               Battery_Voltage_1                   BT1	(needs to be multiplied by 10x)
N_ELEC_VOLT_BAT_2               Battery_voltage_2                   BT2 (needs to be multiplied by 10x)

```