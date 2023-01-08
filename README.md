# GitlabTemplateGenerator
## Gitlab Repository 專案樣板產生工具

### 緣由
GitHub 上面可以直接把 Repository 變成 Template，這樣如果新的 Repository 想要使用同樣的檔案結構，
可以直接套用該 Tempalte，這樣 Repository 就有相同的資料夾結構了。

不過在 Gitlab 上面並沒有這項功能，只有 Premium 以上的版本才有支援。

為此，本工具的目的是，建立一個 GUI工具，可指定樣板 Repository 的 Gitlab 網址，以及新建的 Gitlab Repository，
會先從樣板專案 Clone 下來，再去 Clone 新建專案的 Gitlab 網址把指定專案 Clone 下來，並將樣板專案的資料夾全部複製到
指定的 Repository 資料夾中。

### Todos
- [x] 測試從程式去呼叫 git 指令 clone 是否正常
- [x] 測試複製專案流程是否正常
- [] 建立 WPF 專案讓使用者可以點選決定專案路徑

