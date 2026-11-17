#include <MPU6500_WE.h>

#include <Wire.h>

#define MPU6500_ADDR 0x68

MPU6500_WE myMPU6500 = MPU6500_WE(MPU6500_ADDR);

int buttonF = 8;
int buttonB = 9;

void setup() {
  Serial.begin(115200);
  Serial.println("Starting");
  Wire.begin();
  Serial.println("Wire has begun");
  if(!myMPU6500.init()){
    Serial.println("MPU6500 does not respond");
  }
  else{
    Serial.println("MPU6500 is connected");
  }
  Serial.println("ARGH");
  pinMode(buttonF, INPUT_PULLUP);
  pinMode(buttonB, INPUT_PULLUP);

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

  myMPU6500.setAccDLPF(MPU6500_DLPF_6);
  Serial.println("Hello");
  delay(200);
}

void loop() {
  xyzFloat gValue = myMPU6500.getGValues();
  xyzFloat gyr = myMPU6500.getGyrValues();
  float temp = myMPU6500.getTemperature();
  float resultantG = myMPU6500.getResultantG(gValue);
  
  if (Serial.available()){
    String input = Serial.readStringUntil('\n');
    if (input == "U") {
      pinMode(10, OUTPUT);
      digitalWrite(10, HIGH);
      delay(25);
    }
  }

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
  delay(1000);
}
