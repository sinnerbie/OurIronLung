#include <Adafruit_LiquidCrystal.h>

#include <MPU6500_WE.h>

#include <Wire.h>

#define MPU6500_ADDR 0x68

MPU6500_WE myMPU6500 = MPU6500_WE(MPU6500_ADDR);

Adafruit_LiquidCrystal lcd(0);
String compas = "-------N------NE-------E------SE-------S------SW-------W-------NW"; // length 65

int buttonF = 8;
int buttonB = 9;

void setup() {
  Serial.begin(115200);
  /*Serial.println(compas.length());
  Wire.begin();
  if(!myMPU6500.init()){
    Serial.println("MPU6500 does not respond");
  }
  else{
    Serial.println("MPU6500 is connected");
  }
  pinMode(buttonF, INPUT_PULLUP);
  pinMode(buttonB, INPUT_PULLUP);
  pinMode(7, OUTPUT);

  Serial.println("Position you MPU6500 flat and don't move it - calibrating...");
  delay(1000);
  myMPU6500.autoOffsets();
  Serial.println("Done!");

  myMPU6500.enableGyrDLPF();

  myMPU6500.setGyrDLPF(MPU6500_DLPF_6);

  myMPU6500.setSampleRateDivider(5);

  myMPU6500.setGyrRange(MPU6500_GYRO_RANGE_250);

  myMPU6500.setAccRange(MPU6500_ACC_RANGE_2G);

  myMPU6500.enableAccDLPF(true);

  myMPU6500.setAccDLPF(MPU6500_DLPF_6);*/

  lcd.begin(16, 2);
  for (int i = 0; i < 16; i++)
    lcd.print(compas[i]);
  lcd.setCursor(0, 1);
  lcd.print("x.000  y.000");
  delay(200);
}

void loop() {
  /*if (Serial.available()>0){
    String input = Serial.readStringUntil('\n');
    
    if (input[0] == "C") {
      String toWrite = input;
      toWrite[0] = "";
      UpdateCompas(toWrite.toInt());
      delay(25);
    }

    if (input[0] == "D") {
      String toWrite = input;
      toWrite[0] = "";
      UpdatePosition(toWrite);
      delay(25);
    }
  }
  UpdateInputs();*/
}

void UpdateInputs()
{
  xyzFloat gValue = myMPU6500.getGValues();
  xyzFloat gyr = myMPU6500.getGyrValues();
  float temp = myMPU6500.getTemperature();
  float resultantG = myMPU6500.getResultantG(gValue);

  Serial.print("X");
  Serial.println(gValue.x);
  delay(25);
  Serial.print("Y");
  Serial.println(gValue.y);
  delay(25);
  Serial.print("Z");
  Serial.println(gValue.z);
  delay(25);
  Serial.print("F");
  Serial.println(digitalRead(buttonF));
  delay(25);
  Serial.print("B");
  Serial.println(digitalRead(buttonB));
  delay(500);
}

void UpdateCompas(int heading) {
  int mappedHeading = map(0, 360, 0, 65, heading);
  lcd.setCursor(0,0);
  lcd.print("");
  for (int i = mappedHeading; i < 16; i++)
    lcd.print(compas[i]);
}

void UpdatePosition(String toWrite) {
  lcd.setCursor(0, 1);
  lcd.print(toWrite);
}