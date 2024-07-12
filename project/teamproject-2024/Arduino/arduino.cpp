#include <Servo.h>                      // 서보모터 라이브러리 불러오기
#include <Adafruit_NeoPixel.h>
#include <Adafruit_TCS34725.h>
#include <Wire.h>

#define SERVO_PIN 9                     // 서보모터와 연결된 보드의 핀 정의
#define PIN_LED 5              // LED 연결 핀
#define NUMPIXELS 3            // LED 개수

// 감지할 색상 범위 (R, G, B의 최소 및 최대 값)
#define RED_MIN 0
#define RED_MAX 500
#define GREEN_MIN 0
#define GREEN_MAX 500
#define BLUE_MIN 0
#define BLUE_MAX 500


String red1 = "RED";
String redbox = "RED BOX";
String blue1 = "BLUE";
String bluebox = "BLUE BOX";
String green1 = "GREEN";
String greenbox = "GREEN BOX";




// set 90, 145,180
Servo servo;                            // 서보모터 객체를 생성
Adafruit_TCS34725 tcs = Adafruit_TCS34725(TCS34725_INTEGRATIONTIME_50MS, TCS34725_GAIN_4X);  // 컬러 센서 객체 생성
Adafruit_NeoPixel pixels(NUMPIXELS, PIN_LED, NEO_GRB + NEO_KHZ800);  // LED 스트립 객체 생성


// 모터 제어 변수
int motorSpeedPin = 11; // 1번 모터 회전 속도 
int motorDirectionPin = 13; // 모터 방향제어
String input = ""; // 입력을 저장할 변수

// 적외선 센서 변수
int Obj_Sensor = A0;
int val;

void setup() {
    Serial.begin(9600);

    // 모터제어:
    pinMode(motorDirectionPin, OUTPUT);  // 방향제어핀을 pinmode_OUTPUT으로 지정
    pinMode(motorSpeedPin, OUTPUT); // 모터 속도 제어 핀을 OUTPUT으로 설정

    // 적외선 센서
    pinMode(Obj_Sensor, INPUT);

    // 서브모터
    servo.attach(SERVO_PIN);
    servo.write(90);

    // LED 스트립 초기화
    pixels.begin();
    pixels.setBrightness(50);
    pixels.show(); // Initialize all pixels to 'off'

    // 컬러 센서 초기화
    if (tcs.begin()) {
        Serial.println("TCS34725 found");
    }
    else {
        Serial.println("No TCS34725 found ... check your connections");
        while (1);
    }
}

bool isMotorRunning = false;
bool isForward = true;

void loop() {
    val = analogRead(Obj_Sensor);
    //Serial.print("모터속도 .. ");
    //Serial.println(val);

    if (Serial.available() > 0) {
        input = Serial.readString();
        input.trim();

        if (input == "go") {
            isMotorRunning = true;
            isForward = true;
            motorForward();
        }
        else if (input == "back") {
            isMotorRunning = true;
            isForward = false;
            motorBackward();
        }
        else if (input == "stop") {
            isMotorRunning = false;
            motorStop();
        }
    }

    // 모터가 작동 중일 때 지속적으로 물체 감지 확인
    if (isMotorRunning && val < 300) {
        if (isForward) {
            Detect_Forward();

        }
        else {
            Detect_Backward();
        }
    }
}


void Detect_Forward() { // 물건 감지 함수 --> 차후에 물건이 감지되고 위즈카에 신호 보내는 코드 추가해야함!!!!
    //Serial.println("물건이 감지되었습니다.");
    delay(150);
    motorStop();
    delay(2000);
    int result = detectColor();
    if (result == 1) {
        motorForward();
        servo.write(90);
        delay(1000);
    }
    else if (result == 2) {
        motorForward();
        servo.write(145);
        delay(1000);

    }
    else if (result == 3) {
        motorForward();
        servo.write(180);
        delay(1000);

    }
    else {
        motorStop();
    }
}

void Detect_Backward() {// 물건 감지 함수 --> 차후에 물건이 감지되고 위즈카에 신호 보내는 코드 추가해야함!!!!
    Serial.println("물건이 감지되었습니다.");
    motorStop();
    delay(400);
    motorBackward();
}



void motorForward() {
    digitalWrite(motorDirectionPin, LOW);
    analogWrite(motorSpeedPin, 75);
    //servo.write(90);
    //Serial.println("모터가 정방향으로 동작합니다.");
}

void motorBackward() {
    digitalWrite(motorDirectionPin, LOW);
    analogWrite(motorSpeedPin, 75);
    //Serial.println("모터가 역방향으로 동작합니다.");
}

void motorStop() {
    digitalWrite(motorSpeedPin, LOW); // 모터 멈춤
    //Serial.println("모터가 멈췄습니다.");
    //servo.write(90);
}

void setLEDColor(int r, int g, int b) {
    for (int i = 0; i < NUMPIXELS; i++) {
        pixels.setPixelColor(i, pixels.Color(r, g, b));  // RGB 값으로 LED 색상 설정
    }
    pixels.show();  // LED 색상 변경
}

int detectColor() {
    uint16_t clear, red, green, blue;  // 컬러 센서 데이터 변수


    tcs.getRawData(&red, &green, &blue, &clear);  // 색상 감지 센서에서 측정 값 받아오기

    //Serial.print("R: ");
    //Serial.print(red);  // 시리얼 모니터에 빨간색 값 출력
    //Serial.print(" G: ");
    //Serial.print(green);  // 시리얼 모니터에 초록색 값 출력
    //Serial.print(" B: ");
    //Serial.println(blue);  // 시리얼 모니터에 파란색 값 출력

    // 색상 판단 및 LED와 서보모터 제어
    if (red >= RED_MIN && red <= RED_MAX && green >= GREEN_MIN && green <= GREEN_MAX && blue >= BLUE_MIN && blue <= BLUE_MAX) {
        if (red > green && red > blue && red > 200 && red < 700) {
            //Serial.println(red1);
            Serial.println(red1 + "," + redbox);
            setLEDColor(255, 0, 0);  // 빨간색 LED
            return 1;
        }
        else if (green > red && green > blue) {
            Serial.println(green1 + "," + greenbox);
            setLEDColor(0, 255, 0);  // 초록색 LED

            return 2;

        }
        else if (blue > red && blue > green) {
            Serial.println(blue1 + "," + bluebox);
            setLEDColor(0, 0, 255);  // 파란색 LED
            return 3;

        }
        else {
            Serial.println("알 수 없는 색상");
            setLEDColor(0, 0, 0);  // LED 끄기
            return 0;
        }
    }
    else {
        Serial.println("아무것도 감지되지 않음");
        setLEDColor(0, 0, 0);  // LED 끄기
        return 0;

    }

}
