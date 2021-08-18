
; -------------------------
; spust Evernote 
; -------------------------

send {LWin down}
send {LShift down}

send F

send {LWin up}
send {LShift up}

Sleep, 100


; -------------------------
; spust vyhladavanie 
; -------------------------

send {LCtrl down}

send q

send {LCtrl up}

Sleep, 100

; -------------------------
; vloz poziadavku a vyhladaj
; -------------------------

send %1%

Sleep, 1000

send {Up}

send {ENTER}






