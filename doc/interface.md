# Auth 伺服器

- 目前只提供單設備登入

## 目錄

### 註冊
1. [申請第一步:提供郵箱](申請第一步:提供郵箱)
2. [申請第二步:驗證郵箱](api/registration/step2_POST)
3. [註冊第三步:提供資訊](api/registration/step3_POST)

### 後台使用者操作
1. [後台新增使用者](api/employee_POST)

### 關於認證
1. [登入](api/auth/login_POST)
2. [登出](api/auth/logout_POST)
3. [更新令牌](api/auth/refresh_POST)
4. [取得使用者資訊](api/auth/information_GET)

### 更新
1. [更新密碼第一步:提供郵箱](api/update/password/step1_POST)
2. [更新密碼第二步:驗證郵箱](api/update/password/step2_POST)
3. [更新密碼第三步:輸入新密碼](api/update/password/step3_POST)


### 申請第一步:提供郵箱
- api/Registration/Step/1 POST


#### Request 

* __Body__

  | Parameter | Type   | Required | Description |
  | --------- | ------ | -------- | ----------- | 
  | email     | string | yes      | 郵箱

  __Example__
  > ```json
  > {
  >   "email": "supojen71742@gmail.com"
  > }
  > ```

#### Response

* __Success__

  | Parameter | Type | Description |
  | --------- | ---- | ----------- | 
  | -         | -    | -           |

* __Fail__

  | Status Code | Code                  | Description |
  | ----------- | --------------------- | ----------- |
  | 400         | Reg.EmailGasBeenUsed  | 郵箱已經被使用過



### 申請第二步:驗證郵箱
- api/Registration/Step/2 POST

#### Request 

* __Body__

  | Parameter | Type   | Required | Description |
  | --------- | ------ | -------- | ----------- | 
  | email     | string | yes      | 郵箱
  | code      | int    | yes      | 驗證碼

  __Example__
  > ```json
  > {
  >   "email": "supojen71742@gmail.com",
  >   "code": 123456
  > }
  > ```

#### Response

* __Success__

  | Parameter | Type | Description |
  | --------- | ---- | ----------- | 
  | -         | -    | -           |

* __Fail__

  | Status Code | Code                | Description |
  | ----------- | ------------------- | ----------- |
  | 400         | Req.CodeUncorrect   | 驗證碼不正確
  | 400         | Req.TryToomanyTimes | 驗證碼不對太多次, 請重新申請



### 申請第三步:提供資訊
- api/Registration/Step/3 POST

#### Request 

* __Body__

  | Parameter         | Type   | Required | Description |
  | ----------------- | ------ | -------- | ----------- | 
  | email             | string | yes      | 郵箱
  | password          | string | yes      | 密碼
  | confirmedPassword | string | yes      | 密碼確認
  | userName          | string | yes      | 使用者名稱
  | companyName       | string | yes      | 公司名稱
  | latitude          | double | yes      | 緯度
  | longitude         | double | yes      | 經度
  | formattedAddress  | string | yes      | 完整地址

  __Example__
  > ```json
  > {
  >   "email": "supojen71742@gmail.com",
  >   "password": "XXOOXX",
  >   "comfirmedPassword" : "XXOOXX",
  >   "userName": "蘇柏仁",
  >   "companyName": "六國景觀設計有限公司",
  >   "latitude": 25,
  >   "longitude": 121,
  >   "formattedAddress": "台北市民生東路三段130巷7弄13號2樓"
  > }
  > ```

#### Response

* __Success__

  | Parameter | Type | Description |
  | --------- | ---- | ----------- | 
  | companyId | long | 公司 ID

  __Example__
  > ```json
  > {
  >   "companyId": 1233456
  > }
  > ```

* __Fail__

  | Status Code | Code                | Description |
  | ----------- | ------------------- | ----------- |
  | 400         | Req.WeekPassword    | 密碼不夠安全,要有數字有英文,長度大於8個字母
  | 400         | Req.PasswordUnMatch | 兩次密碼輸入不一致


### 後台新增使用者
- api/employee/login POST
- 新增此用者的初始密碼會寄到後台人員的信箱裡面

