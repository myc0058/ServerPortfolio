# ServerPortpolio

* C#으로 만든 General하게 사용할수 있는 서버 프레임워크입니다.

* 포트폴리오를 확인을 위한 목적으로만 사용할수 있습니다. 개인적, 상용적인 어떤 목적으로든지 사용할 수 없습니다.




# 서버 특징

* Windows, Linux 호환, .Net Core 3.0

* Multi Thread, 오브젝트(User, Room등등)에 의한 Thread할당 (기준이 되는 Object는 Lock을 걸 필요가 없음, 순서가 보장됨)

* 간편한 패킷(Google Protobuf) 추가및 핸들러 추가 

* 패킷 테스트및 부하테스트를 위한 Multi User Bot

* IO(DB, Http통신등등)를 위한 간단한 비동기 처리

* 여러대의 서버를 하나의 Process(Standalone)에서 띄울수 있음

* (예정) Identity Framework(ORM)를 사용하여 편하게 DB 핸들링을 할수 있음.
         DB에 테이블만 있으면 코드는 자동추출 된다.(DB First or Code First)

* (예정) MasterData(or MetaData)를 쉽게 Excel 파일로 작성하고 
        이를 SQL 파일및 Json파일로 Export하고 이 데이타를 로딩하는 코드를 자동추출할수 있음.


# 빌드 및 간단한 설명

* VS 2019 Community & Unity 2019를 사용하였습니다.

* Standalone 프로젝트를 빌드하고 실행하면 모든 서버가 뜨게 됩니다.

* Unity Project를 실행하고 Start 버턴을 클릭하면 로그인부터 랜덤매칭후 방입장까지 이뤄집니다.

* 카메라 이동은 WASD, 캐릭터 이동은 방향키입니다.


# 프로젝트 목표

* 한방안에서 8명정도 플레이가 가능한 FPS를 만드는게 최종 목표입니다.

