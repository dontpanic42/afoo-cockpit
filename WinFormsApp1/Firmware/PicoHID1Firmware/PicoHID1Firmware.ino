
#include <PicoGamepad.h>

#define PIN_THORTTLE_1 26
#define PIN_TILLER_WIPER 27
#define PIN_PEDAL_DISCONNECT 22


PicoGamepad gamepad;

// 16 bit integer for holding input values
int val;

void setup() {
  Serial.begin(115200);

  pinMode(LED_BUILTIN, OUTPUT);

  pinMode(PIN_THORTTLE_1, INPUT);
  pinMode(PIN_TILLER_WIPER, INPUT);
  pinMode(PIN_PEDAL_DISCONNECT, INPUT_PULLUP);

  digitalWrite(LED_BUILTIN, HIGH);
}

void loop() {

  // Get input value from Pico analog pin
  val = analogRead(PIN_THORTTLE_1);
  // Map analog 0-1023 value from pin to max HID range -32767 - 32767
  val = map(val, 0, 1023, -32767, 32767);
  // Send to hid devices
  gamepad.SetThrottle(val);

    // Get input value from Pico analog pin
  val = analogRead(PIN_TILLER_WIPER);
  // Map analog 0-1023 value from pin to max HID range -32767 - 32767
  val = map(val, 60, 980, -32767, 32767);
  // Send to hid devices
  gamepad.SetY(val);

  val = !digitalRead(PIN_PEDAL_DISCONNECT);
  // Send to hid devices
  gamepad.SetButton(0, val);

  // Send all inputs via HID 
  // Nothing is send to your computer until this is called.
  gamepad.send_update();

}