#### Request 

* __Body__

  | Parameter    | Type       | Required | Description |
  | ------------ | ---------- | -------- | ----------- |
  | companyId    | long       | yes      | 公司 ID
  | name         | string     | yes      | 員工姓名
  | email        | string     | yes      | 員工郵箱
  | title        | string     | yes      | 員工職稱
  | boardingTime | DateOnly   | yes      | 員工到職日
  | permission   | int        | yes      | 員工權限
  | departmentId | Department | yes      | 部門
  | shiftId      | Shift      | yes      | 輪班
  | scheduleId   | Schedule   | yes      | 工作表
  | punchGroupId | PunchGroup | yes      | 打卡群組 

  __Example__
  > ```json
  > {
  >   "companyId": 1,
  >   "name": "蘇柏仁",
  >   "email": "brian71742@gmail.com",
  >   "title": "人事",
  >   "boardingTime": "2022-07-01", 
  >   "permission": 0, 
  >   "departmentId": 1,
  >   "shiftId": 1,
  >   "scheduleId": 1,
  >   "punchGroupId": 1
  > }
  > ```

#### Response

* __Success__

  __Department__
  | Parameter | type     | Description |
  | --------- | -------- | ----------- | 
  | id        | long     | 部門ID
  | name      | string   | 部門名稱

  __Shift__
  | Parameter | type     | Description |
  | --------- | -------- | ----------- | 
  | id        | long     | 輪班ID
  | name      | string   | 輪班名稱

  __Schedule__
  | Parameter | type     | Description |
  | --------- | -------- | ----------- | 
  | id        | long     | 工作表ID
  | name      | string   | 工作表名稱

  __PunchGroup__
  | Parameter | type     | Description |
  | --------- | -------- | ----------- | 
  | id        | long     | 打卡群組 ID
  | name      | string   | 打卡群組名稱

  | Parameter    | type       | Description |
  | ------------ | ---------- | ----------- | 
  | id           | long       | 員工ID
  | number       | string     | 員工編號
  | name         | string     | 員工姓名
  | email        | string     | 員工郵箱
  | title        | string     | 員工職稱
  | boardingTime | DateOnly   | 員工到職日
  | permission   | int        | 員工權限(0: 後台權限, 1: 主管權限, 2: 普通權限)
  | department   | Department | 部門
  | shift        | Shift      | 輪班
  | schedule     | Schedule   | 工作表
  | punchGroup   | PunchGroup | 打卡群組 

  __Example__
  > ```json
  > {
  >     "id": 1234556,
  >     "number": "TD00001",
  >     "name": "蘇柏仁",
  >     "email": "brian71742@gmail.com",
  >     "title": "人事",
  >     "boardingTime": "2022-07-01",
  >     "permission": 0,
  >     "department": {
  >       "id": 1,
  >       "name": "人事部"
  >    },
  >     "schedule": {
  >     "id": 1,
  >       "name": "標準工作表"
  >     },
  >     "shift": {
  >       "id": 1,
  >       "name": "標準輪班"
  >     },
  >     "punchGroup": {
  >       "id": 1,
  >       "name": "標準打卡群組"
  >     }
  > }
  > ```

* __Fail__

  | Status Code | Code                 | Description |
  | ----------- | -------------------- | ----------- |
  | 400         | emp.EmailHasBeenUsed | 電子郵箱已經被其他人使用過


### 登入
- api/auth/login POST

#### Request 

* __Body__

  | Parameter   | Type   | Required | Description |
  | ----------- | ------ | -------- | ----------- | 
  | email       | string | yes      | 使用者郵箱
  | password    | string | yes      | 使用者密碼
  | deviceType  | string | yes      | 裝置型號
  | deviceToken | string | no       | 裝置令牌

  __Example__
  > ```json
  > {
  >   "email": "brian71742@gmail.com",
  >   "password": "6705Brian",
  >   "deviceType": "IPHONE 8"
  > }
  > ```

#### Response

* __Success__

