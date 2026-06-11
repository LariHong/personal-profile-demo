# 個人基本資料維護 Demo

這是一個面試展示用的個人基本資料維護系統，重點是用小而完整的垂直切片呈現前後端串接、資料驗證、錯誤處理與可維護性設計。

- 後端：C# ASP.NET Core 10 RESTful API
- 前端：Vue3 + Vite，原始碼位於 `client/`
- 資料庫：EF Core + SQL Server
- 登入頁：依需求刻意不包含

## 展示重點

- RESTful CRUD API 設計，使用 DTO 定義輸入與輸出合約。
- EF Core 持久化，並以唯一索引限制身分證字號不可重複。
- 後端驗證必填欄位、性別值、電話格式與台灣身分證檢核規則。
- 集中處理 API 例外，包含資料重複、資料庫錯誤與非預期錯誤。
- Vue3 表單與列表流程，支援查詢、新增、編輯、刪除、loading 狀態與使用者可讀的錯誤訊息。
- 以面試可維護範圍為核心，刻意維持小而完整的功能切片，不把登入、角色或部署混入第一版。

## 3 分鐘展示流程

1. 開啟 Vue 頁面，確認個人資料列表可以載入。
2. 使用有效的台灣身分證字號新增一筆資料。
3. 使用無效的台灣身分證字號送出，確認畫面會顯示 API 驗證錯誤。
4. 使用相同身分證字號再次新增，確認會顯示資料重複錯誤。
5. 依姓名、縣市、鄉鎮市區、電話或身分證字號查詢。
6. 編輯資料、儲存後再刪除資料。

## 專案結構

```text
src/PersonalProfile.Api/
  Controllers/       HTTP endpoints
  Data/              EF Core DbContext 與 model 設定
  Dtos/              API request/response contracts
  Middleware/        API 錯誤回應處理
  Models/            持久化 entity
  Services/          業務規則與資料操作
client/              Vue3 + Vite 前端原始碼
tests/               輕量驗證檢查專案
docs/slices/         範圍與切片規劃文件
```

## 前端模組

Vue3 前端將頁面責任拆開，讓面試官能看出 API 呼叫、狀態管理與畫面元件的邊界：

- `client/src/api/profiles.js`：個人資料 API 呼叫與 HTTP 錯誤解析。
- `client/src/composables/useProfiles.js`：列表、查詢、表單狀態與 CRUD 流程。
- `client/src/components/ProfileForm.vue`：新增與編輯表單。
- `client/src/components/ProfileTable.vue`：資料列表與列操作。
- `client/src/constants/genders.js`：共用性別選項與顯示文字。

## 執行方式

啟動 API：

```powershell
cd src/PersonalProfile.Api
dotnet restore
dotnet run
```

另開一個終端機啟動 Vue dev server：

```powershell
cd client
npm install
npm run dev
```

開發時開啟 `http://127.0.0.1:5173`。Vite dev server 會將 `/api` proxy 到 `http://127.0.0.1:5188`。

若要以單一 ASP.NET Core demo app 執行：

```powershell
cd client
npm run build
cd ../src/PersonalProfile.Api
dotnet run
```

開啟 `dotnet run` 輸出的 URL。預設 launch profile 使用 `http://localhost:5188`。

開發用 API URL 設定在以下兩處：

- `src/PersonalProfile.Api/Properties/launchSettings.json`
- `client/vite.config.js`

## SQL Server

預設 connection string 使用 LocalDB：

```json
"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=PersonalProfileDemo;Trusted_Connection=True;TrustServerCertificate=True"
```

如果要改用其他 SQL Server instance，可以透過 user secrets、環境變數或 `appsettings.Development.json` 覆寫設定。

## API 合約

本機開發 Base URL：`http://localhost:5188`

目前 API 使用 `v1` 路徑，讓前後端通訊合約可以在後續版本演進時保留相容性。

- `GET /api/v1/profiles?keyword=...`
- `GET /api/v1/profiles/{id}`
- `POST /api/v1/profiles`
- `PUT /api/v1/profiles/{id}`
- `DELETE /api/v1/profiles/{id}`

HTTP status code 約定：

- `200 OK`：查詢或更新成功。
- `201 Created`：新增成功，並回傳新資料。
- `204 No Content`：刪除成功。
- `400 Bad Request`：欄位驗證失敗，回傳 `ValidationProblemDetails`。
- `404 Not Found`：指定資料不存在。
- `409 Conflict`：身分證字號重複。
- `500 Internal Server Error`：資料庫或非預期伺服器錯誤。

錯誤回應統一使用 `application/problem+json`，內容包含 `type`、`title`、`status`、`detail`、`instance` 與 `traceId`。前端 API client 只依照這個 contract 解析錯誤訊息，避免畫面層直接猜測後端錯誤格式。

## 後端橫切關注點

新增、編輯、刪除 API 使用 `AuditActionAttribute` 標記需要稽核的動作，並由 `AuditActionFilter` 透過 DI 取得 `IAuditLogger` 與 `ISystemClock`。這個設計展示 Attribute、DI 與 filter 型的 AOP 思路：

- Controller 只宣告業務動作，例如 `profiles.create`、`profiles.update`、`profiles.delete`。
- Filter 統一記錄 HTTP method、path、status code、duration 與 traceId。
- 目前稽核輸出寫入 structured logging，不建立資料庫 audit table，避免 demo scope 過度膨脹。

必填欄位：

- `nationalId`：台灣身分證字號
- `name`：姓名
- `gender`：`Male`、`Female` 或 `Other`
- `birthday`：生日，格式為 `yyyy-MM-dd`
- `city`：縣市
- `district`：鄉鎮市區
- `address`：地址
- `phone`：聯絡電話

## 驗證方式

執行與 CI workflow 相同的基本檢查：

```powershell
dotnet build PersonalProfileDemo.slnx
dotnet run --project tests/PersonalProfile.ValidationCheck/PersonalProfile.ValidationCheck.csproj
cd client
npm ci
npm run build
```

也可以只執行輕量驗證檢查：

```powershell
dotnet run --project tests/PersonalProfile.ValidationCheck/PersonalProfile.ValidationCheck.csproj
```

目前這個檢查會驗證代表性的有效與無效台灣身分證字號案例。下一個更接近面試正式作品的改善方向，是將 console check 升級成 xUnit 或 NUnit 測試，涵蓋 validator、service rules 與 API endpoints。

## 刻意取捨

- Authentication 與 authorization 目前排除，因為題目重點是個人基本資料維護。
- 系統預設使用 SQL Server LocalDB，方便在 Windows 面試展示環境快速啟動。
- 啟動時使用 EF Core 建立 schema 是 demo 便利性取捨；若要更接近 production handoff，應加入 EF Core migrations 並文件化 migration 指令。
- 分頁與進階排序目前排除，等資料量需求足以支持複雜度時再加入。
