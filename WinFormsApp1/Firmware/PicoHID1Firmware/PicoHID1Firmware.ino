
#include <PicoGamepad.h>

#define PIN_THORTTLE_1 26

#define PIN_TILLER_WIPER 27
#define PIN_PEDAL_DISCONNECT 22
#define MIN_TILLER 60
#define MAX_TILLER 980
#define BUTTON_PEDAL_DISCONNECT 0

#define PIN_SPEED_BRAKE_WIPER 28
#define MIN_SPEED_BRAKE 500
#define MAX_SPEED_BRAKE 1024
#define BUTTON_GROUND_SPOILER 1
#define GROUND_SPOILER_LESS_THAN 500

#define HID_MIN -32767
#define HID_MAX 32767


PicoGamepad gamepad;

// 16 bit integer for holding input values
int val;

void setup() {
  Serial.begin(115200);

  pinMode(LED_BUILTIN, OUTPUT);

  pinMode(PIN_THORTTLE_1, INPUT);
  pinMode(PIN_TILLER_WIPER, INPUT);
  pinMode(PIN_PEDAL_DISCONNECT, INPUT_PULLUP);
  pinMode(PIN_SPEED_BRAKE_WIPER, INPUT);

  digitalWrite(LED_BUILTIN, HIGH);
}

void processSpeedBrake() {
  // put your main code here, to run repeatedly:
  // Get input value from Pico analog pin
  val = analogRead(PIN_SPEED_BRAKE_WIPER);

  // Ground spoiler armed
  if (val < GROUND_SPOILER_LESS_THAN) {
    val = HID_MIN;
    gamepad.SetButton(BUTTON_GROUND_SPOILER, true);
  } else {
    val = max(MIN_SPEED_BRAKE, val);
    val = min(MAX_SPEED_BRAKE, val);
    val = map(val, MIN_SPEED_BRAKE, MAX_SPEED_BRAKE, HID_MIN, HID_MAX);
    gamepad.SetButton(BUTTON_GROUND_SPOILER, false);
  }

  gamepad.SetZ(val);
}

void processTiller() {
    // Get input value from Pico analog pin
  val = analogRead(PIN_TILLER_WIPER);
  
  val = min(MAX_TILLER, val);
  val = max(MIN_TILLER, val);
  val = map(val, MIN_TILLER, MAX_TILLER, HID_MIN, HID_MAX);
  // Send to hid devices
  gamepad.SetY(val);

  val = !digitalRead(PIN_PEDAL_DISCONNECT);
  // Send to hid devices
  gamepad.SetButton(BUTTON_PEDAL_DISCONNECT, val);
}

void processThrottle() {
    // Get input value from Pico analog pin
  val = analogRead(PIN_THORTTLE_1);
  // Map analog 0-1023 value from pin to max HID range -32767 - 32767
  val = map(val, 0, 1023, HID_MIN, HID_MAX);
  // Send to hid devices
  gamepad.SetThrottle(val);
}

void loop() {

  processThrottle();
  // Without this, raspberry crashes ;-)
  delay(10);
  processTiller();
  // Without this, raspberry crashes ;-)
  delay(10);
  processSpeedBrake();

  gamepad.send_update();

}