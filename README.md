# TestDll
Run("C:\Program Files (x86)\Steam\Steam.exe")
; 2. Ожидание появления окна авторизации
WinWaitActive("Войти в Steam")
; 3. Ввод логина (замените "YOUR_USERNAME" на ваш логин)
Sleep(500)
Send("YOUR_USERNAME{TAB}")
Sleep(500)
Send("YOUR_PASSWORD{ENTER}")
Sleep(10000)
Send("YOUR_GUARDCODE{ENTER}")
