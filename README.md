# Avatar Jogging Game

* https://github.com/ganeshsar/UnityPythonMediaPipeAvatar 의 오픈소스를 사용한 조깅 게임 제작
* 실내에서 규칙적으로 조깅을 즐기고 싶은 사람들을 위함

## IntroScene
혼자 달리기, 어제의 나와 달리기, 페이스메이커와 달리기 선택지 존재
![image](https://github.com/user-attachments/assets/16778127-f8c1-40a9-a436-b6739069e04b)

## CalibrationScene
키, 팔 길이, 상체 길이 입력 후 아바타 비율 조절<br/>
![image](https://github.com/user-attachments/assets/9298a96a-4d85-4ff0-8ed7-5e9c1ab05b7f)
## MainScene
![image](https://github.com/user-attachments/assets/081b5c43-c945-46af-90ba-6b33a22dca82)
![image](https://github.com/user-attachments/assets/b3d3fdb5-a614-4f11-9ad9-6abf38b517dd)
![image](https://github.com/user-attachments/assets/a3dd2ec2-2c41-4d14-9979-467e904579f4)

### Run Alone
* 나의 아바타로 조깅 가능
* 카메라에 인식되는 foot 랜드마크의 움직임 속도에 따라 아바타의 속도가 결정됨
### Run with last Record
* 주행 동안 달리기 속도 변화 시 변화 시간과 그 때의 속도를 저장
* `PlayerPrefs`에 달리기 종료 후 변화 시간, 속도 item의 배열을 `json`형태로 저장<br/>
### Run with Pace Maker
* `IntroScene`에서 페이스메이커의 속도 미리보기 및 지정 가능
*  멈추지 않고 시작 후 일정 속도로 계속 달림
