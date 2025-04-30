# AutoPauseMiddleware

Overview:
---------
AutoPauseMiddleware is a lightweight Windows utility that automatically pauses a running game 
when the user switches focus to Cheat Engine (or other memory editors) for safe and effective 
memory editing. This tool ensures the game does not progress or change state while you are 
inspecting or modifying memory values — mimicking Cheat Engine’s own pause functionality.

## Logo
![Logo](https://github.com/user-attachments/assets/3fe66055-d20a-4c09-a668-c762f1ceaaa9)

## Tool Image
![image](https://github.com/user-attachments/assets/bc7631a5-6f56-4abb-b7d3-8bfbf6643ac0)


Why It’s Needed:
----------------
Many games (especially emulators, sandboxed apps, or fullscreen games) don’t support native 
pause functionality or continue running even while Cheat Engine is being used. Editing memory 
values in real-time can lead to missed states, crashes, or unwanted behavior. This tool 
automatically pauses the game process at the system level — ensuring memory integrity and 
more accurate edits.

What It Does:
-------------
✔ Monitors the currently focused window.  
✔ When Cheat Engine is brought to focus:  
   - Simulates a keypress (e.g., ESC or user-defined) to pause the game.  
   - OR suspends the entire game process using `NtSuspendProcess`, completely freezing it.  
✔ When the user switches back to the game window:  
   - Resumes the game (if it was suspended).  
✔ Supports games that don’t respond to keyboard-based pausing.

How It Works:
-------------
The middleware uses native Windows APIs:
- `GetForegroundWindow` to detect the currently focused app.
- `GetWindowText` to match the game window title.
- `PostMessage` to send keypresses (ESC or custom).
- `NtSuspendProcess` and `NtResumeProcess` (via P/Invoke) to control the target process.

Workflow Logic
---------------
The application evaluates scenarios in the following order:

| Scenario                                | Result                  |
| --------------------------------------- | ----------------------- |
| ESC supported                           | Sends ESC               |
| ESC not supported but another key works | Sends alternate key     |
| No key works or no support for keyboard pausing, user agrees to suspend    | Uses `NtSuspendProcess` |
| No key works or no support for keyboard pausing, user says no              | No pause is done at all |

What is NtSuspendProcess?
---------------------------
`NtSuspendProcess` is a low-level, **undocumented Windows NT API** function from `ntdll.dll` that can suspend all threads of a running process, effectively "freezing" it in time. This capability is crucial when you need to pause a game that:
- Doesn’t support keyboard pausing.
- Keeps running in the background.
- Is based on an emulator or sandbox platform.

Unlike traditional input simulation, suspending a process **halts it at the OS level**, preventing *all execution* regardless of the game’s code, input focus, or renderer state.

When to Use NtSuspendProcess?
-------------------------------
✅✅ **Essential when you need a guaranteed freeze regardless of the UI.**  
✅ Useful for games/apps that:
- Don’t support pausing
- Run in fullscreen-exclusive mode
- Are based on custom engines or emulators  
❌ **Not needed** if the game natively pauses using keys (e.g., ESC or P)

Risks & Considerations:
-------------------------
- ⚠️ Some games may **crash or behave abnormally** on resume.
- ❌ **Never use on online or multiplayer games** — may trigger anti-cheat systems.
- 🛡️ Requires **Administrator privileges** for `PROCESS_SUSPEND_RESUME` access.
- ⛔ Not suitable for processes protected by kernel-level DRM or sandboxing.
- It is a **powerful OS-level operation** — intended only for offline/debugging use cases.

How to Use:
-----------
1. **Run AutoPauseMiddleware as Administrator.**
2. Enter the exact title of the game window (as it appears in the title bar) like for GTA V it's 'Grand Theft Auto V'.
3. Choose your pause method:
   - [ ] ESC key
   - [ ] Other key (e.g., F5, SPACE, etc.)
   - [ ] NtSuspendProcess (recommended for games that don't support pause)
4. Click **Start Monitoring**.
5. Launch Cheat Engine and switch focus to it.
   - Game will auto-pause (via keypress or system suspend).
6. Switch back to the game window.
   - Game will auto-resume (if suspended).
7. Click **Stop** or close the app to stop monitoring.

Technical Stack:
----------------
- Language: C#
- Framework: .NET Framework 4.7.2+
- UI: Windows Forms (WinForms)
- API Integration:
  - `user32.dll` (for window focus and messaging)
  - `kernel32.dll` (for process handles)
  - `ntdll.dll` (for `NtSuspendProcess`, `NtResumeProcess`)
- Architecture: 64-bit Windows
- Elevation: Requires Administrator rights

Example Use Case:
------------------
You're playing an emulated game in GTA V, which doesn’t pause when you open Cheat Engine. 
Manually pausing doesn't work and the emulator keeps running in the background. With 
AutoPauseMiddleware:
- You enter “GTA V” as the window title.
- Select “NtSuspendProcess”.
- Once you Alt-Tab to Cheat Engine, the emulator process is instantly frozen.
- After modifying memory values, switching back resumes the game as if nothing happened.

Conclusion:
-----------
AutoPauseMiddleware is an essential tool for reverse engineers, memory editors, and emulator users 
who need precise control over when a game is paused during memory analysis. With both input simulation 
and deep system-level suspension, it enables powerful workflows that were previously error-prone or impossible.

License:
--------
Free for personal, academic, or research use.  
Not intended for multiplayer or commercial cheating purposes.
