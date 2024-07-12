#include <Arduino.h> 

const int leftSensorPin = 36; 
const int rightSensorPin = 39;
int trigPins[2]={0,};
int echoPins[2] = {0,};

int getUltrasonicDistance(int sensorNum)
{
    digitalWrite(trigPins[sensorNum], LOW);
    delayMicroseconds(2);
    digitalWrite(trigPins[sensorNum],HIGH);
    delayMicroseconds(10);
    digitalWrite(trigPins[sensorNum],LOW);

    // sensor read
    uint32_t duration = pulseIn(echoPins[sensorNum],HIGH,23200); // (microsec)
    // speed of sound
    int velocity= 344; // (m/s)
    // (velocity(m/s) * 100(cm/m) ) * ( duration(microsec)/1000000(s/microsec) ) / 2;
    int distance = (((uint32_t)velocity) * duration) / 20000;

    return distance;
}

void DcMotorControl(uint8_t MotNum, uint8_t CwCcw, int Speed)
{
    if(MotNum)      //-- right
    {
        if(CwCcw)   //-- Clockwise
        {
            ledcDetachPin(33);
            pinMode(33, OUTPUT);
            digitalWrite(33, LOW);
            
            ledcSetup(8, 5000, 10); 
            ledcAttachPin(32, 8);
            ledcWrite(8, Speed);
        }
        else        //-- CCW
        {
            ledcDetachPin(32);
            pinMode(32, OUTPUT);
            digitalWrite(32, LOW);
            
            ledcSetup(9, 5000, 10); 
            ledcAttachPin(33, 9);
            ledcWrite(9, Speed);
        }
    }
    else            //-- Left
    {
        if(CwCcw)   //-- Clockwise
        {
            ledcDetachPin(14);
            pinMode(14, OUTPUT);
            digitalWrite(14, LOW);
            
            ledcSetup(0, 5000, 10); 
            ledcAttachPin(13, 0);
            ledcWrite(0, Speed);
        }
        else        //-- CCW
        {
            ledcDetachPin(13);
            pinMode(13, OUTPUT);
            digitalWrite(13, LOW);
            
            ledcSetup(1, 5000, 10); 
            ledcAttachPin(14, 1);
            ledcWrite(1, Speed);
        }
    }
}


void setup()
{
  pinMode(leftSensorPin, INPUT);
  pinMode(rightSensorPin, INPUT);
  trigPins[0]=12;
  echoPins[0]=2;
  pinMode(trigPins[0], OUTPUT);
  pinMode(echoPins[0], INPUT);

}

void loop()
{
  // 왼쪽 적외선 센서 상태 읽기
  int leftSensorValue = digitalRead(leftSensorPin);
    
  // 오른쪽 적외선 센서 상태 읽기
  int rightSensorValue = digitalRead(rightSensorPin);
  
    if(getUltrasonicDistance(0)>=5)
    {
        if (leftSensorValue == HIGH && rightSensorValue == HIGH) {
            DcMotorControl(0, 1, 300); 
            DcMotorControl(1, 0, 300);
        }   

        else if (leftSensorValue == HIGH && rightSensorValue == LOW) {
            DcMotorControl(1, 0, 200); 
            DcMotorControl(0, 1, 400);
        }

        else if (rightSensorValue == HIGH && leftSensorValue == LOW) {
            DcMotorControl(0, 1, 200); 
            DcMotorControl(1, 0, 400);
        }

        else {
            DcMotorControl(0, 1, 0);
            DcMotorControl(1, 0, 0);
        }
    }

    else
    {
        if (leftSensorValue == LOW && rightSensorValue == LOW) {
            DcMotorControl(0, 1, 300); 
            DcMotorControl(1, 0, 300);
        }
           
        else if (leftSensorValue == LOW && rightSensorValue == HIGH) {
            DcMotorControl(1, 0, 200); 
            DcMotorControl(0, 1, 400);
        }
        
        else if (rightSensorValue == LOW && leftSensorValue == HIGH) {
            DcMotorControl(0, 1, 200);  
            DcMotorControl(1, 0, 400);
        }

        else {
            DcMotorControl(0, 1, 0);
            DcMotorControl(1, 0, 0);
        }
    }
}