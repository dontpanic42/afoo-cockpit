/***************************************************************************************
 * Serial Interface Documentation
 ***************************************************************************************/
/*
 * Command: cmd_setupDigitalPin
 *  ID: 0x00
 *  Length: 4
 *  Byte 1: 
 *      1-4: Command name
 *      4-8: Command parameters, 
 *  Byte 2: 
 *      1-8: Pin Number
 *  Bytes 3 - 4 unused
 *  Command parameters (4 bit):
 *      1: Pin direction, 0 = input, 1 = output
 *      2: Pin pullup, 0 = no pullup, 1 = pullup
 *
 * Command: cmd_setDigitalPin
 *  ID: 0x00
 *  Length: 3
 *  Byte 1: 
 *      1-4: Command name
 *      4-8: (unused)
 *  Byte 2: 
 *      1-8: Pin Number
 *  Byte 3:
 *      1: Value, 0: low, 1: hight
 *      2-8: (unused)
 *  Bytes 4:  unused
 *
 * Command: cmd_getDigitalPin
 *  ID: 0x00
 *  Length: 3
 *  Byte 1: 
 *      1-4: Command name
 *      4-8: (unused)
 *  Byte 2: 
 *      1-8: Pin Number
 *  Bytes 3-4:  unused
 */

/***************************************************************************************
 * Config 
 ***************************************************************************************/

 #define BAUDRATE 115200

 #define DIGITAL_PIN_MAX 12
 
 struct __attribute((packed)) DigitalPinConfig {
   bool isOutput;
   bool isPullup;
   bool isConfigured;
 };
 
 DigitalPinConfig digitalPinConfig[DIGITAL_PIN_MAX];
 
 /***************************************************************************************
  * Helper Functions 
  ***************************************************************************************/
 
 #define PROTOCOL_MAGIC 0xDEAD
 #define RESULT_SUCCESS 0
 #define RESULT_ERROR 1
 #define RESULT_ERROR_INVALID_COMMAND 2
 #define RESULT_ERROR_INVALID_CONFIGURATION 3
 #define RESULT_ERROR_INVALID_PARAMETER 4
 #define RESULT_ERROR_INVALID_PIN 5
 #define RESULT_FLAGS_NONE 0
 
 #define RESULT_NO_VALUE 0
 
 struct __attribute((packed)) CommandResult {
     uint8_t resultCode;
     uint16_t resultValue;
     uint8_t unused;
 };
 
 CommandResult handleCommandError(String command, String description, char errorCode) {
     CommandResult result = {errorCode, RESULT_NO_VALUE, RESULT_FLAGS_NONE};
     // Serial.println(command);
     // Serial.println(description);
     return result;
 }
 
 /***************************************************************************************
  * Commands 
  ***************************************************************************************/
 
 #define CMD_PARAM_PIN_DIRECTION_OUTPUT 1
 #define CMD_PARAM_PIN_ENABLE_PULL_UP 1
 
 #define NUM_COMMANDS 3
 #define MAX_COMMAND_LENGTH 4
 
 struct __attribute__((packed)) SetupDigitalPinCommand {
     uint8_t command : 4;
     uint8_t direction : 1;
     uint8_t pullup : 1;
     uint8_t pin;
 };
 
 CommandResult cmd_setupDigitalPin(uint8_t* packedCommand, size_t length) {
     if (sizeof(SetupDigitalPinCommand) > length) {
         return handleCommandError(packedCommand, "cmd_setupDigitalPin: Command is missing paramters (shorter than expected)", RESULT_ERROR_INVALID_PARAMETER);
     }
 
     SetupDigitalPinCommand* command = (SetupDigitalPinCommand*) packedCommand;
 
     uint8_t cmd = command->command;
     uint8_t pin = command->pin;
     bool isOutput = command->direction == CMD_PARAM_PIN_DIRECTION_OUTPUT;
     bool isPullup = command->pullup == CMD_PARAM_PIN_ENABLE_PULL_UP;
 
     if (pin == 0 || pin > DIGITAL_PIN_MAX) {
         return handleCommandError(packedCommand, "cmd_setDigitalPin: Cannot set pin - pin doesn't exist", RESULT_ERROR_INVALID_PIN);
     }
 
     uint8_t mode = (isOutput) ? OUTPUT : isPullup ? INPUT_PULLUP : INPUT;
     pinMode(pin, mode);
 
     digitalPinConfig[pin].isConfigured = true;
     digitalPinConfig[pin].isOutput = isOutput;
     digitalPinConfig[pin].isPullup = isPullup;
 
     CommandResult result = {RESULT_SUCCESS, RESULT_NO_VALUE, RESULT_FLAGS_NONE};
     return result;
 }
 
 struct __attribute__((packed)) SetDigitalPinCommand {
     uint8_t command;
     uint8_t pin;
     uint8_t value;
 };
 
 CommandResult cmd_setDigitalPin(uint8_t* packedCommand, size_t length) {
     if (sizeof(SetDigitalPinCommand) > length) {
         return handleCommandError(packedCommand, "cmd_setDigitalPin: Command is missing paramters (shorter than expected)", RESULT_ERROR_INVALID_PARAMETER);
     }
 
     SetDigitalPinCommand* command = (SetDigitalPinCommand*) packedCommand;
 
     uint8_t cmd = command->command;
     uint8_t pin = command->pin;
     uint8_t val = command->value;
 
     if (pin == 0 || pin > DIGITAL_PIN_MAX) {
         return handleCommandError(packedCommand, "cmd_setDigitalPin: Cannot set pin - pin doesn't exist", RESULT_ERROR_INVALID_PIN);
     }
 
     if (digitalPinConfig[pin].isConfigured == false || digitalPinConfig[pin].isOutput == false) {
         return handleCommandError(packedCommand, "cmd_setDigitalPin: Cannot set pin - pin is not configured or not an output", RESULT_ERROR_INVALID_CONFIGURATION);
     }
 
     char pinValue = (val & 0x01) ? HIGH : LOW;
     digitalWrite(pin, pinValue);
 
     CommandResult result = {RESULT_SUCCESS, RESULT_NO_VALUE, RESULT_FLAGS_NONE};
     return result;
 }
 
 struct __attribute__((packed)) GetDigitalPinCommand {
     uint8_t command;
     uint8_t pin;
 };
 
 CommandResult cmd_getDigitalPin(uint8_t* packedCommand, size_t length) {
     if (sizeof(GetDigitalPinCommand) > length) {
         return handleCommandError(packedCommand, "cmd_getDigitalPin: Command is missing paramters (shorter than expected)", RESULT_ERROR_INVALID_PARAMETER);
     }
 
     GetDigitalPinCommand* command = (GetDigitalPinCommand*) packedCommand;
 
     uint8_t cmd = command->command;
     uint8_t pin = command->pin;
 
     if (pin == 0 || pin > DIGITAL_PIN_MAX) {
         return handleCommandError(packedCommand, "cmd_setDigitalPin: Cannot set pin - pin doesn't exist", RESULT_ERROR_INVALID_PIN);
     }
 
     if (digitalPinConfig[pin].isConfigured == false || digitalPinConfig[pin].isOutput) {
         return handleCommandError(packedCommand, "cmd_setDigitalPin: Cannot set pin - pin is not configured or not an input", RESULT_ERROR_INVALID_CONFIGURATION);
     }
 
     uint8_t value = digitalRead(pin) == HIGH ? 1 : 0;
     CommandResult result = {RESULT_SUCCESS, value, RESULT_FLAGS_NONE};
     return result;
 }
 
 CommandResult (*commands[NUM_COMMANDS])(uint8_t*, size_t) = {
     cmd_setupDigitalPin,
     cmd_setDigitalPin,
     cmd_getDigitalPin,
 };
 
 CommandResult executeCommand(uint8_t* packedCommand, size_t size) {
     if(size == 0) {
         return handleCommandError(packedCommand, "command needs to have at least 1 byte", RESULT_ERROR_INVALID_COMMAND);
     }
 
     char command = packedCommand[0] & 0x0F;
 
     if (command < NUM_COMMANDS) {
         return commands[command](packedCommand, size);
     }
     
     return handleCommandError(packedCommand, "unknown command", RESULT_ERROR_INVALID_COMMAND);
 }
 
 /***************************************************************************************
  * Serial communication 
  ***************************************************************************************/
 
 
 void handleSerialInput(uint8_t* serialInputBuffer, size_t length) {
     // print the string when a newline arrives:
     CommandResult result = executeCommand(serialInputBuffer, length);
 
     size_t outputBufferSize = sizeof(CommandResult) + sizeof(uint16_t);
     uint8_t outputBuffer[outputBufferSize];
     outputBuffer[0] = PROTOCOL_MAGIC & 0xFF;
     outputBuffer[1] = (PROTOCOL_MAGIC >> 8) & 0xFF;
     outputBuffer[2] = result.resultCode;
     outputBuffer[3] = (result.resultValue & 0xFF);
     outputBuffer[4] = (result.resultValue >> 8) & 0xFF;
     //unused/reserved
     outputBuffer[5] = result.unused;
 
     Serial.write(outputBuffer, outputBufferSize);
 }
 
 // Magic event callback that's called whenever there's serial data available
 void serialEvent() {
 
     uint8_t serialInputBuffer[MAX_COMMAND_LENGTH];
 
     while (Serial.available() >= MAX_COMMAND_LENGTH) {
         memset(serialInputBuffer, 0, sizeof(serialInputBuffer));
         Serial.readBytes(serialInputBuffer, sizeof(serialInputBuffer));
         handleSerialInput(serialInputBuffer, sizeof(serialInputBuffer));
     }
 }
 
 /***************************************************************************************
  * Arduino methods
  ***************************************************************************************/
 
 void setup() {
     Serial.begin(BAUDRATE);
 }
 
 void loop() {
     // put your main code here, to run repeatedly:
 }
 
 