| Parameter    | Type   | Description |
| ------------ | ------ | ----------- | 
| accessToken  | string | Access 令牌
| refreshToken | string | Refresh 令牌

  __Example__
  > ```json
  > {
  >   "accessToken": "XXX.OOO.XXX",
  >   "refreshToken": "XXXOOOXXX"
  > }
  > ```

* __Fail__

  | Status Code | Code                   | Description |
  | ----------- | ---------------------- | ----------- |
  | 400         | Auth.UncorrectPassword | 密碼不正確
  | 400         | Auth.UserNotExist      | 使用者不存在



### 登出
- api/auth/logout POST

#### Request 

  | Parameter | Type | Required | Description |
  | --------- | ---- | -------- | ----------- | 
  | -         | -    | -        | -           |

#### Response

  | Parameter | Type | Description |
  | --------- | ---- | ----------- | 
  | -         | -    | -           |

* __Fail__

  | Status Code | Code                  | Description |
  | ----------- | --------------------- | ----------- |
  | -           | -                     | -           | 


### 更新令牌
- api/auth/refresh POST

#### Request

* __Body__

  | Parameter    | Type   | Required | Description |
  | ------------ | ------ | -------- | ----------- |
  | refreshToken | string | yes      | 更新令牌
  | deviceType   | string | yes      | 裝置型號
  | deviceToken  | string | yes      | 裝置令牌

  __Example__
  > ```json
  > {
  >   "refreshToken": "XXXXX",
  >   "deviceType": "IPHONE 8",
  >   "deviceToken": "XXOOXX"
  > }
  > ```

#### Response

* __Success__

| Parameter    | Type   | Description |
| ------------ | ------ | ----------- | 
| accessToken  | string | Access 令牌
| refreshToken | string | Refresh 令牌

  __Example__
  > ```json
  > {
  >   "accessToken": "XXX.OOO.XXX",
  >   "refreshToken": "XXXOOOXXX"
  > }
  > ```

* __Fail__

  | Status Code | Code                     | Description |
  | ----------- | ------------------------ | ----------- |
  | 401         | Auth.RefreshTokenExpried | 更新令牌過期 
  | 401         | Auth.TokenInValid        | 令牌不合法        


### 取得使用者資訊
- api/auth/information GET

#### Request

  | Parameter | Type | Required | Description |
  | --------- | ---- | -------- | ----------- | 
  | -         | -    | -        | -           |

#### Response

* __Body__

| Parameter  | Type   | Description |
| ---------- | ------ | ----------- | 
| id         | long   | Identity
| email      | string | 郵箱
| permission | int    | 權限

  __Example__
  > ```json
  > {
  >   "id": 123456,
  >   "email": "brian71742@gmail.com",
  >   "permission": 0
  > }
  > ```

* __Fail__

  | Status Code | Code                     | Description |
  | ----------- | ------------------------ | ----------- |
  | 401         | Auth.TokenInValid        | 令牌不合法    
  

### 更新密碼第一步
- api/update/password/step1 POST

#### Request 

* __Body__

| Parameter | Type   | Description |
| --------- | ------ | ----------- | 
| email     | string | 郵箱

  __Example__
  > ```json
  > {
  >   "email": "supojen71742@gmail.com"
  > }
  > ```

#### Response

  | Parameter | Type   | Description |
  | --------- | ------ | ----------- | 
  | -         | -      | -           |

* __Fail__

  | Status Code | Code | Description |
  | ----------- | ---- | ----------- |
  | -           | -    | -           | 


### 更新密碼第二步
- api/update/password/step2 POST

#### Request 

* __Body__

| Parameter | Type   | Description |
| --------- | ------ | ----------- | 
| email     | string | 郵箱
| code      | int    | 驗證碼

  __Example__
  > ```json
  > {
  >   "email": "supojen71742@gmail.com",
  >   "code": 123456
  > }
  > ```

#### Response

  | Parameter | Type   | Description |
  | --------- | ------ | ----------- | 
  | -         | -      | -           |

* __Fail__

  | Status Code | Code                   | Description |
  | ----------- | ---------------------- | ----------- |
  | 400         | Update.CodeUncorrect   | 驗證碼不正確
  | 400         | Update.TryTooManyTimes | 嘗試太多次


### 更新密碼第三步