# Auth 伺服器

- 目前只提供單設備登入

## 目錄

### 註冊
1. [申請第一步:提供郵箱](feature_reg_step1)
2. [申請第二步:驗證郵箱](feature_reg_step2)
3. [註冊第三步:提供資訊](feature_reg_step3)

### 關於認證
1. [登入](feature_auth_login)
2. [登出](feature_auth_logout)
3. [更新令牌](feature_auth_refresh)
4. [取得使用者資訊](faeture_auth_information)

### 更新
1. [更新密碼第一步:提供郵箱](feature_update_password_step1)
2. [更新密碼第二步:驗證郵箱](feature_update_password_step2)
3. [更新密碼第三步:輸入新密碼](feature_update_password_step3)


<br><br>

---
### <div id="feature_reg_step1">申請第一步:提供郵箱</div>
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

<br><br>

--- 
### <div id="feature_reg_step2">申請第二步:驗證郵箱</div>
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

  | Parameter | Type   | Description |
  | --------- | ------ | ----------- | 
  | token     | string | 註冊令牌      

  __Example__
  > ```json
  > {
  >   "token": "XXXOOOXXX"
  > }
  > ```

* __Fail__

  | Status Code | Code                | Description |
  | ----------- | ------------------- | ----------- |
  | 400         | Req.CodeUncorrect   | 驗證碼不正確
  | 400         | Req.TryToomanyTimes | 驗證碼不對太多次, 請重新申請

<br><br>

---
### <div id="feature_reg_step3">申請第三步:提供資訊</div>
- api/Registration/Step/3 POST

#### Request 

* __Body__

  | Parameter         | Type   | Required | Description |
  | ----------------- | ------ | -------- | ----------- | 
  | token             | string | yes      | 註冊令牌
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
  >   "token": "XXXOOO",
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

<br><br>

---
### <div id="feature_auth_login">登入</div>
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

<br><br>

---
### <div id="feature_auth_logout">登出</div>
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

<br><br>

---
### <div id="feature_auth_refresh">更新令牌</div>
- api/auth/refresh POST

#### Request

* __Body__

  | Parameter    | Type   | Required | Description |
  | ------------ | ------ | -------- | ----------- |
  | refreshToken | string | yes      | 更新令牌
  | deviceType   | string | yes      | 裝置型號

  __Example__
  > ```json
  > {
  >   "refreshToken": "XXXXX",
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

  | Status Code | Code                     | Description |
  | ----------- | ------------------------ | ----------- |
  | 401         | Auth.RefreshTokenExpried | 更新令牌過期 
  | 401         | Auth.TokenInValid        | 令牌不合法        

<br><br>

---
### <div id="feature_auth_information">取得使用者資訊</div>
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
  
<br><br>

---
### <div id="feature_update_password_step1">更新密碼第一步</div>
- api/update/password/step/1 POST

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

<br><br>

---
### <div id="feature_update_password_step2">更新密碼第二步</div>
- api/update/password/step/2 POST

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
- api/update/password/step/3 POST

#### Request

* __Body__

| Parameter | Type | Required | Description |
| --------- | ---- | -------- | ----------- |
| email     | 
| password  |